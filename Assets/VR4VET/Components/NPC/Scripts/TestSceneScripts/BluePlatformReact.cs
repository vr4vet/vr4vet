using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlatformReact : MonoBehaviour
{
    public GameObject npc;
    private ConversationController conversationController;
    
    void Start() {
        conversationController = npc.GetComponentInChildren<ConversationController>();
    }

    void OnTriggerEnter() {
        conversationController.NextDialogueTree();
    }
}
