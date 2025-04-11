using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public static PlaySound instance;
    public AudioSource audioSource;

    private void Awake()
    {
        // Garantir que só exista uma instância
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Evita duplicação
        }
    }

    public void PlayClickSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource ou AudioClip não está configurado.");
        }
    }
}
