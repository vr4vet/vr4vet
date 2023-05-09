/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class NewMenuManger : MonoBehaviour
{

    [SerializeField]public GameObject player;
    [SerializeField] public Material PauseSkyboxMat;
    [SerializeField] public Material SkyboxMat;
    [SerializeField] private LayerMask _menuLayers;  //layers mask to put on top when the game is paused
    [SerializeField] private Material _wallsMaterial;
    [SerializeField] private bool _holdToOpen;


    // Defined in Unity, refers to image used in loading animation.
    [SerializeField] private Image LoadingWheel;

    private Camera _cam;
    [SerializeField] private GameObject _menuCanvas;
    public GameObject stateSaverComponent;

    private GameObject _savedStates;
    private bool _menuOpen = false;
    private float _holdtime = 1.5f;


    /// <summary>
    /// This Script manages all aspects of the Pause Menu:
    /// Toggle, or Hold to Pause
    /// Change transparency of material while pausing
    /// </summary>

    void Start()
    {
               
        _cam = Camera.main;

        Color c = _wallsMaterial.color;
        c.a = 1f;
        _wallsMaterial.color = c;

    }


    private void ToggleMenu()
    {
        _menuOpen = !_menuOpen;

        if (_menuOpen)
            PauseGame();
        else
            ResumeGame();
    }

     void PauseGame()
    {
        Color c = _wallsMaterial.color;
        c.a = 0.8f;
        _wallsMaterial.color = c;
        Time.timeScale = 0; // pauses time events
        RenderSettings.skybox = PauseSkyboxMat;
        _cam.cullingMask = _menuLayers; //show only the chosen menu layers
        _menuCanvas.SetActive(true);
     

    }

    public void ResumeGame()
    {
        Color c = _wallsMaterial.color;
        c.a = 1f;
        _wallsMaterial.color = c;
        Time.timeScale = 1;
        RenderSettings.skybox = SkyboxMat ;
        _cam.cullingMask = -1; // -1 = "Everything"
        _menuCanvas.SetActive(false);



    }


  

    public void Restart()
    {
        // un-frezes the time and unblocks the player controller
        Time.timeScale = 1;
        //back to the first scene
        SceneManager.LoadScene(0);



    }



    public void OpenSaves()
    {
        _savedStates.SetActive(true);
        _menuCanvas.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            if (_savedStates.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Text>().text != "")
            {
                _savedStates.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void CloseSaves()
    {
        _savedStates.SetActive(false);
        _menuCanvas.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            _savedStates.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ExitGame()
    {
        stateSaverComponent.GetComponent<stateSaver>().saveObjects();
        Application.Quit();
    }



    public void PressHoldMenu(InputAction.CallbackContext context)
    {
        if (_holdToOpen)
        {
            if (context.started)
            {
                StartCoroutine(HoldPause());
            }
        } else
        {
            ToggleMenu();
        }
       
    }

    // Loading wheel to open the pause menu
    public IEnumerator HoldPause()
    {

        for (float I = 0f; I < _holdtime; I += Time.deltaTime)
        {

            // Get left-handed device and its current state (pressed or not).
            UnityEngine.XR.InputDevice _rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            _rightDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool triggerValue);

            // Fill LoadingWheel if trigger is pressed
            if (triggerValue)
            {
                LoadingWheel.fillAmount = I;
            }

            // Open menu if it has been continuously pressed.
            if (I >= 1f)
            {

                LoadingWheel.fillAmount = 0f;
                I = _holdtime+1;

                PauseGame();
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
