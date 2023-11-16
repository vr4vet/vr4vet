using System.Collections;
using System.Collections.Generic;
using Meta.WitAi;
using UnityEngine;

public class SetCharacterModelV2 : MonoBehaviour
{
    // [SerializeField] private GameObject rig;
    // [SerializeField] private GameObject[] meshes;
    [SerializeField] private GameObject parentObject;
    [HideInInspector] private Vector3 spawnLocation;

    [SerializeField] private GameObject characterModelPrefab;

    [SerializeField] private Avatar avatar;

    [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;

    [HideInInspector] private GameObject bonesAndSkin;
    [HideInInspector] private GameObject OldbonesAndSkin;

    [HideInInspector] private FollowThePlayerControllerV2 followThePlayerControllerV2;
    [HideInInspector] private DialogueBoxController dialogueBoxController;

    [HideInInspector] private Animator animator; 
    [HideInInspector] private int isTalkingHash;
    [HideInInspector] private int hasNewDialogueOptionsHash;
    [HideInInspector] private int velocityYHash;
    [HideInInspector] private int velocityXHash;

    [HideInInspector] private bool isTalking;
    [HideInInspector] private bool hasNewDialogueOptions;
    [HideInInspector] private float velocityY;
    [HideInInspector] private float velocityX;



    //[HideInInspector] private List subscribeToAnimatorChange

    // Start is called before the first frame update
    void Awake()
    {
        isTalkingHash = Animator.StringToHash("isTalking");
        hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
        isTalking = false;
        hasNewDialogueOptions = false;
        followThePlayerControllerV2 = parentObject.GetComponent<FollowThePlayerControllerV2>();
        dialogueBoxController = parentObject.GetComponent<DialogueBoxController>();
        spawnLocation = new Vector3(0,0,0);
        VersionTwo();
    }

    // Update is called once per frame
    // void VersionOne()
    // {
    //     GameObject bones = Instantiate(rig, spawnLocation, Quaternion.identity);
    //     bones.transform.SetParent(parentObject.transform);
    //     Vector3 bonesLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
    //     bones.transform.localPosition = bonesLocation;
    //     bones.name = rig.name;

    //     spawnLocation = new Vector3(0,0,0);
    //     for (int i = 0; i < meshes.Length; i++)
    //     {
    //         GameObject skin = Instantiate(meshes[i], spawnLocation, Quaternion.identity);
    //         skin.transform.SetParent(parentObject.transform);
    //         Vector3 skinLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
    //         skin.transform.localPosition = skinLocation;
    //         skin.name = meshes[i].name;
    //         skin.SetActive(false);
    //         skin.SetActive(true);
    //     }
    // }

    private void VersionTwo() {
        // parentObject.GetComponent<Animator>().runtimeAnimatorController = runtimeAnimatorController;
        bonesAndSkin = Instantiate(characterModelPrefab, spawnLocation, Quaternion.identity);
        bonesAndSkin.transform.SetParent(parentObject.transform);
        Vector3 bonesAndSkinLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        bonesAndSkin.transform.localPosition = bonesAndSkinLocation;

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
        //updateOtherScripts();
    }


    public void SetCharacterModel(GameObject characterModelPrefab, Avatar avatar)  {
        // remove old stuff?
        OldbonesAndSkin = bonesAndSkin;
        // set new stuff
        this.characterModelPrefab = characterModelPrefab;
        this.avatar = avatar;
        isTalking = animator.GetBool(isTalkingHash);
        hasNewDialogueOptions = animator.GetBool(hasNewDialogueOptionsHash);
        velocityX = animator.GetFloat(velocityXHash);
        velocityY = animator.GetFloat(velocityYHash);
        VersionTwo();
        Destroy(OldbonesAndSkin);
        //updateOtherScripts();
    }


    private void updateOtherScripts() {
        //Debug.Log("The animator we are setting is:" + animator);
        dialogueBoxController.updateAnimator();
        followThePlayerControllerV2.updateAnimator();
    }
    private void updateOtherScripts(Animator animator) {
        //Debug.Log("The animator we are setting is:" + animator);
        dialogueBoxController.updateAnimator(animator);
        followThePlayerControllerV2.updateAnimator(animator);
    }
}
