using UnityEngine;

public class DialogueEtapas : MonoBehaviour
{
    [Header("Configurações de Diálogo")]
    public Dialogue dialogue;

    [Header("UI")]
    public GameObject teclaEIcon;

    private bool playerInRange = false;

    void Start()
    {
        if (teclaEIcon != null)
            teclaEIcon.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.E) || MobileInteractButton.pressed))
        {
            var dialogueManager = FindObjectOfType<DialogueManager>();
            if (dialogueManager != null && dialogue != null)
            {
                dialogueManager.StartDialogue(dialogue);
            }
            else
            {
                Debug.LogWarning("DialogueManager ou Dialogue não atribuídos.");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (teclaEIcon != null)
                teclaEIcon.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (teclaEIcon != null)
                teclaEIcon.SetActive(false);
        }
    }
}
