using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NovoJogo()
    {
        PlayerPrefs.DeleteKey("PlayerHealth"); // vida cheia
        PlayerPrefs.Save();
        SceneManager.LoadScene("CenaInicial"); // troque pelo nome real
    }
}
