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
    [HideInInspector] public ButtonSpawner buttonSpawner;
    [HideInInspector] public bool dialogueIsActive;
    private int _activatedCount = 0;
    [HideInInspector] public DialogueTree dialogueTreeRestart;
    public bool dialogueEnded;
    public int timesEnded = 0;

    public AIResponseToSpeech _AIResponseToSpeech; // Reference to AIResponseToSpeech script, for dictation

    public AIConversationController _AIConversationController; // Save messages here in order to save them across multiple instances of this AIrequset.

    public bool useWitAI;

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
        useWitAI = false;
        dialogueEnded = false;
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

        // If the AIResponseToSpeech component is not set, attempt to find it
        if (_AIResponseToSpeech == null)
        {
            _AIResponseToSpeech = FindObjectOfType<AIResponseToSpeech>();
            if (_AIResponseToSpeech == null)
            {
                Debug.LogError("AIResponseToSpeech component not found in the scene.");
                return;
            }
        }

        if (_AIConversationController == null)
        {
            _AIConversationController = FindObjectOfType<AIConversationController>();
            if (_AIConversationController == null)
            {
                Debug.LogError("AIConversationController component not found.");
            }
        }
    }

    public void updateAnimator()
    {
        //this.animator = animator;
        this._animator = GetComponentInChildren<Animator>();
        _isTalkingHash = Animator.StringToHash("isTalking");
        _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
    }

    public void updateAnimator(Animator animator)
    {
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
        _activatedCount = 0;
        StartCoroutine(RunDialogue(dialogueTree, startSection));
        _exitButton.SetActive(true);

    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        Debug.Log("useWitAI is: " + useWitAI);
        // Make the "Speak" restart tree the current tree
        dialogueTreeRestart = dialogueTree;
        // Reset the dialogue box dimensions from "Speak" button dimensions
        backgroundRect.sizeDelta = new Vector2(160, 100);
        dialogueTextRect.sizeDelta = new Vector2(150, 60);

        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++)
        {
            // Start talking animation
            _animator.SetBool(_isTalkingHash, true);
            StartCoroutine(revertToIdleAnimation());
            _dialogueText.text = dialogueTree.sections[section].dialogue[i];
            AddDialogueToContext(_dialogueText.text);
            if (useWitAI)
            {
                TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
            }
            else
            {
                StartCoroutine(_AIResponseToSpeech.OpenAIDictate(_dialogueText.text));
            }
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
            dialogueEnded = true;
            timesEnded++;
            OnDialogueEnded?.Invoke(name);
            StartDynamicQuery();
            yield break;
        }

        if (useWitAI)
        {
            TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
        }
        else
        {
            StartCoroutine(_AIResponseToSpeech.OpenAIDictate(_dialogueText.text));
        }
        ShowAnswers(dialogueTree.sections[section].branchPoint);
        while (_answerTriggered == false)
        {
            yield return null;
        }
        _answerTriggered = false;
        _exitButton.SetActive(false);
        _skipLineButton.SetActive(false);
        if (dialogueTree.sections[section].branchPoint.answers[_answerIndex].endAfterAnswer)
        {
            // Exit conversation if the answer is set to exit after answer
            StartDynamicQuery();
        }
        else
        {
            // Continue to section of the dialogue the answer points to
            StartCoroutine(RunDialogue(dialogueTree, dialogueTree.sections[section].branchPoint.answers[_answerIndex].nextElement));
        }
    }

    public void StartComment(DialogueTree dialogueTree, int startSection, string name)
    {
        // Reset dialogue box if active
        dialogueIsActive = false;
        ResetBox();
        // Similar to startDialogue but don't activate the dialogue box
        dialogueIsActive = true;
        _animator.SetBool(_hasNewDialogueOptionsHash, false);
        OnDialogueStarted?.Invoke(name);
        RunComment(dialogueTree, startSection);
    }

    void RunComment(DialogueTree dialogueTree, int section)
    {
        // Runs the current section with no dialogue box, then exits
        _animator.SetBool(_isTalkingHash, true);
        StartCoroutine(ExitComment());
        _dialogueText.text = dialogueTree.sections[section].dialogue[0];
        if (useWitAI)
        {
            TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
        }
        else
        {
            StartCoroutine(_AIResponseToSpeech.OpenAIDictate(_dialogueText.text));
        }
    }

    private IEnumerator ExitComment()
    {
        // When 9 seconds have passed, stop the animation and exit the comment dialogue
        yield return new WaitForSeconds(9.0f);
        _animator.SetBool(_isTalkingHash, false);
        dialogueIsActive = false;
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

    void AddDialogueToContext(string dialogue)
    {
        _AIConversationController.AddMessage(new Message { role = "assistant", content = dialogue });
        Debug.Log("Added message to context: " + dialogue);

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
        _activatedCount++;
        // remove the buttons
        buttonSpawner.removeAllButtons();
    }

    // Reverts to idle animation after 10.267 seconds
    // Time is length of talking animation, should be tweaked to not use value
    private IEnumerator revertToIdleAnimation()
    {
        yield return new WaitForSeconds(9.0f);
        _animator.SetBool(_isTalkingHash, false);
    }

    public int GetActivatedCount()
    {
        return _activatedCount;
    }

    public void ExitConversation()
    {
        //stop talk-animation
        _animator.SetBool(_isTalkingHash, false);
        dialogueIsActive = false;
        ResetBox();
        if (dialogueTreeRestart.speakButtonOnExit)
        {
            //Only start speak canvas if option is not turned off
            StartSpeakCanvas(dialogueTreeRestart);
        }
    }

    public void StartSpeakCanvas(DialogueTree dialogueTree)
    {
        _dialogueBox.SetActive(true);
        _dialogueText.text = null;
        backgroundRect.sizeDelta = new Vector2(50, 30);
        dialogueTextRect.sizeDelta = new Vector2(50, 30);
        buttonSpawner.spawnSpeakButton(dialogueTree);
    }

    public void StartDynamicQuery()
    {
        // Stop previous NPC speech
        TTSSpeaker.GetComponent<TTSSpeaker>().Stop();
        _dialogueBox.SetActive(true);

        // Set text to generic question
        _dialogueText.text = "That's all I have to say. Do you have any questions? Hold B to speak to me.";


        // NPC will speak generic question
        if (useWitAI)
        {
            TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
        }
        else
        {
            StartCoroutine(_AIResponseToSpeech.OpenAIDictate(_dialogueText.text));
        }

        // Display generic question
        StartCoroutine(DisplayResponse(_dialogueText.text));
    }

    public IEnumerator DisplayResponse(string response)
    {
        // Start talking animation
        _animator.SetBool(_isTalkingHash, true);
        _dialogueText.text = response;
        _exitButton.SetActive(true);
        _skipLineButton.SetActive(false);

        // Wait for the player to exit the conversation
        while (_exitButton.activeSelf)
        {
            yield return null;
        }

        // Exit conversation when exit is pressed
        ExitConversation();
        yield return null;

    }

    public IEnumerator DisplayThinking()
    {
        // While waiting for a response, display thinking dialogue
        while (true)
        {
            _dialogueText.text = "I'm thinking...";
            yield return new WaitForSeconds(0.5f);
            _dialogueText.text = "I'm thinking.";
            yield return new WaitForSeconds(0.5f);
            _dialogueText.text = "I'm thinking..";
            yield return new WaitForSeconds(0.5f);
        }

    }

}