using StarterAssets;
using UnityEngine;
using Yarn.Unity;

public class YarnInteractable : MonoBehaviour
{
    // internal properties exposed to editor
    [SerializeField] private string conversationStartNode;

    // internal properties not exposed to editor
    private DialogueRunner dialogueRunner;
    private Collider dialogueTrigger;
    private bool interactable = false;
    private bool isCurrentConversation = false;
    private ThirdPersonController playerController;
    private YarnTriggerBubble bubble;

    public void Start()
    {
        dialogueTrigger = GetComponent<Collider>();
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        playerController = FindObjectOfType<ThirdPersonController>();
        bubble = FindObjectOfType<YarnTriggerBubble>();
        StarterAssetsInputs.InteractPressed += StartConversation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bubble.Show(gameObject);
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            bubble.Hide();
            interactable = false;
        }
    }

    private void StartConversation()
    {
        if (interactable && !dialogueRunner.IsDialogueRunning)
        {
            bubble.Hide();
            Debug.Log($"Started conversation with {name}.");
            isCurrentConversation = true;
            playerController.CanMove = false;
            dialogueRunner.StartDialogue(conversationStartNode);
        }
    }

    private void EndConversation()
    {
        if (isCurrentConversation)
        {
            isCurrentConversation = false;
            playerController.CanMove = true;
            Debug.Log($"Ended conversation with {name}.");
        }
    }
}
