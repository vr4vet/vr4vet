/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 * 
 * This methos calls the teleportastion ray and switches the inteaction method
 * Interaction method: How you interact with the enviroment eg: a ray or with bare hands
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
    public GameObject Reticle;

    [SerializeField] private XRRayInteractor _rayInteractor;
    [SerializeField] private InputActionAsset _actionAsset;
    private InputAction _thumbstick;
    private GameObject _tablet;
    
    


    private void Start()
    {
        //assign the RayActive function to the whenever someone moves the left thumbstick
        _rayInteractor.enabled = false;
        _thumbstick = _actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        _thumbstick.Enable();
        _thumbstick.started += RayActivate;
        _tablet= GameObject.Find("Tablet") ;
        
    }
 
    public void ToggleRightRayTablet()
    {
        var tabP = _tablet.GetComponent<TabletPosition>();
        RayHandRight.SetActive(! tabP.gettabletIsOpened());
    }
    public void ToggleRightRay(bool mode)
    {
        RayHandRight.SetActive(mode);
    }



    //enables tabblet and the ray based on the Tablet status

    void RayActivate(InputAction.CallbackContext context)
    {
        _rayInteractor.enabled = true;

    }


    //This method is called in the -Selected Exited on the Teleportation Area.
    public void RayCancel()
    {
        _rayInteractor.enabled = false;
        Reticle.SetActive(false);
        
    }

}


