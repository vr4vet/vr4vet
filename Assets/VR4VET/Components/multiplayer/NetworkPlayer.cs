using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

using UnityEngine.XR.Interaction.Toolkit;
using System;

public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform tablet;
    private PhotonView photonView;

    public Transform headRig;
    public Transform leftHandRig;
    public Transform rightHandRig;
    public Transform tabletRig;


    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        GameObject rig = GameObject.Find("XR Rig Advanced");
        headRig = rig.transform.Find("HeadCollision");
        leftHandRig = rig.transform.Find("PlayerController/CameraRig/TrackingSpace/LeftHandAnchor/LeftControllerAnchor/LeftController");
        rightHandRig = rig.transform.Find("PlayerController/CameraRig/TrackingSpace/RightHandAnchor/RightControllerAnchor/RightController");
        rig = GameObject.Find("Tablet Advanced");
        tabletRig = rig.transform.Find("Mesh");
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            rightHand.gameObject.SetActive(false);
            leftHand.gameObject.SetActive(false);
            head.gameObject.SetActive(false);
            tablet.gameObject.SetActive(false);
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);
            MapPosition(tablet, tabletRig);

        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
