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
    [SerializeField] private GameObject _parentObject; // Which Gameobject should the instense of the model be a child of (probably the root of the NPC)
    [HideInInspector] private Vector3 _spawnLocation;
    [SerializeField] private GameObject _characterModelPrefab; // what should the NPC look like
    [SerializeField] private Avatar _avatar; // The animation avatar belonging to the model
    [SerializeField] private int _voicePresetId; // what should the NPC sound like
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
    [HideInInspector] private int _hasNewDialogueOptionsHash;
    [HideInInspector] private int _velocityYHash;
    [HideInInspector] private int _velocityXHash;
    [HideInInspector] private bool _isTalking;
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
        _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
        _velocityYHash = Animator.StringToHash("VelocityY");
        _velocityXHash = Animator.StringToHash("VelocityX");
        _isTalking = false;
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
            ttsSpeaker.GetComponentInChildren<TTSSpeaker>().SetVoiceOverride(ttsWitService.GetAllPresetVoiceSettings()[_voicePresetId]);
        }
    }

    public void ChangeCharacter(GameObject characterModelPrefab, Avatar avatar, RuntimeAnimatorController runtimeAnimatorController, int voicePresetId)
    {
        // keep a reference to the old stuff, so it safely can be destroyed later
        _oldbonesAndSkin = _bonesAndSkin;
        // Save all the old animator parameter values
        this._characterModelPrefab = characterModelPrefab;
        this._avatar = avatar;
        this._runtimeAnimatorController = runtimeAnimatorController;
        SaveOldAnimationValues();
        this._voicePresetId = voicePresetId;
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
            _animator.SetBool(_hasNewDialogueOptionsHash, _hasNewDialogueOptions);
            _animator.SetFloat(_velocityXHash, _velocityX);
            _animator.SetFloat(_velocityYHash, _velocityY);
        } else {
            Debug.LogError("You are not settubg the animation values");
        }
    }

}
