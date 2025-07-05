using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HotbarController : MonoBehaviour
{
    [Header("Configuração do Catálogo de Itens")]
    public Sprite[] itemIcons;
    public ItemType[] itemTypes;

    [Header("Slots Visuais da Hotbar")]
    public GameObject[] itemSlots;

    private int[] slotItemID;
    private int selectedSlot = -1;
    private static bool hotbarClearedThisSession = false;

    void Start()
    {
        slotItemID = new int[itemSlots.Length];

        if (!hotbarClearedThisSession)
        {
            ClearHotbarData(); // limpa apenas uma vez por execução
            hotbarClearedThisSession = true;
        }

        LoadHotbar();

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }


    void Update()
    {
        // Teclas 1, 2, 3...
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectSlot(i);
        }

        // Scroll do mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            int newSlot = selectedSlot;
            if (scroll > 0f) newSlot++;
            else if (scroll < 0f) newSlot--;

            if (newSlot < 0) newSlot = itemSlots.Length - 1;
            if (newSlot >= itemSlots.Length) newSlot = 0;

            SelectSlot(newSlot);
        }

        // Usar item (tecla E)
        if (Input.GetKeyDown(KeyCode.E) && selectedSlot != -1 && slotItemID[selectedSlot] != -1)
        {
            UseItem(selectedSlot);
        }
    }

    public void AddItemToHotbar(int itemID)
    {
        for (int i = 0; i < slotItemID.Length; i++)
        {
            if (slotItemID[i] == itemID)
            {
                Debug.Log("Item já está na hotbar.");
                return;
            }
        }

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

    public int GetSelectedItemID()
    {
        return selectedSlot != -1 ? slotItemID[selectedSlot] : -1;
    }

    public bool IsEquipableSelected()
    {
        int id = GetSelectedItemID();
        return id != -1 && itemTypes[id] == ItemType.Equipable;
    }

    void SelectSlot(int i)
    {
        // Desativa destaque de todos
        for (int s = 0; s < itemSlots.Length; s++)
        {
            Transform highlight = itemSlots[s].transform.Find("Highlight");
            if (highlight != null)
                highlight.gameObject.SetActive(false);
        }

        // Ativa destaque do slot selecionado
        Transform selectedHighlight = itemSlots[i].transform.Find("Highlight");
        if (selectedHighlight != null)
            selectedHighlight.gameObject.SetActive(true);

        selectedSlot = i;

        if (slotItemID[i] != -1)
            Debug.Log($"Slot {i + 1} selecionado com item ID {slotItemID[i]}.");
        else
            Debug.Log($"Slot {i + 1} selecionado (vazio).");
    }

    void UseItem(int slotIndex)
    {
        int id = slotItemID[slotIndex];
        if (id == -1) return;

        Debug.Log($"Usando item {id} no slot {slotIndex + 1}");

        if (itemTypes[id] == ItemType.Consumable)
        {
            Transform icon = itemSlots[slotIndex].transform.Find("Icon");
            icon.gameObject.SetActive(false);
            slotItemID[slotIndex] = -1;

            if (selectedSlot == slotIndex)
                selectedSlot = -1;

            Debug.Log("Item consumido.");
        }
        else
        {
            Debug.Log("Item equipável usado (permanece no slot).");
        }
    }

    public enum ItemType { Consumable, Equipable }

    public void SaveHotbar()
    {
        for (int i = 0; i < slotItemID.Length; i++)
        {
            PlayerPrefs.SetInt("HotbarSlot" + i, slotItemID[i]);
        }
        PlayerPrefs.Save();
    }

    public void LoadHotbar()
    {
        for (int i = 0; i < slotItemID.Length; i++)
        {
            int id = PlayerPrefs.GetInt("HotbarSlot" + i, -1);
            slotItemID[i] = id;

            Transform icon = itemSlots[i].transform.Find("Icon");
            if (id != -1)
            {
                icon.GetComponent<Image>().sprite = itemIcons[id];
                icon.gameObject.SetActive(true);
            }
            else
            {
                icon.gameObject.SetActive(false);
            }
        }
    }

    public void ClearHotbarData()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            PlayerPrefs.DeleteKey("HotbarSlot" + i);
        }
        PlayerPrefs.Save();
    }

    public void NovoJogo()
    {
        PlayerPrefs.DeleteAll(); // limpa toda a hotbar e progresso anterior
        PlayerPrefs.Save();
        SceneManager.LoadScene("CenaInicial");
    }


}
