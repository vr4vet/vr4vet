using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformReact : MonoBehaviour
{
    public GameObject npc;
    
    void Start() {
        npc = GameObject.Find("NPC");
    }

    void OnTriggerEnter() {
        Debug.Log("You entered my zone");
        
    }

    void OnTriggerStay() {
        Debug.Log("Stay slay");
    }

    void OnTriggerExit() {
        Debug.Log("Bye");
    }
}
