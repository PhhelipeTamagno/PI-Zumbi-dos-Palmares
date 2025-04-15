using UnityEngine;

public class SoundRange : MonoBehaviour
{
    public AudioClip soundClip;  
    private AudioSource audioSource;

    private void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            if (soundClip != null && audioSource != null)
            {
                audioSource.clip = soundClip;
                audioSource.loop = true;  
                audioSource.Play();
            }
        }
    }

   
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            if (audioSource != null)
            {
                audioSource.Stop();  
            }
        }
    }
}
