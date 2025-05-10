using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public float boostSpeed = 8f;
    private float playerInitialSpeed;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private Vector2 lastMoveDirection;

    public AudioClip stepSound;
    private AudioSource audioSource;
    private bool isWalking = false;

    [Header("Ataque")]
    public int attackDamage = 25;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private bool isAttack = false;
    private bool canAttack = true;
    public float attackCooldown = 0.5f;

    [Header("Armas")]
    private bool hasKnife = false; // <<--- Jogador inicia SEM a faca

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerInitialSpeed = moveSpeed;
        lastMoveDirection = Vector2.right;
    }

    void Update()
    {
        ProcessInput();
        Animate();
        Flip();
        HandleAttack();
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
            anim.SetInteger("Movimento", 2); // Atacando
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

    void HandleAttack()
    {
        if (!hasKnife) return; // <<--- Bloqueia ataque se n�o tiver faca

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Debug.Log("Ataque iniciado!");

            isAttack = true;
            moveSpeed = 0;
            canAttack = false;
            anim.SetTrigger("Attack");
            PerformAttack();
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void PerformAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        if (hitEnemies.Length == 0)
        {
            Debug.Log("Nenhum inimigo atingido.");
        }

        foreach (Collider2D enemy in hitEnemies)
        {
           

 
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
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = boostSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed = playerInitialSpeed * 0.5f;
        }
        else if (!isAttack)
        {
            moveSpeed = playerInitialSpeed;
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
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    // === M�TODO PARA PEGAR A FACA ===
    public void CollectKnife()
    {
        hasKnife = true;
        Debug.Log("Faca coletada! Agora o jogador pode atacar.");
    }

   
}
