using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.XR.Management;

public class EmulatorOffsetEnabler : MonoBehaviour
{
    private bool _hmdExists;
    private PlayerTeleport _teleportScript;
    // Start is called before the first frame update
    void Start()
    {
        _teleportScript = GetComponent<PlayerTeleport>();
        _hmdExists = XRGeneralSettings.Instance.Manager.activeLoader != null;
        if(!_hmdExists)
        {
            _teleportScript.TeleportYOffset = 1.5f;
        }
    }
}
