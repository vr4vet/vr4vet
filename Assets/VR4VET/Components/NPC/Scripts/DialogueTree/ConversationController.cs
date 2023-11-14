using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    //public DialogueTree dialogueTree;
    public GameObject collidingObject;

    public TextAsset data;
    
    private DialogueBoxController controller;
    // Remove this?
    public List<DialogueTree> dialogueTrees;
    private DialogueTree dialogueTree;

    private int currentElement = 0;

    private Animator animator;
    private int hasNewDialogueOptionsHash;

    // Start is called before the first frame update
    void Start()
    {   
        // Attempt to find the "XR Rig Advanced" object in the entire scene
        GameObject xrRigAdvanced = GameObject.Find("XR Rig Advanced/Inventory/HolsterRight");
        if (xrRigAdvanced != null)
        {
            // Get the collider from "HolsterRight"
            Collider holsterCollider = xrRigAdvanced.GetComponent<Collider>();
            if(holsterCollider != null)
            {
                collidingObject = xrRigAdvanced;
            }
            else
            {
                Debug.LogError("HolsterRight does not have a Collider attached!");
            }
        }
        else
        {
            Debug.LogError("No object named 'XR Rig Advanced/Inventory/HolsterRight' found in the scene!");
        }

        if (data != null) {
            GetTreeFromJson(data);
        } else {
            dialogueTree = dialogueTrees.ElementAt(currentElement);
        }

        controller = gameObject.GetComponentInParent<DialogueBoxController>();
        // TODO: test if this works
        GameObject parent = this.transform.parent.gameObject;
        Debug.Log("parent: " + parent);
        animator = parent.GetComponentInChildren<Animator>();
        Debug.Log("Animator: " + animator);
        hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
    }



    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.Equals(collidingObject) && !controller.dialogueIsActive) 
        {
            controller.StartDialogue(dialogueTree, 0, "NPC");
        }
    }

    private void GetTreeFromJson(TextAsset data)
    {
        JsonUtility.FromJsonOverwrite(data.text, dialogueTree);
    }

    public void SetDialogueTreeList(List<DialogueTree> dialogueTrees) {
        this.dialogueTrees = dialogueTrees;
        currentElement = 0;
        this.dialogueTree = dialogueTrees.ElementAt(currentElement);
        animator.SetBool(hasNewDialogueOptionsHash, true);
    }

    public void insertDialogueTreeAndChange(DialogueTree dialogueTree) {
        if (!dialogueTrees.Contains(dialogueTree)) {
            controller.ExitConversation();
            currentElement++;
            dialogueTrees.Insert(currentElement, dialogueTree);
            this.dialogueTree = dialogueTrees.ElementAt(currentElement);
            animator.SetBool(hasNewDialogueOptionsHash, true);
        } 
    }

    public void NextDialogueTree() {
        currentElement++;
        if (currentElement >= dialogueTrees.Count())
        {
            currentElement--;
        } else {
            controller.ExitConversation();
            dialogueTree = dialogueTrees.ElementAt(currentElement);
            // Should the animation observe the dialgoue tree??? To decouple stuff and have the option to add more signals
            animator.SetBool(hasNewDialogueOptionsHash, true);
        }
        
    }



}
