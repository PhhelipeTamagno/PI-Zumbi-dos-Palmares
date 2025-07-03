using UnityEngine;

public class MissaoTriggerProxima : MonoBehaviour
{
    private MissaoNoturnaManager missaoManager;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoNoturnaManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && missaoManager.etapaMissaoNoturna == 1)
        {
            missaoManager.textoMissao.text = "Conserte a ponte quebrada.";
            missaoManager.etapaMissaoNoturna = 2;
        }
    }
}
