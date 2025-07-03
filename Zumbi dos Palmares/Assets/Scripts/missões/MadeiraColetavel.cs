using UnityEngine;

public class MadeiraColetavel : MonoBehaviour
{
    private MissaoNoturnaManager missaoManager;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoNoturnaManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            missaoManager.MadeiraColetada();
            gameObject.SetActive(false); // simula coleta
        }
    }
}
