using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class CustomTeleportation : MonoBehaviour
{
    // code from this video https://www.youtube.com/watch?v=cxRnK8aIUSI




    [SerializeField] InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider;
    private InputAction _thumbstick;
    InputAction movement;
    private bool _isActive = false ;

    void Start()
    {

        rayInteractor.enabled = false;
        var activate = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportationActivate;

        var cancel = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Activate");
     //   cancel.Enable();
    //    cancel.canceled += OnTeleportationCancel;

        _thumbstick = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        _thumbstick.Enable();


        //   movement = gameplayActionMap.FindAction("Move");
        //   movement.started += TeleportRayActive;
        //    movement.Enable();

    }

    void Update()
    {
        if (!_isActive)
            return;

        if (_thumbstick.triggered)
        {
            Debug.Log("thumb being pressed ");
    
            return;
        }
            

        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            rayInteractor.enabled = false;
            _isActive = false;
            return;
        }

        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point ,
        };


        provider.QueueTeleportRequest(request);
    //    _isActive = false;
    //    rayInteractor.enabled = false;
    }

    void OnTeleportationActivate (InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        _isActive = true;
    }

    void OnTeleportationCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        _isActive = false;
        Debug.Log("TP CANCELED");
    }




    
}
