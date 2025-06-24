using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 5f;
    public float boostSpeed = 8f;
    private float defaultSpeed;

    [Header("Referências")]
    public HotbarController hotbarController;
    public int knifeItemID = 0;            // ID da faca no catálogo

    [Header("Combate")]
    public int attackDamage = 25;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackCooldown = 0.5f;
    public AudioClip attackSound1, attackSound2;

    /* ----- internos ----- */
    Rigidbody2D rb;
    Animator anim;
    AudioSource au;
    Vector2 move;
    bool facingRight = true;
    bool isAttacking, canAttack = true;
    private bool knifeCollected = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        defaultSpeed = moveSpeed;

        if (hotbarController == null) hotbarController = FindObjectOfType<HotbarController>();
    }

    void Update()
    {
        ReadInput();
        HandleAttack();
        HandleMoveSpeed();
        Animate();
        FlipSprite();
    }

    void FixedUpdate() => rb.linearVelocity = move * moveSpeed;

    /* ---------- INPUT ---------- */
    void ReadInput()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move.Normalize();
    }

    /* ---------- COMBATE ---------- */
    void HandleAttack()
    {
        bool knifeEquipped = hotbarController && hotbarController.GetSelectedItemID() == knifeItemID;

        if (!knifeEquipped) return;

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            isAttacking = true;
            moveSpeed = 0;
            canAttack = false;

            anim.SetTrigger("Attack");
            PlayAttackSound();

            DealDamage();
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (var col in hits)
        {
            var en = col.GetComponent<Enemy>();
            if (en) en.TakeDamage(attackDamage);
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
    void PlaySecondAttackClip() { if (au && attackSound2) au.PlayOneShot(attackSound2); }

    /* ---------- MOVIMENTO ---------- */
    void HandleMoveSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            moveSpeed = boostSpeed;
        else if (!isAttacking)
            moveSpeed = defaultSpeed;
    }

    /* ---------- VISUAL ---------- */
    void Animate() => anim.SetInteger("Movimento", isAttacking ? 2 : (move.sqrMagnitude > 0 ? 1 : 0));

    void FlipSprite()
    {
        if (move.x > 0 && !facingRight || move.x < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
            if (attackPoint)
            {
                Vector3 lp = attackPoint.localPosition;
                lp.x *= -1;
                attackPoint.localPosition = lp;
            }
        }
    }

    /* ---------- COLETA ---------- */
    /// <summary>Chamado pelo KnifePickup quando o player encosta na faca.</summary>

    public void CollectKnife()
    {
        if (knifeCollected) return;

        if (hotbarController)
        {
            hotbarController.AddItemToHotbar(knifeItemID);
            knifeCollected = true;
        }
    }


    /* ---------- GIZMOS ---------- */
    void OnDrawGizmosSelected()
    {
        if (attackPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
