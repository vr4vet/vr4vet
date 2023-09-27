using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public GameObject collidingObject;
    public TextAsset data;
    
    private DialogueBoxController controller;

    // Start is called before the first frame update
    void Start()
    {    
        GetTreeFromJson(data);
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

    private void GetTreeFromJson(TextAsset data) 
    {
        JsonUtility.FromJsonOverwrite(data.text, dialogueTree);
    }
}
