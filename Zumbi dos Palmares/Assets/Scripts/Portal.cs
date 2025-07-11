
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Portal : MonoBehaviour
{
    [Header("Configuração Geral")]
    public string nomeCenaParaCarregar;
    public GameObject player;
    public Collider triggerSaidaNaNovaCena; // O collider da nova cena que vai detectar a saída
    public GameObject[] objetosParaCongelar;

    private bool cenaCarregada = false;
    private bool portalUsado = false;
    private Scene cenaAtual;
    private Scene cenaNova;

    private void Start()
    {
        cenaAtual = gameObject.scene;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !portalUsado)
        {
            portalUsado = true;
            StartCoroutine(CarregarNovaCena());
        }
    }

    private IEnumerator CarregarNovaCena()
    {
        // Congela os objetos da cena atual
        foreach (var obj in objetosParaCongelar)
            obj.SetActive(false);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nomeCenaParaCarregar, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        cenaNova = SceneManager.GetSceneByName(nomeCenaParaCarregar);

        // Move player pra nova cena
        SceneManager.MoveGameObjectToScene(player, cenaNova);

        // Espera um frame pra garantir que a cena carregou
        yield return null;

        // Ativa o trigger de saída
        if (triggerSaidaNaNovaCena != null)
        {
            triggerSaidaNaNovaCena.gameObject.AddComponent<DetectorDeSaida>().Configurar(this);
        }
    }

    public void VoltarParaCenaAntiga()
    {
        StartCoroutine(VoltarERetomar());
    }

    private IEnumerator VoltarERetomar()
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(cenaNova);
        while (!asyncUnload.isDone)
            yield return null;

        // Move o player de volta para a cena original
        SceneManager.MoveGameObjectToScene(player, cenaAtual);

        // Reativa os objetos da cena original
        foreach (var obj in objetosParaCongelar)
            obj.SetActive(true);

        // Desativa o portal (não pode usar de novo)
        gameObject.SetActive(false);
    }
}
