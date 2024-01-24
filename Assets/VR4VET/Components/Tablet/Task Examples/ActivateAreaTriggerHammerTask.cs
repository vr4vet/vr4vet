using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAreaTriggerHammerTask : MonoBehaviour
{
    public Task.TaskHolder taskHolder;
    // This method is called when another collider enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            // Player entered the trigger area
            taskHolder.GetTask("Grab Hammer").GetSubtask("Go to Table").SetCompleated(true);
        }
    }
}
