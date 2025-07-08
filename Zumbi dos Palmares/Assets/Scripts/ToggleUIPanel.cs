using UnityEngine;
using UnityEngine.UI; // Necessário para trabalhar com elementos da UI

public class ToggleUIPanel : MonoBehaviour
{
    public GameObject uiPanel; // Arraste e solte o seu painel da UI aqui no Inspector
    public Button toggleButton; // Arraste e solte o seu botão aqui no Inspector

    void Start()
    {
        // Garante que o painel e o botão foram atribuídos no Inspector
        if (uiPanel == null)
        {
            Debug.LogError("UI Panel não atribuído! Por favor, arraste o painel da UI para o slot 'UI Panel' no Inspector.");
            return;
        }

        if (toggleButton == null)
        {
            Debug.LogError("Toggle Button não atribuído! Por favor, arraste o botão para o slot 'Toggle Button' no Inspector.");
            return;
        }

        // Adiciona um listener ao botão que chamará a função TogglePanel quando clicado
        toggleButton.onClick.AddListener(TogglePanel);

        // Opcional: Defina o estado inicial do painel (por exemplo, desativado)
        uiPanel.SetActive(false);
    }

    void TogglePanel()
    {
        // Alterna o estado de ativação do painel da UI
        uiPanel.SetActive(!uiPanel.activeSelf);
        Debug.Log("Painel da UI ativado/desativado. Estado atual: " + uiPanel.activeSelf);
    }
}