using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Adicione esta linha no início do seu script

public class DisableButtonNavigation : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Cria um objeto PointerEventData para armazenar a posição do mouse
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition // A posição do mouse
            };

            // Realiza um Raycast para verificar qual objeto o mouse está em cima
            RaycastResult raycastResult = pointerData.pointerCurrentRaycast;

            // Verifica se o objeto sobre o qual o mouse está é um botão com a tag "Botao"
            GameObject buttonHit = raycastResult.gameObject;

            if (buttonHit != null && buttonHit.CompareTag("Botao")) // Se o botão for válido e tiver a tag "Botao"
            {
                Button button = buttonHit.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke(); // Simula o clique no botão
                }
            }
        }
    }
}
