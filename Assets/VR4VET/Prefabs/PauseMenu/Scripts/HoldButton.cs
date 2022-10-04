//Snorre Soknes Forbregd - NTNU IMTEL VR-Lab Dragvoll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;



public class HoldButton : MonoBehaviour
{

    // Defined in Unity, refers to image used in loading animation.
    public Image LoadingWheel;


    // When Open Menu action triggers, start Run() coroutine.
    public void OpenMenu(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            StartCoroutine(Run());
        }
    }

    // Coroutine for checking button state.
    IEnumerator Run()
    {

        for (float I = 0f; I < 1.5f; I += Time.deltaTime)
        {

            // Get left-handed device and its current state (pressed or not).
            UnityEngine.XR.InputDevice _leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            _leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerValue);

            // Fill LoadingWheel if trigger is pressed
            if (triggerValue)
            {
                LoadingWheel.fillAmount = I;
            }

            // Open menu if it has been continuously pressed.
            if (I >= 1f)
            {

                LoadingWheel.fillAmount = 0f;
                I = 1.6f;


                // Add code to open menu here.
            }

            // If trigger is no longer pressed, reset the LoadingWheel.
            if ((!triggerValue))
            {

                LoadingWheel.fillAmount = 0f;
                I = 1.6f;
            }
            yield return null;
        }
    }

}