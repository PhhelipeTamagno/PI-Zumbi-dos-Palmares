using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueAntesCenouras;
    public Dialogue dialogueDepoisCenouras;

    public GameObject objetoParaDesativar; // <- objeto com o BoxCollider2D

    private bool playerInRange = false;
    private MissaoManager missaoManager;
    private bool jaDesativouObjeto = false;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoManager>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            int etapa = missaoManager.etapaMissao;

            if (etapa == 0)
            {
                FindObjectOfType<DialogueManager>()?.StartDialogue(dialogueAntesCenouras);
                missaoManager.FalouComBarao();
                DesativarObjetoSeNecessario();
            }
            else if (etapa == 2)
            {
                FindObjectOfType<DialogueManager>()?.StartDialogue(dialogueDepoisCenouras);
                missaoManager.FalouComBarao();
                DesativarObjetoSeNecessario();
            }
        }
    }

    void DesativarObjetoSeNecessario()
    {
        if (!jaDesativouObjeto && objetoParaDesativar != null)
        {
            objetoParaDesativar.SetActive(false); // ou só desativa o collider se preferir
            // objetoParaDesativar.GetComponent<BoxCollider2D>().enabled = false;
            jaDesativouObjeto = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}
