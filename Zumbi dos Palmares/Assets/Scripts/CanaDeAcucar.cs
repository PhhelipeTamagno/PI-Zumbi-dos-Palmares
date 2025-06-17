using UnityEngine;

public class CanaDeAcucar : MonoBehaviour
{
    private MissaoManager missaoManager;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            missaoManager.CanaColetada();
            // Desativa o objeto para simular a coleta
            gameObject.SetActive(false);
        }
    }
}
