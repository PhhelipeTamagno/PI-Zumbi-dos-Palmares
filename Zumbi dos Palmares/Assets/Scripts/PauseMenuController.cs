using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;      // Painel de pausa
    public GameObject settingsPanel;   // Painel de configurações
    private bool isPaused = false;     // Estado do menu de pausa

    void Start()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false); // Ambos começam fechados
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePausePanel();
        }
    }

    // Método para abrir/fechar o painel de pausa
    public void TogglePausePanel()
    {
        isPaused = !isPaused;

        // Se estiver pausado, mostra o painel de pausa e esconde os outros
        pausePanel.SetActive(isPaused);
        settingsPanel.SetActive(false);

        // Pausa o jogo quando o painel está aberto
        Time.timeScale = isPaused ? 0 : 1;
    }

    // Método para abrir o painel de configurações
    public void OpenSettings()
    {
        pausePanel.SetActive(false);   // Fecha o menu principal
        settingsPanel.SetActive(true); // Abre o painel de configurações
    }

    // Método para voltar ao painel principal do pause
    public void BackToPauseMenu()
    {
        settingsPanel.SetActive(false); // Fecha configurações
        pausePanel.SetActive(true);     // Reabre o painel de pausa
    }

    // Método para o botão "Menu" (Fecha todos os painéis e volta ao jogo)
    public void CloseAllPanels()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1; // Retorna o jogo ao normal
    }
}
