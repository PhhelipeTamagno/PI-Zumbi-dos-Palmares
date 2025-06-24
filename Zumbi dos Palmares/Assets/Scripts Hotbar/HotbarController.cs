using UnityEngine;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
    [Header("Configuração do Catálogo de Itens")]
    public Sprite[] itemIcons;          // indexado pelo ID do item
    public ItemType[] itemTypes;        // mesmo tamanho que itemIcons

    [Header("Slots Visuais da Hotbar")]
    public GameObject[] itemSlots;      // Slot1, Slot2, Slot3…

    /* ----- estado interno ----- */
    private int[] slotItemID;           // ID do item em cada slot (-1 = vazio)
    private int selectedSlot = -1;

    /* ========================== */
    void Start()
    {
        /* inicializa arrays de estado */
        slotItemID = new int[itemSlots.Length];
        for (int i = 0; i < slotItemID.Length; i++)
        {
            slotItemID[i] = -1;                 // slot vazio
            Transform icon = itemSlots[i].transform.Find("Icon");
            if (icon != null) icon.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        /* teclas numéricas 1,2,3… para selecionar slot */
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) SelectSlot(i);
        }

        /* tecla E usa o item do slot selecionado */
        if (Input.GetKeyDown(KeyCode.E) && selectedSlot != -1 && slotItemID[selectedSlot] != -1)
        {
            UseItem(selectedSlot);
        }
    }

    /* ---------- API pública ---------- */

    /// <summary> Adiciona um item (por ID) no primeiro slot livre. </summary>
    public void AddItemToHotbar(int itemID)
    {
        // Evita adicionar duplicado
        for (int i = 0; i < slotItemID.Length; i++)
        {
            if (slotItemID[i] == itemID)
            {
                Debug.Log("Item já está na hotbar.");
                return;
            }
        }

        // Procura slot vazio
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (slotItemID[i] == -1)
            {
                slotItemID[i] = itemID;

                Transform icon = itemSlots[i].transform.Find("Icon");
                icon.GetComponent<Image>().sprite = itemIcons[itemID];
                icon.gameObject.SetActive(true);

                SelectSlot(i);
                return;
            }
        }

        Debug.Log("Hotbar cheia!");
    }


    /// <summary> Devolve o ID do item atualmente selecionado (-1 se nada). </summary>
    public int GetSelectedItemID()
    {
        return selectedSlot != -1 ? slotItemID[selectedSlot] : -1;
    }

    /// <summary> True se o item selecionado é do tipo Equipable. </summary>
    public bool IsEquipableSelected()
    {
        int id = GetSelectedItemID();
        return id != -1 && itemTypes[id] == ItemType.Equipable;
    }

    /* ---------- lógica interna ---------- */

    void SelectSlot(int i)
    {
        if (slotItemID[i] == -1)
        {
            Debug.Log($"Slot {i + 1} vazio.");
            return;
        }
        selectedSlot = i;
        Debug.Log($"Slot {i + 1} selecionado.");
    }

    void UseItem(int slotIndex)
    {
        int id = slotItemID[slotIndex];
        if (id == -1) return;

        Debug.Log($"Usando item {id} no slot {slotIndex + 1}");

        if (itemTypes[id] == ItemType.Consumable)
        {
            // consumir e remover
            Transform icon = itemSlots[slotIndex].transform.Find("Icon");
            icon.gameObject.SetActive(false);
            slotItemID[slotIndex] = -1;
            if (selectedSlot == slotIndex) selectedSlot = -1;
            Debug.Log("Item consumido.");
        }
        else
        {
            // é equipável: só mantém selecionado
            Debug.Log("Item equipável usado (permanece no slot).");
        }
    }

    public enum ItemType { Consumable, Equipable }
}
