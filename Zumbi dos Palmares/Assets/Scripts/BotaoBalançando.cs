using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoBalançando : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float anguloMaximo = 10f;         // Quanto o botão gira para cada lado
    public float velocidadeRotacao = 2f;     // Velocidade da oscilação
    public float aumentoEscala = 1.2f;       // Quanto aumenta de tamanho no hover
    public float velocidadeEscala = 5f;      // Quão rápido muda o tamanho

    private RectTransform rectTransform;
    private bool mouseEmCima = false;
    private Vector3 escalaOriginal;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        escalaOriginal = rectTransform.localScale;
    }

    void Update()
    {
        if (!mouseEmCima)
        {
            float angulo = Mathf.Sin(Time.time * velocidadeRotacao) * anguloMaximo;
            rectTransform.rotation = Quaternion.Euler(0, 0, angulo);

            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, escalaOriginal, Time.deltaTime * velocidadeEscala);
        }
        else
        {
            rectTransform.rotation = Quaternion.Lerp(rectTransform.rotation, Quaternion.identity, Time.deltaTime * velocidadeRotacao);
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, escalaOriginal * aumentoEscala, Time.deltaTime * velocidadeEscala);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEmCima = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseEmCima = false;
    }
}
