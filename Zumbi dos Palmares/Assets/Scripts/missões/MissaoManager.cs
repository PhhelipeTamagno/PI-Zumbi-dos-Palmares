using UnityEngine;
using TMPro;

public class MissaoManager : MonoBehaviour
{
    public TextMeshProUGUI textoMissao;
    private int cenourasTotais;
    private int cenourasColetadas;

    void Start()
    {
        GameObject[] cenouras = GameObject.FindGameObjectsWithTag("Cenoura");
        cenourasTotais = cenouras.Length;
        cenourasColetadas = 0;
        textoMissao.text = "Colete todas as cenouras";
    }

    public void ColetouCenoura()
    {
        cenourasColetadas++;
        if (cenourasColetadas >= cenourasTotais)
        {
            textoMissao.text = "Você esta cansado, vá dormir";
        }
    }
}
