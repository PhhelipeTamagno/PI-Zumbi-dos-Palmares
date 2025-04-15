using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "GameOver"; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Danger"))  
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player morreu!");
        SceneManager.LoadScene(sceneToLoad);
    }
}
