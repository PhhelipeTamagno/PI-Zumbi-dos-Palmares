using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocaDeCenaTrigger : MonoBehaviour
{
    public GameObject imagemInteracao; // arraste o PNG aqui no inspetor
    public string nomeDaCena;          // nome da cena que será carregada

    private bool jogadorDentro = false;

    void Start()
    {
        if (imagemInteracao != null)
            imagemInteracao.SetActive(false);
    }

    void Update()
    {
        if (jogadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(nomeDaCena);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorDentro = true;
            if (imagemInteracao != null)
                imagemInteracao.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorDentro = false;
            if (imagemInteracao != null)
                imagemInteracao.SetActive(false);
        }
    }
}
