using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BNGPlayerLocator
{
    public static readonly BNGPlayerLocator Instance = new();
    private BNGPlayerController playerController;

    public BNGPlayerController PlayerController
    {
        get
        {
            if (playerController == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                playerController = player != null
                    ? player.GetComponentInChildren<BNGPlayerController>()
                    : Object.FindObjectOfType<BNGPlayerController>();
            }

            return playerController;
        }
    }
}
