using System;
using System.Collections.Generic;
using Meta.WitAi.TTS.Data;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class EventTriggerDemo : MonoBehaviour
{

    // TODO: change from public to [SerializeField] private, and see what need to be public
    [SerializeField] private GameObject npcPrefab; // Assign your NPC prefab in the inspector.
    [SerializeField] private GameObject npcPrefabV5; 

    [SerializeField] private DialogueTree[] dialogueTrees; 

    [SerializeField] private GameObject[] characterModels;
    [SerializeField] private Avatar[] characterAvatars;
    [SerializeField] private int[] characterVoices;

    //[SerializeField] private TTSWit ttsWitService;
    // Hardcoded spawn positions
    private Vector3 runtimeNPCSpawnPosition = new Vector3(-11, 0, -4);
    private Vector3 greetingNPCSpawnPosition = new Vector3(-15, 0, -4);
    private Vector3 proximityNPCSpawnPosition = new Vector3(-15, 4, -4);

    private Vector3 taskNPCSpawnPosition = new Vector3(-2, 0, -10);

    private NPCSpawner npcSpawner;
    private GameObject runtimeNPC;
    private GameObject greetingNPC;

    private GameObject proximityNPC;
    private FollowThePlayerControllerV2 proximityNPCController;
    private GameObject taskNPC;

    private float proximityRadius = 12.0f; // Radius for checking proximity to the player.
    private bool taskNPCSpawned = false; // To ensure task NPC is only spawned once.

    private void Start()
    {
        npcSpawner = FindObjectOfType<NPCSpawner>();
        if (npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found in the scene!");
            return;
        }

        // Spawn the runtime NPC at the hardcoded position
        runtimeNPC = npcSpawner.SpawnNPC(runtimeNPCSpawnPosition, false, npcPrefabV5);
        // Rotate the NPC to face the player
        runtimeNPC.transform.rotation = Quaternion.Euler(new Vector3(0, 240, 0));

        // Change the dialogue from the deafult one, to a specific one
        ConversationController conversationControllerRuntimeNPC = runtimeNPC.GetComponentInChildren<ConversationController>();
        conversationControllerRuntimeNPC?.SetDialogueTreeList(dialogueTrees[0]);
        // change the avatar from the deafult one, to a specific one
        updateCharacterModel(runtimeNPC, characterModels[0], characterAvatars[0], characterVoices[0]);
        // change the name of the NPC
        DisplayName displayNameRuntimeNPC = runtimeNPC.GetComponent<DisplayName>();
        if (displayNameRuntimeNPC == null) {
            Debug.Log("The NPC is missing the display name componenent");
        }
        displayNameRuntimeNPC.updateDisplayedName("Bob the Builder");

        // TTSWit ttsWitService = runtimeNPC.GetComponentInChildren<TTSWit>();
        // TTSSpeaker ttsSpeaker = runtimeNPC.GetComponentInChildren<TTSSpeaker>();
        // Change the voice of the NPC
        // int voiceNumber = 3;
        // Debug.Log("You are talking with: " + ttsWitService.GetAllPresetVoiceSettings()[voiceNumber].SettingsId);
        // ttsSpeaker.ClearVoiceOverride();
        // ttsSpeaker.GetComponentInChildren<TTSSpeaker>().SetVoiceOverride(ttsWitService.GetAllPresetVoiceSettings()[voiceNumber]);
        
        // Configure the greeting NPC here with dialogue or other components.

        // Spawn the proximity NPC at the hardcoded position but deactivate it until the player is close enough
        proximityNPC = npcSpawner.SpawnNPC(proximityNPCSpawnPosition, false, npcPrefabV5);
        ConversationController conversationControllerProximityNPC = proximityNPC.GetComponentInChildren<ConversationController>();
        conversationControllerProximityNPC?.SetDialogueTreeList(dialogueTrees[2]);
        // Configure the proximity NPC here with dialogue or other components.
        proximityNPC.SetActive(true);
        HandleProximityNPC();
    }

    private void Update()
    {
        UpdateProximityNPC();

    }

    private void HandleProximityNPC()
    {
        if (proximityNPC != null) {proximityNPCController = proximityNPC.GetComponent<FollowThePlayerControllerV2>();}

    }

    private void UpdateProximityNPC()
    {
        if (proximityNPCController == null) return;

        // Get the player's position and the NPC spawn position
        Vector3 playerPosition = PlayerManager.instance.player.transform.position;
        Vector3 npcPosition = proximityNPCSpawnPosition;

        // Check if the player is at the same or higher vertical position as the NPC
        bool isAtSameOrHigherHeight = playerPosition.y >= npcPosition.y;

        // Calculate horizontal distance by ignoring the y-axis
        Vector3 horizontalDistance = new Vector3(playerPosition.x - npcPosition.x, 0, playerPosition.z - npcPosition.z);

        if (isAtSameOrHigherHeight && horizontalDistance.sqrMagnitude <= proximityRadius * proximityRadius)
        {
            Debug.Log("Player is close enough to the proximity NPC and at the same or higher height");
            // Activate the proximity NPC and make it follow the player
            proximityNPCController.shouldFollow = true;
            // Trigger the initial dialogue for proximity NPC here
        }
        else
        {
            // Deactivate the proximity NPC's follow behavior if the player is not at the same or higher height
            proximityNPCController.shouldFollow = false;
        }
    }


    public GameObject SpawnTaskNPC()
    {
        // Spawn the task NPC at the hardcoded spawn position
		Debug.Log("Spawning task NPC");
        taskNPC = npcSpawner.SpawnNPC(taskNPCSpawnPosition, false, npcPrefabV5);
        ConversationController conversationControllerTaskNPC = taskNPC.GetComponentInChildren<ConversationController>();
        conversationControllerTaskNPC?.SetDialogueTreeList(dialogueTrees[1]);
        taskNPCSpawned = true;
        return taskNPC;
        // Configure the task NPC here with dialogue or other components.
    }

    // private void OnTriggerEnter(Collider other)
    // {           // Check if the colliding object is the player and if the wall is "Wall (2)"
        
    //     if (!taskNPCSpawned) 
    //     {   
    //         Debug.Log("Player entered the trigger");
    //         SpawnTaskNPC();
    //     }

        
    // }

    private void updateCharacterModel(GameObject theNPC, GameObject characterModelPrefab, Avatar characterAvatar, int voicePresetId) {
        SetCharacterModel setCharacterModel = theNPC.GetComponent<SetCharacterModel>();
        if (setCharacterModel == null)
        {
            Debug.Log("The NPC is missing the script SetCharacterModel");
        } else {
            setCharacterModel.ChangeCharacter(characterModelPrefab, characterAvatar, voicePresetId);
        }
    }
}
