using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int itemID; // O ID corresponde à posição do ícone no array

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HotbarController hotbar = FindObjectOfType<HotbarController>();
            hotbar.AddItemToHotbar(itemID);

            Destroy(gameObject); // Remove o item do mundo
        }
    }
}
