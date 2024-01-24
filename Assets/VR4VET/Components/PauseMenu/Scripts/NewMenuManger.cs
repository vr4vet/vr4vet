/* Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class NewMenuManger : MonoBehaviour
{
    [SerializeField] public Material PauseSkyboxMat;
    [SerializeField] public Material SkyboxMat;
    [SerializeField] private LayerMask _menuLayers;  //layers mask to put on top when the game is paused
    [SerializeField] private Material _wallsMaterial;
    [SerializeField] private bool _holdToOpen;

    // Defined in Unity, refers to image used in loading animation.
    [SerializeField] private Image LoadingWheel;

    private Camera _cam;
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _aboutCanvas;
    [SerializeField] private GameObject _languagesCanvas;
    [SerializeField] private GameObject _remapCanvas;
    [SerializeField] private GameObject _audioCanvas;
    // public GameObject stateSaverComponent;

    private GameObject _savedStates;
    [SerializeField]
    private bool _menuOpen = false;
    private float _holdtime = 1.5f;

    private List<GameObject> allMenus = new();
    [SerializeField]
    private float canvasesDistance;
    /// <summary>
    /// This Script manages all aspects of the Pause Menu:
    /// Toggle, or Hold to Pause
    /// Change transparency of material while pausing
    /// </summary>

    private void Start()
    {
        _cam = Camera.main;

        Color c = _wallsMaterial.color;
        c.a = 1f;
        _wallsMaterial.color = c;

        allMenus.AddRange(new List<GameObject>() { _menuCanvas, _settingsCanvas, _aboutCanvas, _languagesCanvas, _remapCanvas, _audioCanvas });
        AdjustCanvasDistances();

        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        _menuCanvas.SetActive(true);
        _menuOpen = true;
    }

    private void ToggleMenu()
    {
        _menuOpen = !_menuOpen;

        if (_menuOpen)
            PauseGame();
        else
            ResumeGame();
    }

    private void PauseGame()
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
        _menuOpen = false;
        Color c = _wallsMaterial.color;
        c.a = 1f;
        _wallsMaterial.color = c;
        Time.timeScale = 1;
        RenderSettings.skybox = SkyboxMat;
        _cam.cullingMask = -1; // -1 = "Everything"
        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
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
        // stateSaverComponent.GetComponent<stateSaver>().saveObjects();
        Application.Quit();
    }

    public void PressHoldMenu(InputAction.CallbackContext context)
    {
        if (_holdToOpen && !_menuOpen)
        {
            if (context.started)
            {
                StartCoroutine(HoldPause(context));
            }
        }
        else if (context.performed)
        {
            ToggleMenu();
        }
    }

    // Loading wheel to open the pause menu
    private IEnumerator HoldPause(InputAction.CallbackContext context)
    {
        for (float I = 0f; I < _holdtime; I += Time.deltaTime)
        {
            // Yes, this is a try catch checking if it can log the context variable.
            // Yes, this is to check if the user releases the button.
            // Yes, this is extremely stupid.
            // But, it's the only thing I got to work.
            try
            {
                Debug.Log(context);
            }
            catch(Exception e)
            {
                LoadingWheel.fillAmount = 0f;
                I = 1.6f;
                yield break;
            }
            
            // Fill LoadingWheel if trigger is pressed
            LoadingWheel.fillAmount = I;
            

            // Open menu if it has been continuously pressed.
            if (I >= 1f)
            {
                LoadingWheel.fillAmount = 0f;
                ToggleMenu();
                yield break;
            }
            yield return null;
        }
    }


    public void SwitchMenuTo(GameObject panelToOpen)
    {
        foreach (var item in allMenus)
        {
            bool shouldSetActive = (item == panelToOpen);
            if (item.activeSelf != shouldSetActive)
            {
                item.SetActive(shouldSetActive);
            }
        }
    }

    void AdjustCanvasDistances()
    {
        CanvasFollow[] canvasFollows = GetComponentsInChildren<CanvasFollow>();

        foreach (CanvasFollow canvasFollow in canvasFollows)
        {
            canvasFollow.AdjustDistance(canvasesDistance);
        }
    }
}