using UnityEngine;

public class Mouse : MonoBehaviour
{
    private bool cursorEnabled = false;

    void Start()
    {
        // Começa com o mouse bloqueado e invisível (como em jogos 2D normais)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cursorEnabled = !cursorEnabled;

            if (cursorEnabled)
            {
                // Mostra e libera o cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                // Esconde e trava o cursor no centro
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
