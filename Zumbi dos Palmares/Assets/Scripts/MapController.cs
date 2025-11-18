using UnityEngine;

public class MapController : MonoBehaviour
{
    // A referência ao Painel do Mapa na UI (MapPanel)
    public GameObject mapPanel;

    // A função é chamada a cada frame
    void Update()
    {
        // Verifica se a tecla 'Tab' foi pressionada **neste frame**
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Alterna o estado de Ativo/Inativo do painel
            ToggleMap();
        }
    }

    // Função para ligar e desligar o painel
    void ToggleMap()
    {
        // Se o painel está **ativo**, desativa-o. Se está **inativo**, ativa-o.
        bool isActive = mapPanel.activeSelf;
        mapPanel.SetActive(!isActive);

        // Opcional: Pausar o jogo quando o mapa está aberto (comentado)
        // Time.timeScale = mapPanel.activeSelf ? 0f : 1f;

        // Opcional: Travar ou Destravar o cursor do mouse
        if (mapPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None; // Destrava o cursor
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Trava o cursor (típico em jogos)
            Cursor.visible = false;
        }
    }
}