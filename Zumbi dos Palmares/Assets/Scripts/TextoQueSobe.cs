using UnityEngine;

public class TextoQueSobe : MonoBehaviour
{
    public RectTransform textoTransform;  // O RectTransform do texto
    public Transform pontoReset;          // GameObject vazio que define o ponto de reset
    public float velocidade = 30f;        // Velocidade do movimento do texto

    void Start()
    {
        ResetarTexto();  // Posiciona o texto no início quando o jogo começar
    }

    void Update()
    {
        // Faz o texto subir
        textoTransform.anchoredPosition += Vector2.up * velocidade * Time.deltaTime;
    }

    public void ResetarTexto()
    {
        // Converte a posição do GameObject vazio (em mundo) para a posição no RectTransform
        Vector2 novaPosicao;
        RectTransform canvasRect = textoTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Camera.main.WorldToScreenPoint(pontoReset.position),
            Camera.main,
            out novaPosicao
        );

        textoTransform.anchoredPosition = novaPosicao; // Resetando a posição do texto
    }
}
