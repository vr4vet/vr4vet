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

public class SaveUserSpeech : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip myAudioClip;
    public const string FILENAME = "conversation.wav";
    public const int MAX_RECORDTIME = 10; // Max recording time in seconds
    private const int SAMPLE_RATE = 12000; // Sample rate of the audio file, 8K to 16K is normal in realtime voice applications
    private string filePath;
    public WhisperManager whisper;
    // NB! There currently is no language dropdown selector on the pause menu. This is on the main branch, not NPCAI. 
    private string[] languages = { "en", "no", "de", "nl" };
    private int currentLanguageIndex = 0;
    private string currentLanguage;
    public MicrophoneRecord microphoneRecord;
    private WhisperStream _stream;
    private string streamResult;

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
        microphoneRecord = GetComponent<MicrophoneRecord>();

      

        // Assign whisper transcription manager and set current language.
        // Users can change language by pressing 'L' on the keyboard for now.
        whisper = GetComponent<WhisperManager>();
        currentLanguage = languages[currentLanguageIndex];
        whisper.language = currentLanguage; // Default value is English

        _stream = await whisper.CreateStream(microphoneRecord);
        _stream.OnResultUpdated += OnResult;
        _stream.OnSegmentUpdated += OnSegmentUpdated;
        _stream.OnSegmentFinished += OnSegmentFinished;
        _stream.OnStreamFinished += OnFinished;

        microphoneRecord.OnRecordStop += OnRecordStop;

        currentScale = minScale;
        microphoneIcon.enabled = false; // Initially hide the microphone icon
    }


    public void Update() {
        // Check if the 'L' key is pressed
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Move to the next language in the list
            currentLanguageIndex++;
            if (currentLanguageIndex >= languages.Length)
            {
                currentLanguageIndex = 0;
            }
            currentLanguage = languages[currentLanguageIndex];
            Debug.Log("Current lang. code: " + currentLanguage);
        }

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
        
    
        if (!microphoneRecord.IsRecording)
        {
            whisper.UpdateLanguage(currentLanguage);
            _stream.StartStream();
            microphoneRecord.StartRecord();
            //microphoneIcon.drawMode = SpriteDrawMode.Simple;
            microphoneIcon.enabled = true; // Show the microphone icon
            isAnimating = true; // Start mic animation
        }
        
    }


    public void EndRecording()
    {
        microphoneRecord.StopRecord();
        isAnimating = false; // Stop mic animation
        microphoneIcon.enabled = false; // Hide the microphone icon
    }

    private void OnResult(string result)
    {
        streamResult+= result;
    }

    private void OnRecordStop(AudioChunk recordedAudio){}
    
    private void OnSegmentUpdated(WhisperResult segment)
    {
        print($"Segment updated: {segment.Result}");
    }
    
    private void OnSegmentFinished(WhisperResult segment)
    {
        print($"Segment finished: {segment.Result}");
    }
    
    private void OnFinished(string finalResult)
    {
        print("Stream finished!");

        // Add components and create OpenAI query based on transcript
        // ReadInput input = gameObject.AddComponent<ReadInput>();
        AIRequest request = gameObject.AddComponent<AIRequest>();
        request.query = streamResult;
        request.contextPrompt = contextPrompt;
        request.maxTokens = maxTokens;
        streamResult = "";
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