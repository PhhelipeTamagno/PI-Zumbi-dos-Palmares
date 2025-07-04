using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Image npcImage; // Imagem do NPC
    public Button nextButton; // Botão "Próximo"
    public Button closeButton; // Botão "Sair"

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);

        nextButton.onClick.AddListener(DisplayNextSentence);
        closeButton.onClick.AddListener(CloseDialogueManually);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        nameText.text = dialogue.characterName;
        npcImage.sprite = dialogue.npcSprite;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f); // Pequeno atraso por letra
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public void CloseDialogueManually()
    {
        dialoguePanel.SetActive(false);
        sentences.Clear();
    }
}
