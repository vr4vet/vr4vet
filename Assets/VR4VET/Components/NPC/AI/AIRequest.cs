using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AIRequest : MonoBehaviour
{
    private string api = "https://api.openai.com/v1/chat/completions";
    private string key;
    public string query;
    public string contextPrompt;
    public int maxTokens;
    public string responseText;

    public AIResponseToSpeech _AIResponseToSpeech; // Reference to AIResponseToSpeech script, for dictation
    public DialogueBoxController _dialogueBoxController;  

    void Start()
    {
        // Get OpenAI key, which must be set in .env file
        key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("OpenAI API key not found.");
            return;
        }
        // Attempt to automatically find and set AIResponseToSpeech
        if (_AIResponseToSpeech == null)
        {
            _AIResponseToSpeech = FindObjectOfType<AIResponseToSpeech>();
            if (_AIResponseToSpeech == null)
            {
                Debug.LogError("AIResponseToSpeech component not found in the scene.");
                return;
            }
        }
        if (_dialogueBoxController == null)
        {
            _dialogueBoxController = FindObjectOfType<DialogueBoxController>();
            if (_dialogueBoxController == null)
            {
                Debug.LogError("DialogueBoxController component not found in the scene.");
                return;
            }
        }

    // Start coroutine for the OpenAI request
    StartCoroutine(OpenAI());
    }

    IEnumerator OpenAI()
    {
        while (string.IsNullOrEmpty(query))
        {
            yield return new WaitForSeconds(0.01f);
        }
        // Debug.Log($"Query: {query}");

        // Creates the OpenAI API request in JSON format, with the query from the user inserted
        string jsonData = $"{{\"model\": \"gpt-4o-mini\", \"messages\": [{{\"role\": \"user\", \"content\": \"{contextPrompt} input:{query}\"}}], \"max_tokens\": {maxTokens}}}";
        Debug.Log($"context: {contextPrompt} q: {query}");
        using (UnityWebRequest request = new UnityWebRequest(api, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {key}");

            // Asynchronously send and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}\nResponse Code: {request.responseCode}");
            }
            else
            {
                OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(request.downloadHandler.text);

                // Retrieve the field with the actual response content, but add backslash before problematic characters
                responseText = response.choices[0].message.content
						.Replace("\\", "\\\\") 
    					.Replace("\"", "\\\"") 
    					.Replace("\n", "\\n")   
    					.Replace("\r", "\\r");
                Debug.Log($"Response: {responseText}");

                // Call AIResponseToSpeech to dictate the response
                if (_AIResponseToSpeech != null)
                {
                    StartCoroutine(_AIResponseToSpeech.OpenAIDictate(responseText));

                    // Display the thinking dialogue while waiting for the response
                    Coroutine thinking = StartCoroutine(_dialogueBoxController.DisplayThinking());
                    yield return new WaitUntil(() => _AIResponseToSpeech.readyToAnswer);
                    StopCoroutine(thinking);

                    // Display the response in the dialogue box
                    StartCoroutine(_dialogueBoxController.DisplayResponse(responseText));
                }
                else
                {
                    Debug.LogError("AIResponseToSpeech reference is not set.");
                }
            }
        }
    }
}