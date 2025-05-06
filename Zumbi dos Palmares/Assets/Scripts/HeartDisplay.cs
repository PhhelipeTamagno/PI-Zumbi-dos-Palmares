using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public Image[] hearts;  // Array de imagens que representam os corações
    public Sprite fullHeart;  // Sprite para o coração cheio
    public Sprite emptyHeart; // Sprite para o coração vazio

    public void UpdateHearts(int currentHealth, int maxHealth)
    {
        // Atualiza a UI de corações
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;  // Define o sprite de coração cheio
            }
            else
            {
                hearts[i].sprite = emptyHeart;  // Define o sprite de coração vazio
            }
        }
    }
}
