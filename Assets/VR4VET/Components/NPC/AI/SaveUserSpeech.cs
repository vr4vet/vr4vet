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
    public const int MAX_RECORDTIME = 10;

	private const int SAMPLE_RATE = 16000;
    private string filePath;
    public bool isRecording = false;

	public void Start() {
		audioSource = GetComponent<AudioSource>();
	}

    public void StartRecording()
    {
        if (!isRecording)
        {
            isRecording = true;
            Record();
        }
    }

    public void EndRecording()
    {
        isRecording = false;
    }

    // Starts a recording of MAX_RECORDTIME seconds
    public void Record()
    {
        Debug.Log("Recording started");
        myAudioClip = Microphone.Start(null, false, MAX_RECORDTIME, SAMPLE_RATE);
        StartCoroutine(SaveWav());
    }

    IEnumerator SaveWav()
    {
        yield return new WaitUntil(() => !isRecording || Microphone.GetPosition(null) >= MAX_RECORDTIME * SAMPLE_RATE);
        Microphone.End(null);
        filePath = Path.Combine(Application.persistentDataPath, FILENAME);
        var byteArray = OpenWavParser.AudioClipToByteArray(myAudioClip);
        File.WriteAllBytes(filePath, byteArray);
        StartCoroutine(Transcribe());
    }

    IEnumerator Transcribe()
    {
        ReadInput ri = gameObject.AddComponent<ReadInput>();
        while (ri.transcript == null)
        {
            yield return new WaitForSeconds(1);
        }

        Debug.Log($"Dette er fra SUS: {ri.transcript}");
        AIRequest oai = gameObject.AddComponent<AIRequest>();
        oai.query = ri.transcript;

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