using UnityEngine;

public class KnifePickup : MonoBehaviour
{
    public int itemID = 0; // ID da faca na hotbar

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMovement>();
            if (player && player.hotbarController)
            {
                player.hotbarController.AddItemToHotbar(itemID);
                Destroy(gameObject); // some com a faca física
            }
        }
    }
}
