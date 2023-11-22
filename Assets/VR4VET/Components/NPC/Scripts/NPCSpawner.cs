using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using System;
using System.Collections.Generic;
public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private NPC[] _nPCs;
    [HideInInspector] public List<GameObject> _npcInstances;

    private void Awake() {
        foreach (var npcSO in _nPCs)
        {
            _npcInstances.Add(SpawnNPC(npcSO));
        }
    }

    public GameObject SpawnNPC(NPC npcSO) {
        // Instantiate the NPC prefab at the defined location
        GameObject newNPC = Instantiate(npcSO.NpcPrefab, npcSO.SpawnPosition, Quaternion.identity);
        // Rotate the NPC 
        newNPC.transform.rotation = Quaternion.Euler(npcSO.SpawnRotation);  
        // Attach the Text-To-Speech componenets
        AttachTTSComponents(newNPC);
        // change the apperance, animation avatar and voice from the deafult one, to a specific one
        SetAppearanceAnimationAndVoice(newNPC, npcSO.CharacterModel, npcSO.CharacterAvatar, npcSO.runtimeAnimatorController , npcSO.VoicePresetId);
        // Should the NPC follow after the player or not? (from the start)
        SetFollowingBehavior(newNPC, npcSO.ShouldFollow);
        // Update the name of the NPC
        SetName(newNPC, npcSO.NameOfNPC);
        // set talking topics aka. dialogueTrees
        SetConversation(newNPC, npcSO.DialogueTreesSO, npcSO.DialogueTreeJSON);
        // return the NPC
        return newNPC;
    }

    public void AttachTTSComponents(GameObject npc)
    {
        // Load the TTS prefab from the Resources folder
        GameObject ttsPrefab = Resources.Load<GameObject>("TTS");
        
        if (ttsPrefab != null)
        {
            // Instantiate the TTS prefab and parent it under the NPC
            GameObject ttsObject = Instantiate(ttsPrefab, npc.transform);

            // Assuming the TTSSpeaker component is either on the TTS prefab 
            // or one of its children, we find it and set it on the DialogueBoxController
            TTSSpeaker ttsSpeaker = ttsObject.GetComponentInChildren<TTSSpeaker>();
            DialogueBoxController dialogueController = npc.GetComponent<DialogueBoxController>();

            if (ttsSpeaker != null && dialogueController != null)
            {
                dialogueController.TTSSpeaker = ttsSpeaker.gameObject;
            }
        }
        else
        {
            Debug.LogError("TTS prefab could not be loaded. Ensure it's located in the Resources folder.");
        }
    }

    public void SetFollowingBehavior(GameObject npc, bool shouldFollow) {
        FollowThePlayerController followThePlayerController = npc.GetComponent<FollowThePlayerController>();
        if (followThePlayerController != null)
        {
            followThePlayerController.ShouldFollow = shouldFollow;
        }
        else
        {
            Debug.LogError("NPC prefab does not have an NPCController component!");
        }
    }

    public void SetAppearanceAnimationAndVoice(GameObject npc, GameObject characterModelPrefab, Avatar characterAvatar, RuntimeAnimatorController runtimeAnimatorController, int voicePresetId)
    {
        SetCharacterModel setCharacterModel = npc.GetComponent<SetCharacterModel>();
        if (setCharacterModel == null)
        {
            Debug.LogError("The NPC is missing the script SetCharacterModel");
        }
        else
        {
            setCharacterModel.ChangeCharacter(characterModelPrefab, characterAvatar, runtimeAnimatorController, voicePresetId);
        }
    }

    public void SetName(GameObject npc, String name) {
        DisplayName displayName = npc.GetComponent<DisplayName>();
        if (displayName == null)
        {
            Debug.LogError("The NPC is missing the display name componenent");
        } else {
            displayName.UpdateDisplayedName(name);
        }
    }

    public void SetConversation(GameObject npc, DialogueTree[] dialogueTreesSO, TextAsset[] dialogueTreesJSON) {
        ConversationController conversationController = npc.GetComponentInChildren<ConversationController>();
        if (conversationController == null)
        {
            Debug.LogError("The NPC is missing the conversationController");
        } else {
            conversationController.SetDialogueTreeList(dialogueTreesSO, dialogueTreesJSON);
        }
    }
}
