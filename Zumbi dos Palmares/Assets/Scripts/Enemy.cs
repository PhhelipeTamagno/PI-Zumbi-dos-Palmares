using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float flashDuration = 0.1f;

    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public int damageToPlayer = 1;
    public float damageCooldown = 1f;

    private Transform player;
    private float lastDamageTime;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Inimigo levou " + damage + " de dano. Vida restante: " + currentHealth);

        if (spriteRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        Debug.Log("Inimigo morreu!");
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= lastDamageTime + damageCooldown)
        {
            PlayerHealthUI playerHealth = collision.gameObject.GetComponent<PlayerHealthUI>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
                lastDamageTime = Time.time;
            }
        }
    }
}
