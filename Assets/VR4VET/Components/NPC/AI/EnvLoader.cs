using System;
using System.IO;
using UnityEngine;

public static class EnvLoader
{
    // Function for loading the environment variable defined in the project's root
    public static void Load(string fileName = ".env")
    {
        string projectRootPath = Path.Combine(Application.dataPath, "..");
        string filePath = Path.Combine(projectRootPath, fileName);

        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.LogWarning($"Environment file '{filePath}' not found.");
            return;
        }

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            var parts = line.Split('=', 2);
            if (parts.Length == 2)
            {
                Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
            }
        }

        UnityEngine.Debug.Log("Environment variables loaded from .env file.");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void OnBeforeSceneLoadRuntimeMethod()
	{
		Load();
	}
}