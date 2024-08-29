using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartTutorial : MonoBehaviour
{
    [SerializeField] private GameObject movedObject;
    [SerializeField] private Transform respawnPoint;


    public void RespawnObject()
    {
        movedObject.transform.localPosition = new Vector3(respawnPoint.localPosition.x, respawnPoint.localPosition.y, respawnPoint.localPosition.z);
        Vector3 eulerRotation = new Vector3(respawnPoint.transform.eulerAngles.x, respawnPoint.transform.eulerAngles.y, respawnPoint.transform.eulerAngles.z);
        movedObject.transform.localRotation = Quaternion.Euler(eulerRotation);
    }
}
