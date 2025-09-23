using UnityEngine;
using TMPro; // se você estiver usando TextMeshPro

public class TextoQueSobe : MonoBehaviour
{
    [Header("Configurações")]
    public float velocidade = 1f; // Velocidade de subida
    public float tempoDeVida = 2f; // Quanto tempo o texto fica na tela antes de ser destruído

    private Vector3 direcao;

    void Start()
    {
        direcao = Vector3.up; // sobe para cima
        Destroy(gameObject, tempoDeVida); // destrói o texto depois do tempo definido
    }

    void Update()
    {
        transform.position += direcao * velocidade * Time.deltaTime;
    }

    // Função opcional para trocar o texto por código
    public void DefinirTexto(string novoTexto)
    {
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = novoTexto;
    }
}
