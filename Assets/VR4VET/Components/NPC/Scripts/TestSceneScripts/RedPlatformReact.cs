using UnityEngine;

// This script exists only to show of how an you can change the dialogue tree of the tree  
public class RedPlatformReact : MonoBehaviour
{
    public GameObject npc;
    public DialogueTree newDialogueTree;
    private ConversationController conversationController;
    
    void Start() {
        conversationController = npc.GetComponentInChildren<ConversationController>();
    }

    void OnTriggerEnter() {
        if (!newDialogueTree.Equals(null)) {
            conversationController.insertDialogueTreeAndChange(newDialogueTree);
        }
    }
}
