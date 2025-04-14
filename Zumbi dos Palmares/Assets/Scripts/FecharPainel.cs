using UnityEngine;

public class FecharPainel : MonoBehaviour
{
    public GameObject painelParaFechar; // Arraste aqui o painel que será fechado

    public void Fechar()
    {
        painelParaFechar.SetActive(false); // Desativa o painel
        gameObject.SetActive(false);        // Desativa o botão que foi clicado
    }
}
