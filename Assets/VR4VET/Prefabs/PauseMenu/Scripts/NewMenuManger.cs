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
    [SerializeField] private Material _walls; 

    private Camera _cam;
    private GameObject _aboutCanvas;
    private GameObject _menuCanvas;
    private bool _menuOpen = false;
    private InputAction _primaryButton;
    private float _height;
    private float _aboutHeight;

    private bool thething=false;

    void Start()
    {
        _aboutCanvas = transform.Find("AboutCanvas").gameObject;
        _menuCanvas = transform.Find("Canvas").gameObject;

        //get half of the height of the canvas to later use a position
        //just saving the initial height is also a good option 
        _height = _menuCanvas.GetComponent<RectTransform>().rect.height * _menuCanvas.GetComponent<RectTransform>().localScale.y /2;
        _aboutHeight = _aboutCanvas.GetComponent<RectTransform>().rect.height * _aboutCanvas.GetComponent<RectTransform>().localScale.y / 2;


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

    void Update()
    {
        this.transform.position = player.transform.position;//set origin of parent object 


        //set the menu to be on front of the player and looking toward them
        _menuCanvas.transform.position = _cam.transform.position + _cam.transform.forward * distanceToCamera;
        _menuCanvas.transform.LookAt(_menuCanvas.transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);

        _aboutCanvas.transform.position = _cam.transform.position + _cam.transform.forward * distanceToCamera * 1.3f; // move a bit further
        _aboutCanvas.transform.LookAt(_aboutCanvas.transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);

        //change the Y position to fix one (height) , and the X,Z rotation to 0 )

        _menuCanvas.transform.position = new Vector3(_menuCanvas.transform.position.x, _height, _menuCanvas.transform.position.z);
        _menuCanvas.transform.eulerAngles = new Vector3(0, _menuCanvas.transform.eulerAngles.y, 0);

        _aboutCanvas.transform.position = new Vector3(_aboutCanvas.transform.position.x, _aboutHeight, _aboutCanvas.transform.position.z);
        _aboutCanvas.transform.eulerAngles = new Vector3(0, _aboutCanvas.transform.eulerAngles.y, 0);




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


     IEnumerator UpdatePosition()
    {
        
        while (true)
        {
            this.transform.position = player.transform.position;//set origin of parent object 


            //set the menu to be on front of the player and looking toward them
            _menuCanvas.transform.position = _cam.transform.position + _cam.transform.forward * distanceToCamera;
            _menuCanvas.transform.LookAt(_menuCanvas.transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);

            _aboutCanvas.transform.position = _cam.transform.position + _cam.transform.forward * distanceToCamera * 1.3f; // move a bit further
            _aboutCanvas.transform.LookAt(_aboutCanvas.transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);

            //change the Y position to fix one (height) , and the X,Z rotation to 0 )

            _menuCanvas.transform.position = new Vector3(_menuCanvas.transform.position.x, _height, _menuCanvas.transform.position.z);
            _menuCanvas.transform.eulerAngles = new Vector3(0, _menuCanvas.transform.eulerAngles.y, 0);

            _aboutCanvas.transform.position = new Vector3(_aboutCanvas.transform.position.x, _aboutHeight, _aboutCanvas.transform.position.z);
            _aboutCanvas.transform.eulerAngles = new Vector3(0, _aboutCanvas.transform.eulerAngles.y, 0);

            yield return new WaitForSeconds(0.1f);

        }


    }


    }
