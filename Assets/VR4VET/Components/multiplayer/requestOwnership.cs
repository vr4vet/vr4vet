using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using BNG;


public class requestOwnership : MonoBehaviourPun
{
    private PhotonView photonView;
    private Grabbable BNGG;
    private bool beingHold = false;
    private Rigidbody rb;
    private bool holdByOthers = false;

    private void Start()
    {
        BNGG = GetComponent<Grabbable>();
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

    }

    void LateUpdate()
    {

        if (!PhotonNetwork.IsConnected)
        {
            // Do something if not connected
            return;
        }

        if (BNGG.BeingHeld)
        {
            if (!beingHold)
            {
                beingHold = true;
                Debug.Log("Requesting now!");
                photonView.RequestOwnership();
                // Call the RPC function on the hit object's PhotonView
                photonView.RPC("RPC_SetKinematicState", RpcTarget.OthersBuffered, true);
            }
        }
        if (!BNGG.BeingHeld & beingHold)
        {
            beingHold = false;
            if (holdByOthers)
            {
                rb.isKinematic = true;
            } else
            {
                // Call the RPC function on the hit object's PhotonView
                photonView.RPC("RPC_SetKinematicState", RpcTarget.OthersBuffered, false);
            }
        }
    }

    public void requestOwnershipOnNetwork()
    {
        Debug.Log("Requesting now!");
        photonView.RequestOwnership();
    }

    public void SetKinematicState(bool isKinematic)
    {
        if (isKinematic)
        {
            holdByOthers = true;
        }
        else
        {
            holdByOthers = false;
        }
        rb.isKinematic = isKinematic;
        // Debug.Log("Set object to kinematic: " + isKinematic);
    }

    [PunRPC]
    public void RPC_SetKinematicState(bool isKinematic)
    {
        SetKinematicState(isKinematic);
    }
}
