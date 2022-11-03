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


    void Start()
    {
        _pt = (PlayerTeleport)PlayerController.GetComponent("PLayerTeleport");
        _pr = (PlayerRotation)PlayerController.GetComponent("PLayerRotation");


    }

    void InverertedMode()
    {

        _pt.HandSide = ControllerHand.Right;
        _pr.inputAxis.Clear();
        _pr.inputAxis.Add(InputAxis.LeftThumbStickAxis);

    }

    void OriginalMode()
    {
        _pt.HandSide = ControllerHand.Left;
        _pr.inputAxis.Clear();
        _pr.inputAxis.Add(InputAxis.RightThumbStickAxis);
    }


}
