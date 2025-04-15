using UnityEngine;

public class TextoQueSobe : MonoBehaviour
{
    public RectTransform textoTransform;  
    public Transform pontoReset;          
    public float velocidade = 30f;        

    void Start()
    {
        ResetarTexto();  
    }

    void Update()
    {
       
        textoTransform.anchoredPosition += Vector2.up * velocidade * Time.deltaTime;
    }

    public void ResetarTexto()
    {
       
        Vector2 novaPosicao;
        RectTransform canvasRect = textoTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Camera.main.WorldToScreenPoint(pontoReset.position),
            Camera.main,
            out novaPosicao
        );

        textoTransform.anchoredPosition = novaPosicao; 
    }
}
