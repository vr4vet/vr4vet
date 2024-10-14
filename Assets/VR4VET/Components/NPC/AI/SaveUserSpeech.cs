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
        }
        
    }


    public void EndRecording()
    {
        microphoneRecord.StopRecord();
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

        if (finalResult.Contains("[ Inaudible ]") || finalResult.Contains("[BLANK_AUDIO]")) {
            Debug.Log("Blank audio returned from Whisper. Please speak into the microphone and hold the microphone button down for a few seconds.");
            finalResult = "Please respond by saying you cannot understand me";
        }

        // Add components and create OpenAI query based on transcript
        // ReadInput input = gameObject.AddComponent<ReadInput>();
        AIRequest request = gameObject.AddComponent<AIRequest>();
        request.query = finalResult;
        request.contextPrompt = contextPrompt;
        request.maxTokens = maxTokens;
    }
}