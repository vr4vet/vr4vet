using System;
using UnityEngine;
using DotNetEnv;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
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
	private string filePath;
	public bool isRecording = false;


	public void StartRecording() {
		if (!isRecording) {
				isRecording = true;
				Record();
			}
	}

	public void EndRecording() {
		isRecording = false;
	}

	// Starts a recording of MAX_RECORDTIME seconds
	public void Record() {
		Debug.Log("Recording started");
		audioSource = GetComponent<AudioSource>();
		myAudioClip = audioSource.clip;
        myAudioClip = Microphone.Start(null, false, MAX_RECORDTIME, 44100);
		StartCoroutine(SaveWavMax());
		StartCoroutine(SaveWavEarly());
	}

	public void SaveWav() {
		filePath = Path.Combine(Application.persistentDataPath, FILENAME);
		var byteArray = OpenWavParser.AudioClipToByteArray(myAudioClip);
		File.WriteAllBytes(filePath, byteArray);
	}

	IEnumerator SaveWavMax() {
		yield return new WaitForSeconds(MAX_RECORDTIME);
		SaveWav();
		StopAllCoroutines();
		StartCoroutine(Transcribe());
	}

	IEnumerator SaveWavEarly() {
		while (isRecording) {
			yield return new WaitForSeconds(0.1f);
		}
		Microphone.End(null);
		SaveWav();
		StopAllCoroutines();
		StartCoroutine(Transcribe());
	}

	IEnumerator Transcribe() {
		ReadInput ri = gameObject.AddComponent<ReadInput>() as ReadInput;
		while (ri.transcript == null) {
			yield return new WaitForSeconds(1);
		}
		Debug.Log($"Dette er fra SUS: {ri.transcript}");
		AIRequest oai = gameObject.AddComponent<AIRequest>() as AIRequest;
		oai.query = ri.transcript;

		// Delete audio file from disk after transcribing.
		// If file not present, File.Delete() throws no errors - it simply does nothing.
		File.Delete(filePath);
	}
}