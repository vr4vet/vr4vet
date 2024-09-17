using System;
using UnityEngine;
using DotNetEnv;

public class DotenvPreloader : MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void OnBeforeSceneLoadRuntimeMethod()
	{
		Env.Load();
		string key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
		if (string.IsNullOrEmpty(key))
		{
			Debug.LogError("OpenAI API key not found");
		}
		else
		{
			Debug.Log("OpenAI API key loaded");
		}
	}
}