using NUnit.Framework;
using UnityEngine;

// Tests for various audio functionality
public class AudioTests
{
	private AudioSource audioSource;
	private AudioClip testClip;
	private string microphoneName;
	private string[] audioFileNames = { "alloy", "echo", "fable", "nova", "onyx", "shimmer" };

	[SetUp]
	public void Setup()
	{
		// Set up an AudioSource component for testing
		GameObject audioObject = new GameObject("AudioTestObject");
		audioSource = audioObject.AddComponent<AudioSource>();

		// Find available microphones
		if (Microphone.devices.Length > 0)
		{
			microphoneName = Microphone.devices[0];
			Debug.Log("Microphone found: " + microphoneName);
		}
		else
		{
			Assert.Fail("No microphone detected.");
		}

		// Load a sample audio clip for testing
		testClip = Resources.Load<AudioClip>("TestAudioClip");
		Assert.IsNotNull(testClip, "Test audio clip could not be loaded.");
		audioSource.clip = testClip;
	}

	// Checks that the no internet audio files exist
	[Test]
	public void GetAudioFiles()
	{
		foreach (var fileName in audioFileNames)
		{
			AudioClip audioClip = Resources.Load<AudioClip>($"AudioFiles/NoInternet/English/{fileName}");
			Assert.IsNotNull(audioClip, $"Audio file '{fileName}' is missing or not found in Resources/AudioFiles/NoInternet.");
		}
	}

	// Tests the microphone
	[Test]
	public void RecordMicrophone()
	{
		// Start recording with the microphone if available
		if (microphoneName != null)
		{
			audioSource.clip = Microphone.Start(microphoneName, false, 2, 44100);
			Assert.IsTrue(Microphone.IsRecording(microphoneName), "Microphone did not start recording.");
			Microphone.End(microphoneName);

			// Check that the audio clip has been created and contains some data
			Assert.IsNotNull(audioSource.clip, "Audio clip is null after recording.");
			Assert.Greater(audioSource.clip.samples, 0, "Audio clip has no data after recording.");
		}
		else
		{
			Assert.Fail("No microphone available for testing.");
		}
	}

	// Tests the headphones
	[Test]
	public void PlaybackHeadphones()
	{
		// Verify that the clip is assigned and try playing it
		Assert.IsNotNull(audioSource.clip, "AudioSource has no audio clip assigned.");
		audioSource.Play();
		Assert.IsTrue(audioSource.isPlaying, "AudioSource did not start playing.");

		// Verify playback state immediately without waiting for it to finish
		Assert.AreEqual(testClip, audioSource.clip, "AudioSource is not playing the expected test clip.");
	}
}