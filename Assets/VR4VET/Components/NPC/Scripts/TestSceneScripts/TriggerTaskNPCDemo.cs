using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTaskNPCDemo : MonoBehaviour
{
    [SerializeField] private NPCSpawner _npcSpawner;

    void OnTriggerEnter(Collider other) {
        if(_npcSpawner._npcInstances.Count >= 2) {
            _npcSpawner._npcInstances[1].SetActive(true);
        }
    }
}
