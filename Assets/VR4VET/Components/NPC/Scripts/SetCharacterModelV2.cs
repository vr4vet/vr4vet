using System;
using System.Collections;
using System.Collections.Generic;
using Meta.WitAi;
using Photon.Voice;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetCharacterModelV2 : MonoBehaviour
{
    // [SerializeField] private GameObject rig;
    // [SerializeField] private GameObject[] meshes;
    [SerializeField] private GameObject parentObject;
    [HideInInspector] private Vector3 spawnLocation;

    [HideInInspector] private Vector3 spawnRotation;

    [SerializeField] private GameObject characterModelPrefab;

    [SerializeField] private Avatar avatar;

    [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;

    [HideInInspector] private GameObject bonesAndSkin;
    [HideInInspector] private GameObject OldbonesAndSkin;

    [HideInInspector] private FollowThePlayerControllerV2 followThePlayerControllerV2;
    [HideInInspector] private DialogueBoxController dialogueBoxController;

    // animation stuff
    [HideInInspector] private Animator animator; 
    [HideInInspector] private int isTalkingHash;
    [HideInInspector] private int hasNewDialogueOptionsHash;
    [HideInInspector] private int velocityYHash;
    [HideInInspector] private int velocityXHash;

    [HideInInspector] private bool isTalking;
    [HideInInspector] private bool hasNewDialogueOptions;
    [HideInInspector] private float velocityY;
    [HideInInspector] private float velocityX;
    
    // Animation constraints
    [HideInInspector] private RigBuilder rigBuilder;
    [HideInInspector] private AnimationSpeakController animationSpeakController;
    [SerializeField] private RigLayer targetTracking;
    [SerializeField] private GameObject targetTrackingPrefab;
    [SerializeField] private GameObject[] ConstrainedObjects;
    [SerializeField] private GameObject SourceObject;

    [SerializeField] private GameObject mixamoRigParent;



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
        //spawnRotation = new Vector3(parentObject.transform.rotation.x, parentObject.transform.rotation.y, parentObject.transform.rotation.z);
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
        Debug.Log("Rotation: " + parentObject.transform.rotation.ToString());
        bonesAndSkin.transform.rotation = Quaternion.Euler(new Vector3(parentObject.transform.eulerAngles.x, parentObject.transform.eulerAngles.y, parentObject.transform.eulerAngles.z));
        //bonesAndSkin.transform.Rotate(new Vector3(parentObject.transform.rotation.x, parentObject.transform.rotation.y, parentObject.transform.rotation.z));

        targetTrackingPrefab.transform.SetParent(bonesAndSkin.transform);

        String mixamoString = mixamoRigParent.name.Split(":")[0];
        Debug.Log(mixamoString);
        GameObject conGameObject = GameObject.Find("mixamorig:Spine");
        Debug.Log("ConstrainedGame object is: " + conGameObject);

        animator = bonesAndSkin.GetComponent<Animator>();
        if (animator == null) {
            Debug.Log("Adding new Animator");
            animator = bonesAndSkin.AddComponent<Animator>();
            //animator = bonesAndSkin.GetComponent<Animator>();
        }
        animator.runtimeAnimatorController = runtimeAnimatorController;
        animator.avatar = avatar;
        // Resetting the old values
        animator.SetBool(isTalkingHash, isTalking);
        animator.SetBool(hasNewDialogueOptionsHash, hasNewDialogueOptions);
        animator.SetFloat(velocityXHash, velocityX);
        animator.SetFloat(velocityYHash, velocityY);
        updateOtherScripts(animator);
        // The rigbuilder
        rigBuilder = bonesAndSkin.GetComponent<RigBuilder>();
        if (rigBuilder == null) {
            rigBuilder = bonesAndSkin.AddComponent<RigBuilder>();
        }
        rigBuilder.layers.Clear();
        rigBuilder.layers.Add(targetTracking);
        
        MultiAimConstraint[] constraints = targetTracking.rig.GetComponentsInChildren<MultiAimConstraint>();
        int i = 0;
        foreach (MultiAimConstraint con in constraints)
        {
            if (i >= ConstrainedObjects.Length) {
                Debug.Log("The RigLayer has more contraints than you have specified in contrainedObjects");
            } else {
                con.data.constrainedObject = conGameObject.transform;
                //con.data.sourceObjects = new WeightedTransformArray{new WeightedTransform(targetRef.transform, 1)};
                i++;
            }
        }
        rigBuilder.Build();

        if (!bonesAndSkin.TryGetComponent<AnimationSpeakController>(out AnimationSpeakController animationSpeakController))
        {
            bonesAndSkin.AddComponent<AnimationSpeakController>();
            // The script also adds the source componenet
        }

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


    // void buildRig() {
    //     // create rig gameobject
    //     GameObject rigGameobject = new GameObject("Rig");
    //     rigGameobject.transform.SetParent(transform, true);
 
    //     // Add constraint to rig
    //     MultiAimConstraint constraint = rigGameobject.AddComponent<MultiAimConstraint>();
    //     constraint.Reset();
    //     constraint.data.constrainedObject = ConstrainedObject.transform;
    //     WeightedTransformArray sources = new WeightedTransformArray();
    //     sources.Add(new WeightedTransform(SourceObject.transform, 1f));
    //     constraint.data.sourceObjects = sources;
 
    //     // Add the rig
    //     Rig rig = rigGameobject.AddComponent<Rig>();
 
    //     // Add rig to rigbuilder
    //     rigBuilder = gameObject.AddComponent<RigBuilder>();
    //     rigBuilder.layers.Clear();
    //     rigBuilder.layers.Add(new RigLayer())
    //     rigBuilder.layers.Add(new RigBuilder.RigLayer(rig, true));
 
    //     // build rig graph (Does not seems to be doing anything)
    //     rigBuilder.Build();
    // }



}
