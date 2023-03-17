﻿/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari & Jorge Garcia
 * Ask your questions by email: a85jafari@gmail.com
 * 
 */

using System.Collections.Generic;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine;

/// <summary>
/// This class will handle the position of tablet
/// This class should be change if you need to open the tablet by other ways like grapping
/// or changing the tablet position
/// </summary>
/// 


public class TabletPosition : MonoBehaviour
{
    [Range(0.1f, 1)]
    public float DistanceFromPlayer = 0.4f;
    public Transform Managers;
    private Vector3 _originalAngles;
    private bool _tabletIsOpened;

    public enum myEnum // your custom enumeration
    {
        TabletOnHand = 0,
        FloatingTablet = 1,
    };
    public myEnum type;

    Camera cam;

    /// <summary>
    /// Open or close the tablet
    /// </summary>
    /// <param name="status"></param>
    public void SelectTablet(bool status)
    {
        _tabletIsOpened = status;
    
    }
    //Sets active all the objects in the tablet
    //No change of position since is assumed that is a child of one of the hands
    public void ToggleTablet()
    {
        
        _tabletIsOpened = !_tabletIsOpened;
        foreach (Transform child in GetComponentInChildren<Transform>())
        {
            if (child != Managers)
            {
                child.gameObject.SetActive(_tabletIsOpened);
            }

        }
    }

    public bool gettabletIsOpened()
    {
        return _tabletIsOpened;
    }

    //override of the method
    public void ToggleTablet(bool isEnabled)
    {

        foreach (Transform child in GetComponentInChildren<Transform>())
        {
            if (child != Managers)
            {
                child.gameObject.SetActive(isEnabled);
            }

        }
    }
    /// <summary>
    /// Unity start method
    /// </summary>
    private void Start()
    {     

        if ((int)type == 1)
        {
            this.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }

        _originalAngles = transform.eulerAngles;

        if (!Camera.main)
        {
            cam = GameObject.FindObjectOfType<Camera>();

        }
        else
        {
            cam = Camera.main;

        }

        _tabletIsOpened = true;
        if ((int)type == 1)
            ToggleTablet();
    }



    /// <summary>
    /// Moves the tablet around if the type 1 is selected
    /// </summary>
    void Update()
    {
        if (_tabletIsOpened && ((int)type == 1) )
        {
            transform.position = (cam.transform.position + new Vector3(0,-0.05f,0)) + cam.transform.forward * DistanceFromPlayer;
            transform.LookAt(cam.transform.position);
            transform.Rotate(_originalAngles);
        }
        else
        {
            if ((int)type == 1)
            {
                transform.position = cam.transform.position - new Vector3(0.5f, 1f, 0);
                transform.rotation = Quaternion.Euler(0, 180, 90);
            }  

        }

    }
}