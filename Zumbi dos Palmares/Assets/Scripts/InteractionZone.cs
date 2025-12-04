using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public GameObject imageToShow;
    private bool playerInZone = false;

    void Start()
    {
        if (InteractionButton.Instance != null)
            InteractionButton.Instance.onInteractionPressed += InteragirMobile;
    }

    void OnDestroy()
    {
        if (InteractionButton.Instance != null)
            InteractionButton.Instance.onInteractionPressed -= InteragirMobile;
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            ToggleImage();
        }
    }

    // --- MOBILE ---
    void InteragirMobile()
    {
        if (playerInZone)
            ToggleImage();
    }

    void ToggleImage()
    {
        imageToShow.SetActive(!imageToShow.activeSelf);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            imageToShow.SetActive(false);
        }
    }
}
