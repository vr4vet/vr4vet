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
        string key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        Assert.IsFalse(string.IsNullOrEmpty(key), "OpenAI API key should not be null or empty.");
    }
}