using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 movement;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;

        if (movement.x != 0)
        {
            anim.SetLayerWeight(0, 0);
            anim.SetLayerWeight(2, 1);

            // Inverte o sprite dependendo da direção
            sprite.flipX = movement.x < 0;
        }

        if (movement.y > 0 && movement.x == 0)
        {
            anim.SetLayerWeight(2, 0);
            anim.SetLayerWeight(1, 1);
        }
        else
        {
            anim.SetLayerWeight(1, 0);
        }

        if (movement != Vector2.zero)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    private void ResetLayers()
    {
        anim.SetLayerWeight(0, 0);
        anim.SetLayerWeight(2, 0);
        // Corrigido: Removida a linha duplicada
    }
}