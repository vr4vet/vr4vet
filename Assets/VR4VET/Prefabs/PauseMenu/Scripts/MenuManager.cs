/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuManager : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Sky;
    [SerializeField] private GameObject menuLight;
    [Header ("Settings")]
    [Range(5,15)]
    [SerializeField] private int distanceToCamera = 10;
    [SerializeField] private LayerMask menuLayers;
    [SerializeField] private bool SoundEffect = true;
    [TextArea(5, 45)]
    [SerializeField] private string about;
    [Header("Image")]
    [SerializeField] private Sprite defualtButtonBG;
    [Space(5)]
    [SerializeField] private Image resumeButtonImage;
    [SerializeField] private Image catalogButtonImage;
    [SerializeField] private Image aboutButtonImage;
    [SerializeField] private Image exitButtonImage;
    [Header("UI")]
    [SerializeField] private Text aboutText;

    private GameObject menu;
    private GameObject warningMenu;
    private GameObject AboutCanvas;
    private LayerMask oldLayers;
    private Camera cam;
    private bool menuIsOpened;
    private Light[] lights;
    private string YesButtonMode;

    /// <summary>
    /// Unity Start method
    /// </summary>
    private void Start()
    {
        menu = transform.Find("MainCanvas").gameObject;
        warningMenu = transform.Find("WarningCanvas").gameObject;
        AboutCanvas = transform.Find("_aboutCanvas").gameObject;

        if (!Camera.main) Debug.LogError("Assign 'MainCamera' tag to your main camera.");

        menu.GetComponent<Canvas>().worldCamera = Camera.main;
        warningMenu.GetComponent<Canvas>().worldCamera = Camera.main;
        AboutCanvas.GetComponent<Canvas>().worldCamera = Camera.main;

        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        oldLayers = cam.cullingMask;

        lights = GameObject.FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
                light.cullingMask = ~(1 << LayerMask.NameToLayer("Menu"));
        }

        if (!SoundEffect) GetComponent<AudioSource>().enabled = false;

        aboutText.text = about;
    }

    /// <summary>
    /// Unity Update method
    /// </summary>
    private void Update()
    {
        transform.position = player.transform.position;

        //transform of menu canvas
        menu.transform.position = cam.transform.position + cam.transform.forward * distanceToCamera;
        menu.transform.LookAt(menu.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

        //transform of warning canvas
        warningMenu.transform.position = menu.transform.position;
        warningMenu.transform.rotation = menu.transform.rotation;

        //transform av about canvas
        AboutCanvas.transform.position = menu.transform.position * 1.2f;
        AboutCanvas.transform.rotation = menu.transform.rotation;
    }


    /// <summary>
    /// Reset the background image of the buttons to defualt image
    /// </summary>
    public void ResetButtons()
    {
        resumeButtonImage.sprite = defualtButtonBG;
        catalogButtonImage.sprite = defualtButtonBG;
        exitButtonImage.sprite = defualtButtonBG;
        aboutButtonImage.sprite = defualtButtonBG;
    }


    /// <summary>
    /// Open and close the menu 
    /// </summary>
    public void ToggleMenu()
    {
        if (!menuIsOpened) //Open menu
        {
           // Time.timeScale = 0;
            cam.cullingMask = menuLayers;
            menuIsOpened = true;
            menu.transform.Rotate(player.transform.forward, Space.World);
            warningMenu.SetActive(!menuIsOpened);
            AboutCanvas.SetActive(!menuIsOpened);
        }
        else //Close menu
        {
          //  Time.timeScale = 1;
            cam.cullingMask = oldLayers;
            menuIsOpened = false;
            menu.SetActive(menuIsOpened);
            warningMenu.SetActive(menuIsOpened);
            AboutCanvas.SetActive(menuIsOpened);
        }

        //shoe the menu environment
        ResetButtons();
        Sky.SetActive(menuIsOpened);
        menuLight.SetActive(menuIsOpened);
        menu.SetActive(menuIsOpened);

        //Deavtive players movment while menu is open
        if (player.GetComponent<LocomotionProvider>())
            player.GetComponent<LocomotionProvider>().enabled = !menuIsOpened;
    }


    /// <summary>
    /// Close the warning panelVi 
    /// </summary>
    public void CloseWarning()
    {
        warningMenu.SetActive(false);
        menu.SetActive(true);
    }


    /// <summary>
    /// Ask the user about exiting the application
    /// the warning menu will be opened
    /// </summary>
    /// <param name="mode"></param>
    public void SureToExit(string mode)
    {
        YesButtonMode = mode;
        warningMenu.SetActive(true);
        menu.SetActive(false);

        Text questionUIText = warningMenu.transform.Find("Question").GetComponent<Text>();
        if (YesButtonMode == "exit")
            questionUIText.text = "Er du sikker på at du vil avslutte appen?";
        else if (YesButtonMode == "launcher")
            questionUIText.text = "Er du sikker på at du vil avslutte appen,\n og gå tilbake til katalogen?";
        else if (YesButtonMode == "restart")
            questionUIText.text = "Er du sikker på at du vil starte appen på nytt?";

    }


    /// <summary>
    /// Yes answer to the warning question
    /// </summary>
    public void IAmSure()
    {
        if (YesButtonMode == "exit")
            ExitGame();
        else if (YesButtonMode == "launcher")
            openCatalog();
        else if (YesButtonMode == "restart")
            Restart();
    }


    /// <summary>
    /// Exit the application
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }


    public void OpenAbout()
    {
        AboutCanvas.SetActive(true);
        menu.SetActive(false);
        warningMenu.SetActive(false);
    }


    public void CloseAbout()
    {
        AboutCanvas.SetActive(false);
        menu.SetActive(true);
    }



    /// <summary>
    /// Exit the application and open the luancher
    /// </summary>
    public void openCatalog()
    {
        string path = Directory.GetParent(Directory.GetParent(Application.dataPath).Parent.FullName).FullName  + "\\Launcher.exe";
        if (File.Exists(path))
        {
            System.Diagnostics.Process.Start(path);
            ExitGame();
        }else
        {
            Debug.LogError(path + " does not exist.");
        }

    }


    /// <summary>
    /// Restart the game
    /// </summary>
    public void Restart()
    {
        /* if you need do something before restart the game just do it here */




        /////
        //load the first scene
        SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).name);

    }
}
