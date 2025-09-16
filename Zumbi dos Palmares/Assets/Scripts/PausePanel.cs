using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public GameObject uiPanel; // Arraste o painel no Inspector
    private bool isPaused = false;

    void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false); // começa fechado
        else
            Debug.LogError("UI Panel não atribuído!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && uiPanel != null)
        {
            // Se o painel estiver fechado, abre
            if (!uiPanel.activeSelf)
            {
                OpenPanel();
            }
            // Se estiver aberto, fecha
            else
            {
                ClosePanel();
            }
        }
    }

    public void OpenPanel()
    {
        uiPanel.SetActive(true);
        isPaused = true;
    }

    public void ClosePanel()
    {
        uiPanel.SetActive(false);
        isPaused = false;
    }
}
