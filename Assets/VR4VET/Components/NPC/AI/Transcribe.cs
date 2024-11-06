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
	This script resides on the TranscriptionManager GameObject, which needs to be present in the scene for transcription 
    to work. Subtitles also require a GameObject in the scene. It handles all transcription locally, and updates the 
    subtitle text (even if they are active or not). Currently English, Norwegian, German and Dutch can be switched 
    between by pressing the 'L' key on the keyboard. It also runs the AIConversationController class' CreateRequest 
    method once transcription has been completed. Subtitles can be toggled by pressing the 'U' key on the keyboard.
*/
public class Transcribe : MonoBehaviour
{
	public WhisperManager whisper; 
    private string[] languages = { "en", "no", "de", "nl" };
    private int currentLanguageIndex = 0;
    public string currentLanguage;
    public MicrophoneRecord microphoneRecord;
    private WhisperStream _stream;
	private bool subtitlesEnabled = true;
	private const int SUBTITLE_DURATION = 8;	// How many seconds it takes before subtitles fade out
	public TextMeshProUGUI subtitle;
    [SerializeField] private AIConversationController _AIConversationController;

	public async void Start()
	{
        microphoneRecord = GetComponent<MicrophoneRecord>();
        whisper = GetComponent<WhisperManager>();  // Assign whisper transcription manager (from the whisper package) and set current language.
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

    /* Function for initializing the Whisper transcription, called by AIConversationController */
	public void StartRecording(AIConversationController _AIConversationController)
    {
        this._AIConversationController = _AIConversationController; // Set current conversation reference
        if (!microphoneRecord.IsRecording)
        {
            whisper.UpdateLanguage(currentLanguage);
            _stream.StartStream();
            microphoneRecord.StartRecord();
        }
    }

    /* Function for stopping the transcription, called by AIConversationController */
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

    /* Function for setting subtitles and creating a TTS request (AIRequest) once the transcription has finialized. */
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

    /* Function for toggling the visibility of subtitles */
	public void ToggleSubtitles()
    {
        subtitlesEnabled = !subtitlesEnabled;
        subtitle.gameObject.SetActive(subtitlesEnabled);
    }

    /* Helper function for fading out subtitles after SUBTITLE_DURATION seconds */
    private IEnumerator WaitForSubtitleFadeOut()
    {
        yield return new WaitForSeconds(SUBTITLE_DURATION);
        subtitle.text = "";
    }
}