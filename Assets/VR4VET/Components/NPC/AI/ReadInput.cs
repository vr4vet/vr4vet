using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ReadInput : MonoBehaviour
{
    private string api = "https://api.openai.com/v1/audio/transcriptions";
    private string key;

    private string originalTranscript;
    public string transcript;

    // The audio file to send to OpenAI, must be saved in Application.persistentDataPath
    public string audioFile = NPCManager.FILENAME;

    // Public dropdown to select the language
    public SupportedLanguage selectedLanguage;

    private AudioSource audioSource;
    private AudioClip audioClip;

    public enum SupportedLanguage
    {
        English, // en
        Norwegian, // no
        German, // de
        Dutch, // nl
    }
    private string GetLanguageCode(SupportedLanguage language)
    {
        switch (language)
        {
            case SupportedLanguage.English: return "en";
            case SupportedLanguage.Norwegian: return "no";
            case SupportedLanguage.German: return "de";
            case SupportedLanguage.Dutch: return "nl";
            default: return "no"; // Default to English
        }
    }

    private string GetMimeType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();
        switch (extension)
        {
            case ".mp3":
                return "audio/mpeg";
            case ".wav":
                return "audio/wav";
            case ".ogg":
                return "audio/ogg";
            case ".m4a":
                return "audio/m4a";
            default:
                return "application/octet-stream"; // Default for unknown file types
        }
    }

    void Start()
    {
        // Get OpenAI key, which must be set in .env file
        key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("OpenAI API key not found.");
            return;
        }

        // Set the path where the transcript is saved
        string audioFilePath = Path.Combine(Application.persistentDataPath, audioFile);
        StartCoroutine(SendAudioToOpenAI(audioFilePath));

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Audio clip to play if no connection
        audioClip = Resources.Load<AudioClip>("AudioFiles/NoInternet/English/fable");
        audioSource.clip = audioClip;
    }

    // Coroutine for sending audio file to OpenAI for transcription
    IEnumerator SendAudioToOpenAI(string audioFilePath)
    {
        if (!File.Exists(audioFilePath))
        {
            Debug.LogError("Audio file not found at path: " + audioFilePath + ".");
            yield break;
        }

        byte[] audioData = File.ReadAllBytes(audioFilePath);
        string mimeType = GetMimeType(audioFilePath);

        // Create API request as a Web form
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioData, audioFile, mimeType);
        form.AddField("model", "whisper-1");
        form.AddField("language", GetLanguageCode(selectedLanguage));

        using (UnityWebRequest request = UnityWebRequest.Post(api, form))
        {
            request.SetRequestHeader("Authorization", $"Bearer {key}");

            // Asynchronously send and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                if (request.responseCode == 0)
                {
                    // Debug.Log("No internet connection.");

                    // If audio source is not already playing, then play voiceline
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
                else
                {
                    Debug.LogError($"Error: {request.error}\nResponse Code: {request.responseCode}");
                }
            }
            else
            {
                // Debug.Log($"OpenAI transcription response: {request.downloadHandler.text}");
                originalTranscript = request.downloadHandler.text.Replace("{", "").Replace("\"", "").Replace(":", "").Replace("}", "").Trim();
                transcript = originalTranscript.Substring(5);
            }
        }
    }
}
