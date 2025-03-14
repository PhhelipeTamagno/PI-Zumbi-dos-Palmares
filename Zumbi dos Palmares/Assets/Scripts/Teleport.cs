using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform destino; // Arraste aqui o destino do teleporte no Inspector
    private bool podeTeleportar = false;
    private Transform player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se é o player
        {
            podeTeleportar = true;
            player = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            podeTeleportar = false;
            player = null;
        }
    }

    void Update()
    {
        if (podeTeleportar && Input.GetKeyDown(KeyCode.E))
        {
            if (destino != null) // Se um destino foi configurado, teleporta
            {
                player.position = destino.position;
            }
            else
            {
                Debug.LogWarning("Destino não atribuído no teleporte " + gameObject.name);
            }
        }
    }
}
