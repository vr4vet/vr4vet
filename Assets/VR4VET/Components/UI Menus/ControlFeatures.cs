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
    //using boolean variables to store the settings value, making it easier to work with toggles 
    private bool _flipModeB = false;
    private bool _snapRotationB= true;
    private bool _teleportRotationB = true;
    private bool _teleportPressB = false;
    void Start()
    {
        _pt = (PlayerTeleport)PlayerController.GetComponent("PlayerTeleport");
        _pr = (PlayerRotation)PlayerController.GetComponent("PlayerRotation");
        

    }

    //switiching the teleport and roation hand 
    public void InverertedMode()
    {
        _flipModeB = !_flipModeB;
        
        if(_flipModeB)
        {
            //lefty mode
            _pt.HandSide = ControllerHand.Right;
            //clear the imput list for rotation and adding the left thumstick
            _pr.inputAxis.Clear();
            _pr.inputAxis.Add(InputAxis.LeftThumbStickAxis);
        }
        else
        {
            //normal mode
            _pt.HandSide = ControllerHand.Left;
            //clear the imput list for rotation and adding the right thumstick
            _pr.inputAxis.Clear();
            _pr.inputAxis.Add(InputAxis.RightThumbStickAxis);
        }

    }

    //snaprotation on/off
    public void SnapRotationMode()
    {
        _snapRotationB = !_snapRotationB;

        if (_snapRotationB)
        {
            _pr.AllowInput = true;
        }
        else
        {
            _pr.AllowInput = false;
        }

    }

    //directional deleportaiton on/off
    public void DirectionalTeleportationMode()
    {
        _teleportRotationB = !_teleportRotationB;

        if (_teleportRotationB)
        {
            _pt.AllowTeleportRotation = true;
        }
        else
        {
            _pt.AllowTeleportRotation = false;
        }

    }

    public void TeleportInput()
    {
        _teleportPressB = !_teleportPressB;

        if (_teleportPressB)
        {
            _pt.ControlType = TeleportControls.ThumbstickDown;
        }
        else
        {
            _pt.ControlType = TeleportControls.ThumbstickRotate;
        }

    }

}
