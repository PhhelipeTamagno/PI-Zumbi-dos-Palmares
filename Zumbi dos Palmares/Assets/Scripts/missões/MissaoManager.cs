using UnityEngine;
using TMPro;

public class MissaoManager : MonoBehaviour
{
    public TextMeshProUGUI textoMissao;
    public AudioClip somVitoria;
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
            textoMissao.text = "Vá dormir, por favor";
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
