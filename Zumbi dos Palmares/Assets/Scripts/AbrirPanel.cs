using UnityEngine;

public class AbrirPanel : MonoBehaviour
{
    public GameObject panel; 
    public void TogglePanel()
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive); 
        }
        else
        {
            Debug.LogWarning("Nenhum painel foi atribuído ao script!");
        }
    }
}
