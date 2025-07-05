using UnityEngine;

public class KnifePickup : MonoBehaviour
{
    public int itemID = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null && player.hotbarController != null)
            {
                player.hotbarController.AddItemToHotbar(itemID);
                Destroy(gameObject); // remove a faca da cena
            }
        }
    }
}
