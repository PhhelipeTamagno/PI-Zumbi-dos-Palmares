using UnityEngine;

public class KnifePickup : MonoBehaviour
{
    public int knifeID = 0; // ID correspondente à faca no catálogo da Hotbar

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HotbarController hotbar = FindObjectOfType<HotbarController>();
            if (hotbar != null)
            {
                hotbar.AddItemToHotbar(knifeID);
            }

            Destroy(gameObject); // Destroi a faca após pegar
        }
    }
}
