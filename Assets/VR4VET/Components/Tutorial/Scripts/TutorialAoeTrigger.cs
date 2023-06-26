using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TutorialAoeTrigger : MonoBehaviour
{
    /// <summary>
    /// Gets an event that is fired when the player enters the box collider.
    /// </summary>
    public UnityEvent OnPlayerEnter;

    /// <summary>
    /// Gets an event that is fired when the player exits the box collider.
    /// </summary>
    public UnityEvent OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            OnPlayerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            OnPlayerExit.Invoke();
        }
    }

    private static bool IsPlayer(Collider collider) => collider.gameObject.tag == "Player";
}
