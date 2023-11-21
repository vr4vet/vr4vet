using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationContraintsController : MonoBehaviour
{
    Animator animator;
    private int isTalkingHash;
    RigBuilder rigBuilder; // Use RigBuilder instead of Rig
    MultiAimConstraint multiAimConstraint;

    /// <summary>
    /// Check that every compontent need exists
    /// Add contraints at run time, the NPC should look at the player (aka. CameraRig)
    /// </summary>
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
                    } else {
                        GameObject targetRef = GameObject.Find("XR Rig Advanced/PlayerController/CameraRig");
                        if (targetRef != null)
                        {
                            // Adds contraints at runtime
                            MultiAimConstraint[] constraints = rig.GetComponentsInChildren<MultiAimConstraint>();
                            foreach (MultiAimConstraint con in constraints)
                            {
                                con.data.sourceObjects = new WeightedTransformArray{new WeightedTransform(targetRef.transform, 1)};
                            }
                            rigBuilder.Build();
                        }
                        else
                        {
                            Debug.LogError("Cannot find XR Rig Advanced/PlayerController/CameraRig in the scene");
                        }
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

    /// <summary>
    /// The NPC should look at the player while talking
    /// </summary>
    void Update()
    {
         bool isTalking = animator.GetBool(isTalkingHash);

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
