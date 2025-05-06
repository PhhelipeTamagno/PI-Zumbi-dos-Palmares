using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float damageCooldown = 1f;
    public int damageAmount = 1;
    public float stunDuration = 2f;

    private Transform player;
    private Rigidbody2D rb;
    private float lastDamageTime;
    private bool isStunned = false;
    private float stunTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
            }
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            PlayerMovement playerScript = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }

    public void Stun()
    {
        isStunned = true;
        rb.linearVelocity = Vector2.zero;
        stunTimer = stunDuration;
        // Se você quiser, pode adicionar uma animação de atordoamento aqui
    }

}
