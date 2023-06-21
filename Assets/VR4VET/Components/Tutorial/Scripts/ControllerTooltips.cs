using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class ControllerTooltips : MonoBehaviour
{
    public Transform Player;
    public Transform ActiveControllers;
    public Transform OVRMod;
    public Transform IndexMod;
    public Transform HTCMod;
    public List<Transform> _oldModelR;
    public List<Transform> _oldModelL;
    public bool EnableOnStart = false;
    private Transform ActiveL;
    private Transform ActiveR;
    private Transform _destinationL;
    private Transform _destinationR;
    private bool _controllerIsActive = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if(!Player)
        {
            try
            {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            catch (UnassignedReferenceException)
            {
                Debug.LogError("Assign the player under the tooltip or with a Player tag");
                throw;
            }
        }

        // Get the location of the players head, and use that for player location moving forward.
        Player = Player.Find("PlayerController/CameraRig/TrackingSpace");
        _destinationL = Player.Find("LeftHandAnchor/LeftControllerAnchor/LeftController/ModelsLeft");
        _destinationR = Player.Find("RightHandAnchor/RightControllerAnchor/RightController/ModelsRight");
        OVRMod = transform.Find("OculusModels");
        IndexMod = transform.Find("IndexModels");
        HTCMod = transform.Find("HTCModels");
        Player = Player.Find("CenterEyeAnchor");

        if (ActiveControllers != null)
        {
            ActiveL = ActiveControllers.Find("LeftTTController");
            ActiveR = ActiveControllers.Find("RightTTController");
            Debug.Log("Controllers overridden in editor");
            Debug.Log(ActiveControllers);
        }
        else
        {
            if(InputBridge.Instance.IsValveIndexController)
            {
                // Load Index model
                ActiveL = IndexMod.Find("LeftTTController");
                ActiveR = IndexMod.Find("RightTTController");
                ActiveControllers = IndexMod;
                Debug.Log("Found Index controllers");
            }
            else if(InputBridge.Instance.IsHTCDevice)
            {
                // Load HTC model
                ActiveL = HTCMod.Find("LeftTTController");
                ActiveR = HTCMod.Find("RightTTController");
                ActiveControllers = HTCMod;
                Debug.Log("Found HTC controllers");
            }
            else
            {
                // Load OVR model
                ActiveL = OVRMod.Find("LeftTTController");
                ActiveR = OVRMod.Find("RightTTController");
                ActiveControllers = OVRMod;
                Debug.Log("Found Oculus controllers, no controllers, or unsupported controllers");
            }
        }
        if(EnableOnStart)
        {
            SetActiveControllerTT();
            
        }
    }

    public List<Transform> EnableTTController() {
        _controllerIsActive = true;
        // Disable other hand models and save them for later use
        for(int i = 0; i < _destinationL.childCount; i++) {
            if (_destinationL.GetChild(i).gameObject.activeSelf) {
                _oldModelL.Add(_destinationL.GetChild(i));
                _destinationL.GetChild(i).gameObject.SetActive(false);
            }
        }
        for(int i = 0; i < _destinationR.childCount; i++) {
            if (_destinationR.GetChild(i).gameObject.activeSelf) {
                _oldModelR.Add(_destinationR.GetChild(i));
                _destinationR.GetChild(i).gameObject.SetActive(false);
            }
        }
        // Set the controllers as the hands in hierarchy
        ActiveL.parent = _destinationL;
        ActiveL.localPosition = new Vector3(0, 0, 0);
        ActiveL.SetAsFirstSibling();
        ActiveR.parent = _destinationR;
        ActiveR.localPosition = new Vector3(0, 0, 0);
        ActiveR.SetAsFirstSibling();
        return new List<Transform>() {ActiveL, ActiveR};
    }

    public void SetActiveControllerTT(List<string> ActiveTooltips = null)
    {
        foreach (Transform TT in ActiveL.Find("ControllerParent"))
        {
            if (TT.name.Contains("Tooltip"))
            {
                TT.gameObject.SetActive(false);
            }
        }
        foreach (Transform TT in ActiveR.Find("ControllerParent"))
        {
            if (TT.name.Contains("Tooltip"))
            {
                TT.gameObject.SetActive(false);
            }
        }

        if(ActiveTooltips != null)
        {
            if(!_controllerIsActive)
            {
                EnableTTController();
            }
            foreach (string TT in ActiveTooltips)
            {
                ActiveL.Find("Controllerparent/" + TT + "Tooltip").gameObject.SetActive(true);
                ActiveR.Find("Controllerparent/" + TT + "Tooltip").gameObject.SetActive(true);
            }
        }
    }

    public void DisableTTController() {
        _controllerIsActive = false;
        _destinationL.Find("LeftTTController").parent = transform.Find("ActiveControllers");
        _destinationR.Find("RightTTController").parent = transform.Find("ActiveControllers");
        _oldModelL.ForEach((Model) => {
            Model.gameObject.SetActive(true);
        });
        _oldModelR.ForEach((Model) => {
            Model.gameObject.SetActive(true);
        });
    }
}
