using UnityEngine;

public class ConsertarPonte : MonoBehaviour
{
    public Sprite ponteConsertada; // nova imagem da ponte
    public GameObject[] objetosParaDesativar; // ex: colliders da ponte
    public SpriteRenderer ponteRenderer; // SpriteRenderer da ponte

    private bool playerNaArea = false;
    private MissaoNoturnaManager missaoManager;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoNoturnaManager>();
    }

    void Update()
    {
        if (playerNaArea && Input.GetKeyDown(KeyCode.E) && missaoManager.etapaMissaoNoturna == 2)
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
