using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Configure if the NPC will follow the player or not, 
/// at what distance away from the player should the NPC begin to follow the player, 
/// and at what distance should the NPC stop away from the player.
/// shouldFollow: if false, the NPC will stand still. If true, the NPC will follow the player around at a distance
/// startFollowingRadius: If the player is further away than this radius, then the NPC will follow after them
/// personalSpaceFactor: The NPC will always keep at least this follow distance from the Player. The player can go closer to the NPC if needed
/// </summary>
public class FollowThePlayerControllerV2 : MonoBehaviour
{

    [SerializeField] private float startFollowingRadius = 8f;
    [SerializeField] private float personalSpaceFactor = 4f;
    [SerializeField] public bool shouldFollow = false; 

    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;

    void Awake() {
        // Find an object to follow on the player
        GameObject targetRef = GameObject.Find("CameraRig");
        if (targetRef != null)
        {
            target = targetRef.transform;
        }
        else
        {
            Debug.LogError("Cannot find XR Rig Advanced/PlayerController/CameraRig in the scene");
        }
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        // animator = GetComponent<Animator>();
        Debug.Log("Animator: " + animator);
    }

    // Update is called once per frame
    void Update()
    {   
        float distance = Vector3.Distance(target.position, transform.position);
        
        if (shouldFollow)
        {
            // If the player is far away, go closer, but let them have some personal space. 
            if (distance >= startFollowingRadius) 
            {
                Vector3 direction = (target.position - transform.position).normalized;
                agent.SetDestination(target.position - direction * personalSpaceFactor);
                animator.SetFloat("VelocityY", agent.velocity.magnitude);
            }
            else 
            {
                animator.SetFloat("VelocityY", agent.velocity.magnitude);
            }
        }
        else
        {
            // NPC should stand still
            animator.SetFloat("VelocityY", 0); // Set the animation state to idle
            agent.SetDestination(transform.position); // Stop the agent from moving
        }
    }
}
