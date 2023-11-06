using UnityEngine;

// This script exists only to show of how an you can change the dialogue tree of the tree  
public class RedPlatformReact : MonoBehaviour
{
    public GameObject npc;
    public DialogueTree newDialogueTree;
    private NPCManager npcManager;
    
    void Start() {
        npcManager = npc.GetComponentInChildren<NPCManager>();
    }

    void OnTriggerEnter() {
        if (!newDialogueTree.Equals(null)) {
            npcManager.insertDialogueTreeAndChange(newDialogueTree);
        }
    }
}
