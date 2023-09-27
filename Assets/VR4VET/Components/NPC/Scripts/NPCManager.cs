using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public GameObject collidingObject;
    public TextAsset data;
    
    private DialogueBoxController controller;

    float _time;

    // Start is called before the first frame update
    void Awake()
    {   
        if (data != null) {
            GetTreeFromJson(data);
        }

        controller = gameObject.GetComponent<DialogueBoxController>();
        _time = 0;
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.Equals(collidingObject)) 
        {
            controller.StartDialogue(dialogueTree, 0, "NPC");
        }
    }

    private void GetTreeFromJson(TextAsset data)
    {
        JsonUtility.FromJsonOverwrite(data.text, dialogueTree);
    }
}
