using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels;
    private GameObject painelAtualAberto = null;

    public void AbrirPainel(int index)
    {
        if (painelAtualAberto != null)
        {
            painelAtualAberto.SetActive(false);
        }

        panels[index].SetActive(true);
        painelAtualAberto = panels[index];
    }
}
