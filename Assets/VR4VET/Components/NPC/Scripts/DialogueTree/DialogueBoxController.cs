using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Import of the TTS namespace
using Meta.WitAi.TTS.Utilities;

public class DialogueBoxController : MonoBehaviour
{
    public static DialogueBoxController instance;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject answerBox;
    [SerializeField] Button[] answerObjects;
    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;
    bool skipLineTriggered;
    bool answerTriggered;
    int answerIndex;
    public GameObject TTSSpeaker;
    private GameObject skipLineButton;
    private GameObject exitButton;
    private Animator animator;
    private int isTalkingHash;
    private int hasNewDialogueOptionsHash;

    private void Awake() 
    {
        Debug.Log("started dialogue box controller");
        if (instance == null)
        {
            Debug.Log("instance is null");
            instance = this;
            skipLineButton = GameObject.Find("DialogueCanvas/SkipLineButton");
            exitButton = GameObject.Find("DialogueCanvas/ExitConversationButton");
            skipLineButton.SetActive(false);
            exitButton.SetActive(false);
            // Animation stuff
            animator = instance.GetComponentInParent<Animator>();
            isTalkingHash = Animator.StringToHash("isTalking");
            hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
        }
        else 
        {
            Debug.Log("instance is not null");
            Destroy(this);
        }
    }

    public void StartDialogue(DialogueTree dialogueTree, int startSection, string name) 
    {
        // stop I-have-something-to-tell-you-animation and start talking
        Debug.Log("StartDialogue");
        animator.SetBool(hasNewDialogueOptionsHash, false);
        animator.SetBool(isTalkingHash, true);
        // Dialogue 
        ResetBox();
        nameText.text = name;
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
                yield return null;
            }
            skipLineTriggered = false;
            skipLineButton.SetActive(false);
        }
        if (dialogueTree.sections[section].endAfterDialogue)
        {
            OnDialogueEnded?.Invoke();
            dialogueBox.SetActive(false);
            yield break;
        }
        dialogueText.text = dialogueTree.sections[section].branchPoint.question;
        TTSSpeaker.GetComponent<TTSSpeaker>().Speak(dialogueText.text);
        ShowAnswers(dialogueTree.sections[section].branchPoint);
        while (answerTriggered == false)
        {
            yield return null;
        }
        answerBox.SetActive(false);
        answerTriggered = false;
        exitButton.SetActive(false);
        skipLineButton.SetActive(false);
        StartCoroutine(RunDialogue(dialogueTree, dialogueTree.sections[section].branchPoint.answers[answerIndex].nextElement));
    }

    void ResetBox() 
    {
        StopAllCoroutines();
        dialogueBox.SetActive(false);
        answerBox.SetActive(false);
        skipLineTriggered = false;
        answerTriggered = false;
        skipLineButton.SetActive(false);
        exitButton.SetActive(false);
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        // Reveals the aselectable answers and sets their text values
        answerBox.SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            if (i < branchPoint.answers.Length)
            {
                answerObjects[i].GetComponentInChildren<TextMeshProUGUI>().text = branchPoint.answers[i].answerLabel;
                answerObjects[i].gameObject.SetActive(true);
            }
            else {
                answerObjects[i].gameObject.SetActive(false);
            }
        }
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    public void AnswerQuestion(int answer)
    {
        answerIndex = answer;
        answerTriggered = true;
    }

    public void ExitConversation()
    {
        // stop talk-animation
        animator.SetBool(isTalkingHash, false);
        ResetBox();
    }
}