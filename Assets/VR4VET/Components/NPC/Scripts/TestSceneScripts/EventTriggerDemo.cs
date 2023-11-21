using System;
using System.Collections.Generic;
using Meta.WitAi.TTS.Data;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class EventTriggerDemo : MonoBehaviour
{

    // TODO: change from public to [SerializeField] private, and see what need to be public
    [SerializeField] private GameObject _npcPrefab; // Assign your NPC prefab in the inspector.
    [SerializeField] private DialogueTree[] _dialogueTrees; // Assign what you want the NPCs to be talking about
    [SerializeField] private GameObject[] _characterModels; // Assign what you want the NPC to be looking like
    [SerializeField] private Avatar[] _characterAvatars; // Assign the animation avatar corresponding with the models assign above. 
    // Important that the avatar and models that belong together are assign at the same place in the list
    [SerializeField] private int[] _characterVoices; // Assign what you want the NPC to sound like
    // The TTSWitService has a list of preset voices with ids, its is the ids I am looking for here. [0, 21]
    // Hardcoded spawn positions
    [HideInInspector] private Vector3 _runtimeNPCSpawnPosition = new Vector3(-11, 0, -4);
    [HideInInspector] private Vector3 _proximityNPCSpawnPosition = new Vector3(-15, 4, -4);
    [HideInInspector] private Vector3 _taskNPCSpawnPosition = new Vector3(-2, 0, -10);
    [HideInInspector] private NPCSpawner _npcSpawner;
    // The NPCs
    [HideInInspector] private GameObject _runtimeNPC;
    [HideInInspector] private GameObject _proximityNPC;
    [HideInInspector] private GameObject _taskNPC;
    [HideInInspector] private FollowThePlayerController _proximityNPCController;
    [HideInInspector] private float _proximityRadius = 12.0f; // Radius for checking proximity to the player.

    private void Start()
    {
        _npcSpawner = FindObjectOfType<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found in the scene!");
            return;
        }

        // Spawn the runtime NPC at the hardcoded position
        _runtimeNPC = _npcSpawner.SpawnNPC(_runtimeNPCSpawnPosition, false, _npcPrefab);
        // Rotate the NPC to face the player
        _runtimeNPC.transform.rotation = Quaternion.Euler(new Vector3(0, 240, 0));

        // Change the dialogue from the deafult one, to a specific one
        ConversationController conversationControllerRuntimeNPC = _runtimeNPC.GetComponentInChildren<ConversationController>();
        conversationControllerRuntimeNPC?.SetDialogueTreeList(_dialogueTrees[0]);
        // change the apperance and avatar from the deafult one, to a specific one
        // change the the voice
        updateCharacterModel(_runtimeNPC, _characterModels[0], _characterAvatars[0], _characterVoices[0]);
        // change the name of the NPC
        DisplayName displayNameRuntimeNPC = _runtimeNPC.GetComponent<DisplayName>();
        if (displayNameRuntimeNPC == null)
        {
            Debug.Log("The NPC is missing the display name componenent");
        }
        displayNameRuntimeNPC.UpdateDisplayedName("Bob the Builder");

        // Spawn the proximity NPC at the hardcoded position but deactivate it until the player is close enough
        _proximityNPC = _npcSpawner.SpawnNPC(_proximityNPCSpawnPosition, false, _npcPrefab);
        ConversationController conversationControllerProximityNPC = _proximityNPC.GetComponentInChildren<ConversationController>();
        conversationControllerProximityNPC?.SetDialogueTreeList(_dialogueTrees[2]);
        // Configure the proximity NPC here with dialogue or other components.
        _proximityNPC.SetActive(true);
        HandleProximityNPC();
    }

    private void Update()
    {
        UpdateProximityNPC();

    }

    private void HandleProximityNPC()
    {
        if (_proximityNPC != null) { _proximityNPCController = _proximityNPC.GetComponent<FollowThePlayerController>(); }

    }

    private void UpdateProximityNPC()
    {
        if (_proximityNPCController == null) return;

        // Get the player's position and the NPC spawn position
        Vector3 playerPosition = NPCToPlayerReferenceManager.Instance.PlayerTarget.transform.position;
        Vector3 npcPosition = _proximityNPCSpawnPosition;

        // Check if the player is at the same or higher vertical position as the NPC
        bool isAtSameOrHigherHeight = playerPosition.y >= npcPosition.y;

        // Calculate horizontal distance by ignoring the y-axis
        Vector3 horizontalDistance = new Vector3(playerPosition.x - npcPosition.x, 0, playerPosition.z - npcPosition.z);

        if (isAtSameOrHigherHeight && horizontalDistance.sqrMagnitude <= _proximityRadius * _proximityRadius)
        {
            // Activate the proximity NPC and make it follow the player
            _proximityNPCController.ShouldFollow = true;
            // Trigger the initial dialogue for proximity NPC here
        }
        else
        {
            // Deactivate the proximity NPC's follow behavior if the player is not at the same or higher height
            _proximityNPCController.ShouldFollow = false;
        }
    }


    public GameObject SpawnTaskNPC()
    {
        // Spawn the task NPC at the hardcoded spawn position
        Debug.Log("Spawning task NPC");
        _taskNPC = _npcSpawner.SpawnNPC(_taskNPCSpawnPosition, false, _npcPrefab);
        ConversationController conversationControllerTaskNPC = _taskNPC.GetComponentInChildren<ConversationController>();
        conversationControllerTaskNPC?.SetDialogueTreeList(_dialogueTrees[1]);
        return _taskNPC;
        // Configure the task NPC here with dialogue or other components.
    }

    private void updateCharacterModel(GameObject theNPC, GameObject characterModelPrefab, Avatar characterAvatar, int voicePresetId)
    {
        SetCharacterModel setCharacterModel = theNPC.GetComponent<SetCharacterModel>();
        if (setCharacterModel == null)
        {
            Debug.Log("The NPC is missing the script SetCharacterModel");
        }
        else
        {
            setCharacterModel.ChangeCharacter(characterModelPrefab, characterAvatar, voicePresetId);
        }
    }
}
