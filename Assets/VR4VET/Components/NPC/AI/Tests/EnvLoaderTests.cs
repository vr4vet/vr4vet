using NUnit.Framework;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

// Tests for loading environment variable (for OpenAI key)
public class PreLoadTests
{
    private const string TestEnvFileName = ".env_test";
    private string testEnvFilePath;

    [SetUp]
    public void SetUp()
    {
        // Set up a temporary .env file
        string projectRootPath = Path.Combine(Application.dataPath, "..");
        testEnvFilePath = Path.Combine(projectRootPath, TestEnvFileName);

        // Write environment variables to the file
        File.WriteAllText(testEnvFilePath, "TEST_KEY=TestValue\nANOTHER_KEY=AnotherValue");
    }

    [TearDown]
    public void TearDown()
    {
        // Delete the temporary.env file after each test
        if (File.Exists(testEnvFilePath))
        {
            File.Delete(testEnvFilePath);
        }

        // Clear environment variables set during the test
        Environment.SetEnvironmentVariable("TEST_KEY", null);
        Environment.SetEnvironmentVariable("ANOTHER_KEY", null);
    }

    [Test]
    public void GetEnvironmentVariablesBeforeLoad()
    {
        // Check that environment variables are not set before calling Load
        Assert.IsNull(Environment.GetEnvironmentVariable("TEST_KEY"));
        Assert.IsNull(Environment.GetEnvironmentVariable("ANOTHER_KEY"));
    }

    [Test]
    public void GetEnvironmentVariablesAfterLoad()
    {
        EnvLoader.Load(TestEnvFileName);

        // Check that the environment variables are set as expected
        Assert.AreEqual("TestValue", Environment.GetEnvironmentVariable("TEST_KEY"));
        Assert.AreEqual("AnotherValue", Environment.GetEnvironmentVariable("ANOTHER_KEY"));
    }
}
