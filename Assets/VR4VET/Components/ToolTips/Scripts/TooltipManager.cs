using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Main class.
public class TooltipManager : MonoBehaviour
{
    // Option to toggle whether opening a tooltip should close all others.
    public bool CloseTooltipsOnNewActivation = true;

    // Array of all tooltips.
    List<TooltipScript> Tooltips;

    // Start is called before the first frame update.
    void Start()
    {
        // Assign all tooltip objects to the tooltips array.
        Tooltips.AddRange(FindObjectsOfType<TooltipScript>());
        
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
}