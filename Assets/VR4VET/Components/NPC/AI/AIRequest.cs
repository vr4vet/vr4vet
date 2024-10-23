using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Meta.WitAi.TTS.Utilities;

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

    public AIConversationController _AIConversationController; // Save messages here in order to save them across multiple instances of this AIrequset.

    private List<Message> messages = new List<Message>();


    void Start()
    {
        // Get OpenAI key, which must be set in .env file

        key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        _AIConversationController = GetComponent<AIConversationController>();

        if (_AIConversationController != null)
        {
            messages = new List<Message>(_AIConversationController.messages);
        }
        else
        {
            Debug.LogError("AIConversationController not found.");
        }

        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("OpenAI API key not found.");
            return;
        }

        // Attempt to automatically find and set AIResponseToSpeech
        if (_AIResponseToSpeech == null)
        {
            _AIResponseToSpeech = GetComponent<AIResponseToSpeech>();
            if (_AIResponseToSpeech == null)
            {
                Debug.LogError("AIResponseToSpeech component not found in the scene.");
                return;
            }
        }

        if (_dialogueBoxController == null)
        {
            _dialogueBoxController = GetComponent<DialogueBoxController>();
            if (_dialogueBoxController == null)
            {
                Debug.LogError("DialogueBoxController component not found in the scene.");
                return;
            }
        }

        Message userMessage = new Message { role = "user", content = query };
        messages.Add(userMessage);
        _AIConversationController.AddMessage(userMessage);

        // Start coroutine for the OpenAI request
        StartCoroutine(OpenAI());
    }

    IEnumerator OpenAI()
    {
        while (string.IsNullOrEmpty(query))
        {
            yield return new WaitForSeconds(0.01f);
        }

        OpenAIRequest openAIRequest = new OpenAIRequest
        {
            model = "gpt-4o-mini",
            messages = messages,
            max_tokens = maxTokens
        };

        string jsonData = JsonUtility.ToJson(openAIRequest);

        using (UnityWebRequest request = new UnityWebRequest(api, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {key}");

            Debug.Log(jsonData);    

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
                    .Replace("\\", "")
                    .Replace("\"", "")
                    .Replace("\n", "")
                    .Replace("*", "")
                    .Replace("[", "")
                    .Replace("]", "")
                    .Replace("_", "")
                    .Replace("#", "")
                    .Replace("\r", "");

                Message assistantMessage = new Message { role = "assistant", content = responseText };
                messages.Add(assistantMessage);
                _AIConversationController.AddMessage(assistantMessage);

                // Call AIResponseToSpeech to dictate the response
                if (_AIResponseToSpeech != null && _dialogueBoxController.useWitAI == false)
                {
                    StartCoroutine(_AIResponseToSpeech.OpenAIDictate(responseText));

                    // Display the thinking dialogue while waiting for the response
                    Coroutine thinking = StartCoroutine(_dialogueBoxController.DisplayThinking());
                    yield return new WaitUntil(() => _AIResponseToSpeech.readyToAnswer);
                    StopCoroutine(thinking);
                    _dialogueBoxController.stopThinking();

                    if (responseText.Length > 280)
                    {
                        responseText = responseText.Substring(0, 280);
                        responseText = $"{responseText}...";
                    }

                    // Display the response in the dialogue box
                    StartCoroutine(_dialogueBoxController.DisplayResponse(responseText));
                }
                else if (_dialogueBoxController.useWitAI == true)
                {
                    StartCoroutine(_AIResponseToSpeech.WitAIDictate(responseText));

                    // Display the thinking dialogue while waiting for the response
                    Coroutine thinking = StartCoroutine(_dialogueBoxController.DisplayThinking());
                    yield return new WaitUntil(() => _AIResponseToSpeech.readyToAnswer);
                    StopCoroutine(thinking);
                    _dialogueBoxController.stopThinking();
                    

                    if (responseText.Length > 280)
                    {
                        responseText = responseText.Substring(0, 280);
                        responseText = $"{responseText}...";
                    }


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
