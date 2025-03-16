using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; // Item a ser coletado

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inventoryManager = other.GetComponent<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.AddItem(item); // Adiciona o item ao inventário
                Destroy(gameObject); // Remove o item do mundo
            }
        }
    }
}
