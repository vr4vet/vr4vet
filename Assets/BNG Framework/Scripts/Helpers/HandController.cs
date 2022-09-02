using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// An example hand controller that sets animation values depending on Grabber state
    /// </summary>
    public class HandController : MonoBehaviour {

        [Tooltip("HandController parent will be set to this on Start if specified")]
        public Transform HandAnchor;

        [Tooltip("If true, this transform will be parented to HandAnchor and it's position / rotation set to 0,0,0.")]
        public bool ResetHandAnchorPosition = true;

        public Animator HandAnimator;

        [Tooltip("(Optional) If specified, this HandPoser can be used when setting poses retrieved from a grabbed Grabbable.")]
        public HandPoser handPoser;

        [Tooltip("(Optional) If specified, this AutoPoser component can be used when if set on the Grabbable, or if AutoPose is set to true")]
        public AutoPoser autoPoser;

        // We can use the HandPoseBlender to blend between an open and closed hand pose, using controller inputs such as grip and trigger as the blend values
        HandPoseBlender poseBlender;

        [Tooltip("How to handle the hand when nothing is being grabbed / idle. Ex : Can use an Animator to control the hand via blending, a HandPoser to control via blend states, AutoPoser to continually auto pose while nothing is being held, or 'None' if you want to handle the idle state yourself.")]
        public HandPoserType IdlePoseType = HandPoserType.HandPoser;

        [Tooltip("If true, the idle hand pose will be determined by the connected Valve Index Controller's finger tracking. Requires the SteamVR SDK. Make sure IdlePoseType is set to 'HandPoser'")]
        public bool UseIndexFingerTracking = true;

        /// <summary>
        /// How fast to Lerp the Layer Animations
        /// </summary>
        [Tooltip("How fast to Lerp the Layer Animations")]
        public float HandAnimationSpeed = 20f;

        [Tooltip("Check the state of this grabber to determine animation state. If null, a child Grabber component will be used.")]
        public Grabber grabber;

        [Header("Shown for Debug : ")]
        /// <summary>
        /// 0 = Open Hand, 1 = Full Grip
        /// </summary>
        public float GripAmount;
        private float _prevGrip;

        /// <summary>
        /// 0 = Index Curled in,  1 = Pointing Finger
        /// </summary>
        public float PointAmount;
        private float _prevPoint;

        /// <summary>
        /// 0 = Thumb Down, 1 = Thumbs Up
        /// </summary>
        public float ThumbAmount;
        private float _prevThumb;
        
        // Raw input values
        private bool _thumbIsNear = false;
        private bool _indexIsNear = false;
        private float _triggerValue = 0f;
        private float _gripValue = 0f;

        public int PoseId;

        ControllerOffsetHelper offset;
        InputBridge input;
        Rigidbody rigid;
        Transform offsetTransform;

        Vector3 offsetPosition {
            get {
                if(offset) {
                    return offset.OffsetPosition;
                }
                return Vector3.zero;
            }
        }

        Vector3 offsetRotation {
            get {
                if (offset) {
                    return offset.OffsetRotation;
                }
                return Vector3.zero;
            }
        }

        void Start() {

            rigid = GetComponent<Rigidbody>();
            offset = GetComponent<ControllerOffsetHelper>();
            offsetTransform = new GameObject("OffsetHelper").transform;
            offsetTransform.parent = transform;

            if (HandAnchor) {
                transform.parent = HandAnchor;
                offsetTransform.parent = HandAnchor;

                if (ResetHandAnchorPosition) {
                    transform.localPosition = offsetPosition;
                    transform.localEulerAngles = offsetRotation;
                }
            }
            
            if(grabber == null) {
                grabber = GetComponentInChildren<Grabber>();
            }

            // Subscribe to grab / release events
            if(grabber != null) {
                grabber.onAfterGrabEvent.AddListener(OnGrabberGrabbed);
                grabber.onReleaseEvent.AddListener(OnGrabberReleased);
            }

            // Try getting child animator
            SetHandAnimator();

            input = InputBridge.Instance;
        }

        public void Update() {

            CheckForGrabChange();

            // Set Hand state according to InputBridge
            UpdateFromInputs();
            
            // Holding something - update the appropriate component
            if(HoldingObject()) {
                UpdateHeldObjectState();
            }
            else {
                UpdateIdleState();
            }
        }

        public virtual void UpdateHeldObjectState() {
            // Holding Animator Grabbable
            if (IsAnimatorGrabbable()) {
                UpdateAnimimationStates();
            }
            // Holding Hand Poser Grabbable
            else if (IsHandPoserGrabbable()) {                
                UpdateHandPoser();
            }
            // Holding Auto Poser Grabbable
            else if (IsAutoPoserGrabbable()) {
                //EnableAutoPoser();
            }
        }

        public virtual void UpdateIdleState() {
            // Not holding something - update the idle state
            if (IdlePoseType == HandPoserType.Animator) {
                UpdateAnimimationStates();
            }
            else if (IdlePoseType == HandPoserType.HandPoser) {
                //UpdateHandPoser();
                UpdateHandPoserIdleState();

            }
            else if (IdlePoseType == HandPoserType.AutoPoser) {
                EnableAutoPoser(true);
            }
        }

        public GameObject PreviousHeldObject;

        public virtual bool HoldingObject() {

            if(grabber != null && grabber.HeldGrabbable != null) {
                return true;
            }

            return false;
        }

        public virtual void CheckForGrabChange() {
            if(grabber != null) {

                // Check for null object but no animator enabled
                if(grabber.HeldGrabbable == null && PreviousHeldObject != null) {                    
                    OnGrabDrop();
                }
                else if(grabber.HeldGrabbable != null && !GameObject.ReferenceEquals(grabber.HeldGrabbable.gameObject, PreviousHeldObject)) {
                    OnGrabChange(grabber.HeldGrabbable.gameObject);
                }
            }
        }

        public virtual void OnGrabChange(GameObject newlyHeldObject) {

            // Update Component state if the held object has changed
            if(HoldingObject()) {

                // Switch components based on held object properties
                // Animator
                if (grabber.HeldGrabbable.handPoseType == HandPoseType.AnimatorID) {
                    EnableHandAnimator();
                }
                // Auto Poser - Once
                else if (grabber.HeldGrabbable.handPoseType == HandPoseType.AutoPoseOnce) {
                    EnableAutoPoser(false);
                }
                // Auto Poser - Continuous
                else if (grabber.HeldGrabbable.handPoseType == HandPoseType.AutoPoseContinuous) {
                    EnableAutoPoser(true);
                }
                // Hand Poser
                else if (grabber.HeldGrabbable.handPoseType == HandPoseType.HandPose) {
                    // If we have a valid hand pose use it, otherwise fall back to a default closed pose
                    if (grabber.HeldGrabbable.SelectedHandPose != null) {
                        EnableHandPoser();

                        // Make sure blender isn't active
                        if(poseBlender != null) {
                            poseBlender.UpdatePose = false;
                        }

                        if(handPoser != null) {
                            handPoser.CurrentPose = grabber.HeldGrabbable.SelectedHandPose;
                        }
                    }
                    else {
                        // Debug.Log("No Selected Hand Pose was found.");
                    }
                }
            }

            PreviousHeldObject = newlyHeldObject;
        }

        /// <summary>
        /// Dropped our held item - nothing currently in our hands
        /// </summary>
        public virtual void OnGrabDrop() {

            // Should we use auto pose when nothing in the hand?
            if (IdlePoseType == HandPoserType.AutoPoser) {
                EnableAutoPoser(true);
            }
            else if (IdlePoseType == HandPoserType.HandPoser) {
                DisableAutoPoser();
            }
            else if (IdlePoseType == HandPoserType.Animator) {
                DisablePoseBlender();
                EnableHandAnimator();
                DisableAutoPoser();
            }

            PreviousHeldObject = null;
        }       

        public virtual void SetHandAnimator() {
            if (HandAnimator == null || !HandAnimator.gameObject.activeInHierarchy) {
                HandAnimator = GetComponentInChildren<Animator>();
            }
        }

        /// <summary>
        /// Update GripAmount, PointAmount, and ThumbAmount based raw input from InputBridge
        /// </summary>
        public virtual void UpdateFromInputs() {

            // Grabber may have been deactivated
            if (grabber == null || !grabber.isActiveAndEnabled) {
                grabber = GetComponentInChildren<Grabber>();
                GripAmount = 0;
                PointAmount = 0;
                ThumbAmount = 0;
                return;
            }

            // Update raw values based on hand side
            if (grabber.HandSide == ControllerHand.Left) {
                _indexIsNear = input.LeftTriggerNear;
                _thumbIsNear = input.LeftThumbNear;
                _triggerValue = input.LeftTrigger;
                _gripValue = input.LeftGrip;
            }
            else if (grabber.HandSide == ControllerHand.Right) {
                _indexIsNear = input.RightTriggerNear;
                _thumbIsNear = input.RightThumbNear;
                _triggerValue = input.RightTrigger;
                _gripValue = input.RightGrip;
            }

            // Massage raw values to get a better value set the animator can use
            GripAmount = _gripValue;
            ThumbAmount = _thumbIsNear ? 0 : 1;

            // Point Amount can vary depending on if touching or our input source
            PointAmount = 1 - _triggerValue; // Range between 0 and 1. 1 == Finger all the way out
            PointAmount *= InputBridge.Instance.InputSource == XRInputSource.SteamVR ? 0.25F : 0.5F; // Reduce the amount our finger points out if Oculus or XRInput

            // If not near the trigger, point finger all the way out
            if (input.SupportsIndexTouch && _indexIsNear == false && PointAmount != 0) {
                PointAmount = 1f;
            }
            // Does not support touch, stick finger out as if pointing if no trigger found
            else if (!input.SupportsIndexTouch && _triggerValue == 0) {
                PointAmount = 1;
            }
        }

        public bool DoUpdateAnimationStates = true;
        public bool DoUpdateHandPoser = true;

        public virtual void UpdateAnimimationStates()
        {
            if(DoUpdateAnimationStates == false) {
                return;
            }

            // Enable Animator if it was disabled by the hand poser
            if(IsAnimatorGrabbable() && !HandAnimator.isActiveAndEnabled) {
                EnableHandAnimator();
            }

            // Update Hand Animator info
            if (HandAnimator != null && HandAnimator.isActiveAndEnabled && HandAnimator.runtimeAnimatorController != null) {

                _prevGrip = Mathf.Lerp(_prevGrip, GripAmount, Time.deltaTime * HandAnimationSpeed);
                _prevThumb = Mathf.Lerp(_prevThumb, ThumbAmount, Time.deltaTime * HandAnimationSpeed);
                _prevPoint = Mathf.Lerp(_prevPoint, PointAmount, Time.deltaTime * HandAnimationSpeed);

                // 0 = Hands Open, 1 = Grip closes                        
                HandAnimator.SetFloat("Flex", _prevGrip);

                HandAnimator.SetLayerWeight(1, _prevThumb);

                //// 0 = pointer finger inwards, 1 = pointing out    
                //// Point is played as a blend
                //// Near trigger? Push finger down a bit
                HandAnimator.SetLayerWeight(2, _prevPoint);

                // Should we use a custom hand pose?
                if (grabber != null && grabber.HeldGrabbable != null) {
                    HandAnimator.SetLayerWeight(0, 0);
                    HandAnimator.SetLayerWeight(1, 0);
                    HandAnimator.SetLayerWeight(2, 0);

                    PoseId = (int)grabber.HeldGrabbable.CustomHandPose;

                    if (grabber.HeldGrabbable.ActiveGrabPoint != null) {

                        // Default Grip to 1 when holding an item
                        HandAnimator.SetLayerWeight(0, 1);
                        HandAnimator.SetFloat("Flex", 1);

                        // Get the Min / Max of our finger blends if set by the user
                        // This allows a pose to blend between states
                        // Index Finger
                        setAnimatorBlend(grabber.HeldGrabbable.ActiveGrabPoint.IndexBlendMin, grabber.HeldGrabbable.ActiveGrabPoint.IndexBlendMax, PointAmount, 2);

                        // Thumb
                        setAnimatorBlend(grabber.HeldGrabbable.ActiveGrabPoint.ThumbBlendMin, grabber.HeldGrabbable.ActiveGrabPoint.ThumbBlendMax, ThumbAmount, 1);                       
                    }
                    else {
                        // Force everything to grab if we're holding something
                        if (grabber.HoldingItem) {
                            GripAmount = 1;
                            PointAmount = 0;
                            ThumbAmount = 0;
                        }
                    }

                    HandAnimator.SetInteger("Pose", PoseId);
                    
                }
                else {
                    HandAnimator.SetInteger("Pose", 0);
                }
            }
        }

        void setAnimatorBlend(float min, float max, float input, int animationLayer) {
            HandAnimator.SetLayerWeight(animationLayer, min + (input) * max - min);
        }

        /// <summary>
        /// Returns true if there is a valid animator and the held grabbable is set to use an Animation ID
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAnimatorGrabbable() {
            return HandAnimator != null && grabber != null && grabber.HeldGrabbable != null && grabber.HeldGrabbable.handPoseType == HandPoseType.AnimatorID;
        }

        public virtual void UpdateHandPoser() {

            if (DoUpdateHandPoser == false) {
                return;
            }

            // HandPoser may have changed - check for new component
            if (handPoser == null || !handPoser.isActiveAndEnabled) {
                handPoser = GetComponentInChildren<HandPoser>();
            }                        

            // Bail early if missing any info
            if(handPoser == null || grabber == null || grabber.HeldGrabbable == null || grabber.HeldGrabbable.handPoseType != HandPoseType.HandPose) {
                return;
            }

            // Make sure blending isn't active
            if(poseBlender != null && poseBlender.UpdatePose) {
                poseBlender.UpdatePose = false;
            }

            // Update hand pose if changed
            if (handPoser.CurrentPose == null || handPoser.CurrentPose != grabber.HeldGrabbable.SelectedHandPose) {
                UpdateCurrentHandPose();
            }
        }

        public virtual bool IsHandPoserGrabbable() {
            return handPoser != null && grabber != null && grabber.HeldGrabbable != null && grabber.HeldGrabbable.handPoseType == HandPoseType.HandPose;
        }

        public virtual void UpdateHandPoserIdleState() {

            // Makde sure animator isn't firing while we do our idle state
            DisableHandAnimator();

            // Check if we need to set up the pose blender
            if(!SetupPoseBlender()) {
                // If Pose Blender couldn't be setup we should just exit
                return;
            }

            // Make sure poseBlender updates the pose
            poseBlender.UpdatePose = true;

            // Check for Valve Index Knuckles finger tracking
            if (UseIndexFingerTracking && InputBridge.Instance.IsValveIndexController) {
                UpdateIndexFingerBlending();
                return;
            }

            // Update pose blender depending on inputs from controller
            // Thumb near can be counted as 'thumbTouch', primaryTouch, secondaryTouch, or primary2DAxisTouch (such as on knuckles controller)
            poseBlender.ThumbValue = Mathf.Lerp(poseBlender.ThumbValue, _thumbIsNear ? 1 : 0, Time.deltaTime * handPoser.AnimationSpeed);

            // Use Trigger for Index Finger
            float targetIndexValue = _triggerValue;

            // If the index finger is on the trigger we can bring the finger in a bit
            if (targetIndexValue < 0.1f && _indexIsNear) {
                targetIndexValue = 0.1f;
            }
            poseBlender.IndexValue = Mathf.Lerp(poseBlender.IndexValue, targetIndexValue, Time.deltaTime * handPoser.AnimationSpeed);

            // Grip
            poseBlender.GripValue = _gripValue;
        }



        public virtual void UpdateIndexFingerBlending() {
#if STEAM_VR_SDK
            if (grabber.HandSide == ControllerHand.Left) {
                poseBlender.IndexValue = InputBridge.Instance.LeftIndexCurl;
                poseBlender.ThumbValue = InputBridge.Instance.LeftThumbCurl;
                poseBlender.MiddleValue = InputBridge.Instance.LeftMiddleCurl;
                poseBlender.RingValue = InputBridge.Instance.LeftRingCurl;
                poseBlender.PinkyValue = InputBridge.Instance.LeftPinkyCurl;
            }
            else if (grabber.HandSide == ControllerHand.Right) {
                poseBlender.IndexValue = InputBridge.Instance.RightIndexCurl;
                poseBlender.ThumbValue = InputBridge.Instance.RightThumbCurl;
                poseBlender.MiddleValue = InputBridge.Instance.RightMiddleCurl;
                poseBlender.RingValue = InputBridge.Instance.RightRingCurl;
                poseBlender.PinkyValue = InputBridge.Instance.RightPinkyCurl;
            }
#endif
        }

        public virtual bool SetupPoseBlender() {

            // Make sure we have a valid handPoser to work with
            if(handPoser == null || !handPoser.isActiveAndEnabled) {
                handPoser = GetComponentInChildren<HandPoser>(false);
            }

            // No HandPoser is found, we should just exit
            if (handPoser == null) {
                return false;
                // Debug.Log("Adding Hand Poser to " + transform.name);
                // handPoser = this.gameObject.AddComponent<HandPoser>();
            }

            // If no pose blender is found, add it and set it up so we can use it in Update()
            if (poseBlender == null || !poseBlender.isActiveAndEnabled) {
                poseBlender = GetComponentInChildren<HandPoseBlender>();
            }

            // If no pose blender is found, add it and set it up so we can use it in Update()
            if (poseBlender == null) {
                if(handPoser != null) {
                    poseBlender = handPoser.gameObject.AddComponent<HandPoseBlender>();
                }
                else {
                    poseBlender = this.gameObject.AddComponent<HandPoseBlender>();
                }

                // Don't update pose in Update since we will be controlling this ourselves
                poseBlender.UpdatePose = false;

                // Set up the blend to use some default poses
                poseBlender.Pose1 = GetDefaultOpenPose();
                poseBlender.Pose2 = GetDefaultClosedPose();
            }

            return true;
        }

        public virtual HandPose GetDefaultOpenPose() {
            return Resources.Load<HandPose>("Open");
        }

        public virtual HandPose GetDefaultClosedPose() {
            return Resources.Load<HandPose>("Closed");
        }

        public virtual void EnableHandPoser() {
            // Disable the hand animator if we have a valid hand pose to use
            if(handPoser != null) {
                // Just need to make sure animator isn't enabled
                DisableHandAnimator();
            }
        }

        public virtual void EnableAutoPoser(bool continuous) {

            // Check if AutoPoser was set
            if (autoPoser == null || !autoPoser.gameObject.activeInHierarchy) {

                if(handPoser != null) {
                    autoPoser = handPoser.GetComponent<AutoPoser>();
                }
                // Check for active children
                else {
                    autoPoser = GetComponentInChildren<AutoPoser>(false);
                }
            }

            // Do the auto pose
            if (autoPoser != null) {
                autoPoser.UpdateContinuously = continuous;

                if(!continuous) {
                    autoPoser.UpdateAutoPoseOnce();
                }

                DisableHandAnimator();

                // Disable pose blending updates
                DisablePoseBlender();
            }
        }

        public virtual void DisablePoseBlender() {
            if (poseBlender != null) {
                poseBlender.UpdatePose = false;
            }
        }

        public virtual void DisableAutoPoser() {
            if (autoPoser != null) {
                autoPoser.UpdateContinuously = false;
            }
        }

        public virtual bool IsAutoPoserGrabbable() {
            return autoPoser != null && grabber != null && grabber.HeldGrabbable != null && (grabber.HeldGrabbable.handPoseType == HandPoseType.AutoPoseOnce || grabber.HeldGrabbable.handPoseType == HandPoseType.AutoPoseContinuous);
        }

        public virtual void EnableHandAnimator() {
            if (HandAnimator != null && HandAnimator.enabled == false) {
                HandAnimator.enabled = true;
            }

            // If using a hand poser reset the currennt pose so it can be set again later
            if(handPoser != null) {
                handPoser.CurrentPose = null;
            }
        }

        public virtual void DisableHandAnimator() {
            if (HandAnimator != null && HandAnimator.enabled) {
                HandAnimator.enabled = false;
            }
        }

        public virtual void OnGrabberGrabbed(Grabbable grabbed) {
            // Set the Hand Pose on our component
            if (grabbed.SelectedHandPose != null) {
                UpdateCurrentHandPose();
            }
            else if(grabbed.handPoseType == HandPoseType.HandPose && grabbed.SelectedHandPose == null) {
                // Debug.Log("No HandPose selected for object '" + grabbed.transform.name + "'. Falling back to default hand pose.");

                // Fall back to the closed pose if no selected hand pose was found
                grabbed.SelectedHandPose = GetDefaultClosedPose();
                UpdateCurrentHandPose();
            }
        }

        public virtual void UpdateCurrentHandPose() {
            if(handPoser != null) {
                // Update the pose
                handPoser.CurrentPose = grabber.HeldGrabbable.SelectedHandPose;
                handPoser.OnPoseChanged();
            }
        }

        public virtual void OnGrabberReleased(Grabbable released) {
            OnGrabDrop();
        }
    }
    
    public enum HandPoserType {
        HandPoser,
        Animator,
        AutoPoser,
        None
    }
}