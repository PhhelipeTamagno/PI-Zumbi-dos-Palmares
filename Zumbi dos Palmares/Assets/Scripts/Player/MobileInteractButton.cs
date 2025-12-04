using UnityEngine;

public class MobileInteractButton : MonoBehaviour
{
    // Variável global que funciona como "pressionou E"
    public static bool pressed = false;

    public void Press()
    {
        pressed = true;
    }

    void LateUpdate()
    {
        // Reset a cada frame (igual GetKeyDown)
        pressed = false;
    }
}
