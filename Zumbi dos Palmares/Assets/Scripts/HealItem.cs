using UnityEngine;

public class HealItem : MonoBehaviour
{
    [Header("Quanto de vida esse item recupera")]
    public int healAmount = 1;

    [Header("Efeito sonoro (opcional)")]
    public AudioClip healSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se colidiu com o jogador
        PlayerHealthUI player = other.GetComponent<PlayerHealthUI>();
        if (player != null)
        {
            player.Heal(healAmount);

            if (healSound != null)
                AudioSource.PlayClipAtPoint(healSound, transform.position);

            Destroy(gameObject);
        }
    }
}

