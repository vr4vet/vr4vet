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

    private const int SUBTITLE_DURATION = 5;

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

        // Initialize subtitles
        subtitle = GameObject.Find("SubtitleUI").GetComponentInChildren<TextMeshProUGUI>();
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

    public void StartRecording()
    {
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

    private void OnResult(string result){}

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
        AIRequest request = gameObject.AddComponent<AIRequest>();
        request.query = finalResult;
        subtitle.text = finalResult;   // Set subtitles based on final result
        StartCoroutine(WaitForSubtitleFadeOut());
    }

    // Fade out subtitles after SUBTITLE_DURATION seconds
    private IEnumerator WaitForSubtitleFadeOut() {
        yield return new WaitForSeconds(SUBTITLE_DURATION);
        subtitle.text = "";
    }
}