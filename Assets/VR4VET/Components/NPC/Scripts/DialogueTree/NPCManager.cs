using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public GameObject collidingObject;
    public TextAsset data;
    
    private DialogueBoxController controller;

    // Start is called before the first frame update
    void Awake()
    {   
        if (data != null) {
            GetTreeFromJson(data);
        }

        controller = gameObject.GetComponentInParent<DialogueBoxController>();
        Debug.Log(controller);
    }

    // TODO: Clean up
    private void OnTriggerEnter(Collider other)
    {   
        Debug.Log("We have entered the zone");
        if (other.gameObject.Equals(collidingObject)) 
        {
            Debug.Log("The dialog should start now");
            controller.StartDialogue(dialogueTree, 0, "NPC");
        }
    }

    // TODO: Remove
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("We have left the zone");
    }

    private void GetTreeFromJson(TextAsset data)
    {
        JsonUtility.FromJsonOverwrite(data.text, dialogueTree);
    }
}
