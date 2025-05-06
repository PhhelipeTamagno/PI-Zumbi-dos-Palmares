using UnityEngine;

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
    private AudioSource audioSource;
    private bool isWalking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerInitialSpeed = moveSpeed;
        lastMoveDirection = Vector2.right;
    }

    void Update()
    {
        ProcessInput();
        Animate();
        Flip();
        HandleAttackAnimation();
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
        anim.SetInteger("Movimento", movement.sqrMagnitude > 0 ? 1 : 0);
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

    void HandleAttackAnimation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }
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
        else
        {
            moveSpeed = playerInitialSpeed;
        }
    }

    void PlayStepSound()
    {
        if (movement.sqrMagnitude > 0)
        {
            if (!isWalking)
            {
                isWalking = true;
                if (stepSound != null && audioSource != null)
                {
                    audioSource.clip = stepSound;
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
                audioSource.Stop();
            }
        }
    }
}
