using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Referências para os painéis e botões
    public GameObject pauseMenu;  // O painel do menu flutuante
    public GameObject imageMenu;  // O painel com as imagens
    public Button backButton;     // Botão de voltar para o menu principal
    public Button openImageMenuButton;  // Botão que abre o menu de imagens
    public Button closeImageMenuButton; // Botão para fechar o menu de imagens e voltar ao menu de pausa

    private bool isPaused = false;  // Indica se o jogo está pausado

    void Start()
    {
        // Inicialmente, esconde os menus
        pauseMenu.SetActive(false);
        imageMenu.SetActive(false);

        // Adiciona as funções aos botões
        backButton.onClick.AddListener(BackToMainMenu);
        openImageMenuButton.onClick.AddListener(OpenImageMenu);
        closeImageMenuButton.onClick.AddListener(CloseImageMenu);
    }

    void Update()
    {
        // Verifica se o jogador apertou a tecla Esc para alternar entre os menus
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                CloseAllMenus();  // Se o jogo estiver pausado, fecha todos os menus
            }
            else
            {
                OpenPauseMenu();  // Se o jogo não estiver pausado, abre o menu de pausa
            }
        }
    }

    // Método que abre o menu de pausa
    public void OpenPauseMenu()
    {
        isPaused = true;
        pauseMenu.SetActive(true);   // Mostra o menu de pausa
        imageMenu.SetActive(false);  // Garante que o menu de imagens esteja oculto
        Time.timeScale = 0f;         // Pausa o jogo
    }

    // Método que fecha todos os menus
    public void CloseAllMenus()
    {
        // Fecha todos os painéis
        pauseMenu.SetActive(false);
        imageMenu.SetActive(false);
        Time.timeScale = 1f;  // Retorna o tempo ao normal
        isPaused = false;     // Marca que o jogo não está mais pausado
    }

    // Método que volta para o menu principal
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;  // Garante que o jogo não está pausado
        SceneManager.LoadScene("Menu 0");  // Volta para a cena do menu principal
    }

    // Método que abre o menu de imagens
    public void OpenImageMenu()
    {
        pauseMenu.SetActive(false);   // Fecha o menu de pausa
        imageMenu.SetActive(true);    // Abre o menu de imagens
    }

    // Método que fecha o menu de imagens e volta ao menu de pausa
    public void CloseImageMenu()
    {
        imageMenu.SetActive(false);   // Fecha o menu de imagens
        pauseMenu.SetActive(true);    // Volta ao menu de pausa
    }
}
