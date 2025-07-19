using UnityEngine;

public class MapaButtonPainel : MonoBehaviour
{
    [Header("Painel do Mapa")]
    public GameObject painelMapa;

    [Header("Painel de Desfoque")]
    public GameObject painelDesfoque;

    private bool painelAtivo = false;

    private void Start()
    {
        painelMapa.SetActive(false);
        painelDesfoque.SetActive(false);
    }

    public void AlternarPainel()
    {
        painelAtivo = !painelAtivo;

        painelMapa.SetActive(painelAtivo);
        painelDesfoque.SetActive(painelAtivo);
    }
}
