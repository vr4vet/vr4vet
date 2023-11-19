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
    private Vector3 runtimeNPCSpawnPosition = new Vector3(2, 0, 2);
    private Vector3 greetingNPCSpawnPosition = new Vector3(0, 0, 0);
    private Vector3 proximityNPCSpawnPosition = new Vector3(1, 0, 1);
    private Vector3 taskNPCSpawnPosition = new Vector3(4, 0, 4);

    private NPCSpawner npcSpawner;
    private GameObject runtimeNPC;
    private GameObject greetingNPC;

    private GameObject proximityNPC;
    private GameObject taskNPC;

    private float proximityRadius = 100.0f; // Radius for checking proximity to the player.
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
        runtimeNPC = npcSpawner.SpawnNPC(runtimeNPCSpawnPosition, true, npcPrefabV5);
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

        TTSWit ttsWitService = runtimeNPC.GetComponentInChildren<TTSWit>();
        TTSSpeaker ttsSpeaker = runtimeNPC.GetComponentInChildren<TTSSpeaker>();
        // Change the voice of the NPC
        // int voiceNumber = 3;
        // Debug.Log("You are talking with: " + ttsWitService.GetAllPresetVoiceSettings()[voiceNumber].SettingsId);
        // ttsSpeaker.ClearVoiceOverride();
        // ttsSpeaker.GetComponentInChildren<TTSSpeaker>().SetVoiceOverride(ttsWitService.GetAllPresetVoiceSettings()[voiceNumber]);
        
        // Configure the greeting NPC here with dialogue or other components.

        // Spawn the proximity NPC at the hardcoded position but deactivate it until the player is close enough
        //proximityNPC = npcSpawner.SpawnNPC(proximityNPCSpawnPosition, true, npcPrefab);
        // Configure the proximity NPC here with dialogue or other components.
       // proximityNPC.SetActive(true);
    }

    private void Update()
    {
        //HandleProximityNPC();

        // Check for the 'B' key to spawn the taskNPC
        if (Input.GetKeyDown(KeyCode.B) && !taskNPCSpawned)
        {	
			Debug.Log("B key pressed");
            //SpawnTaskNPC();
        }
    }

    private void HandleProximityNPC()
    {
        // Check the distance between the player and the proximity NPC spawn position
        if (proximityNPC != null && !proximityNPC.activeSelf)
        {
            float distanceToPlayer = Vector3.Distance(PlayerManager.instance.player.transform.position, proximityNPCSpawnPosition);
            if (distanceToPlayer <= proximityRadius)
            {
                // Activate the proximity NPC and make it follow the player
                proximityNPC.SetActive(true);
                FollowThePlayerControllerV2 proximityNPCController = proximityNPC.GetComponent<FollowThePlayerControllerV2>();
                Debug.Log("proximityNPCController" + proximityNPCController); 
                if (proximityNPCController != null)
                {
                    proximityNPCController.shouldFollow = true;
                    // Trigger the initial dialogue for proximity NPC here
                }
            }
        }
    }

    private void SpawnTaskNPC()
    {
        // Spawn the task NPC at the hardcoded spawn position
		Debug.Log("Spawning task NPC");
        taskNPC = npcSpawner.SpawnNPC(taskNPCSpawnPosition, false, npcPrefab);
        taskNPCSpawned = true;
        // Configure the task NPC here with dialogue or other components.
    }

    private void OnTriggerEnter(Collider other)
    {
        // This can be left empty if you only want the B key to trigger the taskNPC spawn
    }

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
