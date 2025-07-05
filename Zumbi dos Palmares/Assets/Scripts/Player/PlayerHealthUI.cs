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

    /* ----------------------------- */
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("SpriteRenderer não encontrado!");

        /* Se a cena foi carregada pelo botão “Novo Jogo”,
           o menu deve ter chamado PlayerPrefs.DeleteKey(HEALTH_KEY).
           Assim, HasKey = false e a vida começa cheia. */
        if (PlayerPrefs.HasKey(HEALTH_KEY))
            currentHealth = PlayerPrefs.GetInt(HEALTH_KEY, maxHealth);
        else
            currentHealth = maxHealth;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();
    }

    /* ---------- API pública ---------- */
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

    /* ---------- Internos ---------- */
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
        PlayerPrefs.DeleteKey(HEALTH_KEY);   // zera vida salva
        Destroy(gameObject);
        Invoke(nameof(RestartScene), 1f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SaveHealth()
    {
        PlayerPrefs.SetInt(HEALTH_KEY, currentHealth);
        PlayerPrefs.Save();
    }
}
