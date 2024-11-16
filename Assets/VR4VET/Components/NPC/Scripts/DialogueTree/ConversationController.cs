using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    [SerializeField] private DialogueTree _toBeOverwrittenByJSON; // Josn override need an example of an dialogue tree, to turn something from JSON to DialogueTree
    [SerializeField] private List<TextAsset> _dialogueTreesJSONFormat; // list of JSON, will be added to the list below
    [SerializeField] private List<DialogueTree> _dialogueTreesSOFormat; // lsit of ScriptableObjects, JSON will be turned into SO, and added here. Yhis is the list we work with
    [HideInInspector] private DialogueTree _dialogueTree; // The active dialogue
    [HideInInspector] private DialogueTree _oldDialogueTree; // The previous dialogue, for checking if we are on same dialogue
    [HideInInspector] private int _currentElement = 0; // The element number of the active dialogue
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _hasNewDialogueOptionsHash;
    [HideInInspector] private DialogueBoxController _dialogueBoxController;
    [SerializeField] public AIConversationController _AIConversationController;

    [HideInInspector] public bool playerInsideTrigger = false;

    [HideInInspector] public bool isTalkable;  


    // Start is called before the first frame update
    void Start()
    {
        _AIConversationController = GetComponentInParent<AIConversationController>();
        if (_AIConversationController == null)
        {
            Debug.Log("AIConversationController component not found. Add it if the NPC needs AI abilities.");
        }
        _dialogueBoxController = GetComponentInParent<DialogueBoxController>();
        if (_dialogueBoxController == null)
        {
            Debug.LogError("The NPC is missing the DialogueBoxController script");
        }
        // Join the json-version and the dialogueTree-version into one list;
        // The dialogueTree-version will be first
        JoinWithScriptableObjectList(_dialogueTreesJSONFormat);
        if (_dialogueTreesSOFormat.Count > 0)
        {
            _dialogueTree = _dialogueTreesSOFormat.ElementAt(_currentElement);
        }
        updateAnimator();
    }

    public DialogueTree GetDialogueTree() 
    {
        return this._dialogueTree;
    }
    void Update()
    {
        // This code is for testing AI speech solution on computer with a keyboard using "E"
        isTalkable = _dialogueBoxController.isTalkable;
        if (playerInsideTrigger && isTalkable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _AIConversationController.StartRecording();
                _dialogueBoxController.startThinking();
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                _AIConversationController.EndRecording();
            }
        }
    }

    public void updateAnimator()
    {
        GameObject parent = this.transform.parent.gameObject;
        _animator = parent.GetComponentInChildren<Animator>();
        if (_animator == null)
        {
            Debug.LogError("THe NPC is missing the Animator");
            return;
        }
    }

    public void updateAnimator(Animator animator)
    {
        this._animator = animator;
    }

    public bool isDialogueActive()
    {
        return _dialogueBoxController.dialogueIsActive;
    }

    public int GetActivatedCount()
    {
        return _dialogueBoxController.GetActivatedCount();
    }
    /// <summary>
    /// Start the dialogue when the Player is close enough
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.Equals(NPCToPlayerReferenceManager.Instance.PlayerCollider))
        {
            playerInsideTrigger = true;
            Debug.Log("Inside range to talk.");

            // Show dialogue box
            _dialogueBoxController.ShowDialogueBox();
            if (_dialogueTree.shouldTriggerOnProximity && !_dialogueBoxController.dialogueIsActive && _oldDialogueTree != _dialogueTree)
            {
                _oldDialogueTree = _dialogueTree;
                if (_dialogueTree != null)
                {
                    _dialogueBoxController.StartDialogue(_dialogueTree, 0, "NPC");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.Equals(NPCToPlayerReferenceManager.Instance.PlayerCollider))
        {
            playerInsideTrigger = false;
            Debug.Log("Outside range to talk.");
            _dialogueBoxController.HideDialogueBox();
        }
    }



    /// <summary>
    /// Method to trigger dialogue
    /// Should be connected to event of your choosing
    /// Only triggers if there is a new dialogue tree
    /// </summary>
    public void DialogueTrigger()
    {
        if (_oldDialogueTree != _dialogueTree)
        {
            // Change the old tree to be the current tree, to ensure no repeats
            _oldDialogueTree = _dialogueTree;
            if (_dialogueTree != null)
            {
                _dialogueBoxController.StartDialogue(_dialogueTree, 0, "NPC");
            }
            else
            {
                Debug.LogError("The dialogueTree of the NPC is null");
            }
        }
    }

    /// <summary>
    ///  Method to tigger dialogue
    ///  Should be connected to event of your choosing
    ///  Triggers no matter if the same tree has been triggered before
    /// </summary>
    public void DialogueTriggerAbsolute()
    {
        if (_dialogueTree != null)
        {
            _dialogueBoxController.StartDialogue(_dialogueTree, 0, "NPC");
        }
        else
        {
            Debug.LogError("The dialogueTree of the NPC is null");
        }
    }

    /// <summary>
    /// Method to trigger a comment
    /// Comments are dialogue without a dialogue box
    /// Comment content is the current dialogue tree
    /// Use different sections for different comments
    /// Triggered on call with no prerequesites
    /// </summary>
    public void CommentTrigger(int section = 0)
    {
        if (_dialogueTree != null)
        {
            _dialogueBoxController.StartComment(_dialogueTree, section, "NPC");
        }
        else
        {
            Debug.LogError("The dialogueTree of the NPC is null (COMMENT)");
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
            DialogueTree temp = Instantiate(_toBeOverwrittenByJSON);
            JsonUtility.FromJsonOverwrite(dialogueJSON.text, temp);
            _dialogueTreesSOFormat.Add(temp);
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
            DialogueTree temp = Instantiate(_toBeOverwrittenByJSON);
            JsonUtility.FromJsonOverwrite(dialogueJSON.text, temp);
            dialogueTrees.Add(temp);
        }
        return dialogueTrees;
    }

    /// <summary>
    /// Replace the list of dialogueTrees with a new one.
    /// The NPC will singal through animation that they have a new dialogue. 
    /// </summary>
    /// <param name="dialogueTrees"></param>
    public void SetDialogueTreeList(List<DialogueTree> dialogueTrees)
    {
        _dialogueBoxController?.ExitConversation();
        this._dialogueTreesSOFormat = dialogueTrees;
        _currentElement = 0;
        if (_dialogueTreesSOFormat.Count > 0)
        {
            _dialogueTree = _dialogueTreesSOFormat.ElementAt(_currentElement);
        }
        if (_animator)
        {
            _animator.SetBool(_hasNewDialogueOptionsHash, true);
        }
    }

    /// <summary>
    /// Replace the list of dialogueTrees with a new one.
    /// The NPC will singal through animation that they have a new dialogue. 
    /// </summary>
    /// <param name="dialogueTree"></param>
    public void SetDialogueTreeList(DialogueTree dialogueTree)
    {
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
    public void InsertDialogueTreeAndChange(DialogueTree dialogueTree)
    {
        if (!_dialogueTreesSOFormat.Contains(dialogueTree))
        {
            _dialogueBoxController.ExitConversation();
            _currentElement++;
            _dialogueTreesSOFormat.Insert(_currentElement, dialogueTree);
            this._dialogueTree = _dialogueTreesSOFormat.ElementAt(_currentElement);
            _animator.SetBool(_hasNewDialogueOptionsHash, true);
        }
    }

    public void SetDialogueTreeList(List<TextAsset> dialogueTrees)
    {
        SetDialogueTreeList(ConvertFromJSONListToDialogueTreeList(dialogueTrees));
    }

    public void SetDialogueTreeList(TextAsset dialogueTree)
    {
        SetDialogueTreeList(new List<TextAsset>
        {
            dialogueTree
        });
    }

    public void InsertDialogueTreeAndChange(TextAsset dialogueTree)
    {
        DialogueTree temp = Instantiate(_toBeOverwrittenByJSON);
        JsonUtility.FromJsonOverwrite(dialogueTree.text, temp);
        InsertDialogueTreeAndChange(temp);
    }

    public void SetDialogueTreeList(DialogueTree[] dialogueTreesSO, TextAsset[] dialogueTreesJSON)
    {
        SetDialogueTreeList(new List<DialogueTree>(dialogueTreesSO));
        JoinWithScriptableObjectList(new List<TextAsset>(dialogueTreesJSON));
        if (_dialogueTreesSOFormat.Count > 0)
        {
            _dialogueTree = _dialogueTreesSOFormat.ElementAt(_currentElement);
        }
    }

    /// <summary>
    /// Go to the next dialogueTree.
    /// The NPC will signal through animation that the dialogue changed. 
    /// </summary>
    public void NextDialogueTree()
    {
        _currentElement++;
        if (_currentElement >= _dialogueTreesSOFormat.Count())
        {
            _currentElement--;
            Debug.Log("You have already read the last dialogue tree");
        }
        else
        {
            //_dialogueBoxController.ExitConversation();
            _dialogueTree = _dialogueTreesSOFormat.ElementAt(_currentElement);
            // Change the speak canvas to the new dialogue tree
            _dialogueBoxController.StartSpeakCanvas(_dialogueTree);
            if (_animator == null)
            {
                GameObject parent = this.transform.parent.gameObject;
                _animator = parent.GetComponentInChildren<Animator>();
            }
            _animator.SetBool(_hasNewDialogueOptionsHash, true);
        }
    }

    /// <summary>
    /// Go to the prior dialogueTree.
    /// The NPC will signal through animation that the dialogue changed. 
    /// </summary>
    public void PreviousDialogueTree()
    {
        _currentElement--;
        if (_currentElement < 0)
        {
            _currentElement = 0;
            Debug.Log("You have already read the first dialogue tree");
        }
        else
        {
            //_dialogueBoxController.ExitConversation();
            _dialogueTree = _dialogueTreesSOFormat.ElementAt(_currentElement);
            // Change the speak canvas to the new dialogue tree
            _dialogueBoxController.StartSpeakCanvas(_dialogueTree);
            _animator.SetBool(_hasNewDialogueOptionsHash, true);
        }
    }

    // Easy way to test the functionality
    // public List<TextAsset> dialogueTreesJSONFormatTest;
    // public List<DialogueTree> dialogueTreesSOTest;
    // public TextAsset textAssetTest;
    // public DialogueTree dialogueTreeTest;


    // void Update() {
    //     if(Input.GetKeyDown(KeyCode.Alpha1)) {
    //         Debug.Log(KeyCode.Alpha1 + "is pressed. Go back");
    //         previousDialogueTree();
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha2)) {
    //         Debug.Log(KeyCode.Alpha2 + "is pressed. Go forward");
    //         NextDialogueTree();
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha4)) {
    //         Debug.Log(KeyCode.Alpha4 + "is pressed: changing the tree. JSON");
    //         SetDialogueTreeList(dialogueTreesJSONFormatTest);
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha5)) {
    //         Debug.Log(KeyCode.Alpha5 + "is pressed: changing the tree. ScritableObject");
    //         SetDialogueTreeList(dialogueTreesSOTest);
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha7)) {
    //         Debug.Log(KeyCode.Alpha7 + "is pressed: Inserting into the tree. JSON");
    //         insertDialogueTreeAndChange(textAssetTest);
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha8)) {
    //         Debug.Log(KeyCode.Alpha8 + "is pressed: Inserting into the tree. ScritableObject");
    //         insertDialogueTreeAndChange(dialogueTreeTest);
    //     }
    // }
}
