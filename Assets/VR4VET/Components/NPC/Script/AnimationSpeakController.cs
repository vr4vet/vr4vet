using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationSpeakController : MonoBehaviour
{
    Animator animator;
    int isTalkingHash;
    RigBuilder rigBuilder; // Use RigBuilder instead of Rig
    MultiAimConstraint multiAimConstraint;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigBuilder = GetComponent<RigBuilder>();

        // increases performance
        isTalkingHash = Animator.StringToHash("isTalking");
        // Ensure isTalking starts as false
        animator.SetBool(isTalkingHash, false);

        // Find the MultiAimConstraint component within the "TargetTracking" Rig Layer
        if (rigBuilder != null && rigBuilder.layers.Count > 0)
        {
            // Check if a Rig Layer with the name "TargetTracking" exists
            RigLayer rigLayer = rigBuilder.layers.Find(layer => layer.name == "TargetTracking");

            if (rigLayer != null)
            {
                // Access the Rig component within the Rig Layer
                Rig rig = rigLayer.rig;

                if (rig != null)
                {
                    // Access the constraints within the Rig
                    multiAimConstraint = rig.GetComponentsInChildren<MultiAimConstraint>(true).FirstOrDefault();

                    if (multiAimConstraint == null)
                    {
                        Debug.LogError("MultiAimConstraint not found in the 'TargetTracking' Rig Layer.");
                    }
                }
                else
                {
                    Debug.LogError("Rig component not found in the 'TargetTracking' Rig Layer.");
                }
            }
            else
            {
                Debug.LogError("Rig Layer 'TargetTracking' not found.");
            }
        }
        else
        {
            Debug.LogError("RigBuilder component not found.");
        }
    
    }

    // Update is called once per frame
    void Update()
    {
         bool isTalking = animator.GetBool(isTalkingHash);
         bool pressToTalk = Input.GetKey(KeyCode.T);

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

         // Add the code to control the multi-aim constraint here
        if (isTalking)
        {
            // Enable the multi-aim constraint when character is talking
            multiAimConstraint.weight = 1.0f;
        }
        else
        {
            // Disable the multi-aim constraint when character is not talking
            multiAimConstraint.weight = 0.0f;
        }
        
            
    }
}
