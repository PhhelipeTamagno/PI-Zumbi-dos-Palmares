using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public GameObject uiPanel;
    private bool isPaused = false;

    void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);
        else
            Debug.LogError("UI Panel não atribuído!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && uiPanel != null)
        {
            if (!uiPanel.activeSelf)
                OpenPanel();
            else
                ClosePanel();
        }
    }

    public void OpenPanel()
    {
        uiPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePanel()
    {
        uiPanel.SetActive(false);
        Time.timeScale = 1f;
    }


    public static class GameState
    {
        public static bool IsPaused = false;
    }

}
