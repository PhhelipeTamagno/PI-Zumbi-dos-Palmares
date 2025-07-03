using UnityEngine;
using TMPro;

public class MissaoManager : MonoBehaviour
{
    public TextMeshProUGUI textoMissao;
    public AudioClip somVitoria;

    public string textoFaleComBarao = "Fale com o Barão";
    public string textoColeteCenouras = "Colete todas as cenouras";
    public string textoFaleComBaraoNovamente = "Fale com o Barão novamente";
    public string textoColeteCafe = "Agora vá perto da mansão e colete os cafés.";
    public string textoMissaoCafeColetado = "Fale com o Barão novamente.";
    public string textoColeteCana = "Agora vá até o barco e colete as canas de açúcar.";
    public string textoMissaoCanaColetada = "Boa! Pode descansar agora.";

    private int cenourasTotais;
    private int cenourasColetadas;

    private int cafesTotais;
    private int cafesColetados;

    private int canasTotais;
    private int canasColetadas;

    public int etapaMissao = 0;

    private bool cafeColetado = false;
    private bool canaColetada = false;

    public GameObject objetoParaAtivarAposMissoes;

    void Start()
    {
        GameObject[] cenouras = GameObject.FindGameObjectsWithTag("Cenoura");
        cenourasTotais = cenouras.Length;
        cenourasColetadas = 0;

        GameObject[] cafes = GameObject.FindGameObjectsWithTag("Cafe");
        cafesTotais = cafes.Length;
        cafesColetados = 0;

        GameObject[] canas = GameObject.FindGameObjectsWithTag("Cana");
        canasTotais = canas.Length;
        canasColetadas = 0;

        textoMissao.text = textoFaleComBarao;
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
        else if (etapaMissao == 4)
        {
            textoMissao.text = textoColeteCana;
            etapaMissao = 5;
        }
        else if (etapaMissao == 6)
        {
            textoMissao.text = "Volte para sua cela. E nem pense em sair andando de noite.";

            if (objetoParaAtivarAposMissoes != null)
            {
                objetoParaAtivarAposMissoes.SetActive(true);
            }
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

    public void CanaColetada()
    {
        if (etapaMissao != 5) return;

        canasColetadas++;

        if (canasColetadas >= canasTotais && !canaColetada)
        {
            canaColetada = true;
            textoMissao.text = textoMissaoCanaColetada;
            etapaMissao = 6;
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
