using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Meta.WitAi.TTS.Utilities;

public class AIResponseToSpeech : MonoBehaviour
{
    private string api = "https://api.openai.com/v1/audio/speech";
    private string key;
    private AudioSource audioSource;
    public bool readyToAnswer = false;

    public GameObject TTSSpeaker;
    private TTSSpeaker ttsSpeakerComponent;

    private string newResponseText;

    public string OpenAiVoiceId;

    void Start()
    {
        // Get OpenAI key, which must be set in .env file
        key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("OpenAI API key not found.");
            return;
        }

        // Add an AudioSource component to the GameObject if it does not exist already
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (TTSSpeaker == null)
        {
            TTSSpeaker = GetComponentInChildren<TTSSpeaker>()?.gameObject;
            if (TTSSpeaker == null)
            {
                Debug.LogError("TTSSpeaker GameObject is not assigned in AIResponseToSpeech and couldn't be found.");
                return;
            }
        }

        ttsSpeakerComponent = TTSSpeaker.GetComponent<TTSSpeaker>();

        if (ttsSpeakerComponent == null)
        {
            Debug.LogError("TTSSpeaker component not found on the assigned or found TTSSpeaker GameObject.");
            return;
        }
    }

    // Coroutine for dictation through OpenAI's API in JSON request format
    public IEnumerator OpenAIDictate(string responseText)
    {
        // Indicate that the AI is thinking
        yield return readyToAnswer = false;

        newResponseText = responseText.Replace("\n", " ").Replace("\"", "").Replace(":", ""); // Replace newline with space

        // Debug.Log($"Dictating text: {newResponseText}");
        string jsonData = $"{{\"model\": \"tts-1-hd-1106\", \"input\": \"{newResponseText}\", \"voice\": \"{OpenAiVoiceId}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(api, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {key}");

            // Asynchronously send and wait for the respons
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}\nResponse Code: {request.responseCode}");
            }
            else
            {
                // Save the dictation as an audio file locally
                byte[] audioData = request.downloadHandler.data;
                string filePath = Path.Combine(Application.persistentDataPath, "speech.mp3");
                File.WriteAllBytes(filePath, audioData);
                // Debug.Log($"Audio saved to: {filePath}");

                // Indicate that the AI is ready to answer
                yield return readyToAnswer = true;

                // Play the saved audio file through coroutine
                StartCoroutine(PlayAudio(filePath));
            }
        }
    }

    private IEnumerator PlayAudio(string filePath)
    {
        // Load the audio file using UnityWebRequest
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                // Debug.LogError($"Error loading audio file: {request.error}");
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                audioSource.clip = audioClip;
                audioSource.Play();
                Debug.Log("Playing audio response from OpenAI.");
            }
        }
    }
    public IEnumerator WitAIDictate(string responseText)
    {
        readyToAnswer = false;
        Debug.Log("Dictating text: " + responseText);

        if (ttsSpeakerComponent != null)
        {
            ttsSpeakerComponent.Speak(responseText);
            Debug.Log("Playing audio response from WitAI.");
        }
        else
        {
            Debug.LogError("TTSSpeaker component is missing or not assigned.");
        }

        //  Wait until the speaking is finished
        while (ttsSpeakerComponent.IsSpeaking)
        {
            yield return null;
        }

        // Indicate that the AI is ready to answer again
        readyToAnswer = true;
    }
}
