using UnityEngine;
using UnityEngine.SceneManagement;

public class Galinhacena : MonoBehaviour
{
    // Nome da cena que será carregada
    public string nomeCena;

    // Método chamado pelo botão
    public void CarregarCena()
    {
        SceneManager.LoadScene(nomeCena);
    }
}
