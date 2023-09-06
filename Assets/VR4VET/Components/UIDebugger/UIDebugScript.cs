using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class UIDebugScript : MonoBehaviour
{   
    //Declaring the taxt component and the list for the messages
    [SerializeField] TMP_Text TextElement;
    [SerializeField] List<string> LoggedMessages;
    
    //Declaring the method to print log messages to the text object
   public void debug(string DebugMessage){
    //Updates the list with logged messages
    LoggedMessages.Add("["+DateTime.Now+"] "+DebugMessage);
    //Adds the new message to the text object
    TextElement.text = LoggedMessages[LoggedMessages.Count-1]+"<br>"+TextElement.text;
   }
}
