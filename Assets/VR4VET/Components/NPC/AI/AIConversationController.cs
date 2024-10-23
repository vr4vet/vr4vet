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
    private AudioClip myAudioClip;
    public const string FILENAME = "conversation.wav";

    public float animationSpeed = 1.0f; // Speed of the animation
    public float minScale = 2f; // Minimum size
    public float maxScale = 4.0f; // Maximum size
    public SpriteRenderer microphoneIcon;
    private bool growing = true;
    private float currentScale;
    private bool isAnimating = false; // Flag to control animation

    
    void Start()
    {
        _Transcribe = GameObject.Find("TranscriptionManager").GetComponent<Transcribe>();
        _ConversationController = GetComponentInChildren<ConversationController>();

        audioSource = GetComponent<AudioSource>();
        currentScale = maxScale;
        microphoneIcon.enabled = false; // Initially hide the microphone icon
        isAnimating = false;
    }

    public void StartRecording()
    {
        _Transcribe.StartRecording(this);
        microphoneIcon.enabled = true; // Show the microphone icon
        isAnimating = true; // Start mic animation
    }

    // Function for pressing speech button in VR
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
        _Transcribe.EndRecording();
    }

    public void CreateRequest(string transcript) {
        if (transcript == "")
        {
            // Transcription contained contained blank audio or was inaudible
            transcript = "Please respond by saying you cannot understand me, and that the microphone may have to be checked.";
        }

        // Add components and create OpenAI query based on transcript
        AIRequest request = gameObject.AddComponent<AIRequest>();
        request.query = transcript;
        request.contextPrompt = contextPrompt;
        request.maxTokens = maxTokens;
    }

    public void AddMessage(Message message)
    {
        messages.Add(message);
    }

    IEnumerator RecordingInput()
	{
		yield return new WaitForSeconds(0.01f);
		StartRecording();
	}


    public void FixedUpdate()
    {
        // Animation of microphone
        if (isAnimating)
        {
            AnimateMicrophoneIcon();
        }
    }

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




