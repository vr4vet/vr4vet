/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 * 
 * This methos calls the teleportastion ray and switches the inteaction method for the righ
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
    public GameObject reticle;

    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] InputActionAsset actionAsset;
    private InputAction thumbstick;
    private GameObject tablet;
    
    


    private void Start()
    {
        //assign the RayActive function to the whenever someone moves the left thumbstick
        rayInteractor.enabled = false;
        thumbstick = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        thumbstick.Enable();
        thumbstick.started += RayActivate;
        tablet= GameObject.Find("Tablet") ;
        
    }
 
    public void ToggleRightRayTablet()
    {
        var tabP = tablet.GetComponent<TabletPosition>();
        RayHandRight.SetActive(! tabP.gettabletIsOpened());
    }
    public void ToggleRightRay(bool mode)
    {
        RayHandRight.SetActive(mode);
    }



    //enables tabblet and the ray based on the Tablet status

    void RayActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;

    }


    //This method is called in the -Selected Exited on the Teleportation Area.
    public void RayCancel()
    {
        rayInteractor.enabled = false;
        reticle.SetActive(false);
        
    }

}


