using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Fecha o modo Play no Editor
#else
        Application.Quit(); // Funciona no jogo compilado
#endif

        Debug.Log("O jogo foi fechado!");
    }
}
