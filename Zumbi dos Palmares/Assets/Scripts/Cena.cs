using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para gerenciar cenas
using UnityEngine.UI;              // Necessário para trabalhar com UI

public class Cena : MonoBehaviour
{
    // Arraste o seu botão para este slot no Inspector
    public Button changeSceneButton;

    // Define o índice da cena para a qual você quer mudar
    // Você pode alterar isso no Inspector pra cada botão
    public int sceneIndexToLoad = 0; // 0 é o índice da primeira cena na Build Settings

    void Start()
    {
        // Verifica se o botão foi atribuído
        if (changeSceneButton == null)
        {
            Debug.LogError("Botão de troca de cena não atribuído! Por favor, arraste o botão para o slot 'Change Scene Button' no Inspector.");
            return;
        }

        // Adiciona um listener ao botão que chamará a função LoadSpecificScene quando clicado
        changeSceneButton.onClick.AddListener(LoadSpecificScene);
    }

    void LoadSpecificScene()
    {
        // Carrega a cena com o índice especificado
        SceneManager.LoadScene(sceneIndexToLoad);
        Debug.Log("Carregando cena de índice: " + sceneIndexToLoad);
    }

    // Opcional: Se você preferir mudar de cena pelo nome
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Carregando cena pelo nome: " + sceneName);
    }
}