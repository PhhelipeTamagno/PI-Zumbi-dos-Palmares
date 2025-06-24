using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealthUI : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public float flashDuration = 0.1f;
    public int flashCount = 5;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer não encontrado no objeto!");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();

        StartCoroutine(FlashSprite());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    IEnumerator FlashSprite()
    {
        if (spriteRenderer == null) yield break;

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.2f);
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(flashDuration);
        }
    }

    void Die()
    {
        Debug.Log("Player morreu!");
        Destroy(gameObject); // Remove o player da cena
        Invoke(nameof(RestartScene), 1f); // Espera 1s antes de reiniciar
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
