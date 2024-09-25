using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversationController: MonoBehaviour
{
    void Start()
    {
        Debug.Log("AIConversation script is running");
    }



    public void startRecording()
    {
        Debug.Log("Recording started");
    }

    
    public void endRecording()
    {
        Debug.Log("Recording ended");
        answerAI();
    }

    public void answerAI()
    {
        Debug.Log("AI Answered");
    }


}
