using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG {
    public class HandModelSwitcher : MonoBehaviour {

        public int HandModelId = 1;

        public HandModelSelector hms;

        void Start() {
            if(hms == null) {
                hms = FindObjectOfType<HandModelSelector>();
            }

            if(hms == null) {
                Debug.Log("No Hand Model Selector Found in Scene. Will not be able to switch hand models");
            }
        }

        public void OnTriggerEnter(Collider other) {
            if(other.gameObject.GetComponent<Grabber>() != null) {
                // Switch to this hand model
                hms.ChangeHandsModel(HandModelId, false);
            }
        }
    }
}

