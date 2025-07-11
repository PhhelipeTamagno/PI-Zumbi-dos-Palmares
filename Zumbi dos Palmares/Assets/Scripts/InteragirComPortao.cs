using UnityEngine;
using TMPro;

public class InteragirComPortao : MonoBehaviour
{
    [Header("Objetos do Portão")]
    public GameObject portaoFechado;
    public GameObject portaoAberto;

    [Header("UI")]
    public TextMeshProUGUI mensagemInteracao;

    [Header("Áudio")]
    public AudioClip somAbrirPortao;
    public AudioClip somFecharPortao;
    private AudioSource audioSource;

    [Header("Requisitos")]
    public int chaveItemID = 0; // ID do item "chave" no sistema de hotbar
    public HotbarController hotbar; // Referência ao HotbarController na cena

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
            if (hotbar != null && JogadorTemChave())
            {
                AlternarPortao();
            }
            else
            {
                mensagemInteracao.text = "Você precisa de uma chave para abrir o portão.";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pertoDoPortao = true;
            mensagemInteracao.gameObject.SetActive(true);

            if (hotbar != null && JogadorTemChave())
            {
                mensagemInteracao.text = "Aperte E para abrir ou fechar o portão";
            }
            else
            {
                mensagemInteracao.text = "O portão está trancado...";
            }
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
            if (somAbrirPortao != null) audioSource.PlayOneShot(somAbrirPortao);
        }
        else
        {
            portaoFechado.SetActive(true);
            portaoAberto.SetActive(false);
            if (somFecharPortao != null) audioSource.PlayOneShot(somFecharPortao);
        }
    }

    private bool JogadorTemChave()
    {
        for (int i = 0; i < hotbar.itemSlots.Length; i++)
        {
            if (hotbar.GetSelectedItemID() == chaveItemID || ExisteNoInventario(chaveItemID))
                return true;
        }
        return false;
    }

    private bool ExisteNoInventario(int id)
    {
        for (int i = 0; i < hotbar.itemSlots.Length; i++)
        {
            // Verifica se o item está em qualquer slot da hotbar
            if (hotbar.itemSlots[i].transform.Find("Icon").gameObject.activeSelf &&
                hotbar.GetSelectedItemID() == id)
                return true;
        }

        return false;
    }
}
