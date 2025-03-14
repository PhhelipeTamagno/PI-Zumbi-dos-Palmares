using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para gerenciar cenas

public class BotaoTrocarCena : MonoBehaviour
{
    // Função que será chamada quando o botão for clicado
    public void TrocarCena(string nomeCena)
    {
        // Carrega a cena pelo nome
        SceneManager.LoadScene(nomeCena);
    }
}
