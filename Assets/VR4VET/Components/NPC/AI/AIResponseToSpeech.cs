using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AIResponseToSpeech : MonoBehaviour
{
    private string api = "https://api.openai.com/v1/audio/speech";
    private string key;

    public string hi = "Hello";
    void Start()
    {
        key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("OpenAI API key not found");
            return;
        }

        //StartCoroutine(DictateText(responseText));

    }


    public IEnumerator DictateText(String responseText)
    {
        Debug.Log($"Dictating text: {responseText}");
       
        WWWForm form = new WWWForm();
        form.AddField("model", "tts-1");
        form.AddField("input", responseText);
        form.AddField("voice", "alloy");

        using (UnityWebRequest request = UnityWebRequest.Post(api, form))
        {
            request.SetRequestHeader("Authorization", $"Bearer {key}");
            request.SetRequestHeader("Content-Type", "application/json"); // Det er viktig å spesifisere Content-Type

            // Send forespørselen og vent til den er fullført
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}\nResponse Code: {request.responseCode}");
            }
            else
            {
                // Hent resultatet og lagre det som en lydfil (speech.mp3)
                byte[] audioData = request.downloadHandler.data;
                string filePath = Path.Combine(Application.persistentDataPath, "speech.mp3");
                File.WriteAllBytes(filePath, audioData);

                Debug.Log($"Audio saved to: {filePath}");
                // Du kan også spille av lydfilen her hvis ønskelig
            }
        }
    }
}
