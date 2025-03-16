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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerInitialSpeed = moveSpeed;
    }

    void Update()
    {
        ProcessInput();
        Animate();
        Flip();
        OnAttack();
        HandleSpeedBoost();
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
        rb.velocity = movement * moveSpeed;
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
        if (Input.GetMouseButtonDown(0) && canAttack) // Botão esquerdo do mouse
        {
            isAttack = true;
            moveSpeed = 0;
            canAttack = false;
            anim.SetTrigger("Attack"); // Adicionando trigger de ataque na animação
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
            else
            {
                moveSpeed = playerInitialSpeed;
            }
        }
    }
}