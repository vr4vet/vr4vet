using System.IO.Compression;
using System.Linq;
using TMPro.Examples;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationConstraintsController : MonoBehaviour
{
    [HideInInspector] private Animator animator;
    [HideInInspector] private int isTalkingHash;
    [HideInInspector] private RigBuilder rigBuilder; // Use RigBuilder instead of Rig
    [HideInInspector] private MultiAimConstraint multiAimConstraint;

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
                    }
                    else
                    {
                        GameObject targetRef = NPCToPlayerReferenceManager.Instance.PlayerTarget;
                        if (targetRef != null)
                        {
                            // Adds contraints at runtime
                            MultiAimConstraint[] constraints = rig.GetComponentsInChildren<MultiAimConstraint>();
                            foreach (MultiAimConstraint con in constraints)
                            {
                                // Set the player camera as the source object (what the NPC will look at)
                                var sourceObject = con.data.sourceObjects;
                                var newSource = new WeightedTransform(targetRef.transform, 1.0f);
                                sourceObject.Add(newSource);
                                con.data.sourceObjects = sourceObject;

                                // Manage settings for the constrained object(s)
                                con.data.aimAxis = MultiAimConstraintData.Axis.Z;
                                con.data.upAxis = MultiAimConstraintData.Axis.Y;
                                con.data.constrainedXAxis = true;
                                con.data.constrainedYAxis = true;
                                con.data.constrainedZAxis = true;
                                con.data.limits = new Vector2(-70f, 70f);

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
        //Debug.Log(publicCon.data.sourceObjects.Count);
        if (animator != null) {
            bool isTalking = animator.GetBool(isTalkingHash);
            // Add the code to control the multi-aim constraint here
            if (isTalking)
            {
                // Enable the multi-aim constraint when character is talking
                multiAimConstraint.weight += 0.004f;
            }
            else
            {
                // Disable the multi-aim constraint when character is not talking
                multiAimConstraint.weight -= 0.002f;
            }
        } else {
            animator = GetComponentInChildren<Animator>();
        }
    }
}
