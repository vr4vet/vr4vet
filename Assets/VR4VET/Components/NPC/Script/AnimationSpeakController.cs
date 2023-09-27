using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationSpeakController : MonoBehaviour
{

    Animator animator;
    int isTalkingHash;
    MultiAimConstraint multiAimConstraint;
    Rig targetTracking;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // increases performance
        isTalkingHash = Animator.StringToHash("isTalking");
        // Ensure isTalking starts as false
        animator.SetBool(isTalkingHash, false);


        targetTracking = GetComponent<Rig>();
        // Find the MultiAimConstraint component within the "HeadTracking" GameObject
        Transform headTracking = targetTracking.transform.Find("HeadTracking");
        if (headTracking != null)
        {
            multiAimConstraint = headTracking.GetComponent<MultiAimConstraint>();
        }
        else
        {
            Debug.LogError("HeadTracking GameObject not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
         bool isTalking = animator.GetBool(isTalkingHash);
         bool pressToTalk = Input.GetKey("t");

        // if player presses t key
        if (!isTalking && pressToTalk) 
        {
            // then set the isTalking boolean to be true
            animator.SetBool(isTalkingHash, true);
        }

        // if player is not pressing t key
        if (isTalking && !pressToTalk) 
        {
            // then set the isTalking boolean to be false
            animator.SetBool(isTalkingHash, false);
        }

        //  // Add the code to control the multi-aim constraint here
        // if (isTalking)
        // {
        //     // Enable the multi-aim constraint when character is talking
        //     multiAimConstraint.weight = 1.0f;
        // }
        // else
        // {
        //     // Disable the multi-aim constraint when character is not talking
        //     multiAimConstraint.weight = 0.0f;
        // }
        // Debug.Log("MultiAimConstraint weight: " + multiAimConstraint.weight);
            
    }
}
