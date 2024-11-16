using System;
using System.Collections;
using TMPro;
using UnityEngine;
// Import of the TTS namespace
using Meta.WitAi.TTS.Utilities;

public class DialogueBoxController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _dialogueText;
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
    [SerializeField] private GameObject _restartConversationButton;
    [SerializeField] private GameObject _speakButton;
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _isTalkingHash;
    [HideInInspector] private int _isListeningHash;
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

    public bool useWitAI = false;

    [HideInInspector] public bool isTalkable;
    public SpriteRenderer holdBToTalkMessage;

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
            _AIResponseToSpeech = GetComponent<AIResponseToSpeech>();
            if (_AIResponseToSpeech == null)
            {
                Debug.Log("AIResponseToSpeech component not found in the scene. Add it if the NPC needs AI abilities.");
                return;
            }
        }

        if (_AIConversationController == null)
        {
            _AIConversationController = GetComponent<AIConversationController>();
            if (_AIConversationController == null)
            {
                Debug.Log("AIConversationController component not found. Add it if the NPC needs AI abilities.");
            }
        }
    }

    public void updateAnimator()
    {
        //this.animator = animator;
        this._animator = GetComponentInChildren<Animator>();
        _isTalkingHash = Animator.StringToHash("isTalking");
        _isListeningHash = Animator.StringToHash("isListening");
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
        _exitButton.SetActive(false);
    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        if (useWitAI)
        {
            Debug.Log("Using WitAI for text-to-speech. Slightly faster, but sounds worse than OpenAI in some languages.");
        }
        else
        {
            Debug.Log("Using OpenAI for text-to-speech. Slightly slower, but sounds better than WitAI in some languages.");
        }
        // Make the "Speak" restart tree the current tree
        dialogueTreeRestart = dialogueTree;
        // Reset the dialogue box dimensions from "Speak" button dimensions
        backgroundRect.sizeDelta = new Vector2(160, 100);
        dialogueTextRect.sizeDelta = new Vector2(150, 60);
        
        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++)
        {
            // Start talking animation
            StartCoroutine(revertToIdleAnimation());
            _dialogueText.text = dialogueTree.sections[section].dialogue[i];
            if (_dialogueText.text.Length > 280)
            {
                _dialogueText.text = _dialogueText.text.Substring(0, 280);
                _dialogueText.text = $"{_dialogueText.text}...";
            }

            // if the dialogue is not interruptable, it should not be possible to interact with NPC
            isTalkable = dialogueTree.sections[section].interruptableElements[i];  
            if(isTalkable)
            {
                holdBToTalkMessage.enabled = true; // show the msg that you can talk to the NPC
            }
            else
            {
                holdBToTalkMessage.enabled = false; // hide the msg that you can talk to the NPC
            }

            // Add dialogue to context, so the NPC can remember it later
            if (_AIConversationController != null) 
            {
                AddDialogueToContext(_dialogueText.text);
            }

            // Check which TTS to use
            if (useWitAI || _AIConversationController == null)
            {
                TTSSpeaker.GetComponent<TTSSpeaker>().Speak(_dialogueText.text);
            }
            else
            {
                StartCoroutine(_AIResponseToSpeech.OpenAIDictate(_dialogueText.text));
                yield return new WaitForSeconds(1.5f);
            }
            
            _animator.SetBool(_isTalkingHash, true);
            while (!_skipLineTriggered)
            {
                _skipLineButton.SetActive(true);
                _exitButton.SetActive(false);
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
            StartDynamicQuery(dialogueTree);
            yield break;
        }

        // Check which TTS to use
        if (useWitAI || _AIConversationController == null)
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
            StartDynamicQuery(dialogueTree);
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
        yield return new WaitForSeconds(8.0f);
        _animator.SetBool(_isTalkingHash, false);
        _animator.SetBool(_isListeningHash, false);
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
        _restartConversationButton.SetActive(false);
    }

    // This adds the message to the context, so the NPC can remember previous answers
    void AddDialogueToContext(string dialogue)
    {
        _AIConversationController.AddMessage(new Message { role = "assistant", content = dialogue });
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
        yield return new WaitForSeconds(8.0f);
        _animator.SetBool(_isTalkingHash, false);
        _animator.SetBool(_isListeningHash, false);
    }

    public int GetActivatedCount()
    {
        return _activatedCount;
    }

    public void ExitConversation()
    {
        // Stop talk animation
        _animator.SetBool(_isTalkingHash, false);
        _animator.SetBool(_isListeningHash, false);
        dialogueIsActive = false;
        ResetBox();
        if (dialogueTreeRestart.speakButtonOnExit)
        {
            // Only start speak canvas if option is not turned off
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

    // Restarts conversation when the restart button is clicked
    public void RestartConversation()
    {
        StartDialogue(dialogueTreeRestart, 0, "NPC");
    }

    public void StartDynamicQuery(DialogueTree dialogueTree)
    {
        // Stop previous NPC speech
        // buttonSpawner.spawnRepeatButton(dialogueTree);
        _restartConversationButton.SetActive(true);  
        TTSSpeaker.GetComponent<TTSSpeaker>().Stop();
        _exitButton.SetActive(false);
        _dialogueBox.SetActive(true);

        // Set text to generic text for the end of the dialogue tree
        _dialogueText.text = "That is all I have to say.";

        // NPC will speak generic question, based on given TTS setting
        if (useWitAI || _AIConversationController == null)
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
        _exitButton.SetActive(false);
        _skipLineButton.SetActive(false);

        // Wait for the player to exit the conversation
        // while (_exitButton.activeSelf)
        // {
        //     yield return null;
        // }

        // Exit conversation when exit is pressed
        // ExitConversation();

        StartCoroutine(revertToIdleAnimation());
        yield return null;
    }

    public IEnumerator DisplayThinking()
    {
        // While waiting for a response, display thinking dialogue
        while (true)
        {
            _dialogueText.text = ".";
            yield return new WaitForSeconds(0.5f);
            _dialogueText.text = "..";
            yield return new WaitForSeconds(0.5f);
            _dialogueText.text = "...";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void useOpenAiTTS(){
        useWitAI = false;
    }

    public void useWitTTS(){
        useWitAI = true;
    }

    public void startThinking()
    {
        _animator.SetBool(_isListeningHash, true);
    }

    public void stopThinking()
    {
        _animator.SetBool(_isListeningHash, false);
    }

    public void ShowDialogueBox()
    {
        _dialogueBox.SetActive(true);
        _dialogueCanvas.SetActive(true);
        Debug.Log("Dialogue box reactivated.");
    }

    public void HideDialogueBox()
    {
        _dialogueBox.SetActive(false);
        _dialogueCanvas.SetActive(false);
        Debug.Log("Dialogue box hidden.");
    }

}