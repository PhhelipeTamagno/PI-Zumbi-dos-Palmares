using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float boostSpeed = 8f;
    private float playerInitialSpeed;
    private bool isAttack = false;
    private bool canAttack = true;
    public float attackCooldown = 0.5f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private Vector2 lastMoveDirection;

    public AudioClip stepSound;
    private AudioSource audioSource;
    private bool isWalking = false;
    public int maxHealth = 3;
    private int currentHealth;
    public HeartDisplay heartDisplay;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerInitialSpeed = moveSpeed;
        lastMoveDirection = Vector2.right;
        currentHealth = maxHealth;
        heartDisplay.UpdateHearts(currentHealth, maxHealth);
    }

    void Update()
    {
        ProcessInput();
        Animate();
        Flip();
        OnAttack();
        HandleSpeedBoost();
        PlayStepSound();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement;
        }
    }

    void Move()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    void Animate()
    {
        if (isAttack)
        {
            anim.SetInteger("Movimento", 2);
        }
        else
        {
            anim.SetInteger("Movimento", movement.sqrMagnitude > 0 ? 1 : 0);
        }
    }

    void Flip()
    {
        if (movement.x > 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (movement.x < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    void OnAttack()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            isAttack = true;
            moveSpeed = 0;
            canAttack = false;
            anim.SetTrigger("Attack");

            float direction = transform.eulerAngles.y == 0 ? 1f : -1f;
            attackPoint.localPosition = new Vector2(attackRange * direction, 0f);

            // Detecta inimigos na área
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                // Atordoa o inimigo, se ele tiver o script EnemyAI
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.Stun(); // Aplica atordoamento
                }

                // Se o inimigo tiver vida (como no caso de outro tipo), pode aplicar dano também aqui se quiser
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }

            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }


    void ResetAttack()
    {
        isAttack = false;
        moveSpeed = playerInitialSpeed;
        canAttack = true;
    }

    void HandleSpeedBoost()
    {
        if (!isAttack)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                moveSpeed = boostSpeed;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                moveSpeed = playerInitialSpeed * 0.5f;
            }
            else
            {
                moveSpeed = playerInitialSpeed;
            }
        }
    }

    void PlayStepSound()
    {
        if (movement.sqrMagnitude > 0 && !isAttack)
        {
            if (!isWalking)
            {
                isWalking = true;
                if (stepSound != null && audioSource != null)
                {
                    audioSource.clip = stepSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                audioSource.Stop();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public void TakeDamage(int amount)
    {
        Debug.Log("Dano recebido: " + amount);  // Debug: verificar se a função está sendo chamada
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        heartDisplay.UpdateHearts(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }



    void Die()
    {
        Debug.Log("Player morreu!");
        // Aqui você pode tocar animação, desabilitar controle, etc.
    }
    // Exemplo de colisão no inimigo
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Reduz a vida do jogador quando colide com o inimigo
            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                TakeDamage(1);  // Pode ajustar o valor do dano se necessário
            }
        }
    }



}
