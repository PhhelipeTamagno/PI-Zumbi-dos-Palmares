using UnityEngine;
using TMPro; 

public class InteragirComPortao : MonoBehaviour
{
    public GameObject portaoFechado;  
    public GameObject portaoAberto;   
    public TextMeshProUGUI mensagemInteracao; 

    public AudioClip somAbrirPortao;   
    public AudioClip somFecharPortao;  
    private AudioSource audioSource;    

    private bool pertoDoPortao = false; 

    void Start()
    {
        
        mensagemInteracao.gameObject.SetActive(false);

        
        portaoFechado.SetActive(true); 
        portaoAberto.SetActive(false); 

       
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if (pertoDoPortao && Input.GetKeyDown(KeyCode.E))
        {
            AlternarPortao();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            pertoDoPortao = true;
            mensagemInteracao.gameObject.SetActive(true); 
            mensagemInteracao.text = "Aperte E para abrir ou fechar o portão";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            pertoDoPortao = false;
            mensagemInteracao.gameObject.SetActive(false); 
        }
    }

    private void AlternarPortao()
    {
        
        if (portaoFechado.activeSelf)
        {
            
            portaoFechado.SetActive(false);
            portaoAberto.SetActive(true);
            Debug.Log("Portão aberto!");

            
            if (somAbrirPortao != null && audioSource != null)
            {
                audioSource.PlayOneShot(somAbrirPortao);
            }
        }
        else
        {
            
            portaoFechado.SetActive(true);
            portaoAberto.SetActive(false);
            Debug.Log("Portão fechado!");

            
            if (somFecharPortao != null && audioSource != null)
            {
                audioSource.PlayOneShot(somFecharPortao);
            }
        }
    }
}
