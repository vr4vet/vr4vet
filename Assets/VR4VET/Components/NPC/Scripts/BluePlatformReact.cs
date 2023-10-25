using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlatformReact : MonoBehaviour
{
    public GameObject npc;
    private NPCManager npcManager;
    
    void Start() {
        npcManager = npc.GetComponentInChildren<NPCManager>();
    }

    void OnTriggerEnter() {
        npcManager.NextDialogueTree();
    }
}
