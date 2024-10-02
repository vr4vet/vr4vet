using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AIResponseToSpeech : MonoBehaviour
{
    private string api = "https://api.openai.com/v1/audio/speech";
    private string key;
    private AudioSource audioSource;

    void Start()
    {
        key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("OpenAI API key not found");
            return;
        }

        // Add an AudioSource component to this GameObject if it doesn't already have one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public IEnumerator OpenAIDictate(string responseText)
    {
        Debug.Log($"Dictating text: {responseText}");
        string jsonData = $"{{\"model\": \"tts-1-hd-1106\", \"input\": \"{responseText}\", \"voice\": \"alloy\"}}";

        using (UnityWebRequest request = new UnityWebRequest(api, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {key}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}\nResponse Code: {request.responseCode}");
            }
            else
            {
                // Save the audio data as a file (speech.mp3)
                byte[] audioData = request.downloadHandler.data;
                string filePath = Path.Combine(Application.persistentDataPath, "speech.mp3");
                File.WriteAllBytes(filePath, audioData);
                Debug.Log($"Audio saved to: {filePath}");

                // Play the saved mp3 file
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
                Debug.LogError($"Error loading audio file: {request.error}");
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                audioSource.clip = audioClip;
                audioSource.Play();
                Debug.Log("Playing audio.");
            }
        }
    }
}