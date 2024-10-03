using NUnit.Framework; 
using UnityEngine;
using System;
using DotNetEnv;

// Small sample unit test that checks whether the OpenAI key is null or empty, meaning that it has not been set in the .env file
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