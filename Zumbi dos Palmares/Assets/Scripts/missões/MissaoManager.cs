using UnityEngine;
using TMPro;

public class MissaoManager : MonoBehaviour
{
    public TextMeshProUGUI textoMissao;
    public AudioClip somVitoria;

    public string textoFaleComBarao = "Fale com o Barão";
    public string textoColeteCenouras = "Colete todas as cenouras";
    public string textoFaleComBaraoNovamente = "Fale com o Barão novamente";
    public string textoColeteCafe = "Agora vá perto da mansão e colete os cafés. E vê se não bisbilhota por aí!";
    public string textoMissaoCafeColetado = "Cafés coletados! Agora vá para a próxima etapa.";

    private int cenourasTotais;
    private int cenourasColetadas;

    private int cafesTotais;
    private int cafesColetados;

    public int etapaMissao = 0;

    private bool cafeColetado = false;

    void Start()
    {
        GameObject[] cenouras = GameObject.FindGameObjectsWithTag("Cenoura");
        cenourasTotais = cenouras.Length;
        cenourasColetadas = 0;

        GameObject[] cafes = GameObject.FindGameObjectsWithTag("Cafe");
        cafesTotais = cafes.Length;
        cafesColetados = 0;

        textoMissao.text = textoFaleComBarao; // Começa com falar com barão
    }

    public void ColetouCenoura()
    {
        if (etapaMissao != 1) return;

        cenourasColetadas++;

        if (cenourasColetadas >= cenourasTotais)
        {
            textoMissao.text = textoFaleComBaraoNovamente;
            etapaMissao = 2;
            TocarSomVitoria();
        }
    }

    public void FalouComBarao()
    {
        if (etapaMissao == 0)
        {
            textoMissao.text = textoColeteCenouras;
            etapaMissao = 1;
        }
        else if (etapaMissao == 2)
        {
            textoMissao.text = textoColeteCafe;
            etapaMissao = 3;
        }
    }

    public void CafeColetado()
    {
        if (etapaMissao != 3) return;

        cafesColetados++;

        if (cafesColetados >= cafesTotais && !cafeColetado)
        {
            cafeColetado = true;
            textoMissao.text = textoMissaoCafeColetado;
            etapaMissao = 4;
            TocarSomVitoria();
        }
    }

    void TocarSomVitoria()
    {
        GameObject som = new GameObject("SomVitoria");
        AudioSource audioSource = som.AddComponent<AudioSource>();
        audioSource.PlayOneShot(somVitoria);
        Destroy(som, somVitoria.length);
    }
}
