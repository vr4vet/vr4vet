using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIConversationController : MonoBehaviour
{

    [SerializeField] private NPCManager _NPCManager;

    [SerializeField] private Transcribe _Transcribe;
    
    [TextArea(3, 10)]
    public string contextPrompt;
    public int maxTokens = 50;

    public List<Message> messages = new List<Message>();

    private ConversationController _ConversationController;

    
    void Start()
    {
        _NPCManager = GetComponent<NPCManager>();
        _Transcribe = GameObject.Find("TranscriptionManager").GetComponent<Transcribe>();
        _ConversationController = GetComponentInChildren<ConversationController>();
    }

    public void StartRecording()
    {
        // Passing current NPCManager object to the TranscriptionManager so that it correctly handles awaiting transcripts
        _Transcribe.StartRecording(_NPCManager);
        _NPCManager.StartRecording(contextPrompt, maxTokens);
    }

    // Function for pressing speech button in VR
    public void PressButton(InputAction.CallbackContext context)
	{
		if (context.started && _ConversationController.playerInsideTrigger)
		{
            StartRecording();
		}

		if (context.canceled)
		{
			EndRecording();
		}
	}


    public async void EndRecording()
    {
        _Transcribe.EndRecording();
    }

    public void AddMessage(Message message)
    {
        messages.Add(message);
    }

    IEnumerator RecordingInput()
	{
		yield return new WaitForSeconds(0.01f);
		StartRecording();
	}
}



public class PressButton : MonoBehaviour
{
	public AIConversationController _AIConversationController;
	

	

	

	
}
