using UnityEngine;
using System.Collections;

public class KnifePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().CollectKnife();
            Destroy(gameObject); // remove a faca da cena
        }
    }
}
