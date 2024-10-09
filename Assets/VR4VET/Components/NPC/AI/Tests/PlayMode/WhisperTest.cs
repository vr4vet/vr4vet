using NUnit.Framework; 
using UnityEngine;
using System;
using Whisper;
using System.IO;
using System.Collections;

// TODO
public class WhisperTest
{
	private WhisperManager whisperManager;
	private AudioSource audioSource;
    private AudioClip audioClip;
	private WhisperParams _params;
	private WhisperWrapper _whisper;
	private GameObject go;
	private string _modelPath = Path.Combine(Application.streamingAssetsPath, "Whisper/ggml-tiny.bin");

	 [SetUp]
        public void Setup()
        {
            _whisper = WhisperWrapper.InitFromFile(_modelPath);
            _params = WhisperParams.GetDefaultParams();
        }

    [Test]
    public void IsModelSet()
    {
		//var clip = AudioClip.Create();
		go = new GameObject();
		go.AddComponent<AudioSource>();
		audioSource = go.GetComponent<AudioSource>();
		audioSource.clip = Resources.Load("AudioFiles/NoInternet/English/alloy.mp3") as AudioClip;
		audioSource.Play();
        var res = _whisper.GetText(audioSource.clip, _params);
        Assert.NotNull(res);
        
    }

	
}


