using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HotbarController : MonoBehaviour
{
    [Header("Catálogo de Itens")]
    public Sprite[] itemIcons;
    public ItemType[] itemTypes;

    [Header("Slots Visuais")]
    public GameObject[] itemSlots;
    // public TextMeshProUGUI[] quantidadeTexts; // REMOVIDO

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
                // UpdateQuantidadeText(i); // REMOVIDO
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

                // UpdateQuantidadeText(i); // REMOVIDO
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
    }

    void UseItem(int slot)
    {
        int id = slotItemID[slot];
        if (id == -1) return;

        if (itemTypes[id] == ItemType.Consumable)
        {
            slotItemQtd[slot]--;
            if (slotItemQtd[slot] <= 0)
            {
                slotItemID[slot] = -1;
                itemSlots[slot].transform.Find("Icon").gameObject.SetActive(false);
                if (selectedSlot == slot) selectedSlot = -1;
            }

            // UpdateQuantidadeText(slot); // REMOVIDO
        }

        SaveHotbar();
    }

    /*
    void UpdateQuantidadeText(int index)
    {
        if (quantidadeTexts == null || quantidadeTexts.Length <= index) return;

        if (slotItemID[index] != -1 && slotItemQtd[index] > 1)
        {
            quantidadeTexts[index].text = slotItemQtd[index].ToString();
            quantidadeTexts[index].gameObject.SetActive(true);
        }
        else
        {
            quantidadeTexts[index].text = "";
            quantidadeTexts[index].gameObject.SetActive(false);
        }
    }
    */

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

            // UpdateQuantidadeText(i); // REMOVIDO
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
