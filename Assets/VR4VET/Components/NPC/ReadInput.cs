using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ReadInput : MonoBehaviour
{
    private string api = "https://api.openai.com/v1/audio/transcriptions";
    private string key;

	public string audioFile;

    void Start()
    {
        key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("OpenAI API key not found");
            return;
        }

        string audioFilePath = Path.Combine(Application.streamingAssetsPath, audioFile);
        StartCoroutine(SendAudioToOpenAI(audioFilePath, "no")); 
    }

    IEnumerator SendAudioToOpenAI(string audioFilePath, string language)
    {
        if (!File.Exists(audioFilePath))
        {
            Debug.LogError("Audio file not found at path: " + audioFilePath);
            yield break;
        }

        byte[] audioData = File.ReadAllBytes(audioFilePath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioData, "mp31.m4a", "audio/m4a");
        form.AddField("model", "whisper-1");
		form.AddField("language", language); 


        using (UnityWebRequest request = UnityWebRequest.Post(api, form))
        {
            request.SetRequestHeader("Authorization", $"Bearer {key}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}\nResponse Code: {request.responseCode}");
            }
            else
            {
                Debug.Log($"OpenAI Transcription Response: {request.downloadHandler.text}");
            }
        }
    }
}
