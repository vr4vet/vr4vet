using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecializedNPCBehaviorDemo : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;
    [HideInInspector] private FollowThePlayerController _followThePlayerController;
    [HideInInspector] private float _proximityRadius = 12.0f; // Radius for checking proximity to the player.

    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        _npcSpawner._npcInstances[1].gameObject.SetActive(false);

        _npc = _npcSpawner._npcInstances[2];
        HandleProximityNPC();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProximityNPC();
    }

    private void HandleProximityNPC()
    {
        if (_npc != null) { 
            _followThePlayerController = _npc.GetComponent<FollowThePlayerController>(); 
        if (_followThePlayerController != null) {
                _followThePlayerController.PersonalSpaceFactor = 2;
                _followThePlayerController.StartFollowingRadius = 3;
            }
        }
    }

    private void UpdateProximityNPC()
    {
        if (_followThePlayerController == null) {
            return;
        } 
        // Get the player's position and the NPC spawn position
        Vector3 playerPosition = NPCToPlayerReferenceManager.Instance.PlayerTarget.transform.position;
        Vector3 npcPosition = _npc.transform.position;

        // Check if the player is at the same or higher vertical position as the NPC
        bool isAtSameOrHigherHeight = playerPosition.y >= (npcPosition.y - 1);

        // Calculate horizontal distance by ignoring the y-axis
        Vector3 horizontalDistance = new Vector3(playerPosition.x - npcPosition.x, 0, playerPosition.z - npcPosition.z);

        if (isAtSameOrHigherHeight && horizontalDistance.sqrMagnitude <= _proximityRadius * _proximityRadius)
        {
            // Activate the proximity NPC and make it follow the player
            _followThePlayerController.ShouldFollow = true;
        }
        else
        {
            // Deactivate the proximity NPC's follow behavior if the player is not at the same or higher height
            _followThePlayerController.ShouldFollow = false;
        }
    }
}
