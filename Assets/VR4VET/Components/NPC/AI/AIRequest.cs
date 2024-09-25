using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using static OpenAIResponse;

public class AIRequest : MonoBehaviour
{
	private string api = "https://api.openai.com/v1/chat/completions";
	private string key;
	public string query;
	public string responseText;

	void Start()
	{
		key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

		if (string.IsNullOrEmpty(key))
		{
			Debug.LogError("OpenAI API key not found");
			return;
		}

		StartCoroutine(OpenAI());
	}

	// test

	IEnumerator OpenAI()
	{
		while (query == null) {
			yield return new WaitForSeconds(1);
		}
		Debug.Log($"Query: {query.GetType()}");
		string jsonData = $"{{\"model\": \"gpt-4o-mini\", \"messages\": [{{\"role\": \"user\", \"content\": \"{query}\"}}], \"max_tokens\": 50}}";
		using (UnityWebRequest request = new UnityWebRequest(api, "POST"))
		{
			byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");
			request.SetRequestHeader("Authorization", $"Bearer {key}");

			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError
								|| request.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError($"Error: {request.error}\nResponse Code: {request.responseCode}");
			}
			else
			{	
				OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(request.downloadHandler.text);
				responseText = response.choices[0].message.content;
				Debug.Log($"Response: {responseText}");
			}
		}
	}
}