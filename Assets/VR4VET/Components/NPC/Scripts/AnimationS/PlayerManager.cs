using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This keeps the instance alive across different scenes.

            // Assign the player field if it's not set in the inspector.
            if (player == null)
            {
                // TODO: Fix
                // Dette blir feil.  Player er ikke XR Rig, men Camera Rig
                player = GameObject.FindGameObjectWithTag("Player");
                if (player == null)
                {
                    Debug.LogError("PlayerManager: Player GameObject is not assigned and could not be found with 'Player' tag.");
                }
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject); // This destroys any duplicate instances that might arise.
        }
    }

    #endregion

    public GameObject player;
}
