using UnityEngine;
using TMPro;

public class MissaoNoturnaManager : MonoBehaviour
{
    public TextMeshProUGUI textoMissao;
    public AudioClip somVitoria;

    public string textoColetarMadeira = "É noite. Colete toda a madeira pela área, e rápido.";
    public string textoMissaoMadeiraColetada = "Madeira coletada. Volte para a cela sem fazer barulho.";

    private int madeirasTotais;
    private int madeirasColetadas;

    public int etapaMissaoNoturna = 0;
    private bool madeiraColetada = false;

    void Start()
    {
        GameObject[] madeiras = GameObject.FindGameObjectsWithTag("Madeira");
        madeirasTotais = madeiras.Length;
        madeirasColetadas = 0;

        textoMissao.text = textoColetarMadeira;
    }

    public void MadeiraColetada()
    {
        if (etapaMissaoNoturna != 0) return;

        madeirasColetadas++;

        if (madeirasColetadas >= madeirasTotais && !madeiraColetada)
        {
            madeiraColetada = true;
            textoMissao.text = textoMissaoMadeiraColetada;
            etapaMissaoNoturna = 1;
            TocarSomVitoria();
        }
    }

    void TocarSomVitoria()
    {
        GameObject som = new GameObject("SomVitoriaNoturna");
        AudioSource audioSource = som.AddComponent<AudioSource>();
        audioSource.PlayOneShot(somVitoria);
        Destroy(som, somVitoria.length);
    }
}
