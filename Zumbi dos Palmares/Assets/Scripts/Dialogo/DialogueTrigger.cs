using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueAntesCenouras;
    public Dialogue dialogueDepoisCenouras;
    public Dialogue dialogueDepoisCafe;
    public Dialogue dialogueDepoisCana;

    public GameObject objetoParaDesativar;

    private bool playerInRange = false;
    private MissaoManager missaoManager;
    private bool jaDesativouObjeto = false;

    void Start()
    {
        missaoManager = FindObjectOfType<MissaoManager>();
    }

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.E) || MobileInteractButton.pressed))
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
