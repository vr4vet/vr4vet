using System;
using System.Collections;
using System.Collections.Generic;
using Meta.WitAi;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS.Utilities;
using Photon.Voice;
using UnityEngine;

public class SetCharacterModel : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    [HideInInspector] private Vector3 spawnLocation;
    [SerializeField] private GameObject characterModelPrefab;
    [SerializeField] private Avatar avatar;
    [SerializeField] private int voicePresetId;
    [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;
    
    [HideInInspector] private GameObject bonesAndSkin;
    [HideInInspector] private GameObject OldbonesAndSkin;

    [HideInInspector] private FollowThePlayerControllerV2 followThePlayerControllerV2;
    [HideInInspector] private DialogueBoxController dialogueBoxController;
    [HideInInspector] private ConversationController conversationController;

    [HideInInspector] private Animator animator; 
    [HideInInspector] private int isTalkingHash;
    [HideInInspector] private int hasNewDialogueOptionsHash;
    [HideInInspector] private int velocityYHash;
    [HideInInspector] private int velocityXHash;
    [HideInInspector] private bool isTalking;
    [HideInInspector] private bool hasNewDialogueOptions;
    [HideInInspector] private float velocityY;
    [HideInInspector] private float velocityX;

    // Start is called before the first frame update
    void Awake()
    {
        isTalkingHash = Animator.StringToHash("isTalking");
        hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
        isTalking = false;
        hasNewDialogueOptions = false;
        followThePlayerControllerV2 = parentObject.GetComponent<FollowThePlayerControllerV2>();
        dialogueBoxController = parentObject.GetComponent<DialogueBoxController>();
        conversationController = parentObject.GetComponentInChildren<ConversationController>();
        spawnLocation = new Vector3(0,0,0);
        //spawnRotation = new Vector3(parentObject.transform.rotation.x, parentObject.transform.rotation.y, parentObject.transform.rotation.z);
        CheckMissingComponents();
        NewCharacter();
    }

    private void CheckMissingComponents() {
        if (followThePlayerControllerV2 == null)
        {
            Debug.LogError("You are missing the script called followThePlayerController");
        }
        if (dialogueBoxController == null)
        {
            Debug.LogError("You are missing the script called dialogueBoxController");
        }
        if (conversationController == null)
        {
            Debug.LogError("You are missing the script called conversationController");
        }
    }

    private void NewCharacter() {
        bonesAndSkin = Instantiate(characterModelPrefab, spawnLocation, Quaternion.identity);
        bonesAndSkin.transform.SetParent(parentObject.transform);
        Vector3 bonesAndSkinLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        bonesAndSkin.transform.localPosition = bonesAndSkinLocation;
        Debug.Log("Rotation: " + parentObject.transform.rotation.ToString());
        bonesAndSkin.transform.rotation = Quaternion.Euler(new Vector3(parentObject.transform.eulerAngles.x, parentObject.transform.eulerAngles.y, parentObject.transform.eulerAngles.z));
        //bonesAndSkin.transform.Rotate(new Vector3(parentObject.transform.rotation.x, parentObject.transform.rotation.y, parentObject.transform.rotation.z));

        animator = bonesAndSkin.GetComponent<Animator>();
        if (animator == null) {
            Debug.Log("Adding new Animator");
            bonesAndSkin.AddComponent<Animator>();
            animator = bonesAndSkin.GetComponent<Animator>();
        }
        animator.runtimeAnimatorController = runtimeAnimatorController;
        animator.avatar = avatar;
        // Resetting the old values
        animator.SetBool(isTalkingHash, isTalking);
        animator.SetBool(hasNewDialogueOptionsHash, hasNewDialogueOptions);
        animator.SetFloat(velocityXHash, velocityX);
        animator.SetFloat(velocityYHash, velocityY);
        updateOtherScripts(animator);

        // change the voice
        TTSWit ttsWitService = parentObject.GetComponentInChildren<TTSWit>();
        TTSSpeaker ttsSpeaker = parentObject.GetComponentInChildren<TTSSpeaker>();
        Debug.Log("TtsWitService: " + ttsWitService);
        Debug.Log("ttsSpeaker: " + ttsSpeaker);
        if (ttsWitService != null && ttsSpeaker != null)
        {
            // Change the voice of the NPC
            Debug.Log("You are talking with: " + ttsWitService.GetAllPresetVoiceSettings()[voicePresetId].SettingsId);
            ttsSpeaker.ClearVoiceOverride();
            ttsSpeaker.GetComponentInChildren<TTSSpeaker>().SetVoiceOverride(ttsWitService.GetAllPresetVoiceSettings()[voicePresetId]);
        }

    }


    public void ChangeCharacter(GameObject characterModelPrefab, Avatar avatar, int voicePresetId)  {
        // remove old stuff
        OldbonesAndSkin = bonesAndSkin;
        // set new stuff 
        this.characterModelPrefab = characterModelPrefab;
        this.avatar = avatar;
        isTalking = animator.GetBool(isTalkingHash);
        hasNewDialogueOptions = animator.GetBool(hasNewDialogueOptionsHash);
        velocityX = animator.GetFloat(velocityXHash);
        velocityY = animator.GetFloat(velocityYHash);
        this.voicePresetId = voicePresetId;
        NewCharacter();
        Destroy(OldbonesAndSkin);
        //updateOtherScripts();
    }

    // remove?
    private void updateOtherScripts() {
        //Debug.Log("The animator we are setting is:" + animator);
        dialogueBoxController.updateAnimator();
        followThePlayerControllerV2.updateAnimator();
        conversationController.updateAnimator();
    }
    // keep
    private void updateOtherScripts(Animator animator) {
        //Debug.Log("The animator we are setting is:" + animator);
        dialogueBoxController.updateAnimator(animator);
        followThePlayerControllerV2.updateAnimator(animator);
        conversationController.updateAnimator(animator);
    }
}
