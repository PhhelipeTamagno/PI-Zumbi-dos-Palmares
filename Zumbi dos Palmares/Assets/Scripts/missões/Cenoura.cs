using UnityEngine;

public class Cenoura : MonoBehaviour
{
    private MissaoManager missaoManager;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            missaoManager.ColetouCenoura();
            Destroy(gameObject);
        }
    }
}
