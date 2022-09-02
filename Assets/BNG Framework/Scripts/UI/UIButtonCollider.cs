using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {
    public class UIButtonCollider : MonoBehaviour {

        [Header("Hold Button Down Option")]

        [Tooltip("If true, this button can be held down and the button's click event will be fired repeatedly. If false, the button's click event will only be called once per trigger enter")]
        public bool CanBeHeldDown = true;

        [Tooltip("Amount of time that must first pass before continuing to fire the button's click event. This value is used once. The 'HoldDownDelay' value is then used for each subsequent delay. ")]
        public float InitialHoldDownDelay = 0.5f;

        [Tooltip("The amount of time that must pass in-between click events while the button is held down. ")]
        public float HoldDownDelay = 0.1f;

        [Header("Animate Key")]
        [Tooltip("If true, this transform will be animated down on the Z axis by the PressedInZValue amount.")]
        public bool AnimateKey = true;

        [Tooltip("If AnimateKey is true, this transform will be animated down on the Z axis by this amount.")]
        public float PressedInZValue = 0.01f;

        [Tooltip("How fast to Lerp the key in")]
        public float PressInSpeed = 15f;

        // The Unity Button to invoke events on
        UnityEngine.UI.Button uiButton;

        protected int itemsInTrigger = 0;

        // Can we fire the event
        protected bool readyForDownEvent = true;
        protected int clickCount = 0;
        protected float lastPressTime = 0f;

        // Used for animating the button in
        protected BoxCollider boxCollider;
        protected float colliderInitialCenterZ = 0;

        void Awake() {
            uiButton = GetComponentInParent<UnityEngine.UI.Button>();
            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider) {
                colliderInitialCenterZ = boxCollider.center.z;
            }
        }

        void Update() {

            if (itemsInTrigger > 0) {

                //  Something is in the trigger area, so push the key down
                if (AnimateKey) {
                    transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, new Vector3(transform.parent.localPosition.x, transform.parent.localPosition.y, PressedInZValue), Time.deltaTime * PressInSpeed);

                    // Need to move the collider up since we're pushing the key down
                    if (boxCollider) {
                        boxCollider.center = Vector3.Lerp(boxCollider.center, new Vector3(boxCollider.center.x, boxCollider.center.y, colliderInitialCenterZ - PressedInZValue), Time.deltaTime * PressInSpeed);
                    }
                }

                // Can hold down the button in order to keep pressing events
                float requiredDelay = clickCount == 1 ? InitialHoldDownDelay : HoldDownDelay;
                if (CanBeHeldDown && !readyForDownEvent && (Time.time - lastPressTime >= requiredDelay)) {
                    readyForDownEvent = true;
                }

                if (readyForDownEvent) {
                    // Call the event
                    if (uiButton != null && uiButton.onClick != null) {
                        uiButton.onClick.Invoke();
                        // uiButton.Select(); // May want to select the ui button
                    }

                    clickCount++;
                    lastPressTime = Time.time;

                    // Just pressed the button, so no longer ready for down event
                    readyForDownEvent = false;
                }
            }
            else {
                if (AnimateKey) {
                    transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, new Vector3(transform.parent.localPosition.x, transform.parent.localPosition.y, 0), Time.deltaTime * PressInSpeed);

                    // Need to move the collider back in place as we're no longer pushing the key down
                    if (boxCollider) {
                        boxCollider.center = Vector3.Lerp(boxCollider.center, new Vector3(boxCollider.center.x, boxCollider.center.y, colliderInitialCenterZ), Time.deltaTime * PressInSpeed);
                    }
                }

                // Nothing in the trigger - ready for down event
                clickCount = 0;
                readyForDownEvent = true;
            }
        }

        void OnTriggerEnter(Collider other) {
            //if (other.GetComponent<UITrigger>() != null || other.GetComponent<Grabber>() != null) {
            if (other.GetComponent<UITrigger>() != null) {
                itemsInTrigger++;
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.GetComponent<UITrigger>() != null) {
                itemsInTrigger--;
            }
        }
    }
}

