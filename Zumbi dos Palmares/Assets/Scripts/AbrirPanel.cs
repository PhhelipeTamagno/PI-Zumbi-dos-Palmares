using UnityEngine;

public class AbrirPanel : MonoBehaviour
{
    public GameObject panel; // Arraste o painel desejado no Inspector

    public void TogglePanel()
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive); // Alterna entre abrir/fechar o painel
        }
        else
        {
            Debug.LogWarning("Nenhum painel foi atribuído ao script!");
        }
    }
}
