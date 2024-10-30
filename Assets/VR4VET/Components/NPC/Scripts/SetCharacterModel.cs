using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using Meta.WitAi;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS.Utilities;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetCharacterModel : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject; // Which Gameobject should the instense of the model be a child of (probably the root of the NPC)
    [HideInInspector] private Vector3 _spawnLocation;
    [SerializeField] private GameObject _characterModelPrefab; // what should the NPC look like
    [SerializeField] private Avatar _avatar; // The animation avatar belonging to the model
    [SerializeField] private int _WitVoiceId; // what should the NPC sound like
    [SerializeField] private RuntimeAnimatorController _runtimeAnimatorController;
    // The model with rig
    [HideInInspector] private GameObject _bonesAndSkin;
    [HideInInspector] private GameObject _oldbonesAndSkin;
    // scripts references
    [HideInInspector] private FollowThePlayerController _followThePlayerController;
    [HideInInspector] private DialogueBoxController _dialogueBoxController;
    [HideInInspector] private ConversationController _conversationController;
    // Animator and its parameters
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _isTalkingHash;
    [HideInInspector] private int _isListeningHash;
    [HideInInspector] private int _hasNewDialogueOptionsHash;
    [HideInInspector] private int _velocityYHash;
    [HideInInspector] private int _velocityXHash;
    [HideInInspector] private bool _isTalking;
    [HideInInspector] private bool _isListening;
    [HideInInspector] private bool _hasNewDialogueOptions;
    [HideInInspector] private float _velocityY;
    [HideInInspector] private float _velocityX;

    void Awake()
    {
        // set variables
        PrepareAnimationValues();
        _followThePlayerController = _parentObject.GetComponent<FollowThePlayerController>();
        _dialogueBoxController = _parentObject.GetComponent<DialogueBoxController>();
        _conversationController = _parentObject.GetComponentInChildren<ConversationController>();
        _spawnLocation = new Vector3(0, 0, 0);
        CheckMissingComponents();
        NewCharacter();
    }

    private void PrepareAnimationValues() {
        _isTalkingHash = Animator.StringToHash("isTalking");
        _isListeningHash = Animator.StringToHash("isListening");
        _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
        _velocityYHash = Animator.StringToHash("VelocityY");
        _velocityXHash = Animator.StringToHash("VelocityX");
        _isTalking = false;
        _isListening = false;
        _hasNewDialogueOptions = false;
        _velocityY = 0;
        _velocityX = 0;
    }

    private void CheckMissingComponents()
    {
        if (_followThePlayerController == null)
        {
            Debug.LogError("You are missing the script called followThePlayerController");
        }
        if (_dialogueBoxController == null)
        {
            Debug.LogError("You are missing the script called dialogueBoxController");
        }
        if (_conversationController == null)
        {
            Debug.LogError("You are missing the script called conversationController");
        }
    }

    private void NewCharacter()
    {
        // Instansiate the model (consisting of skin and bones (rig) as a child of the ParentObject
        // at the parentObjects location and with the same rotation
        _bonesAndSkin = Instantiate(_characterModelPrefab, _spawnLocation, Quaternion.identity);
        _bonesAndSkin.transform.SetParent(_parentObject.transform);
        Vector3 bonesAndSkinLocation = new Vector3(_spawnLocation.x, _spawnLocation.y, _spawnLocation.z);
        _bonesAndSkin.transform.localPosition = bonesAndSkinLocation;
        _bonesAndSkin.transform.rotation = Quaternion.Euler(new Vector3(_parentObject.transform.eulerAngles.x, _parentObject.transform.eulerAngles.y, _parentObject.transform.eulerAngles.z));

        // get or create a new animator and transfer over the old animation paramet values to this new one
        _animator = _bonesAndSkin.GetComponent<Animator>();
        if (_animator == null)
        {
            _bonesAndSkin.AddComponent<Animator>();
            _animator = _bonesAndSkin.GetComponent<Animator>();
        }
        _animator.runtimeAnimatorController = _runtimeAnimatorController;
        _animator.avatar = _avatar;
        // Resetting the old values
        SetNewAnimationValues();
        // We need to tell some other scripts that we changed the animator, so they can continue to refernce it correctly
        UpdateOtherScripts(_animator);

        // change the voice
        TTSWit ttsWitService = _parentObject.GetComponentInChildren<TTSWit>();
        TTSSpeaker ttsSpeaker = _parentObject.GetComponentInChildren<TTSSpeaker>();
        if (ttsWitService != null && ttsSpeaker != null)
        {
            // Change the voice of the NPC
            ttsSpeaker.ClearVoiceOverride();
            ttsSpeaker.GetComponentInChildren<TTSSpeaker>().SetVoiceOverride(ttsWitService.GetAllPresetVoiceSettings()[_WitVoiceId]);
        }

        // Attach rigbuilder to the head of the NPC to enable looking at player
        // Start by attaching rigbuilder to character (bonesAndSkin)
        if (_bonesAndSkin.GetComponent<RigBuilder>() == null) {
            RigBuilder rigBuilder = _bonesAndSkin.AddComponent<RigBuilder>();
            // Create new rig object and add it to layers of rigbuilder
            GameObject rigObject = new GameObject("TargetTracking");
            rigObject.transform.parent = _bonesAndSkin.transform;
            Rig rig = rigObject.AddComponent<Rig>();
            rigBuilder.layers.Add(new RigLayer(rig));
            // Add last object to containt multi-aim constraints
            GameObject aimObjectHead = new GameObject("AimObjectHead");
            GameObject aimObjectSpine = new GameObject("AimObjectSpine");
            aimObjectHead.transform.parent = rigObject.transform;
            aimObjectSpine.transform.parent = rigObject.transform;

            // Add multi-aim constraints components
            MultiAimConstraint constraintsHead = aimObjectHead.AddComponent<MultiAimConstraint>();
            MultiAimConstraint constraintsSpine = aimObjectSpine.AddComponent<MultiAimConstraint>();
            // Find the NPC head
            Component[] bodyParts = _bonesAndSkin.GetComponentsInChildren<Component>(true);
            GameObject NPCHead = null;
            GameObject NPCSpine = null;
            foreach (Component comp in bodyParts) {
                // Return component if name contains "Head"
                if (comp.gameObject.name.Contains(":Head") && NPCHead == null) { NPCHead = comp.gameObject; }
                // Return component if name contains "Spine"
                if (comp.gameObject.name.Contains(":Spine2") && NPCSpine == null) {NPCSpine = comp.gameObject; }
                // Stop checking body parts if both are found
                if (NPCHead != null && NPCSpine != null) { break; }
            }
            if (NPCHead == null) {
                Debug.LogError("Could not find NPC head");
            } else {
                // Set constrained object to NPC head
                constraintsHead.data.constrainedObject = NPCHead.transform;
            }
            if (NPCSpine == null) {
                Debug.LogError("Could not find NPC spine");
            } else {
                // Set constrained object to NPC spine
                constraintsSpine.data.constrainedObject = NPCSpine.transform;
            }

            // If there is already a animation constraints controller (f.ex from previous model) remove it
            if (_bonesAndSkin.GetComponent<AnimationConstraintsController>() != null) {
                Destroy(_bonesAndSkin.GetComponent<AnimationConstraintsController>());
            }

            // Add the constraints lastly at runtime so the animation constraints fit to the NEW model
            // Makes the rig "rebuild", could be perfomance heavy
            _bonesAndSkin.AddComponent<AnimationConstraintsController>();
            
            Debug.Log("Rigbuilder instantiated and configurated for NPC");

        } else {
            Debug.Log("RigBuilder already instantiated on NPC");
        }
    }

    public void ChangeCharacter(GameObject characterModelPrefab, Avatar avatar, RuntimeAnimatorController runtimeAnimatorController, int WitVoiceId)
    {
        // keep a reference to the old stuff, so it safely can be destroyed later
        _oldbonesAndSkin = _bonesAndSkin;
        // Save all the old animator parameter values
        this._characterModelPrefab = characterModelPrefab;
        this._avatar = avatar;
        this._runtimeAnimatorController = runtimeAnimatorController;
        SaveOldAnimationValues();
        this._WitVoiceId = WitVoiceId;
        // destory the old stuff
        Destroy(_oldbonesAndSkin);
        // Add the new model and set the saved values to the new animator
        NewCharacter();
    }

    private void UpdateOtherScripts(Animator animator)
    {
        _dialogueBoxController.updateAnimator(animator);
        _followThePlayerController.UpdateAnimator(animator);
        _conversationController.updateAnimator(animator);
    }

    private void SaveOldAnimationValues() {
        if (_runtimeAnimatorController.name.Contains("NPCHumanoidAnimationController")) {
            _isTalking = _animator.GetBool(_isTalkingHash);
            _isListening = _animator.GetBool(_isListeningHash);
            _hasNewDialogueOptions = _animator.GetBool(_hasNewDialogueOptionsHash);
            _velocityX = _animator.GetFloat(_velocityXHash);
            _velocityY = _animator.GetFloat(_velocityYHash);
        } else {
            Debug.LogError("You are not settubg the animation values");
        }
    }

    private void SetNewAnimationValues() {
        if (_runtimeAnimatorController.name.Contains("NPCHumanoidAnimationController")) {
            _animator.SetBool(_isTalkingHash, _isTalking);
            _animator.SetBool(_isListeningHash, _isListening);
            _animator.SetBool(_hasNewDialogueOptionsHash, _hasNewDialogueOptions);
            _animator.SetFloat(_velocityXHash, _velocityX);
            _animator.SetFloat(_velocityYHash, _velocityY);
        } else {
            Debug.LogError("You are not settubg the animation values");
        }
    }

}
