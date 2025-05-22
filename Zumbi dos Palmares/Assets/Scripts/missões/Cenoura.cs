using UnityEngine;

public class Cenoura : MonoBehaviour
{
    private MissaoManager missaoManager;
    public AudioClip[] sonsColetar;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (sonsColetar.Length > 0)
            {
                int index = Random.Range(0, sonsColetar.Length);
                GameObject som = new GameObject("SomColetar");
                AudioSource audioSource = som.AddComponent<AudioSource>();
                audioSource.PlayOneShot(sonsColetar[index]);
                Destroy(som, sonsColetar[index].length);
            }

            missaoManager.ColetouCenoura();
            Destroy(gameObject);
        }
    }
}
