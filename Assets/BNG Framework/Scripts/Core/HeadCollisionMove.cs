using System.Collections;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// This component can push the player backwards when it collides with a wall / object. Attach to the 
    /// </summary>
    public class HeadCollisionMove : MonoBehaviour {

        [Tooltip("Enable collision? Set to false if you don't want to enable this")]
        public bool CollisionEnabled = true;

        [Tooltip("Only collide against the specified World Tag?")]
        public bool OnlyCollideAgainstWorld = true;

        [SerializeField]
        private string worldTag = "World";

        [SerializeField]
        private GameObject cameraRig;

        [SerializeField]
        private Transform centerEyeAnchor;

        void Start() {
            
        }

        void OnCollisionStay(Collision collision) {

            // Component may not be enabled
            if (!CollisionEnabled) {
                return;
            }

            // Additionally check for world collision
            if (OnlyCollideAgainstWorld && !collision.collider.CompareTag(worldTag)) {
                return;
            }

            StartCoroutine(PushBackPlayer());
        }

        void OnCollisionExit(Collision collision) {
            if (OnlyCollideAgainstWorld && !collision.collider.CompareTag(worldTag)) {
                return;
            }

            StopCoroutine(PushBackPlayer());
        }

        IEnumerator PushBackPlayer() {
            if (!CollisionEnabled) {
                yield break;
            }

            var delta = transform.position - centerEyeAnchor.position;
            delta.y = 0f;
            cameraRig.transform.position += delta;
        }
    }
}
