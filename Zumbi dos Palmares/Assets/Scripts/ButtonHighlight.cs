using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public GameObject arrows; // a arte com as duas setas

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (arrows != null) arrows.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (arrows != null) arrows.SetActive(false);
    }

    // Quando o botão é selecionado via teclado/controle
    public void OnSelect(BaseEventData eventData)
    {
        if (arrows != null) arrows.SetActive(true);
    }

    // Quando o botão perde a seleção
    public void OnDeselect(BaseEventData eventData)
    {
        if (arrows != null) arrows.SetActive(false);
    }
}
