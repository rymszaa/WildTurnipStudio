using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;
    public GameObject pressE_UI;

    private string[] currentLines;
    private int currentIndex;
    private bool isTalking;

    void Update()
    {
        if (!isTalking) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    public void StartDialogue(string[] lines)
    {
        currentLines = lines;
        currentIndex = 0;
        isTalking = true;

        dialoguePanel.SetActive(true);
        pressE_UI.SetActive(false);
        dialogueText.text = currentLines[currentIndex];
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = currentLines[currentIndex];
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        isTalking = false;
    }

    public bool IsTalking()
    {
        return isTalking;
    }
}