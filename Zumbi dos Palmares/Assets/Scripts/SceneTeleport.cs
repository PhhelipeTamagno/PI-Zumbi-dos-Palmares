using UnityEngine;
using UnityEngine.SceneManagement; // necessário para mudar de cena

public class SceneTeleport : MonoBehaviour
{
    public string nomeDaCena; // nome da cena para mudar
    public GameObject imagemIndicativa;
    private bool podeTrocarCena = false;

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
            podeTrocarCena = true;

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
            podeTrocarCena = false;

            if (imagemIndicativa != null)
            {
                imagemIndicativa.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (podeTrocarCena && Input.GetKeyDown(KeyCode.E))
        {
            if (!string.IsNullOrEmpty(nomeDaCena))
            {
                SceneManager.LoadScene(nomeDaCena);
            }
            else
            {
                Debug.LogWarning("Nome da cena não atribuído no objeto " + gameObject.name);
            }
        }
    }
}
