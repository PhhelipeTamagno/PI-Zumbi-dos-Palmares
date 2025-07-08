using UnityEngine;
using UnityEngine.UI; // Necessário para trabalhar com elementos da UI

public class CloseUIPanel : MonoBehaviour
{
    public GameObject uiPanelToClose; // Arraste e solte o painel que você quer fechar aqui no Inspector
    public Button closeButton;        // Arraste e solte o botão que vai fechar o painel aqui no Inspector

    void Start()
    {
        // Garante que o painel e o botão foram atribuídos no Inspector
        if (uiPanelToClose == null)
        {
            Debug.LogError("UI Panel a ser fechado não atribuído! Por favor, arraste o painel da UI para o slot 'UI Panel To Close' no Inspector.");
            return;
        }

        if (closeButton == null)
        {
            Debug.LogError("Botão de fechar não atribuído! Por favor, arraste o botão para o slot 'Close Button' no Inspector.");
            return;
        }

        // Adiciona um listener ao botão que chamará a função ClosePanel quando clicado
        closeButton.onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        // Desativa o painel da UI, fazendo-o desaparecer
        uiPanelToClose.SetActive(false);
        Debug.Log("Painel da UI fechado.");
    }
}