using UnityEngine;

// This script exists only to show of how an you can change the dialogue tree of the tree  
public class RedPlatformReact : MonoBehaviour
{
    public EventTriggerDemo eventTrigger;
    private GameObject taskNPC;
    [SerializeField] public DialogueTree[] dialogueTrees; 

    private ConversationController conversationController;
    

    void OnTriggerEnter(Collider other) {
        // print the name of the object that collided with the trigger
        Rigidbody otherRigidbody = other.attachedRigidbody;

        if (otherRigidbody != null) {
            // If there is a Rigidbody, print the name of the GameObject it's attached to
            Debug.Log("Rigidbody GameObject Name: " + otherRigidbody.gameObject.name);
        } else {
            // If there is no Rigidbody, you can still print the name of the collider's GameObject
            Debug.Log("Collider GameObject Name: " + other.gameObject.name);
        }
        if (!taskNPC) {
            Debug.Log("RedPlatformReact is triggered");
            taskNPC = eventTrigger.SpawnTaskNPC();
            // conversationController = taskNPC.GetComponentInChildren<ConversationController>();
            // conversationController?.insertDialogueTreeAndChange(dialogueTrees[0]);
        }
    }
}
