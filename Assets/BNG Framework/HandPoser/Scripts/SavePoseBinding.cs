using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace BNG {

    public class SavePoseBinding : MonoBehaviour {
        [Header("Save Input : ")]        
#if ENABLE_INPUT_SYSTEM
        [Tooltip("If this InputAction returns true, save the current hand pose using 'handPoser.CreateUniquePose(SaveNamePrefix)'")]
        public InputAction SaveInput;
#elif ENABLE_LEGACY_INPUT_MANAGER
        public KeyCode SaveKey = KeyCode.Space;
#endif

        [Header("Save name prefix : ")]
        [Tooltip("Prefix of the hand pose file name to use. For example, a prefix of 'HandPose' will save as 'HandPose 1', 'HandPose 2', etc.")]
        public string SaveNamePrefix = "HandPose";

        [Header("Debug : ")]
        [Tooltip("If true, the SaveInput binding will be shown on the screen gui. Will not show in VR.")]
        public bool ShowKeybindingToolTip = true;

        HandPoser handPoser;

        void Start() {
            handPoser = GetComponent<HandPoser>();

#if ENABLE_INPUT_SYSTEM
            // Action must be enabled before it can be used
            if(SaveInput != null) {
                SaveInput.Enable();
            }
#endif
        }

        void Update() {
#if ENABLE_INPUT_SYSTEM
            // New Input Save
            if (SaveInput != null && SaveInput.triggered) {
                handPoser.CreateUniquePose(SaveNamePrefix);
                Debug.Log("Created Hand Pose with prefix " + SaveNamePrefix);
            }
#elif ENABLE_LEGACY_INPUT_MANAGER
            // Legacy Save
            if (Input.GetKeyDown(SaveKey)) {
                handPoser.CreateUniquePose(SaveNamePrefix);
                Debug.Log("Created Hand Pose with prefix " + SaveNamePrefix);
            }
#endif
        }

        void OnGUI() {
            if(ShowKeybindingToolTip) {
#if ENABLE_INPUT_SYSTEM
                GUI.Box(new Rect(20, 20, 480, 24), "Press '<b>" + SaveInput.bindings[0].path + "</b>' to save the current hand pose");
#elif ENABLE_LEGACY_INPUT_MANAGER
                GUI.Box(new Rect(20, 20, 480, 24), "Press '<b>" + SaveKey.ToString() + "</b>' to save the current hand pose");
#endif
            }
        }
    }
}

