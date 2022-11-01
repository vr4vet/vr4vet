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

      [SerializeField]public GameObject player;

 

    [SerializeField] public Material PauseSkyboxMat;
    [SerializeField] public Material SkyboxMat;
    [SerializeField] private LayerMask _menuLayers;  //layers mask to put on top when the game is paused
    [SerializeField] private InputActionAsset _actionAsset; //we need this to block certain actions
    [SerializeField] private Material _walls; 

    private Camera _cam;
    private GameObject _aboutCanvas;
    private GameObject _menuCanvas;
    private bool _menuOpen = false;
    private InputAction _primaryButton;


  

    void Start()
    {
        _aboutCanvas = transform.Find("AboutCanvas").gameObject;
        _menuCanvas = transform.Find("Canvas").gameObject;

        //get half of the height of the canvas to later use a position
        //just saving the initial height is also a good option 



        _cam = Camera.main;
        _menuCanvas.SetActive(false);
        _primaryButton = new InputAction();
        try
        {
            _primaryButton = _actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("PrimaryButton");
        }
        catch
        {
            Debug.Log("not using XRI currently");
        }
       
        // InvokeRepeating("UpdatePosition", 0.1f, 0.1f);

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
        Color c = _walls.color;
        c.a = 0.7f;
        _walls.color = c;
        Time.timeScale = 0; // pauses time events
        RenderSettings.skybox = PauseSkyboxMat;
        _cam.cullingMask = _menuLayers; //show only the chosen menu layers
        _menuCanvas.SetActive(true);
        _primaryButton.Disable();

    }

    void ResumeGame()
    {
        Color c = _walls.color;
        c.a = 1f;
        _walls.color = c;
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
