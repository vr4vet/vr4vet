using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using System;
using System.Collections.Generic;
public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private NPC[] _nPCs;
    [HideInInspector] public List<GameObject> _npcInstances;

    [TextArea(3, 10)]
    public string globalContextPrompt;


    private GameObject spawnedNpc;

    private void Awake()
    {
        foreach (var npcSO in _nPCs)
        {
            _npcInstances.Add(SpawnNPC(npcSO));
        }
    }

    public GameObject SpawnNPC(NPC npcSO)
    {
        // Instantiate the NPC prefab at the defined location
        GameObject newNPC = Instantiate(npcSO.NpcPrefab, npcSO.SpawnPosition, Quaternion.identity);
        // Rotate the NPC 
        newNPC.transform.rotation = Quaternion.Euler(npcSO.SpawnRotation);
        // Attach the Text-To-Speech componenets
        AttachTTSComponents(newNPC, npcSO.SpatialBlend, npcSO.MinDistance);
        // change the apperance, animation avatar and voice from the deafult one, to a specific one
        SetAppearanceAnimationAndVoice(newNPC, npcSO.CharacterModel, npcSO.CharacterAvatar, npcSO.runtimeAnimatorController, npcSO.WitVoiceId);
        // Should the NPC follow after the player or not? (from the start)
        SetFollowingBehavior(newNPC, npcSO.ShouldFollow);
        // Update the name of the NPC
        SetName(newNPC, npcSO.NameOfNPC);
        // set talking topics aka. dialogueTrees
        SetConversation(newNPC, npcSO.DialogueTreesSO, npcSO.DialogueTreeJSON);

        setAIBehaviour(newNPC, npcSO.contextPrompt, npcSO.maxTokens);

        setTTSProvider(newNPC, npcSO.selectedTTSProvider, npcSO.OpenAiVoiceId);

        spawnedNpc = newNPC;
        // return the NPC

        return newNPC;
    }

    public void AttachTTSComponents(GameObject npc, float spatialBlend, float minDistance)
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

                // Set the TTSSpeakerAudio settings to those defined in NPC-scriptable
                // If they are not specified use preset blend 1 and minDistance 5
                AudioSource speakerAudio = ttsSpeaker.GetComponentInChildren<AudioSource>();

                if (spatialBlend != 0)
                {
                    speakerAudio.spatialBlend = spatialBlend;
                }
                else
                {
                    speakerAudio.spatialBlend = 1;
                }
                if (minDistance != 0)
                {
                    speakerAudio.minDistance = minDistance;
                }
                else
                {
                    speakerAudio.minDistance = 5;
                }


            }
        }
        else
        {
            Debug.LogError("TTS prefab could not be loaded. Ensure it's located in the Resources folder.");
        }
    }

    public void SetFollowingBehavior(GameObject npc, bool shouldFollow)
    {
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

    public void SetAppearanceAnimationAndVoice(GameObject npc, GameObject characterModelPrefab, Avatar characterAvatar, RuntimeAnimatorController runtimeAnimatorController, int WitVoiceId)
    {
        SetCharacterModel setCharacterModel = npc.GetComponent<SetCharacterModel>();
        if (setCharacterModel == null)
        {
            Debug.LogError("The NPC is missing the script SetCharacterModel");
        }
        else
        {
            setCharacterModel.ChangeCharacter(characterModelPrefab, characterAvatar, runtimeAnimatorController, WitVoiceId);
        }
    }

    public void SetName(GameObject npc, String name)
    {
        DisplayName displayName = npc.GetComponent<DisplayName>();
        if (displayName == null)
        {
            Debug.LogError("The NPC is missing the display name componenent");
        }
        else
        {
            displayName.UpdateDisplayedName(name);
        }
    }

    public void SetConversation(GameObject npc, DialogueTree[] dialogueTreesSO, TextAsset[] dialogueTreesJSON)
    {
        ConversationController conversationController = npc.GetComponentInChildren<ConversationController>();
        if (conversationController == null)
        {
            Debug.LogError("The NPC is missing the conversationController");
        }
        else
        {
            conversationController.SetDialogueTreeList(dialogueTreesSO, dialogueTreesJSON);
        }
    }

    public void setAIBehaviour(GameObject npc, string contextPrompt, int maxTokens)
    {
        AIConversationController aiConversationController = npc.GetComponent<AIConversationController>();
        
        if (aiConversationController == null)
        {
            Debug.LogError("The NPC is missing the AIConversationController");
        }
        else
        {
            aiConversationController.contextPrompt = contextPrompt;
            aiConversationController.maxTokens = maxTokens;
            
            aiConversationController.AddMessage(new Message { role = "system", content = globalContextPrompt });
            aiConversationController.AddMessage(new Message { role = "system", content = contextPrompt });
        }
    }
    public void setTTSProvider(GameObject npc, TTSProvider ttsProvider, string openAiVoiceId)
    {
        DialogueBoxController dialogueBoxController = npc.GetComponent<DialogueBoxController>();
        
        if (dialogueBoxController == null)
        {
            Debug.LogError("The NPC is missing the DialogueBoxController");
        }
        else if (ttsProvider == TTSProvider.OpenAI)
        {
            {
                dialogueBoxController.useOpenAiTTS();
            }

            AIResponseToSpeech aiResponseToSpeech = npc.GetComponent<AIResponseToSpeech>();
            if (aiResponseToSpeech == null){
                Debug.LogError("The NPC is missing the AIresponseToSpeech");
            }
            else{
                aiResponseToSpeech.OpenAiVoiceId = openAiVoiceId;
            }
        }
        else {
            dialogueBoxController.useWitTTS();
        }
    }
    public GameObject getNpc()
    {
        return spawnedNpc;
    }
}
