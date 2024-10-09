using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UpdateSubtitles : MonoBehaviour
{
	public TextMeshPro subtitles;

	void Start()
	{
		
	}

	public void UpdateInput(string input) {
		subtitles.text = input;
	}

	
}