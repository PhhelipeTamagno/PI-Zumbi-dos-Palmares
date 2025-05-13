using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public Image icon;
    private Item currentItem;

    public bool IsEmpty()
    {
        return !icon.enabled;
    }

    public void AddItem(Item newItem)
    {
        currentItem = newItem;
        icon.sprite = currentItem.icon;
        icon.enabled = true;
    }


    public void ClearSlot()
    {
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void UseItem()
    {
        if (currentItem != null && currentItem.isUsable)
        {
            Debug.Log("Usando item: " + currentItem.itemName);

            if (currentItem.itemName == "Faca")
            {
                Debug.Log("Faca usada!");
                // Pode remover ou manter
                ClearSlot();
            }
        }
    }
}
