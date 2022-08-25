/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// This class will handle the position of tablet
/// This class should be change if you need to open the tablet by other ways like grapping
/// or changing the tablet position
/// </summary>
public class TabletPositionHand : MonoBehaviour
{

    private bool tabletIsOpened;
    [SerializeField] public GameObject LeftHand;
    [SerializeField]
    private Transform managers;


    /// <summary>
    /// Open or close the tablet
    /// </summary>
    /// <param name="status"></param>
    public void SelectTablet(bool status)
    {
        tabletIsOpened = status;
    }

    public void ToggleTablet()
    {
        tabletIsOpened = !tabletIsOpened;
        foreach (Transform child in GetComponentInChildren<Transform>())
        {
            if (child != managers)
            {
                child.gameObject.SetActive(tabletIsOpened);
            }

        }
    }
}