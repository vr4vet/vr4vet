using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowThePlayerControllerV2 : MonoBehaviour
{

    [SerializeField] private float startFollowingRadius = 8f;
    [SerializeField] private float stopFollowingRadius = 4f;
    [SerializeField] public bool shouldFollow = false; 

    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;


    void Awake() {
        target = GameObject.Find("XR Rig Advanced/PlayerController/CameraRig").transform;
        Debug.Log("Target: " + target);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
                agent.SetDestination(target.position - direction * stopFollowingRadius);
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
