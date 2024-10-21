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

public class SaveUserSpeech : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip myAudioClip;
    public const string FILENAME = "conversation.wav";
    public const int MAX_RECORDTIME = 10; // Max recording time in secon
    public WhisperManager whisper;
    // NB! There currently is no language dropdown selector on the pause menu. This is on the main branch, not NPCAI. 
    private string[] languages = { "en", "no", "de", "nl" };
    private int currentLanguageIndex = 0;
    private string currentLanguage;
    public MicrophoneRecord microphoneRecord;
    private WhisperStream _stream;

    public TextMeshProUGUI subtitle;

    private const int SUBTITLE_DURATION = 8;

    private string contextPrompt;
    private int maxTokens;


    public float animationSpeed = 1.0f; // Speed of the animation
    public float minScale = 2.0f; // Minimum size
    public float maxScale = 4.0f; // Maximum size
    public SpriteRenderer microphoneIcon;

    private bool growing = true;
    private float currentScale;
    private bool isAnimating = false; // Flag to control animation

    private bool subtitlesEnabled = true;

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

        // Initialize subtitles
        subtitle = GameObject.Find("SubtitleUI").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Update()
    {
        // Check if the "L" key is pressed
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

        // Check if the "U" key is pressed
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleSubtitles();
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

    private void OnResult(string result) { }

    private void OnRecordStop(AudioChunk recordedAudio) { }

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

        if (finalResult.Contains("[ Inaudible ]") || finalResult.Contains("[BLANK_AUDIO]"))
        {
            Debug.Log("Blank audio returned from Whisper. Make sure to hold the microphone button down for a few seconds.");
            finalResult = "Please respond by saying you cannot understand me, and that the microphone may have to be checked.";
            // Add components and create OpenAI query based on transcript
            AIRequest request = gameObject.AddComponent<AIRequest>();
            request.query = finalResult;
            request.contextPrompt = contextPrompt;
            request.maxTokens = maxTokens;
        }
        else
        {
            // Add components and create OpenAI query based on transcript
            AIRequest request = gameObject.AddComponent<AIRequest>();
            request.query = finalResult;
            request.contextPrompt = contextPrompt;
            request.maxTokens = maxTokens;

            // Enable subtitles if toggled
            if (subtitlesEnabled)
            {
                subtitle.text = finalResult;
                StartCoroutine(WaitForSubtitleFadeOut());
            }
        }
    }

    public void ToggleSubtitles()
    {
        subtitlesEnabled = !subtitlesEnabled;
        subtitle.gameObject.SetActive(subtitlesEnabled);
    }

    // Fade out subtitles after SUBTITLE_DURATION seconds
    private IEnumerator WaitForSubtitleFadeOut()
    {
        yield return new WaitForSeconds(SUBTITLE_DURATION);
        subtitle.text = "";
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