using UnityEngine;

public class DesativarNaEtapa : MonoBehaviour
{
    public int etapaParaDesativar = 3; // por exemplo: 3 = início da missão do café
    private MissaoManager missaoManager;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoManager>();
    }

    void Update()
    {
        if (missaoManager != null && missaoManager.etapaMissao == etapaParaDesativar)
        {
            gameObject.SetActive(false);
        }
    }
}
