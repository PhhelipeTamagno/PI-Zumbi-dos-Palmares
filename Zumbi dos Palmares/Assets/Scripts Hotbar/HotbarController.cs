using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HotbarController : MonoBehaviour
{
    [Header("Catálogo de Itens")]
    public Sprite[] itemIcons;
    public ItemType[] itemTypes;

    [Header("Slots Visuais")]
    public GameObject[] itemSlots;

    private int[] slotItemID;
    private int selectedSlot = -1;
    private static bool hotbarClearedThisSession = false;

    /* ---------- UNITY ---------- */
    void Start()
    {
        slotItemID = new int[itemSlots.Length];

        if (!hotbarClearedThisSession)
        {
            ClearHotbarData();              // limpa só na PRIMEIRA cena da execução
            hotbarClearedThisSession = true;
        }

        LoadHotbar();
        // 🔴 NÃO coloque DeleteAll aqui!
    }

    void Update()
    {
        // Teclas 1‑2‑3…
        for (int i = 0; i < itemSlots.Length; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectSlot(i);

        // Scroll do mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            int newSlot = selectedSlot + (scroll > 0 ? 1 : -1);
            if (newSlot < 0) newSlot = itemSlots.Length - 1;
            if (newSlot >= itemSlots.Length) newSlot = 0;
            SelectSlot(newSlot);
        }

        // Usar item – tecla E
        if (Input.GetKeyDown(KeyCode.E) && selectedSlot != -1 && slotItemID[selectedSlot] != -1)
            UseItem(selectedSlot);
    }

    /* ---------- API PÚBLICA ---------- */
    public void AddItemToHotbar(int itemID)
    {
        // Evita duplicar
        for (int i = 0; i < slotItemID.Length; i++)
            if (slotItemID[i] == itemID) { Debug.Log("Item já está na hotbar."); return; }

        // Procura slot vazio
        for (int i = 0; i < itemSlots.Length; i++)
            if (slotItemID[i] == -1)
            {
                slotItemID[i] = itemID;
                itemSlots[i].transform.Find("Icon").GetComponent<Image>().sprite = itemIcons[itemID];
                itemSlots[i].transform.Find("Icon").gameObject.SetActive(true);

                SelectSlot(i);
                SaveHotbar();                // salva mudança
                return;
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

    /* ---------- INTERNOS ---------- */
    void SelectSlot(int i)
    {
        // Desliga highlights
        for (int s = 0; s < itemSlots.Length; s++)
            itemSlots[s].transform.Find("Highlight").gameObject.SetActive(false);

        // Liga highlight do selecionado
        itemSlots[i].transform.Find("Highlight").gameObject.SetActive(true);
        selectedSlot = i;
    }

    void UseItem(int slot)
    {
        int id = slotItemID[slot];
        if (id == -1) return;

        if (itemTypes[id] == ItemType.Consumable)
        {
            itemSlots[slot].transform.Find("Icon").gameObject.SetActive(false);
            slotItemID[slot] = -1;
            if (selectedSlot == slot) selectedSlot = -1;
        }

        SaveHotbar();                        // salva mudança
    }

    /* ---------- SALVAR / CARREGAR ---------- */
    public enum ItemType { Consumable, Equipable }

   public void SaveHotbar()
    {
        for (int i = 0; i < slotItemID.Length; i++)
            PlayerPrefs.SetInt("HotbarSlot" + i, slotItemID[i]);
        PlayerPrefs.Save();
    }

    void LoadHotbar()
    {
        for (int i = 0; i < slotItemID.Length; i++)
        {
            int id = PlayerPrefs.GetInt("HotbarSlot" + i, -1);
            slotItemID[i] = id;

            var icon = itemSlots[i].transform.Find("Icon").GetComponent<Image>();
            icon.gameObject.SetActive(id != -1);
            if (id != -1) icon.sprite = itemIcons[id];
        }
    }

    void ClearHotbarData()
    {
        for (int i = 0; i < itemSlots.Length; i++)
            PlayerPrefs.DeleteKey("HotbarSlot" + i);
        PlayerPrefs.Save();
    }

    /* ---------- NOVO JOGO ---------- */
    public void NovoJogo()
    {
        PlayerPrefs.DeleteAll();             // limpa tudo de propósito
        PlayerPrefs.Save();
        SceneManager.LoadScene("CenaInicial");
    }
}
