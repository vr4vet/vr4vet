using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipManager : MonoBehaviour
{
    TooltipScript[] Tooltips;

    void Start()
    {
        Debug.Log("Start");
        Tooltips = FindObjectsOfType<TooltipScript>();
        Debug.Log("Found " + Tooltips.Length + " tooltips.");
        foreach (TooltipScript Tooltip in Tooltips)
        {
            Tooltip.ActivationEvent.AddListener(TooltipActivationListener);
        }
        
    }

    void TooltipActivationListener(GameObject CurrentTooltip)
    {
        foreach (TooltipScript Tooltip in Tooltips)
        {
            if(Tooltip.gameObject != CurrentTooltip)
            {
                Tooltip.Deactivate();
            }
        }
    }
}