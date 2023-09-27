using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public GameObject collidingObject;
    
    private DialogueBoxController controller;

    float _time;

    // Start is called before the first frame update
    void Start()
    {   
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

    // Update is called once per frame
    void Update()
    {
    }
}
