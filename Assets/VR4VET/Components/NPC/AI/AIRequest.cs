using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AIRequest : MonoBehaviour
{
    private string api = "https://api.openai.com/v1/chat/completions";
    private string key;
    public string query;
    public string responseText;

    // Reference to AIResponseToSpeech script
    public AIResponseToSpeech aiResponseToSpeech;

    void Start()
{
    key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

    if (string.IsNullOrEmpty(key))
    {
        Debug.LogError("OpenAI API key not found");
        return;
    }

    // Attempt to automatically find and set the AIResponseToSpeech reference if it's not assigned in the Inspector
    if (aiResponseToSpeech == null)
    {
        aiResponseToSpeech = FindObjectOfType<AIResponseToSpeech>();
        
        if (aiResponseToSpeech == null)
        {
            Debug.LogError("AIResponseToSpeech component not found in the scene.");
            return;
        }
    }

    StartCoroutine(OpenAI());
}

    IEnumerator OpenAI()
    {
        while (string.IsNullOrEmpty(query))
        {
            yield return new WaitForSeconds(1);
        }
        Debug.Log($"Query: {query}");

        string jsonData = $"{{\"model\": \"gpt-4o-mini\", \"messages\": [{{\"role\": \"user\", \"content\": \"{query}\"}}], \"max_tokens\": 50}}";
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
                OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(request.downloadHandler.text);
                responseText = response.choices[0].message.content;
                Debug.Log($"Response: {responseText}");

                // Call AIResponseToSpeech to dictate the response
                if (aiResponseToSpeech != null)
                {
                    StartCoroutine(aiResponseToSpeech.OpenAIDictate(responseText));
                }
                else
                {
                    Debug.LogError("AIResponseToSpeech reference is not set.");
                }
            }
        }
    }
}