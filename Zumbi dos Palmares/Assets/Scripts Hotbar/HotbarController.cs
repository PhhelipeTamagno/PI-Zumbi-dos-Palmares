using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HotbarController : MonoBehaviour
{
    [Header("Itens Especiais")]
    public int cenouraID = 1;
    public int curaPorCenoura = 1;

    [Header("Item Tocha")]
    public int tochaID = 2;
    public GameObject luzTocha;

    [Header("Catálogo de Itens")]
    public Sprite[] itemIcons;
    public ItemType[] itemTypes;

    [Header("Slots Visuais")]
    public GameObject[] itemSlots;

    private int[] slotItemID;
    private int[] slotItemQtd;
    private int selectedSlot = -1;
    private static bool hotbarClearedThisSession = false;

    void Start()
    {
        slotItemID = new int[itemSlots.Length];
        slotItemQtd = new int[itemSlots.Length];

        if (!hotbarClearedThisSession)
        {
            ClearHotbarData();
            hotbarClearedThisSession = true;
        }

        LoadHotbar();

        // Desativa luz da tocha no início
        if (luzTocha != null)
            luzTocha.SetActive(false);
    }

    void Update()
    {
        for (int i = 0; i < itemSlots.Length; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectSlot(i);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            int newSlot = selectedSlot + (scroll > 0 ? 1 : -1);
            if (newSlot < 0) newSlot = itemSlots.Length - 1;
            if (newSlot >= itemSlots.Length) newSlot = 0;
            SelectSlot(newSlot);
        }

        if (Input.GetKeyDown(KeyCode.E) && selectedSlot != -1 && slotItemID[selectedSlot] != -1)
            UseItem(selectedSlot);
    }

    public void AddItemToHotbar(int itemID)
    {
        for (int i = 0; i < slotItemID.Length; i++)
        {
            if (slotItemID[i] == itemID)
            {
                slotItemQtd[i]++;
                SaveHotbar();
                return;
            }
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (slotItemID[i] == -1)
            {
                slotItemID[i] = itemID;
                slotItemQtd[i] = 1;

                itemSlots[i].transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite = itemIcons[itemID];
                itemSlots[i].transform.Find("Icon").gameObject.SetActive(true);

                SelectSlot(i);
                SaveHotbar();
                return;
            }
        }

        Debug.Log("Hotbar cheia!");
    }

    public int GetSelectedItemID() =>
        selectedSlot != -1 ? slotItemID[selectedSlot] : -1;

    public bool IsEquipableSelected()
    {
        int id = GetSelectedItemID();
        return id != -1 && itemTypes[id] == ItemType.Equipable;
    }

    void SelectSlot(int i)
    {
        for (int s = 0; s < itemSlots.Length; s++)
            itemSlots[s].transform.Find("Highlight").gameObject.SetActive(false);

        itemSlots[i].transform.Find("Highlight").gameObject.SetActive(true);
        selectedSlot = i;

        // 🔥 Ativa a luz da tocha se o item for a tocha
        if (slotItemID[i] == tochaID && itemTypes[tochaID] == ItemType.Equipable)
        {
            if (luzTocha != null)
                luzTocha.SetActive(true);
        }
        else
        {
            if (luzTocha != null)
                luzTocha.SetActive(false);
        }
    }

    void UseItem(int slot)
    {
        int id = slotItemID[slot];
        if (id == -1) return;

        PlayerHealthUI playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerHealthUI>();
        if (playerHealth == null) return;

        if (id == cenouraID && itemTypes[id] == ItemType.Consumable)
        {
            if (playerHealth.currentHealth >= playerHealth.maxHealth)
            {
                Debug.Log("Você está com a vida cheia.");
                return;
            }

            playerHealth.Heal(curaPorCenoura);

            slotItemQtd[slot]--;
            if (slotItemQtd[slot] <= 0)
            {
                slotItemID[slot] = -1;
                itemSlots[slot].transform.Find("Icon").gameObject.SetActive(false);
                if (selectedSlot == slot) selectedSlot = -1;

                // Desativa a luz da tocha se for ela
                if (luzTocha != null)
                    luzTocha.SetActive(false);
            }
        }

        SaveHotbar();
    }

    public enum ItemType { Consumable, Equipable }

    public void SaveHotbar()
    {
        for (int i = 0; i < slotItemID.Length; i++)
        {
            PlayerPrefs.SetInt("HotbarSlot" + i, slotItemID[i]);
            PlayerPrefs.SetInt("HotbarQtd" + i, slotItemQtd[i]);
        }
        PlayerPrefs.Save();
    }

    void LoadHotbar()
    {
        for (int i = 0; i < slotItemID.Length; i++)
        {
            int id = PlayerPrefs.GetInt("HotbarSlot" + i, -1);
            int qtd = PlayerPrefs.GetInt("HotbarQtd" + i, 0);

            slotItemID[i] = id;
            slotItemQtd[i] = qtd;

            var icon = itemSlots[i].transform.Find("Icon").GetComponent<UnityEngine.UI.Image>();
            icon.gameObject.SetActive(id != -1);
            if (id != -1) icon.sprite = itemIcons[id];
        }
    }

    void ClearHotbarData()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            PlayerPrefs.DeleteKey("HotbarSlot" + i);
            PlayerPrefs.DeleteKey("HotbarQtd" + i);
        }
        PlayerPrefs.Save();
    }

    public void NovoJogo()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene("CenaInicial");
    }
}
