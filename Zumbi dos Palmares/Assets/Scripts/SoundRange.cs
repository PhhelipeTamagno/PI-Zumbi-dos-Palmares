using UnityEngine;

public class SoundRange : MonoBehaviour
{
    public AudioClip soundClip;  // Som que será tocado quando o jogador entra na área
    private AudioSource audioSource;

    private void Start()
    {
        // Inicializa o AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // Método chamado quando o jogador entra na área de trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se é o jogador
        {
            if (soundClip != null && audioSource != null)
            {
                audioSource.clip = soundClip;
                audioSource.loop = true;  // Faz o som tocar em loop enquanto o jogador está na área
                audioSource.Play();
            }
        }
    }

    // Método chamado quando o jogador sai da área de trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se é o jogador
        {
            if (audioSource != null)
            {
                audioSource.Stop();  // Para o som quando o jogador sair da área
            }
        }
    }
}
