using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicaoDepoisDeTempo : MonoBehaviour
{
    public string proximaCena; // Nome da próxima cena
    public float tempoDeEspera = 10f; // Tempo em segundos

    void Start()
    {
        Invoke("TrocarCena", tempoDeEspera);
    }

    void TrocarCena()
    {
        SceneManager.LoadScene(proximaCena);
    }
}
