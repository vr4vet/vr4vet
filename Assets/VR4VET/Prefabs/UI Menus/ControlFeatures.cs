/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class ControlFeatures : MonoBehaviour
{
    [SerializeField] private GameObject PlayerController;
    private PlayerTeleport _pt;
    private PlayerRotation _pr;
    private bool _flipMode = false;

    void Start()
    {
        _pt = (PlayerTeleport)PlayerController.GetComponent("PlayerTeleport");
        _pr = (PlayerRotation)PlayerController.GetComponent("PlayerRotation");


    }

    public void InverertedMode()
    {
        _flipMode = !_flipMode;
        
        if(_flipMode)
        {
            //lefty mode
            _pt.HandSide = ControllerHand.Right;
            _pr.inputAxis.Clear();
            _pr.inputAxis.Add(InputAxis.LeftThumbStickAxis);
        }
        else
        {
            //normal mode
            _pt.HandSide = ControllerHand.Left;
            _pr.inputAxis.Clear();
            _pr.inputAxis.Add(InputAxis.RightThumbStickAxis);
        }

    }


}
