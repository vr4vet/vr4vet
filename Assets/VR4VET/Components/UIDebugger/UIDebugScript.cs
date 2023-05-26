using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class UIDebugScript : MonoBehaviour
{
    [SerializeField] TMP_Text TextElement;
    [SerializeField] List<string> LoggedMessages;

   public void debug(string DebugMessage){
    LoggedMessages.Add("["+DateTime.Now+"] "+DebugMessage);
    TextElement.text = LoggedMessages[LoggedMessages.Count-1]+"<br>"+TextElement.text;
   }
}
