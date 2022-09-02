using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG {
    public class SlidingDoorMover : MonoBehaviour {

        public float DoorSpeed = 5f; // Ex : Move 5m per second

        // Where the local X value should be for the door to be fully open
        public float OpenXValue = -1f;

        float targetXPosition = 0;
        float smoothedPosition = 0;

        void Update() {

            // Take our target value (0-1) and smooth it
            smoothedPosition = Mathf.Lerp(smoothedPosition, targetXPosition, Time.deltaTime * DoorSpeed);

            transform.localPosition = new Vector3(smoothedPosition, 0, 0);
        }

        // Call this from your wheel script. Should be 0-1, where 0 is closed, 1 is open, 0.5 is halfway, etc.
        public void SetTargetPosition(float targetValue) {

            // Convert this 0-1 number into a local x position
            targetXPosition = OpenXValue * targetValue; // Ex: 0.5 means the door is halfway open, or at -0.25 local X position
        }
    }
}

