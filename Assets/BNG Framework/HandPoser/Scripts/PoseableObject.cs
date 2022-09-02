using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// A helper component you can place on grabbable object to decide which hand pose method and definition to use
    /// For example : When grabbing an object, you can use GetComponent<PoseableObject> to check whether to apply a specific HandPose to the HandPoser, or to enable AutoPose, set an ID on a hand animator, or implement your own custom solution
    /// </summary>
    public class PoseableObject : MonoBehaviour {

        [Header("Pose Type")]
        public PoseType poseType = PoseType.HandPose;

        [Header("Hand Pose Properties")]
        [Tooltip("Set this HandPose on the Handposer when PoseType is set to 'HandPose'")]
        public HandPose EquipHandPose;

        [Header("Auto Pose Properties")]
        [Tooltip("If PoseType = AutoPoseOnce, AutoPose will be run for this many seconds")]
        public float AutoPoseDuration = 0.15f;

        [Header("Animator Properties")]
        [Tooltip("Set animator ID to this value if PoseType is set to 'Animator'")]
        public int HandPoseID;

        public enum PoseType {
            HandPose,
            AutoPoseOnce,
            AutoPoseContinuous,
            Animator,
            Other,
            None
        }
    }
}
