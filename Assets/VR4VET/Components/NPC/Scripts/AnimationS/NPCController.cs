using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

public class NPCController: MonoBehaviour
{

    // Video Animation + movement
    // https://www.youtube.com/watch?v=TpQbqRNCgM0
    // Video Brackeys + NavMeshAgent etc.
    // https://www.youtube.com/watch?v=xppompv1DBg&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=11
    public float lookRadius = 8f;

    public float personalSpaceFactor = 4f;
    Transform target;
    NavMeshAgent agent;

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        
        // If the player is far away, go closer, but let them have some personal space. 
        if (distance >= lookRadius) {
            Vector3 direction = (target.position - transform.position).normalized;
            agent.SetDestination(target.position - direction * personalSpaceFactor);
            // we only walk forward
            animator.SetFloat("VelocityY", agent.velocity.magnitude);
        } else {
            // when the navMeshAgent has angular speed 1200 (aka. turns instantly)
            // we only walk forward
            animator.SetFloat("VelocityY", agent.velocity.magnitude);
        }
            // A failed attempt to get the npc to turn and look at the player
            // looks weird with the current animations
            // Vector3 whereToLook = new Vector3(target.transform.position.x, agent.transform.position.y, target.transform.position.z);
            // Debug.Log("where to Loookk: " + whereToLook);
            // agent.transform.LookAt(whereToLook);
    }


    // // visulaization of personal space and lookRadius
    // void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, lookRadius);
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(transform.position, personalSpaceFactor);
    // }
}
