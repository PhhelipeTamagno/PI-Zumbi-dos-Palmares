
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 5f;
    public float boostSpeed = 8f;

    [Header("Referências")]
    public HotbarController hotbarController;
    public int knifeItemID = 0;
    public SpriteRenderer spriteRenderer;

    [Header("Idle - Frente")]
    public Sprite[] idleFrente;

    [Header("Idle - Costas")]
    public Sprite[] idleCostas;

    [Header("Idle - Lado (feito para a direita)")]
    public Sprite[] idleLado;

    [Header("Walk - Frente")]
    public Sprite[] walkFrente;

    [Header("Walk - Costas")]
    public Sprite[] walkCostas;

    [Header("Walk - Lado (feito para a direita)")]
    public Sprite[] walkLado;

    [Header("Attack - Frente")]
    public Sprite[] attackFrente;

    [Header("Attack - Costas")]
    public Sprite[] attackCostas;

    [Header("Attack - Lado (feito para a direita)")]
    public Sprite[] attackLado;

    [Header("Velocidade das Animações")]
    public float idleFPS = 5f;
    public float walkFPS = 8f;
    public float attackFPS = 12f;

    [Header("Flip Horizontal")]
    public bool usarFlipHorizontal = true;

    [Header("Combate")]
    public int attackDamage = 25;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackCooldown = 0.5f;
    public AudioClip attackSound1;
    public AudioClip attackSound2;

    [Header("Som de Passos")]
    public AudioClip footstepSound;

    [Header("Stamina")]
    public Slider staminaSlider;
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRecoveryRate = 15f;

    [Header("Vida")]
    public int maxHealth = 3;
    public int currentHealth = 3;

    [Header("Controles Mobile")]
    public bool mobileUp, mobileDown, mobileLeft, mobileRight;
    public bool mobileRun;
    public bool mobileAttack;

    private Rigidbody2D rb;
    private AudioSource au;

    private Vector2 move;
    private Vector2 lastDirection = Vector2.down;

    private float defaultSpeed;
    private float currentStamina;
    private float animationTimer;

    private bool isRunning;
    private bool isAttacking;
    private bool canAttack = true;
    private bool attackBlockedByZone;
    private bool facingRight = true;
    private bool deathStarted;

    private enum AnimationType
    {
        Idle,
        Walk,
        Attack
    }

    private AnimationType currentAnimation = AnimationType.Idle;
    private int currentFrame;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        au = GetComponent<AudioSource>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        defaultSpeed = moveSpeed;
        currentStamina = maxStamina;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }

        if (hotbarController == null)
            hotbarController = FindObjectOfType<HotbarController>();

        AtualizarDirecaoVisual();
        ChangeAnimation(AnimationType.Idle);
    }

    void Update()
    {
        if (deathStarted)
            return;

        ReadInput();
        UpdateDirection();
        HandleAttack();
        HandleMoveSpeed();
        HandleFootsteps();
        AtualizarDirecaoVisual();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        rb.linearVelocity = deathStarted
            ? Vector2.zero
            : move * moveSpeed;
    }

    // MOVIMENTAÇÃO SEM DIAGONAL
    void ReadInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        float mobileX = 0f;
        float mobileY = 0f;

        if (mobileLeft) mobileX = -1f;
        else if (mobileRight) mobileX = 1f;

        if (mobileDown) mobileY = -1f;
        else if (mobileUp) mobileY = 1f;

        if (x == 0f)
            x = mobileX;

        if (y == 0f)
            y = mobileY;

        // Prioriza a direção horizontal quando ambas são pressionadas.
        if (x != 0f)
            move = new Vector2(Mathf.Sign(x), 0f);
        else if (y != 0f)
            move = new Vector2(0f, Mathf.Sign(y));
        else
            move = Vector2.zero;
    }

    // DIREÇÃO DO PERSONAGEM
    void UpdateDirection()
    {
        if (move.sqrMagnitude < 0.01f)
            return;

        lastDirection = move;
    }

    // FLIP PARA ESQUERDA E DIREITA
    void AtualizarDirecaoVisual()
    {
        if (spriteRenderer == null || !usarFlipHorizontal)
            return;

        if (lastDirection.x < 0f)
            facingRight = false;
        else if (lastDirection.x > 0f)
            facingRight = true;

        // Idle, Walk e Attack usam o sprite lateral original,
        // desenhado para a direita.
        spriteRenderer.flipX = !facingRight;
    }

    // ATAQUE
    void HandleAttack()
    {
        bool knifeEquipped =
            hotbarController != null &&
            hotbarController.GetSelectedItemID() == knifeItemID;

        bool pressed = Input.GetMouseButtonDown(0) || mobileAttack;
        mobileAttack = false;

        if (!knifeEquipped || attackBlockedByZone)
            return;

        if (pressed && canAttack)
        {
            isAttacking = true;
            canAttack = false;

            ChangeAnimation(AnimationType.Attack);
            PlayAttackSound();

            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    public void ApplyDamage()
    {
        if (attackPoint == null)
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D col in hits)
        {
            Enemy enemy = col.GetComponent<Enemy>();

            if (enemy != null)
                enemy.TakeDamage(attackDamage);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        canAttack = true;

        if (deathStarted)
            return;

        ChangeAnimation(
            move.sqrMagnitude > 0.01f
                ? AnimationType.Walk
                : AnimationType.Idle
        );

        AtualizarDirecaoVisual();
    }

    void PlayAttackSound()
    {
        if (au == null)
            return;

        if (attackSound1 != null)
            au.PlayOneShot(attackSound1);

        if (attackSound2 != null)
            au.PlayOneShot(attackSound2);
    }

    // STAMINA
    void HandleMoveSpeed()
    {
        bool wantsRun =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift) ||
            mobileRun;

        bool moving = move.sqrMagnitude > 0.01f;

        isRunning =
            wantsRun &&
            moving &&
            !isAttacking &&
            currentStamina > 0f;

        if (isRunning)
        {
            moveSpeed = boostSpeed;
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }
        else
        {
            moveSpeed = defaultSpeed;
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(
            currentStamina,
            0f,
            maxStamina
        );

        if (staminaSlider != null)
            staminaSlider.value = currentStamina;
    }

    // SONS DE PASSOS
    void HandleFootsteps()
    {
        if (au == null || footstepSound == null)
            return;

        bool moving = move.sqrMagnitude > 0.01f;

        if (isAttacking || !moving)
        {
            if (au.clip == footstepSound && au.isPlaying)
                au.Stop();

            return;
        }

        if (!au.isPlaying)
        {
            au.clip = footstepSound;
            au.loop = true;
            au.Play();
        }
    }

    // ANIMAÇÕES
    void UpdateAnimation()
    {
        if (spriteRenderer == null)
            return;

        AnimationType desired;

        if (isAttacking)
            desired = AnimationType.Attack;
        else if (move.sqrMagnitude > 0.01f)
            desired = AnimationType.Walk;
        else
            desired = AnimationType.Idle;

        if (desired != currentAnimation)
            ChangeAnimation(desired);

        Sprite[] frames = GetCurrentFrames();

        if (frames == null || frames.Length == 0)
            return;

        float fps = Mathf.Max(1f, GetCurrentFPS());

        animationTimer += Time.deltaTime;

        while (animationTimer >= 1f / fps)
        {
            animationTimer -= 1f / fps;

            if (currentAnimation == AnimationType.Attack)
            {
                if (currentFrame < frames.Length - 1)
                    currentFrame++;
            }
            else
            {
                currentFrame = (currentFrame + 1) % frames.Length;
            }

            spriteRenderer.sprite = frames[currentFrame];
        }
    }

    void ChangeAnimation(AnimationType newAnimation)
    {
        currentAnimation = newAnimation;
        currentFrame = 0;
        animationTimer = 0f;

        UpdateSprite();
        AtualizarDirecaoVisual();
    }

    void UpdateSprite()
    {
        if (spriteRenderer == null)
            return;

        Sprite[] frames = GetCurrentFrames();

        if (frames != null && frames.Length > 0)
            spriteRenderer.sprite = frames[0];
    }

    Sprite[] GetCurrentFrames()
    {
        bool costas = lastDirection.y > 0f;
        bool frente = lastDirection.y < 0f;
        bool lateral = lastDirection.x != 0f;

        switch (currentAnimation)
        {
            case AnimationType.Idle:
                if (costas) return idleCostas;
                if (lateral) return idleLado;
                return idleFrente;

            case AnimationType.Walk:
                if (costas) return walkCostas;
                if (lateral) return walkLado;
                return walkFrente;

            case AnimationType.Attack:
                if (costas) return attackCostas;
                if (lateral) return attackLado;
                if (frente) return attackFrente;
                return attackFrente;
        }

        return idleFrente;
    }

    float GetCurrentFPS()
    {
        switch (currentAnimation)
        {
            case AnimationType.Idle:
                return idleFPS;

            case AnimationType.Walk:
                return walkFPS;

            case AnimationType.Attack:
                return attackFPS;

            default:
                return idleFPS;
        }
    }

    // ZONA SEM ATAQUE
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("ZonaSemAtaque"))
            attackBlockedByZone = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("ZonaSemAtaque"))
            attackBlockedByZone = false;
    }

    // VIDA
    public void TakeDamage(int damage)
    {
        if (deathStarted)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        if (deathStarted)
            return;

        deathStarted = true;
        move = Vector2.zero;
        moveSpeed = 0f;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        CancelInvoke(nameof(ResetAttack));
        Invoke(nameof(RestartScene), 2f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene("jogo 5");
    }

    // BOTÕES MOBILE
    public void MobileUpDown() { mobileUp = true; }
    public void MobileUpUp() { mobileUp = false; }

    public void MobileDownDown() { mobileDown = true; }
    public void MobileDownUp() { mobileDown = false; }

    public void MobileLeftDown() { mobileLeft = true; }
    public void MobileLeftUp() { mobileLeft = false; }

    public void MobileRightDown() { mobileRight = true; }
    public void MobileRightUp() { mobileRight = false; }

    public void MobileRunDown() { mobileRun = true; }
    public void MobileRunUp() { mobileRun = false; }

    public void MobileAttackDown() { mobileAttack = true; }

    // GIZMOS
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
