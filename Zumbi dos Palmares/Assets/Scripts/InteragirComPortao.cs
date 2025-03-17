using UnityEngine;
using TMPro; // Para usar o TextMeshPro

public class InteragirComPortao : MonoBehaviour
{
    public GameObject portaoFechado;  // O GameObject do portão fechado
    public GameObject portaoAberto;   // O GameObject do portão aberto (que aparece depois)
    public TextMeshProUGUI mensagemInteracao; // A mensagem que será exibida para interação

    public AudioClip somAbrirPortao;   // Som para abrir o portão
    public AudioClip somFecharPortao;  // Som para fechar o portão
    private AudioSource audioSource;    // Componente AudioSource para tocar os sons

    private bool pertoDoPortao = false; // Verifica se o jogador está perto do portão

    void Start()
    {
        // Inicialmente, a mensagem deve estar invisível
        mensagemInteracao.gameObject.SetActive(false);

        // Inicialize o estado inicial dos portões
        portaoFechado.SetActive(true); // O portão fechado está ativo no começo
        portaoAberto.SetActive(false); // O portão aberto está invisível no começo

        // Inicializa o AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Se o jogador estiver perto e apertar "E", abre ou fecha o portão
        if (pertoDoPortao && Input.GetKeyDown(KeyCode.E))
        {
            AlternarPortao();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Quando o jogador entra na área de interação, exibe a mensagem
        if (other.CompareTag("Player"))
        {
            pertoDoPortao = true;
            mensagemInteracao.gameObject.SetActive(true); // Ativa a mensagem
            mensagemInteracao.text = "Aperte E para abrir ou fechar o portão";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Quando o jogador sai da área de interação, esconde a mensagem
        if (other.CompareTag("Player"))
        {
            pertoDoPortao = false;
            mensagemInteracao.gameObject.SetActive(false); // Desativa a mensagem
        }
    }

    private void AlternarPortao()
    {
        // Alterna entre o portão fechado e o portão aberto
        if (portaoFechado.activeSelf)
        {
            // Se o portão fechado está ativo, fecha o portão e abre o outro
            portaoFechado.SetActive(false);
            portaoAberto.SetActive(true);
            Debug.Log("Portão aberto!");

            // Toca o som de abrir o portão
            if (somAbrirPortao != null && audioSource != null)
            {
                audioSource.PlayOneShot(somAbrirPortao);
            }
        }
        else
        {
            // Se o portão aberto está ativo, fecha o portão e abre o outro
            portaoFechado.SetActive(true);
            portaoAberto.SetActive(false);
            Debug.Log("Portão fechado!");

            // Toca o som de fechar o portão
            if (somFecharPortao != null && audioSource != null)
            {
                audioSource.PlayOneShot(somFecharPortao);
            }
        }
    }
}
