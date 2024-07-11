using System;
using System.Collections;
using TMPro;
using UnityEngine;
// Import of the TTS namespace
using Meta.WitAi.TTS.Utilities;

public class DialogueBoxController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private GameObject _answerBox;
    [SerializeField] private GameObject _dialogueCanvas;
    [SerializeField] public GameObject TTSSpeaker;

    [HideInInspector] public static event Action<string> OnDialogueStarted;
    [HideInInspector] public static event Action<string> OnDialogueEnded;
    [HideInInspector] private bool _skipLineTriggered;
    [HideInInspector] private bool _answerTriggered;
    [HideInInspector] private int _answerIndex;
    [SerializeField] private GameObject _skipLineButton;
    [SerializeField] private GameObject _exitButton;
    [SerializeField] private GameObject _speakButton; 
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _isTalkingHash;
    [HideInInspector] private int _hasNewDialogueOptionsHash;
    [HideInInspector] private RectTransform backgroundRect;
    [HideInInspector] private RectTransform dialogueTextRect;

    public ButtonSpawner buttonSpawner;

    [HideInInspector] public bool dialogueIsActive;

    // For testing purposes
    public DialogueTree dialogueTreeRestart;

    private void Awake() 
    {
        buttonSpawner = GetComponent<ButtonSpawner>();
        if (buttonSpawner == null) 
        {
            Debug.LogError("The NPC missing the Button spawner script");
        }
        ResetBox();
        dialogueIsActive = false;

        // Animation stuff
        updateAnimator();
    }

    private void Start()
    {
        // Assign the event camera
        if (_dialogueCanvas != null)
        {
            GameObject cameraCaster = GameObject.Find("CameraCaster");
            if (cameraCaster != null)
            {
                Camera eventCamera = cameraCaster.GetComponent<Camera>();
                if (eventCamera != null)
                {
                    _dialogueCanvas.GetComponent<Canvas>().worldCamera = eventCamera;
                }
                else
                {
                    Debug.LogError("CameraCaster does not have a Camera component!");
                }
            }
            else
            {
                Debug.LogError("CameraCaster GameObject not found in the scene!");
            }
        }
        else
        {
            Debug.LogError("DialogueCanvas not found or does not have a Canvas component!");
        }
        // Get the background transform for dimension changes
        backgroundRect = _dialogueBox.transform.Find("BasicDialogueItems").transform.Find("Background").GetComponent<RectTransform>();
        dialogueTextRect = _dialogueBox.transform.Find("BasicDialogueItems").transform.Find("DialogueText").GetComponent<RectTransform>();
    }

    public void updateAnimator() {
        //this.animator = animator;
        this._animator = GetComponentInChildren<Animator>();
        _isTalkingHash = Animator.StringToHash("isTalking");
        _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
    }

    public void updateAnimator(Animator animator) {
        this._animator = animator;
    }


    public void StartDialogue(DialogueTree dialogueTree, int startSection, string name) 
    {
        dialogueIsActive = true;
        // stop I-have-something-to-tell-you-animation and start talking
        _animator.SetBool(_hasNewDialogueOptionsHash, false);
        //_animator.SetBool(_isTalkingHash, true);
        // Dialogue 
        ResetBox();
        _dialogueBox.SetActive(true);
        OnDialogueStarted?.Invoke(name);
        StartCoroutine(RunDialogue(dialogueTree, startSection));
        _exitButton.SetActive(true);

    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        // Make the "Speak" restart tree the current tree
        dialogueTreeRestart = dialogueTree;
        // Reset the dialogue box dimensions from "Speak" button dimensions
        backgroundRect.sizeDelta = new Vector2(160,100);
        dialogueTextRect.sizeDelta = new Vector2(150,60);

        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++) 
        {   
            // Start talking animation
            _animator.SetBool(_isTalkingHash, true);
            StartCoroutine(revertToIdleAnimation());
            _dialogueText.text = dialogueTree.sections[section].dialogue[i];
            TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
            while (!_skipLineTriggered)
            {
                _skipLineButton.SetActive(true);
                _exitButton.SetActive(true);
                yield return null;
            }
            _skipLineTriggered = false;
            _skipLineButton.SetActive(false);
        }
        if (dialogueTree.sections[section].endAfterDialogue)
        {
            OnDialogueEnded?.Invoke(name);
            ExitConversation();
            yield break;
        }
        _dialogueText.text = dialogueTree.sections[section].branchPoint.question;
        TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
        ShowAnswers(dialogueTree.sections[section].branchPoint);
        while (_answerTriggered == false)
        {
            yield return null;
        }
        _answerTriggered = false;
        _exitButton.SetActive(false);
        _skipLineButton.SetActive(false);
        StartCoroutine(RunDialogue(dialogueTree, dialogueTree.sections[section].branchPoint.answers[_answerIndex].nextElement));
    }

    public void ResetBox() 
    {
        StopAllCoroutines();
        _dialogueBox.SetActive(false);
        buttonSpawner.removeAllButtons();
        _skipLineTriggered = false;
        _answerTriggered = false;
        _skipLineButton.SetActive(false);
        _exitButton.SetActive(false);
          
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        // Reveals the selectable answers and sets their text values
        buttonSpawner.spawnAnswerButtons(branchPoint.answers);
    }

    public void SkipLine()
    {
        _skipLineTriggered = true;
    }

    public void AnswerQuestion(int answer)
    {
        _answerIndex = answer;
        _answerTriggered = true;
        // remove the buttons
        buttonSpawner.removeAllButtons();
    }

    // Reverts to idle animation after 10.267 seconds
    // Time is length of talking animation, should be tweaked to not use value
    private IEnumerator revertToIdleAnimation() {
        yield return new WaitForSeconds(10.267f);
        _animator.SetBool(_isTalkingHash, false);

    }

    public void ExitConversation()
    {
        // stop talk-animation
        _animator.SetBool(_isTalkingHash, false);
        dialogueIsActive = false;
        StartSpeakCanvas(dialogueTreeRestart);
    }

    public void StartSpeakCanvas(DialogueTree dialogueTree)
    {
        ResetBox();
        _dialogueBox.SetActive(true);
        _dialogueText.text = null;
        backgroundRect.sizeDelta = new Vector2(50,30);
        dialogueTextRect.sizeDelta = new Vector2(50,30);
        buttonSpawner.spawnSpeakButton(dialogueTree);
    }
}