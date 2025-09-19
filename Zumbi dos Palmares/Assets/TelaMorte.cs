using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaMorte : MonoBehaviour
{
    public GameObject deathScreenPanel; // arraste seu painel aqui no Inspector

    public void ShowDeathScreen()
    {
        deathScreenPanel.SetActive(true);
        Time.timeScale = 0f; // pausa o jogo
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
