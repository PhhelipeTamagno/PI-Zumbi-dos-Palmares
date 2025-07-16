using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    [Header("Cena para carregar")]
    [SerializeField] private string sceneName;

    [Header("UI")]
    public GameObject teclaEIcon; // Ícone para pressionar E

    private bool playerNearby = false;

    private void Start()
    {
        // Garante que o ícone começa desativado
        if (teclaEIcon != null)
            teclaEIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (teclaEIcon != null)
                teclaEIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (teclaEIcon != null)
                teclaEIcon.SetActive(false);
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
        // Salva os itens da hotbar antes de trocar de cena
        HotbarController hotbar = FindObjectOfType<HotbarController>();
        if (hotbar != null)
        {
            hotbar.SaveHotbar();
        }

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
