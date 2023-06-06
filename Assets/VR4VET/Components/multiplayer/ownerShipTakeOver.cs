using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ownerShipTakeOver : MonoBehaviourPun
{
    public void requestOwnership()
    {
        Debug.Log("Requesting now!");
        base.photonView.RequestOwnership();
    }
}
