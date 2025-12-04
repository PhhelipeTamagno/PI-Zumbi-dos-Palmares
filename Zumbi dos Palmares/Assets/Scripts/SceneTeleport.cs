using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    public string nomeDaCena;
    public GameObject imagemIndicativa;

    private bool podeTrocarCena = false;

    void Start()
    {
        if (imagemIndicativa != null)
        {
            imagemIndicativa.SetActive(false);
        }

        // registra o método do botão mobile
        if (InteractionButton.Instance != null)
        {
            InteractionButton.Instance.onInteractionPressed += InteragirMobile;
        }
    }

    void OnDestroy()
    {
        if (InteractionButton.Instance != null)
        {
            InteractionButton.Instance.onInteractionPressed -= InteragirMobile;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            podeTrocarCena = true;

            if (imagemIndicativa != null)
                imagemIndicativa.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            podeTrocarCena = false;

            if (imagemIndicativa != null)
                imagemIndicativa.SetActive(false);
        }
    }

    void Update()
    {
        // interação no PC
        if (podeTrocarCena && Input.GetKeyDown(KeyCode.E))
        {
            TrocarCena();
        }
    }

    // interação vinda do botão mobile
    void InteragirMobile()
    {
        if (podeTrocarCena)
        {
            TrocarCena();
        }
    }

    void TrocarCena()
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
