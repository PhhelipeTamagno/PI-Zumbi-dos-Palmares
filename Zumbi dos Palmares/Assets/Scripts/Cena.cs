using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cena : MonoBehaviour
{
    public Button changeSceneButton;
    public int sceneIndexToLoad = 0;

    void Start()
    {
        if (changeSceneButton == null)
        {
            Debug.LogError("Botão de troca de cena não atribuído!");
            return;
        }

        changeSceneButton.onClick.AddListener(LoadSpecificScene);
    }

    void LoadSpecificScene()
    {
        Time.timeScale = 1f; // garante que a próxima cena não comece pausada

        SceneManager.LoadScene(sceneIndexToLoad);

        Debug.Log("Carregando cena de índice: " + sceneIndexToLoad);
    }

    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f; // também aqui

        SceneManager.LoadScene(sceneName);

        Debug.Log("Carregando cena pelo nome: " + sceneName);
    }
}
