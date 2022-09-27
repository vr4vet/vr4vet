/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NewMenuManger : MonoBehaviour
{

    public GameObject player;

    public float distanceToCamera;
 

    [SerializeField] public Material PauseSkyboxMat;
    [SerializeField] public Material SkyboxMat;
    [SerializeField] private LayerMask _menuLayers;  //layers mask to put on top when the game is paused
    [SerializeField] private InputActionAsset _actionAsset; //we need this to block certain actions

    private Camera _cam;
    private GameObject _aboutCanvas;
    private GameObject _menuCanvas;
    private bool _menuOpen = false;
    private InputAction _primaryButton;

    void Start()
    {
        _aboutCanvas = transform.Find("AboutCanvas").gameObject;
        _menuCanvas = transform.Find("Canvas").gameObject;
        _cam = Camera.main;
        _menuCanvas.SetActive(false);
        _primaryButton = new InputAction();
        try
        {
            _primaryButton = _actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("PrimaryButton");
        }
        catch
        {
            Debug.Log("different controller");
        }


    }

    void Update()
    {
        transform.position = player.transform.position;

        //transform of menu canvas
        _menuCanvas.transform.position = _cam.transform.position + _cam.transform.forward * distanceToCamera;
        _menuCanvas.transform.LookAt(_menuCanvas.transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);

        _aboutCanvas.transform.position = _cam.transform.position + _cam.transform.forward * distanceToCamera;
        _aboutCanvas.transform.LookAt(_menuCanvas.transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);

    }

    public void ToggleMenu()
    {
        _menuOpen = !_menuOpen;

        if (_menuOpen)
            PauseGame();
        else
            ResumeGame();
    }

     void PauseGame()
    {
        Time.timeScale = 0; // pauses time events
        RenderSettings.skybox = PauseSkyboxMat;
        _cam.cullingMask = _menuLayers; //show only the chosen menu layers
        _menuCanvas.SetActive(true);
        _primaryButton.Disable();

    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        RenderSettings.skybox = SkyboxMat ;
        _cam.cullingMask = -1; // -1 = "Everything"
        _menuCanvas.SetActive(false);
        _aboutCanvas.SetActive(false);
        _primaryButton.Enable();

    }



    public void Restart()
    {
        // un-frezes the time and unblocks the player controller
        _primaryButton.Enable();
        Time.timeScale = 1;
        //back to the first scene
        SceneManager.LoadScene(0);



    }



    public void OpenAbout()
    {
        _aboutCanvas.SetActive(true);
        _menuCanvas.SetActive(false);
    }


    public void CloseAbout()
    {
        _aboutCanvas.SetActive(false);
        _menuCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
