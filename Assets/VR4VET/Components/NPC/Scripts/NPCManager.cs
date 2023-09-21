using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public GameObject collidingObject;
    
    private DialogueBoxController controller;

    // Start is called before the first frame update
    void Start()
    {   
        controller = gameObject.AddComponent<DialogueBoxController>();
        controller.StartDialogue(dialogueTree, 0, "NPC");
    }

    private void OnTriggerEnter(Collider other)
    {   

        
        if (other.gameObject.Equals(collidingObject)) 
        {
            Debug.Log(dialogueTree.sections[0].dialogue[0]);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
