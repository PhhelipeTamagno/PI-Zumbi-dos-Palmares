using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName; // Nome da cena para onde o player irá

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Cutscene");
        }
    }
}
