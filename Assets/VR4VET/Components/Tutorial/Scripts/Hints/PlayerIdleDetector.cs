using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerIdleDetector : MonoBehaviour, ITutorial
{
    [SerializeField]
    public int secondsBeforeIdle;
    public UnityEvent OnPlayerIdle;
    public UnityEvent OnPlayerIdleDismissed;

    private readonly BNGPlayerLocator playerLocator = new();
    private Vector3 playerPosition;
    private DateTime positionUpdatedAt;
    private bool isIdleNotified;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var playerController = playerLocator.PlayerController;
        var current = playerController.transform.position;
        if (playerPosition != current)
        {
            playerPosition = current;
            positionUpdatedAt = DateTime.Now;

            if (isIdleNotified)
            {
                OnPlayerIdleDismissed.Invoke();
                isIdleNotified = false;
            }
            return;
        }

        if (isIdleNotified)
        {
            return;
        }

        if ((DateTime.Now - positionUpdatedAt) > TimeSpan.FromSeconds(secondsBeforeIdle))
        {
            OnPlayerIdle.Invoke();
            isIdleNotified = true;
        }
    }
}
