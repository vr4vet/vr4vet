using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BNG {

    [CustomEditor(typeof(Explosive))]
    [CanEditMultipleObjects]
    public class ExplosiveEditor : Editor {

        Explosive explosive;

        public override void OnInspectorGUI() {

            explosive = (Explosive)target;
            
            if (GUILayout.Button("Explode!")) {
                if(Application.isPlaying) {
                    explosive.DoExplosion();
                }
                else {
                    Debug.Log("(Play Mode Only)");
                }
            }

            base.OnInspectorGUI();
        }
    }
}

