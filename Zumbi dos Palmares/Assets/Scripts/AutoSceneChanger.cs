using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName; // Nome da cena para trocar
    [SerializeField] private float delay = 25f; // Tempo em segundos

    void Start()
    {
        Invoke("ChangeScene", delay);
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Nome da cena não foi definido no Inspector!");
        }
    }
}
