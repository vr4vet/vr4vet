/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Quick script to call the the task mananger methods " FerdighetManager" 
 * its not possible to call them from the editor
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tablet;

public class TaskEventCaller : MonoBehaviour
{

    private bool[] accuracy = new bool[3];

   
    public void HelmetGrabbed ()
    {
        FerdighetManager.ferdighetManager.AddPoeng("Find the helmet", "Accuracy", 10);
        accuracy[0] = true;
    }
    public void HammperGrabbed()
    {
        FerdighetManager.ferdighetManager.AddPoeng("Grab the hammer", "Accuracy", 10);
        accuracy[1] = true;
       
    }

    public void VideoGrabbed()
    {
        FerdighetManager.ferdighetManager.AddPoeng("Watch the 360 video", "Accuracy", 10);
        accuracy[3] = true;
    }

    private void CheckMeessage()
    {
        int count = 0;
        foreach (bool flagg in accuracy){
            if (flagg == true)
                count++;
        }
        if (count == 3)
            FerdighetManager.ferdighetManager.GiveFeedback("Accuracy", "Your accuracy has been great!");
        else
            FerdighetManager.ferdighetManager.GiveFeedback("Accuracy", "You still have some task to finish");
    }

}
