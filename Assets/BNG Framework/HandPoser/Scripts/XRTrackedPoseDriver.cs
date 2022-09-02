using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace BNG {

    /// <summary>
    /// A simple alternative to the TrackedPoseDriver component.
    /// Feel free to swap this out with a TrackedPoseDriver from the XR Legacy Input Helpers package or using the new Unity Input System
    /// </summary>
    public class XRTrackedPoseDriver : MonoBehaviour {

        public TrackableXRDevice Device = TrackableXRDevice.HMD;

        protected InputDevice deviceToTrack;

        protected Vector3 initialLocalPosition;
        protected Quaternion initialLocalRotation;

        protected Vector3 currentLocalPosition;
        protected Quaternion currentLocalRotation;

        static List<InputDevice> devices = new List<InputDevice>();

        protected virtual void Awake() {
            initialLocalPosition = transform.localPosition;
            initialLocalRotation = transform.localRotation;
        }

        protected virtual void OnEnable() {
            Application.onBeforeRender += OnBeforeRender;
        }

        protected virtual void OnDisable() {
            Application.onBeforeRender -= OnBeforeRender;
        }

        protected virtual void Update() {
            RefreshDeviceStatus();

            UpdateDevice();
        }

        protected virtual void FixedUpdate() {
            UpdateDevice();
        }

        public virtual void RefreshDeviceStatus() {
            if (!deviceToTrack.isValid) {
                if (Device == TrackableXRDevice.HMD) {
                    deviceToTrack = GetHMD();
                }
                else if (Device == TrackableXRDevice.LeftController) {
                    deviceToTrack = GetLeftController();
                }
                else if (Device == TrackableXRDevice.RightController) {
                    deviceToTrack = GetRightController();
                }
            }
        }

        public virtual void UpdateDevice() {

            // Check and assign our device status
            if (deviceToTrack.isValid) {

                if (Device == TrackableXRDevice.HMD) {
                    transform.localPosition = currentLocalPosition = GetHMDLocalPosition();
                    transform.localRotation = currentLocalRotation = GetHMDLocalRotation();
                }
                else if (Device == TrackableXRDevice.LeftController) {
                    transform.localPosition = currentLocalPosition = GetControllerLocalPosition(ControllerHandedness.Left);
                    transform.localRotation = currentLocalRotation = GetControllerLocalRotation(ControllerHandedness.Left);
                }
                else if (Device == TrackableXRDevice.RightController) {
                    transform.localPosition = currentLocalPosition = GetControllerLocalPosition(ControllerHandedness.Right);
                    transform.localRotation = currentLocalRotation = GetControllerLocalRotation(ControllerHandedness.Right);
                }
            }
        }

        protected virtual void OnBeforeRender() {
            UpdateDevice();
        }

        public Vector3 GetHMDLocalPosition() {
            Vector3 localPosition;

            GetHMD().TryGetFeatureValue(CommonUsages.devicePosition, out localPosition);

            return localPosition;
        }

        public InputDevice GetHMD() {
            InputDevices.GetDevices(devices);

            var hmds = new List<InputDevice>();
            var dc1 = InputDeviceCharacteristics.HeadMounted;
            InputDevices.GetDevicesWithCharacteristics(dc1, hmds);

            return hmds.FirstOrDefault();
        }

        public Quaternion GetHMDLocalRotation() {
            Quaternion localRotation;

            GetHMD().TryGetFeatureValue(CommonUsages.deviceRotation, out localRotation);

            return localRotation;
        }

        public Vector3 GetControllerLocalPosition(ControllerHandedness handSide) {
            Vector3 localPosition = Vector3.zero;

            if (handSide == ControllerHandedness.Left) {
                GetLeftController().TryGetFeatureValue(CommonUsages.devicePosition, out localPosition);
            }
            else if (handSide == ControllerHandedness.Right) {
                GetRightController().TryGetFeatureValue(CommonUsages.devicePosition, out localPosition);
            }

            return localPosition;
        }

        public Quaternion GetControllerLocalRotation(ControllerHandedness handSide) {
            Quaternion localRotation = Quaternion.identity;

            if (handSide == ControllerHandedness.Left) {
                GetLeftController().TryGetFeatureValue(CommonUsages.deviceRotation, out localRotation);
            }
            else if (handSide == ControllerHandedness.Right) {
                GetRightController().TryGetFeatureValue(CommonUsages.deviceRotation, out localRotation);
            }

            return localRotation;
        }

        public InputDevice GetLeftController() {
            InputDevices.GetDevices(devices);

            var leftHandedControllers = new List<InputDevice>();
            var dc = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(dc, leftHandedControllers);
            return leftHandedControllers.FirstOrDefault();
        }

        public InputDevice GetRightController() {
            InputDevices.GetDevices(devices);

            var rightHandedControllers = new List<InputDevice>();
            var dc = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(dc, rightHandedControllers);

            return rightHandedControllers.FirstOrDefault();
        }
    }

    public enum TrackableXRDevice {
        HMD,
        LeftController,
        RightController
    }
}

