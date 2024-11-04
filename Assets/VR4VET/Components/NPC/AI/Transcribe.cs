using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Threading.Tasks;
using Whisper;
using Whisper.Utils;
using TMPro;

/* 
	This script resides on the TranscriptionManager GameObject, which needs to be present in the scene for transcription to work.
	It handles all transcription locally, and updates the subtitle text (even if they are active or not).
	Currently English, Norwegian, German and Dutch can be switched between by pressing the 'L' key on the keyboard.
	It also runs the AIConversationController class' CreateRequest method once transcription has been completed. 
*/
public class Transcribe : MonoBehaviour
{
	public WhisperManager whisper; 
    private string[] languages = { "en", "no", "de", "nl" };
    private int currentLanguageIndex = 0;
    public string currentLanguage;
    public MicrophoneRecord microphoneRecord;
    private WhisperStream _stream;
	public const int MAX_RECORDTIME = 10; // Max recording time in secon
	private bool subtitlesEnabled = true;
	private const int SUBTITLE_DURATION = 8;	// How long it takes before subtitles fade
	public TextMeshProUGUI subtitle;
    [SerializeField] private AIConversationController _AIConversationController;


	public async void Start()
	{
        microphoneRecord = GetComponent<MicrophoneRecord>();
        // Assign whisper transcription manager (from the whisper package) and set current language.
        whisper = GetComponent<WhisperManager>();
		Debug.Log(whisper);
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
	}

	public void StartRecording(AIConversationController _AIConversationController)
    {
        this._AIConversationController = _AIConversationController;
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
	
    private void OnResult(string result) {}

    private void OnRecordStop(AudioChunk recordedAudio) {}

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

        Debug.Log(finalResult);

		if (finalResult.Contains("[ Inaudible ]")) {
			finalResult = finalResult.Replace("[ Inaudible ]", "");
		}

		if (finalResult.Contains("[BLANK_AUDIO]")) {
			finalResult = finalResult.Replace("[BLANK_AUDIO]", "");
		}
        // Set subtitles if active
        if (subtitlesEnabled)
        {
            subtitle.text = finalResult;
            StartCoroutine(WaitForSubtitleFadeOut());
        }
        _AIConversationController.CreateRequest(finalResult);
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
}