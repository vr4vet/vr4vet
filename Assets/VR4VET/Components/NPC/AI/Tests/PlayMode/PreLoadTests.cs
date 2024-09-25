using NUnit.Framework; 
using UnityEngine;
using System;
using DotNetEnv;

public class PreLoadTests
{
    [Test]
    public void IsKeyRetrieved()
    {
        Env.Load();
        string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        Assert.IsFalse(string.IsNullOrEmpty(apiKey), "OpenAI API key should not be null or empty.");
    }
}