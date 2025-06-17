using UnityEngine;
using TMPro;

public class MissaoManager : MonoBehaviour
{
    public TextMeshProUGUI textoMissao;
    public AudioClip somVitoria;

    private int cenourasTotais;
    private int cenourasColetadas;
    public int etapaMissao = 0; // 0: fale com Barão, 1: coletar, 2: falar dnv

    void Start()
    {
        GameObject[] cenouras = GameObject.FindGameObjectsWithTag("Cenoura");
        cenourasTotais = cenouras.Length;
        cenourasColetadas = 0;
        textoMissao.text = "Fale com o Barão";
    }

    public void ColetouCenoura()
    {
        if (etapaMissao != 1) return;

        cenourasColetadas++;

        if (cenourasColetadas >= cenourasTotais)
        {
            textoMissao.text = "Fale com o Barão novamente";
            etapaMissao = 2;
            TocarSomVitoria();
        }
    }

    public void FalouComBarao()
    {
        if (etapaMissao == 0)
        {
            textoMissao.text = "Colete todas as cenouras";
            etapaMissao = 1;
        }
        else if (etapaMissao == 2)
        {
            textoMissao.text = "Vá para sua cela dormir";
            etapaMissao = 3;
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
