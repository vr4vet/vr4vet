using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressButton : MonoBehaviour
{
	public AIConversationController _AIConversationController;
	private ConversationController _ConversationController;

	void Start()
	{
		_AIConversationController = GetComponent<AIConversationController>();
		_ConversationController = GetComponentInChildren<ConversationController>();
	}

	public void StartRecording(InputAction.CallbackContext context)
	{
		if (context.started && _ConversationController.playerInsideTrigger)
		{
			// Debug.Log("Recording button clicked.");
			StartCoroutine(RecordingInput());
		}

		if (context.canceled)
		{
			_AIConversationController.EndRecording();
		}
	}

	IEnumerator RecordingInput()
	{
		yield return new WaitForSeconds(0.1f);
		_AIConversationController.StartRecording();
	}
}