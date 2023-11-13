using System;
using System.Collections;
using TMPro;
using UnityEngine;
// Import of the TTS namespace
using Meta.WitAi.TTS.Utilities;

public class DialogueBoxController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject answerBox;

    [SerializeField] GameObject dialogueCanvas;

    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;
    bool skipLineTriggered;
    bool answerTriggered;
    int answerIndex;
    public GameObject TTSSpeaker;
    public GameObject skipLineButton;
    public GameObject exitButton;
    private Animator animator;
    private int isTalkingHash;
    private int hasNewDialogueOptionsHash;

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
        if (dialogueCanvas != null)
        {
            GameObject cameraCaster = GameObject.Find("CenterEyeAnchor");
            if (cameraCaster != null)
            {
                Camera eventCamera = cameraCaster.GetComponent<Camera>();
                if (eventCamera != null)
                {
                    dialogueCanvas.GetComponent<Canvas>().worldCamera = eventCamera;
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
        animator = GetComponentInParent<Animator>();
        Debug.Log("Animator" + animator);
        isTalkingHash = Animator.StringToHash("isTalking");
        hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
    }


    public void StartDialogue(DialogueTree dialogueTree, int startSection, string name) 
    {
        dialogueIsActive = true;
        // stop I-have-something-to-tell-you-animation and start talking
        Debug.Log("StartDialogue");
        animator.SetBool(hasNewDialogueOptionsHash, false);
        animator.SetBool(isTalkingHash, true);
        // Dialogue 
        ResetBox();
        dialogueBox.SetActive(true);
        OnDialogueStarted?.Invoke();
        StartCoroutine(RunDialogue(dialogueTree, startSection));
        exitButton.SetActive(true);

    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
    {
        for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++) 
        {   
            dialogueText.text = dialogueTree.sections[section].dialogue[i];
            TTSSpeaker.GetComponent<TTSSpeaker>().Speak(dialogueText.text);
            Debug.Log(dialogueText.text);
            while (!skipLineTriggered)
            {
                skipLineButton.SetActive(true);
                exitButton.SetActive(true);
                yield return null;
            }
            skipLineTriggered = false;
            skipLineButton.SetActive(false);
        }
        if (dialogueTree.sections[section].endAfterDialogue)
        {
            OnDialogueEnded?.Invoke();
            ExitConversation();
            yield break;
        }
        dialogueText.text = dialogueTree.sections[section].branchPoint.question;
        TTSSpeaker.GetComponent<TTSSpeaker>().Speak(dialogueText.text);
        ShowAnswers(dialogueTree.sections[section].branchPoint);
        while (answerTriggered == false)
        {
            yield return null;
        }
        answerTriggered = false;
        exitButton.SetActive(false);
        skipLineButton.SetActive(false);
        StartCoroutine(RunDialogue(dialogueTree, dialogueTree.sections[section].branchPoint.answers[answerIndex].nextElement));
    }

    void ResetBox() 
    {
        StopAllCoroutines();
        dialogueBox.SetActive(false);
        buttonSpawner.removeAllButtons();
        skipLineTriggered = false;
        answerTriggered = false;
        skipLineButton.SetActive(false);
        exitButton.SetActive(false);
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        // Reveals the selectable answers and sets their text values
        buttonSpawner.spawnAnswerButtons(branchPoint.answers);
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    public void AnswerQuestion(int answer)
    {
        Debug.Log("Answer question: " + answer);
        answerIndex = answer;
        answerTriggered = true;
        // remove the buttons
        buttonSpawner.removeAllButtons();
    }

    public void ExitConversation()
    {
        // stop talk-animation
        animator.SetBool(isTalkingHash, false);
        dialogueIsActive = false;
        ResetBox();
    }
}