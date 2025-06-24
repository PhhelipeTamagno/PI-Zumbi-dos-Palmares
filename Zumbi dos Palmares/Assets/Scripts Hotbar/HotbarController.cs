using UnityEngine;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
    public ItemType[] itemTypes; // deve ter o mesmo tamanho que itemIcons
    public GameObject[] itemSlots;      // Slot1, Slot2, Slot3...
    public Sprite[] itemIcons;          // Ícones dos itens
    private bool[] hasItem;

    private int selectedSlot = -1;      // Nenhum slot selecionado no início

    void Start()
    {
        hasItem = new bool[itemSlots.Length];

        for (int i = 0; i < itemSlots.Length; i++)
        {
            hasItem[i] = false;

            Transform icon = itemSlots[i].transform.Find("Icon");
            if (icon != null)
                icon.gameObject.SetActive(false); // Esconde o ícone
        }
    }

    void Update()
    {
        // Seleciona o slot com as teclas 1, 2, 3...
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i); // ← CHAMA ESTA FUNÇÃO
            }
        }

        // Usa o item ao pressionar a tecla E
        if (Input.GetKeyDown(KeyCode.E) && selectedSlot != -1 && hasItem[selectedSlot])
        {
            UseItem(selectedSlot);
        }
    }

    public void AddItemToHotbar(int itemID)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!hasItem[i])
            {
                Transform icon = itemSlots[i].transform.Find("Icon");
                icon.GetComponent<Image>().sprite = itemIcons[itemID];
                icon.gameObject.SetActive(true);

                hasItem[i] = true;

                // (Opcional) selecionar automaticamente o primeiro slot preenchido
                SelectSlot(i);

                return;
            }
        }

        Debug.Log("Hotbar cheia!");
    }

    // ⬇️ AQUI ESTÁ O MÉTODO QUE VOCÊ PRECISA ADICIONAR
    void SelectSlot(int i)
    {
        if (!hasItem[i])
        {
            Debug.Log("Slot " + (i + 1) + " está vazio.");
            return;
        }

        selectedSlot = i;
        Debug.Log("Slot " + (i + 1) + " selecionado.");
    }

    void UseItem(int i)
    {
        Debug.Log("Usando item do slot " + (i + 1));

        // Aqui você aplicaria o efeito real (curar, equipar, etc.)

        // Verifica o tipo do item
        if (itemTypes.Length > i && itemTypes[i] == ItemType.Consumable)
        {
            // Consumir o item (poções, etc.)
            Transform icon = itemSlots[i].transform.Find("Icon");
            icon.gameObject.SetActive(false);
            hasItem[i] = false;
            selectedSlot = -1;
            Debug.Log("Item consumido.");
        }
        else
        {
            // Item é equipável (ex: espada), apenas ativa o uso
            Debug.Log("Item equipável usado (não removido da hotbar).");
        }
    }

    public enum ItemType
    {
        Consumable,
        Equipable
    }

}
