using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para gerenciar cenas

public class BotaoTrocarCena : MonoBehaviour
{
    // Função que será chamada quando o botão for clicado
    public void TrocarCena(string nomeCena)
    {
        Time.timeScale = 1; // Garante que o tempo volte ao normal antes de trocar de cena
        SceneManager.LoadScene(nomeCena); // Carrega a cena pelo nome
    }
}
