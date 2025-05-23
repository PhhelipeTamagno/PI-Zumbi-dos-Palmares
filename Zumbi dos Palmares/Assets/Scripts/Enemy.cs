using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Inimigo levou " + damage + " de dano. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Inimigo morreu!");
        Destroy(gameObject);
    }
}
