using System;
using UnityEngine;
using System.IO;
using System.Collections;
using static ReadInput;
using static AIRequest;
using static OpenWavParser;
using Whisper;
using Whisper.Utils;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class NPCManager : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip myAudioClip;
    public const string FILENAME = "conversation.wav";

    private string contextPrompt;
    private int maxTokens;

    public float animationSpeed = 1.0f; // Speed of the animation
    public float minScale = 1f; // Minimum size
    public float maxScale = 4.0f; // Maximum size
    public SpriteRenderer microphoneIcon;

    private bool growing = true;
    private float currentScale;
    private bool isAnimating = false; // Flag to control animation


    public async void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentScale = minScale;
        microphoneIcon.enabled = false; // Initially hide the microphone icon
        isAnimating = false;
    }

    public void Update()
    {
        // Animation of microphone
        if (isAnimating)
        {
            AnimateMicrophoneIcon();
        }
    }

    public void StartRecording(string prompt, int max_tokens)
    {
        maxTokens = max_tokens;
        contextPrompt = prompt;
        microphoneIcon.enabled = true; // Show the microphone icon
        isAnimating = true; // Start mic animation
    }


    public void EndRecording(string transcript)
    {
        isAnimating = false; // Stop mic animation
        microphoneIcon.enabled = false; // Hide the microphone icon


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


    private void AnimateMicrophoneIcon()
    {
        if (growing)
        {
            currentScale += animationSpeed * Time.deltaTime;
            if (currentScale >= maxScale)
            {
                currentScale = maxScale;
                growing = false;
            }
        }
        else
        {
            currentScale -= animationSpeed * Time.deltaTime;
            if (currentScale <= minScale)
            {
                currentScale = minScale;
                growing = true;
            }
        }

        microphoneIcon.transform.localScale = new Vector3(currentScale, currentScale, 1);
    }
}