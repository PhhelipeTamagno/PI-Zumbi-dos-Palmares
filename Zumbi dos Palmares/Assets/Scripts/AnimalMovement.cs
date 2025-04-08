using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionTime = 2f;
    private Vector2 moveDirection;
    private float timer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeDirection();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
        FlipSprite();
    }

    void ChangeDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        timer = changeDirectionTime;
    }

    void FlipSprite()
    {
        if (moveDirection.x > 0)
            spriteRenderer.flipX = false;
        else if (moveDirection.x < 0)
            spriteRenderer.flipX = true;
    }
}
