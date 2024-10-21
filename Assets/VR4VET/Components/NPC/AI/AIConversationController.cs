using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversationController : MonoBehaviour
{

    [SerializeField] private NPCManager _NPCManager;

    [SerializeField] private Transcribe _Transcribe;
    
    [TextArea(3, 10)]
    public string contextPrompt;
    public int maxTokens = 50;

    public List<Message> messages = new List<Message>();

    
    void Start()
    {
        _NPCManager = GetComponent<NPCManager>();
        _Transcribe = GameObject.Find("TranscriptionManager").GetComponent<Transcribe>();
    }

    public void StartRecording()
    {
        // Passing current NPCManager object to the TranscriptionManager so that it correctly handles awaiting transcripts
        _Transcribe.StartRecording(_NPCManager);
        _NPCManager.StartRecording(contextPrompt, maxTokens);
    }


    public async void EndRecording()
    {
        _Transcribe.EndRecording();
    }

    public void AddMessage(Message message)
    {
        messages.Add(message);
    }
}
