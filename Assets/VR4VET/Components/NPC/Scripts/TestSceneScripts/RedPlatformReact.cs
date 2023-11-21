using UnityEngine;

// This script exists only to show of how an you can change the dialogue tree of the tree  
public class RedPlatformReact : MonoBehaviour
{
    public EventTriggerDemo eventTrigger;
    private GameObject taskNPC;
    [SerializeField] public DialogueTree[] dialogueTrees; 

    void OnTriggerEnter(Collider other) {
        
        if (!taskNPC) {
            Debug.Log("RedPlatformReact is triggered");
            taskNPC = eventTrigger.SpawnTaskNPC();
        }
    }
}
