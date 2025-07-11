using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    [Header("Alerta Visual")]
    public GameObject exclamationMark;
    private bool playerInRange = false;

    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Combate")]
    public int damageToPlayer = 1;
    public float damageCooldown = 1f;
    private float lastDamageTime;

    [Header("Detecção e Movimento")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    private Transform player;

    [Header("Feedback Visual")]
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float flashDuration = 0.1f;

    [Header("Animação")]
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            MoveTowardsPlayer();
            SetAnimationState(isWalking: true, isAttacking: false);

            if (!playerInRange)
            {
                ShowExclamation(true);
                playerInRange = true;
            }
        }
        else
        {
            SetAnimationState(isWalking: false, isAttacking: false);

            if (playerInRange)
            {
                ShowExclamation(false);
                playerInRange = false;
            }
        }

        FlipTowardsPlayer(); // <- Atualizado para virar com localScale
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        float direction = player.position.x - transform.position.x;

        if (direction < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // olha para esquerda
        else if (direction > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // olha para direita
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= lastDamageTime + damageCooldown)
        {
            PlayerHealthUI playerHealth = collision.gameObject.GetComponent<PlayerHealthUI>();
            if (playerHealth != null)
            {
                SetAnimationState(isWalking: false, isAttacking: true);
                playerHealth.TakeDamage(damageToPlayer);
                lastDamageTime = Time.time;

                // Parar de atacar após 0.5s
                Invoke(nameof(StopAttacking), 0.5f);
            }
        }
    }

    void StopAttacking()
    {
        SetAnimationState(isWalking: false, isAttacking: false);
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
            Die();
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

    void ShowExclamation(bool show)
    {
        if (exclamationMark != null)
            exclamationMark.SetActive(show);
    }

    void SetAnimationState(bool isWalking, bool isAttacking)
    {
        if (animator == null) return;

        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsAttacking", isAttacking);
    }
}
