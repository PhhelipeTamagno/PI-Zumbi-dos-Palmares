using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform destino; 
    public GameObject imagemIndicativa; 
    private bool podeTeleportar = false;
    private Transform player;

    void Start()
    {
        if (imagemIndicativa != null)
        {
            imagemIndicativa.SetActive(false); 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            podeTeleportar = true;
            player = other.transform;

            if (imagemIndicativa != null)
            {
                imagemIndicativa.SetActive(true); 
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            podeTeleportar = false;
            player = null;

            if (imagemIndicativa != null)
            {
                imagemIndicativa.SetActive(false); 
            }
        }
    }

    void Update()
    {
        if (podeTeleportar && Input.GetKeyDown(KeyCode.E))
        {
            if (destino != null) 
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
