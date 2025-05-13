using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public HotbarSlot[] slots;

    public void AddItemToHotbar(Item item)
    {
        foreach (HotbarSlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                Debug.Log("Adicionando item à hotbar: " + item.itemName);
                slot.AddItem(item);
                return;
            }
        }

        Debug.Log("Nenhum slot vazio encontrado.");
    }

}
