using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
        updateAnimator();
    }

    public void updateAnimator()
    {
        GameObject parent = this.transform.parent.gameObject;
        Debug.Log("parent: " + parent);
        animator = parent.GetComponentInChildren<Animator>();
        // animator = GetComponentInParent<Animator>();
        Debug.Log("Animator: " + animator);
        hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
    }

    public void updateAnimator(Animator animator) {
        this.animator = animator;
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

    public void SetDialogueTreeList(DialogueTree dialogueTree) {
        SetDialogueTreeList(new List<DialogueTree>
        {
            dialogueTree
        });
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
            Debug.Log("You have read the last dialogue tree");
        } else {
            controller.ExitConversation();
            dialogueTree = dialogueTrees.ElementAt(currentElement);
            animator.SetBool(hasNewDialogueOptionsHash, true);
        }
    }

    public void previousDialogueTree() {
        currentElement--;
        if (currentElement < 0)
        {
            currentElement=0;
            Debug.Log("You have read the first dialogue tree");
        } else {
            controller.ExitConversation();
            dialogueTree = dialogueTrees.ElementAt(currentElement);
            animator.SetBool(hasNewDialogueOptionsHash, true);
        }
    }
}
