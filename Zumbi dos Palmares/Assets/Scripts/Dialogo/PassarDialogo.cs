using UnityEngine;

public class PassarDialogo : MonoBehaviour
{
    public Dialogue dialogue; // ScriptableObject com os dados do diálogo

    private DialogueManager dialogueManager;

    private void Start()
    {
        // Pega o DialogueManager presente na cena
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager não encontrado na cena!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogue != null && dialogueManager != null)
            {
                dialogueManager.StartDialogue(dialogue);
            }
        }
    }
}
