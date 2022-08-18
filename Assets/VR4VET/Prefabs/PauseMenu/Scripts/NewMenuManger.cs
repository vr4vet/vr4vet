/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Based on code made by: Abbas Jafari
 * Ask your questions by email: jorgeega@ntnu.no
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMenuManger : MonoBehaviour
{

    public GameObject player;

    public float distanceToCamera;
 

    [SerializeField] public Material PauseSkyboxMat;
    [SerializeField] public Material SkyboxMat;
    [SerializeField] private LayerMask menuLayers;  //layers mask to put on top when the game is paused

    private Camera cam;
    private GameObject AboutCanvas;
    private GameObject MenuCanvas;
    private bool menuOpen = false;
    private LayerMask oldLayers;
    Animator anim;


    void Start()
    {
        AboutCanvas = transform.Find("AboutCanvas").gameObject;
        MenuCanvas = transform.Find("Canvas").gameObject;
        cam = Camera.main;
        MenuCanvas.SetActive(false);
    }

    void Update()
    {
        transform.position = player.transform.position;

        //transform of menu canvas
        MenuCanvas.transform.position = cam.transform.position + cam.transform.forward * distanceToCamera;
        MenuCanvas.transform.LookAt(MenuCanvas.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }

    public void ToggleMenu()
    {
        menuOpen = !menuOpen;

        if (menuOpen)
            PauseGame();
        else
            ResumeGame();
    }

     void PauseGame()
    {
        Time.timeScale = 0; // pauses time events
        RenderSettings.skybox = PauseSkyboxMat;
        cam.cullingMask = menuLayers; //show only the chosen menu layers
        MenuCanvas.SetActive(true);


    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        RenderSettings.skybox = SkyboxMat ;
        cam.cullingMask = -1; // -1 = "Everything"
        MenuCanvas.SetActive(false);

    }



    public void Restart()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene(0);

        // reload the current scene for good messuare
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 


    }



    public void OpenAbout()
    {
        AboutCanvas.SetActive(true);
        MenuCanvas.SetActive(false);
    }


    public void CloseAbout()
    {
        AboutCanvas.SetActive(false);
        MenuCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
