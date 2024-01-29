using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// Create an event that stores data.
[System.Serializable]
public class TTActivationEvent : UnityEvent<GameObject>
{
}

// Main class.
public class TooltipScript : MonoBehaviour, IPointerClickHandler
{
    // The player transform.
    public Transform Player;

    
    // The different states the tooltip might be in.
    public enum StateOptions
    {
        Open,
        Minimized,
        Closed
    }

    public StateOptions StartingState = StateOptions.Open;

    // Settings on how the tooltip should behave.
    public bool RemoveHeader = false;
    public bool Unclosable = false;
    public bool FacePlayer = true;
    public bool AlwaysAboveParent = true;
    public bool CloseWhenDistant = false;
    public float CloseThreshold = 10;

    // Content variables for the tooltip.
    public string Header;
    [TextArea(10,15)]
    public string TextContent;

    // The transform this tooltip is a child of.
    private Transform _parent;
    // The linerenderer component attached to this tooltip.
    private LineRenderer _line;
    // The child of this object that displays the main tooltip contents.
    private Transform _panel;
    // The button that closes the tooltip.
    private Button _closeButton;
    // The animator component that handles expanding and minimizing the tooltip.
    private Animator _animator;
    // Reference in the script to see if tooltip is expanded or minimized.
    private bool _isOpen;

    // Event used by the tooltip manager to close other tooltips when this one activates.
    public TTActivationEvent ActivationEvent;

    // Start is called before the first frame update.
    void Awake()
    {
        // If there is not already an activation event, create a new one.
        if (ActivationEvent == null)
        {
            ActivationEvent = new TTActivationEvent();
        }

        // Set the parent variable to be the parent transform of the tooltip.
        _parent = transform.parent;

        // Check whether the tooltip should be rotated independently from the main object
        if(AlwaysAboveParent)
        {
            transform.position = _parent.position + new Vector3(0, 0.2f, 0);
        }

        // Adjust the linerenderer to connect the parent transform and the tooltip.
        _line = gameObject.GetComponent<LineRenderer>();
        _line.widthMultiplier = 0.005f;
        _line.SetPosition(1, _parent.position);
        _line.SetPosition(0, transform.position);

        // Get the player transform, or throw an error.
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
        Player = Player.Find("PlayerController/CameraRig/TrackingSpace/CenterEyeAnchor");

        // Initialize animator.
        _panel = transform.Find("Card");
        _closeButton = _panel.Find("Button").GetComponent<Button>();
        _closeButton.onClick.AddListener(Deactivate);
        _animator = _panel.GetComponent<Animator>();
        _animator.SetBool("open", (StartingState == StateOptions.Minimized)? false:true);

        // Deactivate the tooltip if it should not start opened.
        if (StartingState == StateOptions.Closed)
        {
            Deactivate();
        }
        if(Unclosable)
        {
            _closeButton.gameObject.SetActive(false);
        }
        if(RemoveHeader)
        {
            _panel.Find("header").gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame.
    void Update()
    {

        // If the player is too far away, and the tooltip is set to close when the player is too far away, close the tooltip.
        if(Vector3.Distance(transform.position, Player.position) >= CloseThreshold && CloseWhenDistant)
        {
            Deactivate();
        }

        // If tooltip should always stay above the parent transform, independently of rotation, set its location to a global position.
        if(AlwaysAboveParent)
        {
            transform.position = _parent.position + new Vector3(0, 0.2f, 0);
        }

        // If the tooltip should face the player, rotate it to face the head.
        if(FacePlayer)
        {
            transform.LookAt(Player);
        }

        // Set linerenderer positions so it always connects tooltip and parent transform.
        _line.SetPosition(1, _parent.position);
        _line.SetPosition(0, transform.position);

        // Dynamically update the contents of the tooltip.
        if(!RemoveHeader)
        {
            _panel.Find("header").Find("Text").GetComponent<Text>().text = Header;
        }
        _panel.Find("TextField").Find("Text").GetComponent<Text>().text = TextContent;
    }

    // Function used to activate the tooltip.
    // Also invokes the activation event for the tooltip manager.
    public void Activate()
    {
        ActivationEvent.Invoke(gameObject);
        gameObject.SetActive(true);
    }

    // Function used to close the tooltip.
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // Function called when the tooltip is interacted with through the pointer.
    // Activates the animation of the animation handler.
    public void Minimize()
    {
        if (_panel != null || _animator != null)
        {
        _isOpen = _animator.GetBool("open");
        _animator.SetBool("open", !_isOpen);
        Debug.Log("The animation is " + _animator.GetBool("open"));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Minimize();
    }
}
