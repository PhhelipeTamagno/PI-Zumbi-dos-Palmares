using UnityEngine;

public class DetectorDeSaida : MonoBehaviour
{
    private Portal portal;

    public void Configurar(Portal portalOrigem)
    {
        portal = portalOrigem;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            portal.VoltarParaCenaAntiga();
        }
    }
}
