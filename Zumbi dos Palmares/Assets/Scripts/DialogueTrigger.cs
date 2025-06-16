using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueAntesCenouras;
    public Dialogue dialogueDepoisCenouras;

    private bool playerInRange = false;
    private MissaoManager missaoManager;

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
            }
            else if (etapa == 2)
            {
                FindObjectOfType<DialogueManager>()?.StartDialogue(dialogueDepoisCenouras);
                missaoManager.FalouComBarao();
            }
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
