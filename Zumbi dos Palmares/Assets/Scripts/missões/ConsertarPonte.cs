using UnityEngine;

public class ConsertarPonte : MonoBehaviour
{
    public Sprite ponteConsertada;
    public GameObject[] objetosParaDesativar;
    public SpriteRenderer ponteRenderer;

    private bool playerNaArea = false;
    private MissaoNoturnaManager missaoManager;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoNoturnaManager>();

        // Registrar evento do botão mobile
        if (InteractionButton.Instance != null)
            InteractionButton.Instance.onInteractionPressed += InteragirMobile;
    }

    void OnDestroy()
    {
        if (InteractionButton.Instance != null)
            InteractionButton.Instance.onInteractionPressed -= InteragirMobile;
    }

    void Update()
    {
        if (playerNaArea && Input.GetKeyDown(KeyCode.E))
        {
            TentarConsertar();
        }
    }

    // --- MOBILE ---
    void InteragirMobile()
    {
        if (playerNaArea)
            TentarConsertar();
    }

    void TentarConsertar()
    {
        if (missaoManager.etapaMissaoNoturna == 2)
        {
            ponteRenderer.sprite = ponteConsertada;

            foreach (GameObject obj in objetosParaDesativar)
                obj.SetActive(false);

            missaoManager.textoMissao.text = "Ponte consertada. Siga em frente.";
            missaoManager.etapaMissaoNoturna = 3;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNaArea = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNaArea = false;
    }
}
