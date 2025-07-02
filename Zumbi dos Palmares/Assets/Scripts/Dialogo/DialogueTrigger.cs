using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueAntesCenouras;
    public Dialogue dialogueDepoisCenouras;
    public Dialogue dialogueDepoisCafe;        // diálogo após café (barão fala para ir coletar cana)
    public Dialogue dialogueDepoisCana;        // diálogo após cana (barão fala para ir dormir)

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
            else if (etapa == 4)
            {
                FindObjectOfType<DialogueManager>()?.StartDialogue(dialogueDepoisCafe);
                missaoManager.FalouComBarao();
                DesativarObjetoSeNecessario();
            }
            else if (etapa == 6)
            {
                FindObjectOfType<DialogueManager>()?.StartDialogue(dialogueDepoisCana);
                missaoManager.FalouComBarao();
                DesativarObjetoSeNecessario();
            }
        }
    }

    void DesativarObjetoSeNecessario()
    {
        if (!jaDesativouObjeto && objetoParaDesativar != null)
        {
            objetoParaDesativar.SetActive(false);
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
