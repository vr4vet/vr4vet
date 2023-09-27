using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public GameObject collidingObject;
    
    private DialogueBoxController controller;

    // Start is called before the first frame update
    void Start()
    {   
        controller = gameObject.GetComponent<DialogueBoxController>();
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.Equals(collidingObject)) 
        {
            controller.StartDialogue(dialogueTree, 0, "NPC");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
