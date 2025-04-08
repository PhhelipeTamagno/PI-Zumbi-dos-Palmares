using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float boostSpeed = 8f;
    private float playerInitialSpeed;
    private bool isAttack = false;
    private bool canAttack = true;
    public float attackCooldown = 0.5f; // Tempo de recarga do ataque

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;

    // Novas vari�veis para a m�sica dos passos
    public AudioClip stepSound;           // Som dos passos
    private AudioSource audioSource;      // Componente AudioSource para tocar o som dos passos
    private bool isWalking = false;      // Flag para verificar se o jogador est� andando

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Inicializa o AudioSource
        playerInitialSpeed = moveSpeed;
    }

    void Update()
    {
        ProcessInput();
        Animate();
        Flip();
        OnAttack();
        HandleSpeedBoost();
        PlayStepSound(); // Chama o m�todo para verificar e tocar o som dos passos
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
        // Ataque acontece ao pressionar o bot�o esquerdo do mouse
        if (Input.GetMouseButtonDown(0) && canAttack) // Bot�o esquerdo do mouse
        {
            isAttack = true;
            moveSpeed = 0; // Parar o movimento durante o ataque
            canAttack = false;
            anim.SetTrigger("Attack"); // Adicionando trigger de ataque na anima��o
            Invoke(nameof(ResetAttack), attackCooldown); // Resetar ataque ap�s o tempo de cooldown
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
                moveSpeed = boostSpeed; // Aumenta a velocidade ao segurar Shift
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                moveSpeed = playerInitialSpeed * 0.5f; // Reduz a velocidade ao segurar Ctrl
            }
            else
            {
                moveSpeed = playerInitialSpeed;
            }
        }
    }


    void PlayStepSound()
    {
        // Verifica se o jogador est� se movendo
        if (movement.sqrMagnitude > 0 && !isAttack) // Se o jogador est� se movendo e n�o est� atacando
        {
            if (!isWalking) // Se n�o est� tocando o som dos passos
            {
                isWalking = true;
                if (stepSound != null && audioSource != null)
                {
                    audioSource.clip = stepSound;
                    audioSource.loop = true; // O som dos passos vai se repetir enquanto o jogador estiver andando
                    audioSource.Play();
                }
            }
        }
        else
        {
            if (isWalking) // Se o jogador parou de andar
            {
                isWalking = false;
                audioSource.Stop(); // Para o som dos passos
            }
        }
    }
}
