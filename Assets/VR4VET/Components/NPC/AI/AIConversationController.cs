using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversationController : MonoBehaviour
{

    [SerializeField] private SaveUserSpeech _SaveUserSpeech;
    public string contextPrompt;
    public int maxTokens = 50;

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
}
