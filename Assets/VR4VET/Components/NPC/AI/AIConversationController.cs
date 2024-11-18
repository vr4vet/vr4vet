using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIConversationController : MonoBehaviour
{
    [SerializeField] private Transcribe _Transcribe;
    
    [TextArea(3, 10)]
    public string contextPrompt;
    public int maxTokens = 50;
    public List<Message> messages = new List<Message>();
    private ConversationController _ConversationController;
    private AudioSource audioSource;
    public float animationSpeed = 1.0f; // Speed of the animation
    public float minScale = 2f; // Minimum size
    public float maxScale = 4.0f; // Maximum size
    public SpriteRenderer microphoneIcon;
    private bool growing = true;
    private float currentScale;
    private bool isAnimating = false; // Flag to control animation

    void Start()
    {
        // Set transcription manager and conversation controller
        _Transcribe = GameObject.Find("TranscriptionManager").GetComponent<Transcribe>();
        _ConversationController = GetComponentInChildren<ConversationController>();

        // Set up microphone animation
        audioSource = GetComponent<AudioSource>();
        currentScale = maxScale;
        microphoneIcon.enabled = false; // Initially hide the microphone icon
        isAnimating = false;
    }

    /* Function that starts the transcription of user speech and animates the microphone icon accordingly */
    public void StartRecording()
    {
        _Transcribe.StartRecording(this); // Start transcribing and set reference to this conversation (this NPC)
        microphoneIcon.enabled = true; // Show the microphone icon
        isAnimating = true; // Start mic animation
    }

    public Transcribe GetTranscribe() {
        return this._Transcribe;
    }

    /* Function for pressing speech button in VR. Pressing the 'B' button in VR while within range of an NPC that currently 
    is on a interruptable element in the dialogue tree will start the recording. When the button is let go, the transcriptions 
    stops. */
    public void PressButton(InputAction.CallbackContext context)
	{
		if (context.started && _ConversationController.playerInsideTrigger && _ConversationController.isTalkable)
		{
            StartRecording();
		}

		if (context.canceled)
		{
			EndRecording();
		}
	}

    public void EndRecording()
    {
        isAnimating = false; // Stop mic animation
        microphoneIcon.enabled = false; // Hide the microphone icon
        _Transcribe.EndRecording(); // Stop transcribing
    }

    /* Function for creating an AIRequest (request to OpenAI GPT model) based on the transcribed text. */
    public void CreateRequest(string transcript) 
    {
        if (transcript == "")
        {
            // Transcription contained blank audio or was inaudible
            transcript = "Please respond by saying you cannot understand me, and that the microphone button needs to be held for a couple of seconds.";
        }

        // Add components and create OpenAI query based on transcript
        AIRequest request = gameObject.AddComponent<AIRequest>();
        request.query = transcript + " and answer in language:" + _Transcribe.currentLanguage;
        request.maxTokens = maxTokens;
    }

    /* Function for adding a message to the local NPC context prompt. Used for adding both user transcript and the AI's reply. */
    public void AddMessage(Message message)
    {
        messages.Add(message);
    }

    public void FixedUpdate()
    {
        // Animation of microphone
        if (isAnimating)
        {
            AnimateMicrophoneIcon();
        }
    }

    /* Function that handles animation of the microphone icon in interruptable dialogue boxes. */
    private void AnimateMicrophoneIcon()
    {
        if (growing)
        {
            // Increase the scale of the icon
            currentScale += animationSpeed * Time.deltaTime;
            
            // Check if the scale has reached or exceeded the maximum limit
            if (currentScale >= maxScale)
            {
                // Clamp the scale to the maximum value and start shrinking
                currentScale = maxScale;
                growing = false; 
            }
        }
        else
        {
            // Decrease the scale of the icon
            currentScale -= animationSpeed * Time.deltaTime;
            
            // Check if the scale has reached or fallen below the minimum limit
            if (currentScale <= minScale)
            {
                // Clamp the scale to the minimum value and start growing
                currentScale = minScale;
                growing = true;  
            }
        }
        // Apply the updated scale to the microphone icon
        microphoneIcon.transform.localScale = new Vector3(currentScale, currentScale, 1);
    }
}