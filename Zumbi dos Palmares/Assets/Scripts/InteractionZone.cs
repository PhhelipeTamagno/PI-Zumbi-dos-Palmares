using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public GameObject imageToShow;
    private bool playerInZone = false;

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            // Alterna entre ativar e desativar a imagem
            bool isActive = imageToShow.activeSelf;
            imageToShow.SetActive(!isActive);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            // Se a imagem ainda estiver ativa, desativa ao sair
            if (imageToShow.activeSelf)
            {
                imageToShow.SetActive(false);
            }
        }
    }
}
