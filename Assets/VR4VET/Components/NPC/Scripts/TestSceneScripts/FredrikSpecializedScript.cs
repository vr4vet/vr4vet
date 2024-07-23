using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FredrikSpecializedScript : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    private GameObject platform;
    private ConversationController _conversationController;

    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = GameObject.Find("NPCSpawner").GetComponent<NPCSpawner>();
        if (_npcSpawner == null) {
            Debug.Log("Cant find spawner");
        }
        _npc = _npcSpawner._npcInstances[0];
        platform = GameObject.Find("CommentPlatform");
        _conversationController = _npc.GetComponentInChildren<ConversationController>();
    }

    void OnTriggerEnter() {
        Debug.Log("Entered platform trigger");
        _conversationController.CommentTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
