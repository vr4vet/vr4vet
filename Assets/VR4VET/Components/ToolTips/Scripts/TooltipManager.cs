using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// Main class.
public class TooltipManager : MonoBehaviour
{
    // Option to toggle whether opening a tooltip should close all others.
    public bool CloseTooltipsOnNewActivation = true;
    public GameObject TooltipPrefab;

    // Array of all tooltips.
    [SerializeField]
    private List<TooltipScript> Tooltips;

    // Start is called before the first frame update.
    void Start()
    {
        // Assign all tooltip objects to the tooltips array.
        Tooltips.AddRange(FindObjectsOfType(typeof(TooltipScript), true) as TooltipScript[]);
        
        // Write how many tooltips were found to Debug console.
        Debug.Log($"Found {Tooltips.Count} tooltips.");

        // Add a listener to each tooltip that listens for tooltip activation.
        foreach (TooltipScript Tooltip in Tooltips)
        {
            Tooltip.ActivationEvent.AddListener(TooltipActivationListener);
        }
        
    }

    // Allow new tooltips to be added to the array dynamically
    public void AddNewTooltip(TooltipScript newTooltip)
    {
        Tooltips.Add(newTooltip);
        
        newTooltip.ActivationEvent.AddListener(TooltipActivationListener);
    }

    // Function that deactivates all tooltips except the newly opened tooltip.
    void TooltipActivationListener(GameObject currentTooltip)
    {
        foreach (TooltipScript tooltip in Tooltips)
        {
            if (tooltip.gameObject != currentTooltip && CloseTooltipsOnNewActivation)
            {
                tooltip.Deactivate();
            }
        }
    }

    public TooltipScript InstantiateTooltip(Transform Parent, string Header, string Content, string StartState = "Open", bool RemoveHeader = false, bool Unclosable = false, bool FacePlayer = true, bool AlwaysAboveParent = true, bool CloseWhenDistant = false, float CloseThreshold = 10f)
    {
        GameObject instTooltip = Instantiate(TooltipPrefab, Parent) as GameObject;
        TooltipScript InstTooltip = instTooltip.GetComponent<TooltipScript>();
        Debug.Log(InstTooltip);
        AddNewTooltip(InstTooltip);
        InstTooltip.Header = Header;
        InstTooltip.TextContent = Content;
        InstTooltip.RemoveHeader = RemoveHeader;
        InstTooltip.Unclosable = Unclosable;
        InstTooltip.FacePlayer = FacePlayer;
        InstTooltip.AlwaysAboveParent = AlwaysAboveParent;
        InstTooltip.CloseWhenDistant = CloseWhenDistant;
        InstTooltip.CloseThreshold = CloseThreshold;
        Debug.Log(StartState);
        switch (StartState.ToLower())
        {
            case("closed"):
                InstTooltip.Deactivate();
                break;
            case("minimized"):
                InstTooltip.Minimize();
                Debug.Log("Minimizing");
                break;
        }
        return InstTooltip;
    }
}