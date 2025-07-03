using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 5f;
    public float boostSpeed = 8f;
    private float defaultSpeed;

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

    // Internos
    private float currentStamina;
    private bool isRunning = false;
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource au;
    private Vector2 move;
    private bool facingRight = true;
    private bool isAttacking = false, canAttack = true;
    private bool knifeCollected = false;
    private bool attackBlockedByZone = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        defaultSpeed = moveSpeed;

        currentStamina = maxStamina;
        if (staminaSlider != null)
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

    void ReadInput()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move.Normalize();
    }

    void HandleAttack()
    {
        bool knifeEquipped = hotbarController && hotbarController.GetSelectedItemID() == knifeItemID;

        if (!knifeEquipped || attackBlockedByZone) return;

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            isAttacking = true;
            moveSpeed = 0;
            canAttack = false;

            anim.SetTrigger("Attack");
            PlayAttackSound();

            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    public void ApplyDamage()
    {
        Debug.Log("Tentando aplicar dano...");

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (var col in hits)
        {
            var enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Inimigo atingido!");
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        moveSpeed = defaultSpeed;
        canAttack = true;
    }

    void PlayAttackSound()
    {
        if (!au) return;

        au.Stop();
        au.loop = false;
        if (attackSound1) au.PlayOneShot(attackSound1);
        if (attackSound2) Invoke(nameof(PlaySecondAttackClip), 0.2f);
    }

    void PlaySecondAttackClip()
    {
        if (au && attackSound2)
            au.PlayOneShot(attackSound2);
    }

    void HandleMoveSpeed()
    {
        bool wantsToRun = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
        bool moving = move.sqrMagnitude > 0;
        bool canRun = currentStamina > 0;

        isRunning = wantsToRun && moving && !isAttacking && canRun;

        if (isRunning)
        {
            moveSpeed = boostSpeed;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else
        {
            if (!isAttacking)
                moveSpeed = defaultSpeed;

            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRecoveryRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }

        if (staminaSlider != null)
            staminaSlider.value = currentStamina;
    }

    void HandleFootsteps()
    {
        if (!au || isAttacking || footstepSound == null) return;

        bool isMoving = move.sqrMagnitude > 0;

        if (isMoving && !au.isPlaying)
        {
            au.clip = footstepSound;
            au.loop = true;
            au.Play();
        }
        else if (!isMoving && au.clip == footstepSound)
        {
            au.Stop();
        }
    }

    void Animate()
    {
        anim.SetInteger("Movimento", isAttacking ? 2 : (move.sqrMagnitude > 0 ? 1 : 0));
    }

    void FlipSprite()
    {
        if ((move.x > 0 && !facingRight) || (move.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);

            if (attackPoint)
            {
                Vector3 localPos = attackPoint.localPosition;
                localPos.x *= -1;
                attackPoint.localPosition = localPos;
            }
        }
    }

    public void CollectKnife()
    {
        if (knifeCollected) return;

        if (hotbarController)
        {
            hotbarController.AddItemToHotbar(knifeItemID);
            knifeCollected = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ZonaSemAtaque"))
        {
            if (hotbarController && hotbarController.GetSelectedItemID() == knifeItemID)
                attackBlockedByZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ZonaSemAtaque"))
        {
            if (hotbarController && hotbarController.GetSelectedItemID() == knifeItemID)
                attackBlockedByZone = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
