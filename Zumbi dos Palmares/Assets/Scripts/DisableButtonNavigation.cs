using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class DisableButtonNavigation : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition 
            };

           
            RaycastResult raycastResult = pointerData.pointerCurrentRaycast;

           
            GameObject buttonHit = raycastResult.gameObject;

            if (buttonHit != null && buttonHit.CompareTag("Botao")) 
            {
                Button button = buttonHit.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke(); 
                }
            }
        }
    }
}
