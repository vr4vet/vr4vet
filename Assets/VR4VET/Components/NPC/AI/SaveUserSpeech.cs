using System;
using UnityEngine;
using DotNetEnv;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static ReadInput;
using static OpenAITest;
using static OpenWavParser;

public class SaveUserSpeech : MonoBehaviour
{	
	private AudioSource audioSource;
	private AudioClip myAudioClip;
	public const string FILENAME = "conversation.wav";
	public const int RECORDTIME = 5;

	void Start()
    {
		audioSource = GetComponent<AudioSource>();
		myAudioClip = audioSource.clip;
        myAudioClip = Microphone.Start(null, false, 5, 44100);
        audioSource.Play();
		
		StartCoroutine(SaveWav());
    }

	IEnumerator SaveWav() {
		yield return new WaitForSeconds(RECORDTIME);
		var byteArray = OpenWavParser.AudioClipToByteArray(myAudioClip);
    	string filePath = Path.Combine(Application.persistentDataPath, FILENAME);
		File.WriteAllBytes(filePath, byteArray);

		ReadInput ri = gameObject.AddComponent<ReadInput>() as ReadInput;
		while (ri.transcript == null) {
			yield return new WaitForSeconds(1);
		}
		Debug.Log($"Dette er fra SUS: {ri.transcript}");
		OpenAITest oai = gameObject.AddComponent<OpenAITest>() as OpenAITest;
		oai.query = ri.transcript;

		// Delete audio file from disk after transcribing.
		// If file not present, File.Delete() throws no errors - it simply does nothing.
		File.Delete(filePath);
	}
}