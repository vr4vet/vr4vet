using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCToPlayerReferenceManager : MonoBehaviour
{
    public GameObject PlayerTarget; // something that moves with the player and often represents the players position (e.g. CameraRig)
    public Collider PlayerCollider; // Something actives TriggerOnEnter and other collison stuff (e.g. HolseterRight)
    #region Singleton

    // Only the singleton can set the refernces
    public static NPCToPlayerReferenceManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (PlayerTarget == null)
        {
            Debug.LogError("You need to assign a PlayerTarget to the NPCToPlayerReferenceManager. Many of the NPC scripts need this variable to function");
        }
        if (PlayerCollider == null)
        {
            Debug.LogError("You need to assign a PlayerCollider to the NPCToPlayerReferenceManager. Many of the NPC scripts need this variable to function");
        }
    }

    #endregion
}
