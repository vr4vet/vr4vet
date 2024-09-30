using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressButton : MonoBehaviour
{
	public AIConversationController controller;

	void Start()
	{
		controller = GetComponent<AIConversationController>();
	}

	public void StartRecording(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Debug.Log("button clicked");
			StartCoroutine(RecordingInput());
		}

		if (context.canceled)
		{
			controller.endRecording();
		}
	}

	IEnumerator RecordingInput()
	{
		yield return new WaitForSeconds(0.1f);
		controller.startRecording();
	}
}