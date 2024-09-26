using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialHelper : MonoBehaviour
{
    public UnityEvent playerEnter;
    public UnityEvent hammerEnter;
    public Transform spawnPoint;
    public GameObject player;
    public GameObject training;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Hammer") {
            hammerEnter.Invoke();
        } else if (other.gameObject.tag == "Player") {
            playerEnter.Invoke();
        }
    }

    public void Teleport() {
        var playerController = BNGPlayerLocator.Instance.PlayerController;
        var destination = spawnPoint.position;
        var playerStartRotation = playerController.transform.rotation;
        playerController.transform.SetPositionAndRotation(destination, playerStartRotation);
        /*foreach (Transform child in player.transform){
            child.position = player.transform.position;
        }
        player.transform.position = spawnPoint.position;*/
    }

    public void SelfDestruct() {
        Destroy(training);
    }
}
