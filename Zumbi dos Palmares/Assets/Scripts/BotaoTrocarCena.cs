using UnityEngine;
using UnityEngine.SceneManagement; 

public class BotaoTrocarCena : MonoBehaviour
{
    
    public void TrocarCena(string nomeCena)
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(nomeCena); 
    }
}
