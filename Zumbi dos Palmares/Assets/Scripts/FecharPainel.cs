using UnityEngine;

public class FecharPainel : MonoBehaviour
{
    public GameObject painelParaFechar; 

    public void Fechar()
    {
        painelParaFechar.SetActive(false); 
        gameObject.SetActive(false);        
    }
}
