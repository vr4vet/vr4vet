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

    public GameObject RayHandRight;
    public GameObject DirectHandRight;
    public TeleportationProvider tpProvider;

    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] InputActionAsset actionAsset;
    private InputAction thumbstick;


    private void Start()
    {
        rayInteractor.enabled = false;
        thumbstick = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Activate");
        thumbstick.Enable();
        thumbstick.performed += RayActivate;

    }
 
    public void SwitchInteractionMethod()
    {
       // DirectHandRight.SetActive(!DirectHandRight.activeSelf);
        RayHandRight.SetActive(!RayHandRight.activeSelf);
    //    tpProvider.enabled = ! tpProvider.isActiveAndEnabled;
    }
    void RayActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
     
    }

   public void RayCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;

    }

    public void CancelMePleaseThanks()
    {
        rayInteractor.enabled = false;

    }

}


