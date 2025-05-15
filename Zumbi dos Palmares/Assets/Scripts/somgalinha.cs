using UnityEngine;

public class somgalinha : MonoBehaviour
{
    public AudioClip soundToPlay;
    public float minInterval = 5f;
    public float maxInterval = 7f;

    private AudioSource audioSource;
    private bool playerInArea = false;
    private float timer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = GetRandomInterval();
    }

    void Update()
    {
        if (!playerInArea) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (soundToPlay != null)
            {
                audioSource.PlayOneShot(soundToPlay);
            }
            timer = GetRandomInterval();
        }
    }

    float GetRandomInterval()
    {
        return Random.Range(minInterval, maxInterval);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = false;
        }
    }
}
