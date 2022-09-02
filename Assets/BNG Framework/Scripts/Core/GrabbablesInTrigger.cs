using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG {

    /// <summary>
    /// Keep track of all grabbables within this trigger
    /// </summary>
    public class GrabbablesInTrigger : MonoBehaviour {

        /// <summary>
        /// All grabbables in trigger that are considered valid
        /// </summary>
        public Dictionary<Collider, Grabbable> NearbyGrabbables;

        /// <summary>
        /// All nearby Grabbables that are considered valid. I.e. Not being held, within range, etc.
        /// </summary>
        public Dictionary<Collider, Grabbable> ValidGrabbables;

        /// <summary>
        /// The closest valid grabbable. If grab button is pressed this is the object that will be grabbed.
        /// </summary>
        public Grabbable ClosestGrabbable;

        /// <summary>
        /// All grabbables in trigger that are considered valid
        /// </summary>
        public Dictionary<Collider, Grabbable> ValidRemoteGrabbables;

        /// <summary>
        /// Closest Valid Remote Grabbable may be highlighted
        /// </summary>
        public Grabbable ClosestRemoteGrabbable;

        /// <summary>
        /// Should we call events on grabbables
        /// </summary>
        [Header("Events")]
        public bool FireGrabbableEvents = true;

        /// <summary>
        /// If true, Grabbables in the trigger will only be considered valid if no objects are in the way between it and this transform
        /// </summary>
        [Header("Collision Checks")]
        [Tooltip("If true, Grabbables in the trigger will only be considered valid if no objects are in the way between it and this transform")]
        public bool RaycastRemoteGrabbables = false;

        /// <summary>
        /// If true, Remote Grabbables must not have any collisions between the Main Camera and the Remote Grabbable we are trying to reach. This can help prevent grabbing items through walls or around corners.
        /// </summary>
        [Tooltip(" If true, Remote Grabbables must not have any collisions between the Main Camera and the Remote Grabbable we are trying to reach. This can help prevent grabbing items through walls or around corners.")]
        public bool RemoteGrabbablesMustBeVisible = false;

        /// <summary>
        /// If RaycastRemoteGrabbables is true, use these layers to detect collisions between the grabber and the potential grabbable object. By Default only looking for collisions on the "Default" layer
        /// </summary>
        [Tooltip("If RaycastRemoteGrabbables is true, use these layers to detect collisions between the grabber and the potential grabbable object. By Default only looking for collisions on the 'Default' layer")]
        public LayerMask RemoteCollisionLayers = 1;


        // Cache these variables for GC
        private Grabbable _closest;
        private float _lastDistance;
        private float _thisDistance;
        private Dictionary<Collider, Grabbable> _valids;
        private Dictionary<Collider, Grabbable> _filtered;
        private Transform _eyeTransform;


        void Start() {
            NearbyGrabbables = new Dictionary<Collider, Grabbable>();
            ValidGrabbables = new Dictionary<Collider, Grabbable>();
            ValidRemoteGrabbables = new Dictionary<Collider, Grabbable>();

            // Used to check if an object is between the eye and this object if RemoteGrabbablesMustBeVisible is true
            if (Camera.main != null) {
                _eyeTransform = Camera.main.transform;
            }
        }

        void Update() {
            // Sort Grabbales by Distance so we can use that information later if we need it
            updateClosestGrabbable();
            updateClosestRemoteGrabbables();
        }

        void updateClosestGrabbable() {

            // Remove any Grabbables that may have been destroyed, deactivated, etc.
            NearbyGrabbables = SanitizeGrabbables(NearbyGrabbables);

            // Find any grabbables that can potentially be picked up
            ValidGrabbables = GetValidGrabbables(NearbyGrabbables);

            // Assign closest grabbable
            ClosestGrabbable = GetClosestGrabbable(ValidGrabbables);
        }

        void updateClosestRemoteGrabbables() {

            // Assign closest remote grabbable
            ClosestRemoteGrabbable = GetClosestGrabbable(ValidRemoteGrabbables, true, RaycastRemoteGrabbables);

            // We can't have a closest remote grabbable if we are over a grabbable.
            // The closestGrabbable always takes precedent of the closestRemoteGrabbable
            if (ClosestGrabbable != null) {
                ClosestRemoteGrabbable = null;
            }
        }

        public virtual Grabbable GetClosestGrabbable(Dictionary<Collider, Grabbable> grabbables, bool remoteOnly = false, bool raycastCheck = false) {
            _closest = null;
            _lastDistance = 9999f;

            if(grabbables == null) {
                return null;
            }

            foreach (var kvp in grabbables) {                

                if (kvp.Value == null || !kvp.Value.IsGrabbable()) {
                    continue;
                }

                // Use Collider transform as position
                _thisDistance = Vector3.Distance(kvp.Value.transform.position, transform.position);
                if (_thisDistance < _lastDistance && kvp.Value.isActiveAndEnabled) {
                    // Not a valid option
                    if (remoteOnly && !kvp.Value.RemoteGrabbable) {
                        continue;
                    }

                    // Not within remote grab range
                    if (remoteOnly && _thisDistance > kvp.Value.RemoteGrabDistance) {
                        continue;
                    }

                    // Do raycast check last to save a physics check
                    if(raycastCheck && !kvp.Value.RemoteGrabbing) {
                        if(CheckObjectBetweenGrabbable(transform.position, kvp.Value)) {
                            continue;
                        }

                        // So far no obstructions. Check if an object is between the camera
                        if(RemoteGrabbablesMustBeVisible && _eyeTransform != null) {
                            if (CheckObjectBetweenGrabbable(_eyeTransform.position, kvp.Value)) {
                                continue;
                            }
                        }
                    }

                    // This is now our closest grabbable
                    _lastDistance = _thisDistance;
                    _closest = kvp.Value;
                }
            }

            return _closest;
        }


        /// <summary>
        /// Check if there is an object / collision between the starting transform (our Grabber) and the grabbable object
        /// </summary>        
        /// <returns>True if there is an object between the starting position and the grabbable position</returns>
        public virtual bool CheckObjectBetweenGrabbable(Vector3 startingPosition, Grabbable theGrabbable) {
            RaycastHit hit;
            if (Physics.Linecast(startingPosition, theGrabbable.transform.position, out hit, RemoteCollisionLayers, QueryTriggerInteraction.Ignore)) {

                // Something in the way
                float hitDistance = Vector3.Distance(startingPosition, hit.point);
                if (hit.collider.gameObject != theGrabbable.gameObject) {
                    if(hitDistance > 0.09f) {
                        // Debug.Log("Something in-between : " + hit.collider.gameObject.name + " At Distance : " + hitDistance);
                        return true;
                    }
                    else {
                        // Debug.Log("Something in between but very close : " + hit.collider.gameObject.name);
                    }
                }
            }

            return false;
        }

        public Dictionary<Collider, Grabbable> GetValidGrabbables(Dictionary<Collider, Grabbable> grabs) {
            _valids = new Dictionary<Collider, Grabbable>();

            if (grabs == null) {
                return _valids;
            }

            // Check for objects that need to be removed from RemoteGrabbables
            foreach (var kvp in grabs) {
                if (isValidGrabbable(kvp.Key, kvp.Value) && !_valids.ContainsKey(kvp.Key)) {
                    _valids.Add(kvp.Key, kvp.Value);
                }
            }

            return _valids;
        }

        protected virtual bool isValidGrabbable(Collider col, Grabbable grab) {

            // Object has been deactivated. Remove it
            if (col == null || grab == null || !grab.isActiveAndEnabled || !col.enabled) {
                return false;
            }
            // Not considered grabbable any longer. May have been picked up, marked, etc.
            else if (!grab.IsGrabbable()) {
                return false;
            }
            // Snap Zone without an item isn't a valid grab. Want to skip this unless something is inside
            else if(grab.GetComponent<SnapZone>() != null && grab.GetComponent<SnapZone>().HeldItem == null) {
                return false;
            }
            // Position was manually set outside of break distance
            // No longer possible for it to be the closestGrabbable
            else if (grab == ClosestGrabbable) {
                if (grab.BreakDistance > 0 && Vector3.Distance(grab.transform.position, transform.position) > grab.BreakDistance) {
                    return false;
                }
            }

            return true;
        }

        public virtual Dictionary<Collider, Grabbable> SanitizeGrabbables(Dictionary<Collider, Grabbable> grabs) {
            _filtered = new Dictionary<Collider, Grabbable>();

            if (grabs == null) {
                return _filtered;
            }

            foreach (var g in grabs) {
                if (g.Key != null && g.Key.enabled && g.Value.isActiveAndEnabled) {

                    // If outside of distance then this collider may have been disabled / re-enabled. Scrub from Nearby
                    if (g.Value.BreakDistance > 0 && Vector3.Distance(g.Key.transform.position, transform.position) > g.Value.BreakDistance) {
                        continue;
                    }

                    // Collision check via raycast
                    _filtered.Add(g.Key, g.Value);
                }
            }

            return _filtered;
        }

        public virtual void AddNearbyGrabbable(Collider col, Grabbable grabObject) {

            if(NearbyGrabbables == null) {
                NearbyGrabbables = new Dictionary<Collider, Grabbable>();
            }

            if (grabObject != null && !NearbyGrabbables.ContainsKey(col)) {
                NearbyGrabbables.Add(col, grabObject);
            }
        }

        public virtual void RemoveNearbyGrabbable(Collider col, Grabbable grabObject) {
            if (grabObject != null && NearbyGrabbables != null && NearbyGrabbables.ContainsKey(col)) {
                NearbyGrabbables.Remove(col);
            }
        }

        public virtual void RemoveNearbyGrabbable(Grabbable grabObject) {
            if (grabObject != null) {

                foreach (var x in NearbyGrabbables) {
                    if (x.Value == grabObject) {
                        NearbyGrabbables.Remove(x.Key);
                        break;
                    }
                }
            }
        }

        public virtual void AddValidRemoteGrabbable(Collider col, Grabbable grabObject) {
            
            // Sanity check
            if(col == null || grabObject == null) {
                return;
            }

            // Ensure our collection has been initialized
            if (ValidRemoteGrabbables == null) {
                ValidRemoteGrabbables = new Dictionary<Collider, Grabbable>();
            }

            try {
                if (grabObject != null && grabObject.RemoteGrabbable && col != null && !ValidRemoteGrabbables.ContainsKey(col)) {
                    
                    ValidRemoteGrabbables.Add(col, grabObject);
                }
            }
            catch(System.Exception e) {
                Debug.Log("Could not add Collider " + col.transform.name + " " + e.Message);
            }
        }

        public virtual void RemoveValidRemoteGrabbable(Collider col, Grabbable grabObject) {
            if (grabObject != null && ValidRemoteGrabbables != null && ValidRemoteGrabbables.ContainsKey(col)) {
                ValidRemoteGrabbables.Remove(col);
            }
        }

        void OnTriggerEnter(Collider other) {

            // Check for standard Grabbables first
            Grabbable g = other.GetComponent<Grabbable>();
            if (g != null) {
                AddNearbyGrabbable(other, g);
                return;
            }

            // Check for Child Grabbables that reference a parent
            GrabbableChild gc = other.GetComponent<GrabbableChild>();
            if (gc != null && gc.ParentGrabbable != null) {
                AddNearbyGrabbable(other, gc.ParentGrabbable);
                return;
            }
        }

        void OnTriggerExit(Collider other) {
            Grabbable g = other.GetComponent<Grabbable>();
            if (g != null) {
                RemoveNearbyGrabbable(other, g);
                return;
            }

            GrabbableChild gc = other.GetComponent<GrabbableChild>();
            if (gc != null) {
                RemoveNearbyGrabbable(other, gc.ParentGrabbable);
                return;
            }
        }
    }
}