using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    // Chamado quando o jogador termina o jogo
    public void PlayerFinishedGame()
    {
        PlayerPrefs.SetInt("GameCompleted", 1); // salva que o jogo foi zerado
        PlayerPrefs.Save();
        Debug.Log("Jogo zerado! Informação salva.");
    }
}
