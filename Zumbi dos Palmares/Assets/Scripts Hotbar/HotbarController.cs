using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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
    public string[] itemNomes;
    public string[] itemDescricoes;

    [Header("Slots Visuais")]
    public GameObject[] itemSlots;

    [Header("Painel de Informações")]
    public GameObject itemInfoPanel;
    public TMP_Text itemInfoText;
    public Image itemInfoIcon;

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

        if (luzTocha != null)
            luzTocha.SetActive(false);

        if (itemInfoPanel != null)
            itemInfoPanel.SetActive(false);
    }

    void Update()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectSlot(i);
        }

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

                itemSlots[i].transform.Find("Icon").GetComponent<Image>().sprite = itemIcons[itemID];
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

    public bool IsEquipavelSelected()
    {
        int id = GetSelectedItemID();
        return id != -1 && itemTypes[id] == ItemType.Equipavel;
    }

    void SelectSlot(int i)
    {
        for (int s = 0; s < itemSlots.Length; s++)
            itemSlots[s].transform.Find("Highlight").gameObject.SetActive(false);

        itemSlots[i].transform.Find("Highlight").gameObject.SetActive(true);
        selectedSlot = i;

        if (itemInfoPanel != null)
            itemInfoPanel.SetActive(false); // Oculta automaticamente ao trocar item

        int id = slotItemID[i];

        // Ativa a luz da tocha se o item for a tocha
        if (id == tochaID && itemTypes[tochaID] == ItemType.Equipavel)
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

        if (id == cenouraID && itemTypes[id] == ItemType.Consumivel)
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

                if (luzTocha != null)
                    luzTocha.SetActive(false);

                if (itemInfoPanel != null)
                    itemInfoPanel.SetActive(false);
                if (itemInfoIcon != null)
                    itemInfoIcon.gameObject.SetActive(false);
            }
        }

        SaveHotbar();
    }

    public void MostrarInfoDoItemSelecionado()
    {
        if (selectedSlot == -1 || slotItemID[selectedSlot] == -1)
        {
            if (itemInfoPanel != null)
                itemInfoPanel.SetActive(false);
            return;
        }

        int id = slotItemID[selectedSlot];

        if (itemInfoPanel != null && itemInfoText != null && itemInfoIcon != null)
        {
            itemInfoPanel.SetActive(true);
            itemInfoText.text = $"<b>{itemNomes[id]}</b>\n{itemDescricoes[id]}\nTipo: {itemTypes[id]}";
            itemInfoIcon.sprite = itemIcons[id];
            itemInfoIcon.gameObject.SetActive(true);
        }
    }

    public enum ItemType { Consumivel, Equipavel }

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

            var icon = itemSlots[i].transform.Find("Icon").GetComponent<Image>();
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

    public void FecharPainelInfo()
    {
        if (itemInfoPanel != null)
            itemInfoPanel.SetActive(false);
    }
}
