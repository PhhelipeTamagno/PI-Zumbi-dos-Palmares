using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    /* ---------- INSPECTOR ---------- */
    [Header("Movimentação")]
    public float moveSpeed = 5f;
    public float boostSpeed = 8f;

    [Header("Referências")]
    public HotbarController hotbarController;
    public int knifeItemID = 0;

    [Header("Combate")]
    public int attackDamage = 25;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackCooldown = 0.5f;
    public AudioClip attackSound1, attackSound2;

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

    /* ---------- MOBILE CONTROLS ---------- */
    public bool mobileUp, mobileDown, mobileLeft, mobileRight;
    public bool mobileRun = false;   // << NOVO: botão de correr no mobile

    /* ---------- INTERNOS ---------- */
    private float defaultSpeed;
    private float currentStamina;
    private bool isRunning, isAttacking, canAttack = true;
    private bool facingRight = true, attackBlockedByZone = false;
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource au;
    private Vector2 move;

    /* ---------- UNITY ---------- */
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        au = GetComponent<AudioSource>();

        defaultSpeed = moveSpeed;
        currentStamina = maxStamina;

        if (staminaSlider)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = maxStamina;
        }

        if (hotbarController == null)
            hotbarController = FindObjectOfType<HotbarController>();
    }

    void Update()
    {
        ReadInput();
        HandleAttack();
        HandleMoveSpeed();
        HandleFootsteps();
        Animate();
        FlipSprite();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = move * moveSpeed;
    }

    /* ---------- MOVIMENTO ---------- */
    void ReadInput()
    {
        // PC
        float pcX = Input.GetAxisRaw("Horizontal");
        float pcY = Input.GetAxisRaw("Vertical");

        // Mobile
        float mX = 0f;
        float mY = 0f;

        if (mobileUp) mY = 1f;
        if (mobileDown) mY = -1f;
        if (mobileLeft) mX = -1f;
        if (mobileRight) mX = 1f;

        move.x = pcX != 0 ? pcX : mX;
        move.y = pcY != 0 ? pcY : mY;

        move.Normalize();
    }

    /* ---------- COMBATE ---------- */
    void HandleAttack()
    {
        bool knifeEquipped = hotbarController && hotbarController.GetSelectedItemID() == knifeItemID;

        if (!knifeEquipped || attackBlockedByZone) return;

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            isAttacking = true;
            canAttack = false;

            anim.SetTrigger("Attack");
            PlayAttackSound();
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    public void ApplyDamage() // Animation Event
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (var col in hits)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy) enemy.TakeDamage(attackDamage);
        }
    }

    void ResetAttack() { isAttacking = false; canAttack = true; }

    void PlayAttackSound()
    {
        if (!au) return;
        au.Stop();
        if (attackSound1) au.PlayOneShot(attackSound1);
        if (attackSound2) Invoke(nameof(PlaySecondAttackClip), 0.2f);
    }

    void PlaySecondAttackClip()
    {
        if (au && attackSound2) au.PlayOneShot(attackSound2);
    }

    /* ---------- STAMINA / CORRIDA ---------- */
    void HandleMoveSpeed()
    {
        // PC SHIFT OU BOTÃO MOBILE
        bool wantsRun =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift) ||
            mobileRun;   // << AQUI FAZ O BOTÃO FUNCIONAR

        bool moving = move.sqrMagnitude > 0f;
        bool canRun = currentStamina > 0f;

        isRunning = wantsRun && moving && !isAttacking && canRun;

        if (isRunning)
        {
            moveSpeed = boostSpeed;
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }
        else
        {
            moveSpeed = defaultSpeed;
            if (currentStamina < maxStamina)
                currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        if (staminaSlider) staminaSlider.value = currentStamina;
    }

    /* ---------- SONS DE PASSO ---------- */
    void HandleFootsteps()
    {
        if (!au || isAttacking || !footstepSound) return;

        if (move.sqrMagnitude > 0 && !au.isPlaying)
        {
            au.clip = footstepSound;
            au.loop = true;
            au.Play();
        }
        else if (move.sqrMagnitude == 0 && au.clip == footstepSound)
        {
            au.Stop();
        }
    }

    /* ---------- ANIMAÇÃO ---------- */
    void Animate()
    {
        int state = isAttacking ? 2 : (move.sqrMagnitude > 0 ? 1 : 0);
        anim.SetInteger("Movimento", state);
    }

    /* ---------- FLIP ---------- */
    void FlipSprite()
    {
        if ((move.x > 0 && !facingRight) || (move.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);

            if (attackPoint)
            {
                Vector3 p = attackPoint.localPosition;
                p.x *= -1;
                attackPoint.localPosition = p;
            }
        }
    }

    /* ---------- ZONA SEM ATAQUE ---------- */
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

    /* ---------- VIDA ---------- */
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Invoke(nameof(RestartScene), 2f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene("jogo 5");
    }

    /* ---------- MOBILE BUTTONS ---------- */
    public void MobileUpDown() { mobileUp = true; }
    public void MobileUpUp() { mobileUp = false; }

    public void MobileDownDown() { mobileDown = true; }
    public void MobileDownUp() { mobileDown = false; }

    public void MobileLeftDown() { mobileLeft = true; }
    public void MobileLeftUp() { mobileLeft = false; }

    public void MobileRightDown() { mobileRight = true; }
    public void MobileRightUp() { mobileRight = false; }

    // ------ BOTÃO DE CORRER (NOVO) ------
    public void RunButtonDown() { mobileRun = true; }
    public void RunButtonUp() { mobileRun = false; }

    /* ---------- GIZMOS ---------- */
    void OnDrawGizmosSelected()
    {
        if (!attackPoint) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
