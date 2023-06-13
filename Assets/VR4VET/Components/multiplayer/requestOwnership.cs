using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using BNG;


public class requestOwnership : MonoBehaviour
{
    private PhotonView photonView;
    private Grabbable BNGG;
    private bool beingHold = false;

    private void Start()
    {
        BNGG = GetComponent<Grabbable>();
        photonView = GetComponent<PhotonView>();

    }

    void LateUpdate()
    {
        if (BNGG.BeingHeld)
        {
            if (!beingHold)
            {
                beingHold = true;
                Debug.Log("Requesting now!");
                photonView.RequestOwnership();
            }
        }
        if (!BNGG.BeingHeld)
        {
            beingHold = false;
        }
    }

    public void requestOwnershipOnNetwork()
    {
        Debug.Log("Requesting now!");
        photonView.RequestOwnership();
    }
}
