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

    private void Start()
    {
        BNGG = GetComponent<Grabbable>();
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

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
                // Call the RPC function on the hit object's PhotonView
                photonView.RPC("RPC_SetKinematicState", RpcTarget.OthersBuffered, true);
            }
        }
        if (!BNGG.BeingHeld &! beingHold)
        {
            // Call the RPC function on the hit object's PhotonView
            photonView.RPC("RPC_SetKinematicState", RpcTarget.OthersBuffered, false);
            beingHold = false;
        }
    }

    public void requestOwnershipOnNetwork()
    {
        Debug.Log("Requesting now!");
        photonView.RequestOwnership();
    }

    public void SetKinematicState(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
    }

    [PunRPC]
    public void RPC_SetKinematicState(bool isKinematic)
    {
        SetKinematicState(isKinematic);
    }
}
