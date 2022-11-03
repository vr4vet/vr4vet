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
    [SerializeField] private GameObject _aboutCanvas;
    [SerializeField] private GameObject _menuCanvas;
    private bool _menuOpen = false;



  

    void Start()
    {
               
        _cam = Camera.main;
  

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


    }

    public void Restart()
    {
        // un-frezes the time and unblocks the player controller
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
