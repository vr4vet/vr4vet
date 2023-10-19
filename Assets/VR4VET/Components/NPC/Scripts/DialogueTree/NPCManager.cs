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

    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.Equals(collidingObject)) 
        {
            controller.StartDialogue(dialogueTree, 0, "NPC");
            JsonUtility.ToJson(data, true);
            Debug.Log(JsonUtility.ToJson(data, true).ToString());
        }
    }

    private void GetTreeFromJson(TextAsset data)
    {
        JsonUtility.FromJsonOverwrite(data.text, dialogueTree);
    }

    public void setDialogueTree(DialogueTree dialogueTree) {
        this.dialogueTree = dialogueTree;
    } 

}
