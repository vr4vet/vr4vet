using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace BNG {
    public class SampleHandController : MonoBehaviour {

        public ControllerHandedness ControllerSide = ControllerHandedness.Right;

        public PoseableObject HeldObject;
        protected bool wasHoldingObject = false;

        // Used to set the handpose depending on if we're holding an object or in the idle state
        Animator handAnimator;
        HandPoser handPoser;
        AutoPoser autoPoser;

        // We can use the HandPoseBlender to blend between an open and closed hand pose, using controller inputs such as grip and trigger as the blend values
        HandPoseBlender poseBlender;

        void Start() {
            handAnimator = GetComponentInChildren<Animator>();
            handPoser = GetComponentInChildren<HandPoser>();
            autoPoser = GetComponentInChildren<AutoPoser>();

            poseBlender = GetComponentInChildren<HandPoseBlender>();

            // If no pose blender is found, add it and set it up so we can use it in Update()
            if (poseBlender == null) {
                poseBlender = this.gameObject.AddComponent<HandPoseBlender>();
                poseBlender.Pose1 = Resources.Load<HandPose>("Open");
                poseBlender.Pose2 = Resources.Load<HandPose>("Closed");
            }

            // Don't update pose in Update since we will be controlling this ourselves
            poseBlender.UpdatePose = false;
        }

        public virtual void Update() {
            DoHandControllerUpdate();
        }

        public virtual void DoHandControllerUpdate() {
            // Make sure we're reading inputs from the most up to date controllers
            UpdateXRDevices();

            // Set the finger inputs on the Pose Blender
            UpdateFingerInputs();

            // Check if we are holding something
            if (HoldingObject()) {
                // Set hand poser or autoposer depending on the item set
                DoHeldItemPose();
            }
            else {
                // If nothing equipped, do idle pose based on finger inputs
                DoIdlePose();
            }
        }

        public virtual void SetCurrentlyHeldObject(GameObject holdObject) {
            if(holdObject != null) {
                HeldObject = holdObject.GetComponent<PoseableObject>();
            }
            else {
                HeldObject = null;
            }
        }

        public virtual void ClearCurrentlyHeldObject() {
            if(HeldObject != null) {
                HeldObject = null;
                ResetToIdleComponents();
            }

            wasHoldingObject = false;
        }

        // Just use the HandPoser and make sure animator / auto pose don't interfere
        public virtual void ResetToIdleComponents() {
            handPoser.enabled = true;
            handPoser.CurrentPose = null;

            if (autoPoser) {
                autoPoser.enabled = false;
            }

            if(handAnimator) {
                handAnimator.enabled = false;
            }
        }

        public virtual void UpdateFingerInputs() {
            // Update pose blender depending on inputs from controller
            // Thumb near can be counted as 'thumbTouch', primaryTouch, secondaryTouch, or primary2DAxisTouch (such as on knuckles controller)
            poseBlender.ThumbValue = Mathf.Lerp(poseBlender.ThumbValue, GetThumbIsNear() ? 1 : 0, Time.deltaTime * handPoser.AnimationSpeed);

            // Use Trigger for Index Finger
            float targetIndexValue = correctValue(getFeatureUsage(controller, CommonUsages.trigger));

            // If the index finger is on the trigger we can bring the finger in a bit
            if (targetIndexValue < 0.1f && GetIndexIsNear()) {
                targetIndexValue = 0.1f;
            }
            poseBlender.IndexValue = Mathf.Lerp(poseBlender.IndexValue, targetIndexValue, Time.deltaTime * handPoser.AnimationSpeed);

            // Grip
            poseBlender.GripValue = correctValue(getFeatureUsage(controller, CommonUsages.grip));
        }

        public virtual void DoHeldItemPose() {

            // One time switch
            if(!wasHoldingObject) {
                // Auto Pose 
                if ((HeldObject.poseType == PoseableObject.PoseType.AutoPoseContinuous || HeldObject.poseType == PoseableObject.PoseType.AutoPoseOnce) && autoPoser != null) {
                    // Continuous
                    handAnimator.enabled = false;
                    autoPoser.enabled = true;
                    autoPoser.UpdateContinuously = true;
                    handPoser.CurrentPose = null; // No pose here

                    // Single
                    // Disable Continuos Auto Pose After Duration
                    Invoke("DisableContinousAutoPose", HeldObject.AutoPoseDuration);
                }

                // Hand Pose?
                else if (HeldObject.poseType == PoseableObject.PoseType.HandPose) {
                    handAnimator.enabled = false;
                    autoPoser.enabled = false;
                    handPoser.CurrentPose = HeldObject.EquipHandPose;
                }
                // Animator Id?
                else if (HeldObject.poseType == PoseableObject.PoseType.HandPose) {
                    autoPoser.enabled = false;
                    handPoser.enabled = false;
                    handAnimator.enabled = true;
                    handAnimator.SetInteger("HandPoseId", HeldObject.HandPoseID);
                }
            }

            wasHoldingObject = true;
        }

        public virtual void DisableContinousAutoPose() {
            if(autoPoser) {
                autoPoser.UpdateContinuously = false;
            }
        }

        public virtual void DoIdlePose() {
            poseBlender.DoIdleBlendPose();
        }

        public virtual bool HoldingObject() {
            return HeldObject != null;
        }

        float correctValue(float inputValue) {
            return (float)System.Math.Round(inputValue * 1000f) / 1000f;
        }

        #region XRInputs

        // Used in XRInput
        static List<InputDevice> devices = new List<InputDevice>();
        InputDevice controller;

        public virtual void UpdateXRDevices() {
            // Refresh XR devices
            InputDevices.GetDevices(devices);

            // Assign Controller based on left / right handedness
            if (ControllerSide == ControllerHandedness.Right) {
                controller = GetRightController();
            }
            else {
                controller = GetLeftController();
            }
        }

        float getFeatureUsage(InputDevice device, InputFeatureUsage<float> usage) {
            float val;
            device.TryGetFeatureValue(usage, out val);

            return Mathf.Clamp01(val);
        }

        bool getFeatureUsage(InputDevice device, InputFeatureUsage<bool> usage) {
            bool val;
            if (device.TryGetFeatureValue(usage, out val)) {
                return val;
            }

            return val;
        }

        public virtual InputDevice GetLeftController() {
            InputDevices.GetDevices(devices);

            var leftHandedControllers = new List<InputDevice>();
            var dc = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(dc, leftHandedControllers);
            return leftHandedControllers.FirstOrDefault();
        }

        public virtual InputDevice GetRightController() {
            InputDevices.GetDevices(devices);

            var rightHandedControllers = new List<InputDevice>();
            var dc = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(dc, rightHandedControllers);

            return rightHandedControllers.FirstOrDefault();
        }

        public virtual bool GetThumbIsNear() {
#pragma warning disable 0618
            return getFeatureUsage(controller, CommonUsages.thumbTouch) > 0 ||
                getFeatureUsage(controller, CommonUsages.primaryTouch) ||
                getFeatureUsage(controller, CommonUsages.secondaryTouch) ||
                getFeatureUsage(controller, CommonUsages.primary2DAxisTouch);
#pragma warning restore 0618
        }

        public virtual bool GetIndexIsNear() {
#pragma warning disable 0618
            return getFeatureUsage(controller, CommonUsages.indexTouch) > 0;
#pragma warning restore 0618
        }

        #endregion
    }

    public enum ControllerHandedness {
        Left,
        Right,
        None
    }
}

