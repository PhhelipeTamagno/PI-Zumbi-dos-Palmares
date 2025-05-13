using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public Hotbar hotbar;
    public Item knifeItem; // arraste o ScriptableObject da faca aqui

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Faca"))
        {
            Debug.Log("Faca coletada!");
            hotbar.AddItemToHotbar(knifeItem);
            Destroy(collision.gameObject);
        }
    }

}
