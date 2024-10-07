using System;
using UnityEngine;
using System.IO;
using System.Collections;
using static ReadInput;
using static AIRequest;
using static OpenWavParser;

public class SaveUserSpeech : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip myAudioClip;
    public const string FILENAME = "conversation.wav";
    public const int MAX_RECORDTIME = 10; // Max recording time in seconds

    private const int SAMPLE_RATE = 12000; // Sample rate of the audio file, 8K to 16K is normal in realtime voice applications
    private string filePath;
    public bool isRecording = false;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartRecording(string contextPrompt, int maxTokens)
    {
        if (!isRecording)
        {
            isRecording = true;
            Record(contextPrompt, maxTokens);
        }
    }

    public void EndRecording()
    {
        isRecording = false;
    }

    // Starts a recording of MAX_RECORDTIME seconds
    public void Record(string contextPrompt, int maxTokens)
    {
        Debug.Log("Recording started.");
        myAudioClip = Microphone.Start(null, false, MAX_RECORDTIME, SAMPLE_RATE);
        StartCoroutine(SaveWav(contextPrompt, maxTokens));
    }

    IEnumerator SaveWav(string contextPrompt, int maxTokens)
    {
        // Wait until recording is stopped or maximum recording time is reached
        yield return new WaitUntil(() => !isRecording || Microphone.GetPosition(null) >= MAX_RECORDTIME * SAMPLE_RATE);
        Debug.Log("Recording stopped.");
        Microphone.End(null);
        filePath = Path.Combine(Application.persistentDataPath, FILENAME);
        var byteArray = OpenWavParser.AudioClipToByteArray(myAudioClip);
        File.WriteAllBytes(filePath, byteArray);
        StartCoroutine(Transcribe(contextPrompt, maxTokens));
    }

    // Coroutine for transcription
    IEnumerator Transcribe(string contextPrompt, int maxTokens)
    {
        ReadInput input = gameObject.AddComponent<ReadInput>();
        while (input.transcript == null)
        {
            yield return new WaitForSeconds(1);
        }

        Debug.Log($"Transcript: {input.transcript}");
        AIRequest request = gameObject.AddComponent<AIRequest>();
        request.query = input.transcript;
		request.contextPrompt = contextPrompt;
		request.maxTokens = maxTokens;

        // Delete the audio file after getting the transcript
        try
        {
            File.Delete(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting audio file: {e.Message}");
        }
    }
}