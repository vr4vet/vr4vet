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

    [HideInInspector] public static event Action OnDialogueStarted;
    [HideInInspector] public static event Action OnDialogueEnded;
    [HideInInspector] private bool _skipLineTriggered;
    [HideInInspector] private bool _answerTriggered;
    [HideInInspector] private int _answerIndex;
    [SerializeField] private GameObject _skipLineButton;
    [SerializeField] private GameObject _exitButton;
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _isTalkingHash;
    [HideInInspector] private int _hasNewDialogueOptionsHash;

    private ButtonSpawner buttonSpawner;

    [HideInInspector] public bool dialogueIsActive;

    private void Awake() 
    {
        buttonSpawner = GetComponent<ButtonSpawner>();
        if (buttonSpawner == null) 
        {
            Debug.LogError("The NPC missing the Button spawner script");
        }
        ResetBox();
        dialogueIsActive = false;

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

        // Animation stuff
        updateAnimator();
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
        _animator.SetBool(_isTalkingHash, true);
        // Dialogue 
        ResetBox();
        _dialogueBox.SetActive(true);
        OnDialogueStarted?.Invoke();
        StartCoroutine(RunDialogue(dialogueTree, startSection));
        _exitButton.SetActive(true);

    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++) 
        {   
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
            OnDialogueEnded?.Invoke();
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

    void ResetBox() 
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

    public void ExitConversation()
    {
        // stop talk-animation
        _animator.SetBool(_isTalkingHash, false);
        dialogueIsActive = false;
        ResetBox();
    }
}