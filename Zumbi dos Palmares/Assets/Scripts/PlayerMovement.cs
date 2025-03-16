using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float playerInitialSpeed;
    private bool isAttack = false;

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
        if (Input.GetMouseButtonDown(0)) // Botão esquerdo do mouse
        {
            isAttack = true;
            moveSpeed = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isAttack = false;
            moveSpeed = playerInitialSpeed;
        }
    }
}