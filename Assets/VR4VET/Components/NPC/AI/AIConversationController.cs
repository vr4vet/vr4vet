using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversationController : MonoBehaviour
{

    [SerializeField] private SaveUserSpeech _SaveUserSpeech;

    
    [TextArea(3, 10)]
    public string contextPrompt;
    public int maxTokens = 50;

    public List<Message> messages = new List<Message>();

    

    void Start()
    {
        _SaveUserSpeech = GetComponent<SaveUserSpeech>();
    }

    public void StartRecording()
    {
        _SaveUserSpeech.StartRecording(contextPrompt, maxTokens);
    }


    public void EndRecording()
    {
        _SaveUserSpeech.EndRecording();
    }

    public void AddMessage(Message message)
    {
        messages.Add(message);
    }
}
