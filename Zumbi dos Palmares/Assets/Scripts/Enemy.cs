using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(Rigidbody2D))]
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

    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;

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
            SetAnimationState(true, false);

            if (!playerInRange)
            {
                ShowExclamation(true);
                playerInRange = true;
            }
        }
        else
        {
            SetAnimationState(false, false);

            if (playerInRange)
            {
                ShowExclamation(false);
                playerInRange = false;
            }
        }

        FlipTowardsPlayer();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
            MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        float direction = player.position.x - transform.position.x;

        if (direction < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (direction > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= lastDamageTime + damageCooldown)
        {
            PlayerHealthUI playerHealth = collision.gameObject.GetComponent<PlayerHealthUI>();
            if (playerHealth != null)
            {
                SetAnimationState(false, true);
                playerHealth.TakeDamage(damageToPlayer);
                lastDamageTime = Time.time;

                Invoke(nameof(StopAttacking), 0.5f);
            }
        }
    }

    void StopAttacking()
    {
        SetAnimationState(false, false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

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
