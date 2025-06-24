using UnityEngine;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
    public GameObject[] itemSlots; // Slots no UI
    public Sprite[] itemIcons;     // Ícones dos itens por tipo
    private bool[] hasItem;        // Se o slot tem item

    void Start()
    {
        hasItem = new bool[itemSlots.Length];

        // Desativa todos os slots no começo
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].SetActive(false);
            hasItem[i] = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && hasItem[i])
            {
                UseItem(i);
            }
        }
    }

    public void AddItemToHotbar(int itemID)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!hasItem[i])
            {
                itemSlots[i].SetActive(true);
                itemSlots[i].GetComponent<Image>().sprite = itemIcons[itemID];
                hasItem[i] = true;
                Debug.Log("Item adicionado ao slot " + (i + 1));
                return;
            }
        }

        Debug.Log("Hotbar cheia!");
    }

    void UseItem(int index)
    {
        Debug.Log("Usando item do slot " + (index + 1));
        itemSlots[index].SetActive(false);
        hasItem[index] = false;
        // Aqui você pode aplicar o efeito real do item, tipo curar, dar dano, etc.
    }
}
