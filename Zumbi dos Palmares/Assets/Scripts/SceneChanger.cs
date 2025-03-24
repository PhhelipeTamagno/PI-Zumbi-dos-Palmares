using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName; // Nome da cena para mudar
    private bool playerNearby = false; // Verifica se o player está dentro do trigger

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Certifique-se de que o Player tem a tag "Player"
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Nome da cena não definido no Inspector!");
        }
    }
}
