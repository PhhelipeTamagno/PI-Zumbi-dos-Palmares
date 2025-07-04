using UnityEngine;

public class DialogueEtapas : MonoBehaviour
{
    public Dialogue dialogue; // arraste o ScriptableObject do diálogo aqui

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
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
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
