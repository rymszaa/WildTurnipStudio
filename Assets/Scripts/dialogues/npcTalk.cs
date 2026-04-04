using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    public string[] dialogueLines;

    public GameObject pressE_UI;
    public DialogueManager dialogueManager;
    private bool hasTalked = false;

    private bool inRange;

    void Start()
    {
        pressE_UI.SetActive(false);
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E) && !dialogueManager.IsTalking() && !hasTalked)
        {
            dialogueManager.StartDialogue(dialogueLines);
            hasTalked = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            pressE_UI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            pressE_UI.SetActive(false);
            hasTalked = false;
        }
    }
}