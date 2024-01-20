using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CompleteTaskOnGrab : MonoBehaviour
{
    private Grabbable grabbable;
    public Task.TaskHolder taskHolder;

    void Start()
    {
        // Get the Grabbable component
        grabbable = GetComponent<Grabbable>();

    }

    void Update()
    {
        // Check for grab input manually
        if (grabbable.BeingHeld)
        {
            taskHolder.GetTask("Grab Hammer").GetSubtask("Perform Grab").SetCompleated(true);
        }
    }
}
