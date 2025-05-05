using UnityEngine;
using TMPro;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;
    public GameObject damagePopupPrefab;
    public Transform damagePopupSpawnPoint;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        StartCoroutine(FlashRed());
        ShowDamagePopup(damageAmount);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }

    void ShowDamagePopup(int damageAmount)
    {
        if (damagePopupPrefab != null && damagePopupSpawnPoint != null)
        {
            GameObject popup = Instantiate(damagePopupPrefab, damagePopupSpawnPoint.position, Quaternion.identity);
            TextMeshProUGUI damageText = popup.GetComponentInChildren<TextMeshProUGUI>();
            if (damageText != null)
            {
                damageText.text = damageAmount.ToString();
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}