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
    public AudioClip woodStepSound;
    public AudioClip attackSound1;
    public AudioClip attackSound2;
    private AudioClip originalStepSound;
    private AudioSource audioSource;
    private bool isWalking = false;
    private bool isOnWood = false;

    [Header("Ataque")]
    public int attackDamage = 25;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private bool isAttack = false;
    private bool canAttack = true;
    public float attackCooldown = 0.5f;

    [Header("Armas")]
    private bool hasKnife = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerInitialSpeed = moveSpeed;
        lastMoveDirection = Vector2.right;
        originalStepSound = stepSound;
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

    void HandleAttack()
    {
        if (!hasKnife) return;

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Debug.Log("Ataque iniciado!");

            isAttack = true;
            moveSpeed = 0;
            canAttack = false;
            anim.SetTrigger("Attack");

            PlayAttackSound();

            PerformAttack();
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void PlayAttackSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false;

            if (attackSound1 != null)
            {
                audioSource.PlayOneShot(attackSound1);
            }

            if (attackSound2 != null)
            {
                Invoke(nameof(PlaySecondAttackSound), 0.2f); // ajuste esse tempo conforme sua animação
            }
        }
    }

    void PlaySecondAttackSound()
    {
        if (audioSource != null && attackSound2 != null)
        {
            audioSource.PlayOneShot(attackSound2);
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
            // aplicar dano no inimigo
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

                if (audioSource != null)
                {
                    audioSource.clip = isOnWood ? woodStepSound : originalStepSound;
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
                if (!isAttack && audioSource.clip != attackSound1 && audioSource.clip != attackSound2)
                    audioSource.Stop();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wood"))
        {
            isOnWood = true;

            if (isWalking && audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = woodStepSound;
                audioSource.Play();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wood"))
        {
            isOnWood = false;

            if (isWalking && audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = originalStepSound;
                audioSource.Play();
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

    public void CollectKnife()
    {
        hasKnife = true;
        Debug.Log("Faca coletada! Agora o jogador pode atacar.");
    }
}
