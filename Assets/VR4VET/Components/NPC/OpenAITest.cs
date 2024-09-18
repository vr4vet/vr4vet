using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAITest : MonoBehaviour
{
	private string api = "https://api.openai.com/v1/chat/completions";
	private string key;

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
		string query = "Hei. Kan du hjelpe meg med fisking?";
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
				Debug.Log($"OpenAI Response: {request.downloadHandler.text}");
			}
		}
	}
}