/* Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using BNG;
using UnityEngine;


/// <summary>
/// This script allows changing multiple settings of : teleportation, rotation, and movement on runtime
/// This is script was made, so the player can change this control settings from a menu, but could be used elsewhere
/// </summary>

public class ControlFeatures : MonoBehaviour
{
    [SerializeField] private GameObject PlayerController;
    private PlayerTeleport _pt;
    private PlayerRotation _pr;

    //using boolean variables to store the settings value, making it easier to work with toggles
    private bool _flipModeB = false;

    private bool _snapRotationB = true;
    private bool _teleportRotationB = true;
    private bool _teleportPressB = false;

    private void Start()
    {
        _pt = (PlayerTeleport)PlayerController.GetComponent("PlayerTeleport");
        _pr = (PlayerRotation)PlayerController.GetComponent("PlayerRotation");
    }

    //switiching the teleport and roation hand
    public void InverertedMode()
    {
        _flipModeB = !_flipModeB;

        if (_flipModeB)
        {
            //lefty mode
            _pt.HandSide = ControllerHand.Right;
            //clear the imput list for rotation and adding the left thumstick
            _pr.inputAxis.Clear();
            _pr.inputAxis.Add(InputAxis.LeftThumbStickAxis);
        }
        else
        {
            //normal mode
            _pt.HandSide = ControllerHand.Left;
            //clear the imput list for rotation and adding the right thumstick
            _pr.inputAxis.Clear();
            _pr.inputAxis.Add(InputAxis.RightThumbStickAxis);
        }
    }

    //snaprotation on/off
    public void SnapRotationMode()
    {
        _snapRotationB = !_snapRotationB;

        if (_snapRotationB)
        {
            _pr.AllowInput = true;
        }
        else
        {
            _pr.AllowInput = false;
        }
    }

    //directional deleportaiton on/off
    public void DirectionalTeleportationMode()
    {
        _teleportRotationB = !_teleportRotationB;

        if (_teleportRotationB)
        {
            _pt.AllowTeleportRotation = true;
        }
        else
        {
            _pt.AllowTeleportRotation = false;
        }
    }
    //changes the teleport button from the moving the thumstick to -> pressing down the thumbsting
    public void TeleportInput()
    {
        _teleportPressB = !_teleportPressB;

        if (_teleportPressB)
        {
            _pt.ControlType = TeleportControls.ThumbstickDown;
        }
        else
        {
            _pt.ControlType = TeleportControls.ThumbstickRotate;
        }
    }
}