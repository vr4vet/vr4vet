using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowThePlayerController: MonoBehaviour
{
    public float startFollowingRadius = 8f;
    public float stopFollowingRadius = 4f;
    public bool shouldFollow = false;  // New parameter to control NPC's behavior.

    Transform target;
    NavMeshAgent agent;
    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("NPCController.Start() called");
        if (PlayerManager.instance == null)
        {
            // TODO: Is this reference okey? It should only be on playermanager in the scene
            // How to actually use this stupid singleton stuff. we could probably change this to not singleton
            PlayerManager.instance = FindObjectOfType<PlayerManager>();
            if (PlayerManager.instance == null)
            {
                Debug.LogError("PlayerManager instance was not found. Please ensure it exists in the scene.");
                return;
            }
        }

        // Check if the player is set in the PlayerManager instance
        if (PlayerManager.instance.player == null)
        {
            Debug.LogError("The player is not set in the PlayerManager instance.");
            return;
        }

            // find player manager object and get the player object
            target = PlayerManager.instance.player.transform;
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
