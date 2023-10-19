using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformReact : MonoBehaviour
{
    public GameObject npc;
    public DialogueTree newDialogueTree;
    private NPCManager npcManager;
    
    void Start() {
        //npc = GameObject.Find("NPC");
        npcManager = npc.GetComponentInChildren<NPCManager>();
    }

    void OnTriggerEnter() {
        Debug.Log("You entered my zone");
        Debug.Log(npc);
        Debug.Log(newDialogueTree);
        Debug.Log(npcManager);
        npcManager.setDialogueTree(newDialogueTree);
        
    }

    void OnTriggerStay() {
        Debug.Log("Stay slay");
    }

    void OnTriggerExit() {
        Debug.Log("Bye");
    }
}
