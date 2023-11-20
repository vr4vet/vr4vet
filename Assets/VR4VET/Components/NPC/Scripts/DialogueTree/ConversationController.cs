using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    //public DialogueTree dialogueTree;
    public GameObject collidingObject;

    public DialogueTree ToBeOverwrittenByJSON;
    
    private DialogueBoxController controller;
    
    public List<TextAsset> dialogueTreesJSONFormat;

    public List<DialogueTree> dialogueTreesSOFormat;
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
        // Join the json-version and the dialogueTree-version into one list;
        // The dialogueTree-version will be first
        JoinWithScriptableObjectList(dialogueTreesJSONFormat);
        dialogueTree = dialogueTreesSOFormat.ElementAt(currentElement);

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

    /// <summary>
    /// Start the dialogue when the Player is close enough
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.Equals(collidingObject) && !controller.dialogueIsActive) 
        {
            // string json = JsonUtility.ToJson(dialogueTree);
            // Debug.Log(json);
            controller.StartDialogue(dialogueTree, 0, "NPC");
        }
    }

    /// <summary>
    /// Join the json-version and the dialogueTree-version into one list.
    /// The dialogueTree-version will be first.
    /// </summary>
    /// <param name="dialogueTreesJSONFormat"></param>
    private void JoinWithScriptableObjectList(List<TextAsset> dialogueTreesJSONFormat)
    {
        foreach (var dialogueJSON in dialogueTreesJSONFormat)
        {
            DialogueTree temp = Instantiate(ToBeOverwrittenByJSON);
            JsonUtility.FromJsonOverwrite(dialogueJSON.text, temp);
            //DialogueTree dia = JsonUtility.FromJson<DialogueTree>(dialogueJSON.text);
            Debug.Log("Deserialized dialogue Tree: " + temp);
            dialogueTreesSOFormat.Add(temp);
        }
    }

    /// <summary>
    /// Converts a list of text assest into a list of dialogue trees
    /// </summary>
    /// <param name="dialogueTreesJSONFormat"></param>
    /// <returns>a list of dialogue trees</returns>
    private List<DialogueTree> ConvertFromJSONListToDialogueTreeList(List<TextAsset> dialogueTreesJSONFormat)
    {
        List<DialogueTree> dialogueTrees = new List<DialogueTree>();
        foreach (var dialogueJSON in dialogueTreesJSONFormat)
        {
            DialogueTree temp = Instantiate(ToBeOverwrittenByJSON);
            JsonUtility.FromJsonOverwrite(dialogueJSON.text, temp);
            //DialogueTree dia = JsonUtility.FromJson<DialogueTree>(dialogueJSON.text);
            Debug.Log("Deserialized dialogue Tree: " + temp);
            dialogueTrees.Add(temp);
        }
        return dialogueTrees;
    }

    /// <summary>
    /// Replace the list of dialogueTrees with a new one.
    /// The NPC will singal through animation that they have a new dialogue. 
    /// </summary>
    /// <param name="dialogueTrees"></param>
    public void SetDialogueTreeList(List<DialogueTree> dialogueTrees) {
        this.dialogueTreesSOFormat = dialogueTrees;
        currentElement = 0;
        this.dialogueTree = dialogueTrees.ElementAt(currentElement);
        animator.SetBool(hasNewDialogueOptionsHash, true);
    }

    /// <summary>
    /// Replace the list of dialogueTrees with a new one.
    /// The NPC will singal through animation that they have a new dialogue. 
    /// </summary>
    /// <param name="dialogueTree"></param>
    public void SetDialogueTreeList(DialogueTree dialogueTree) {
        SetDialogueTreeList(new List<DialogueTree>
        {
            dialogueTree
        });
    }

    /// <summary>
    /// Insert a new dialogue tree as the next item in the list.
    /// End the old conversation and change to the new one.
    /// The NPC will singal through animation that they have a new dialogue.
    /// </summary>
    /// <param name="dialogueTree"></param>
    public void insertDialogueTreeAndChange(DialogueTree dialogueTree) {
        if (!dialogueTreesSOFormat.Contains(dialogueTree)) {
            controller.ExitConversation();
            currentElement++;
            dialogueTreesSOFormat.Insert(currentElement, dialogueTree);
            this.dialogueTree = dialogueTreesSOFormat.ElementAt(currentElement);
            animator.SetBool(hasNewDialogueOptionsHash, true);
        } 
    }

    public void SetDialogueTreeList(List<TextAsset> dialogueTrees) {
        SetDialogueTreeList(ConvertFromJSONListToDialogueTreeList(dialogueTrees));
    }

    public void SetDialogueTreeList(TextAsset dialogueTree) {
        SetDialogueTreeList(new List<TextAsset>
        {
            dialogueTree
        });
    }

    public void insertDialogueTreeAndChange(TextAsset dialogueTree) {
        DialogueTree temp = Instantiate(ToBeOverwrittenByJSON);
        JsonUtility.FromJsonOverwrite(dialogueTree.text, temp);
        insertDialogueTreeAndChange(dialogueTree);
    }

    /// <summary>
    /// Go to the next dialogueTree.
    /// The NPC will signal through animation that the dialogue changed. 
    /// </summary>
    public void NextDialogueTree() {
        currentElement++;
        if (currentElement >= dialogueTreesSOFormat.Count())
        {
            currentElement--;
            Debug.Log("You have read the last dialogue tree");
        } else {
            controller.ExitConversation();
            dialogueTree = dialogueTreesSOFormat.ElementAt(currentElement);
            animator.SetBool(hasNewDialogueOptionsHash, true);
        }
    }

    /// <summary>
    /// Go to the prior dialogueTree.
    /// The NPC will signal through animation that the dialogue changed. 
    /// </summary>
    public void previousDialogueTree() {
        currentElement--;
        if (currentElement < 0)
        {
            currentElement=0;
            Debug.Log("You have read the first dialogue tree");
        } else {
            controller.ExitConversation();
            dialogueTree = dialogueTreesSOFormat.ElementAt(currentElement);
            animator.SetBool(hasNewDialogueOptionsHash, true);
        }
    }
}
