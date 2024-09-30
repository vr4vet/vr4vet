using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversationController : MonoBehaviour
{

    [SerializeField] private SaveUserSpeech _SaveUserSpeech;

    void Start()
    {
        _SaveUserSpeech = GetComponent<SaveUserSpeech>();
    }

    public void startRecording()
    {
        _SaveUserSpeech.StartRecording();
    }


    public void endRecording()
    {
        _SaveUserSpeech.EndRecording();
    }
}
