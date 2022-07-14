/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InteractionSwitch : MonoBehaviour
{

    public GameObject RayHand;
    public GameObject DirectHand; 
   


    public void Test(string val)
    {
        Debug.Log("pressed"+val);
    }

    public void SwitchInteractionMethod()
    {
        DirectHand.SetActive(!DirectHand.activeSelf);
        RayHand.SetActive(!RayHand.activeSelf);

    }

 

}


