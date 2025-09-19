using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Configuração de Vida")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Corações na UI")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Feedback de Dano")]
    public float flashDuration = 0.1f;
    public int flashCount = 5;

    private SpriteRenderer spriteRenderer;
    private const string HEALTH_KEY = "PlayerHealth";

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("SpriteRenderer não encontrado!");

        if (PlayerPrefs.HasKey(HEALTH_KEY))
            currentHealth = PlayerPrefs.GetInt(HEALTH_KEY, maxHealth);
        else
            currentHealth = maxHealth;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        UpdateHearts();
        SaveHealth();

        StartCoroutine(FlashSprite());

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
     {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHearts();
        SaveHealth();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
    }

    IEnumerator FlashSprite()
    {
        if (spriteRenderer == null) yield break;

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.2f);
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    void Die()
    {
        Debug.Log("Player morreu!");
        PlayerPrefs.DeleteKey(HEALTH_KEY);

        // Tenta parar o movimento do jogador
        PlayerMovement pm = GetComponent<PlayerMovement>();
        if (pm != null)
            pm.enabled = false;

        // Mostra a tela de morte
        TelaMorte ds = FindObjectOfType<TelaMorte>();
        if (ds != null)
            ds.ShowDeathScreen();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // fallback
    }


    void SaveHealth()
    {
        PlayerPrefs.SetInt(HEALTH_KEY, currentHealth);
        PlayerPrefs.Save();
    }
}
