using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    public GameObject panel;      // Painel desativado no Inspector
    public Animator panelAnimator;

    private bool isOpen = false;

    public void TogglePanel()
    {
        if (!isOpen)
        {
            panel.SetActive(true);               // Ativa o painel
            panelAnimator.SetBool("IsOpen", true);
            isOpen = true;
        }
        else
        {
            panelAnimator.SetBool("IsOpen", false);
            isOpen = false;
        }
    }

    // Método chamado no final da animação de fechar
    public void OnCloseAnimationEnd()
    {
        panel.SetActive(false); // Desativa o painel após a animação de saída
    }
}
