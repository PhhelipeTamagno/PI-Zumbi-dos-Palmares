using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class EnemyInteligente : MonoBehaviour
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

    [Header("Movimento e Detecção")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    private Transform player;
    private bool playerHidden = false;

    [Header("Patrulha Aleatória")]
    public float changeDirectionTime = 2f;
    private float lastDirectionChangeTime;
    private Vector2 randomDirection;

    [Header("Distância mínima das moitas")]
    public float bushAvoidDistance = 1.5f;

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

        ChooseRandomDirection();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // SE PLAYER VISÍVEL E NÃO ESTÁ ESCONDIDO
        if (distance <= detectionRange && !playerHidden)
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
            Patrol();

            if (playerInRange)
            {
                ShowExclamation(false);
                playerInRange = false;
            }
        }

        FlipTowardsMovement();
    }

    // ----------------- MOVIMENTO -----------------
    void Patrol()
    {
        if (Time.time >= lastDirectionChangeTime + changeDirectionTime || IsNearBush())
        {
            ChooseRandomDirection();
        }

        transform.position += (Vector3)randomDirection * moveSpeed * Time.deltaTime;
        SetAnimationState(isWalking: true, isAttacking: false);
    }


    void ChooseRandomDirection()
    {
        randomDirection = Random.insideUnitCircle.normalized;
        lastDirectionChangeTime = Time.time;
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        if (!playerHidden)
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        randomDirection = direction; // salva para virar sprite corretamente
    }

    void FlipTowardsMovement()
    {
        if (randomDirection.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (randomDirection.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    // ----------------- EVITAR MOITAS -----------------
    bool IsNearBush()
    {
        GameObject[] bushes = GameObject.FindGameObjectsWithTag("Bush");
        foreach (GameObject bush in bushes)
        {
            float dist = Vector2.Distance(transform.position, bush.transform.position);
            if (dist < bushAvoidDistance)
            {
                // Calcula direção oposta à moita
                Vector2 awayFromBush = (transform.position - bush.transform.position).normalized;
                randomDirection = awayFromBush;

                return true;
            }
        }
        return false;
    }


    // ----------------- COLISÃO -----------------
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

                Invoke(nameof(StopAttacking), 0.5f);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ChooseRandomDirection();
        }
    }

    // ----------------- PLAYER ESCONDIDO -----------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bush"))
        {
            playerHidden = true;
            ShowExclamation(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bush"))
        {
            playerHidden = false;
        }
    }

    // ----------------- DANO -----------------
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

    // ----------------- VISUAL -----------------
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
