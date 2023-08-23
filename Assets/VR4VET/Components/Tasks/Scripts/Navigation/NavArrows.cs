/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using UnityEngine;
using UnityEngine.AI;

namespace Task
{
    /// <summary>
    /// This will control every single arrow that will be created in navigation system
    /// </summary>
    public class NavArrows : MonoBehaviour
    {
        NavigationManager arrayManager;
        private NavMeshAgent agent;


        void Start()
        {
            arrayManager = NavigationManager.navigationManager;
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.GetComponent<Renderer>().enabled = false;
        }


        void Update()
        {
            if (!arrayManager.target)
                return;

            agent.GetComponent<Renderer>().enabled = true;
            agent.SetDestination(arrayManager.target.transform.position);

            if (agent.velocity != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(agent.velocity, Vector3.up);

            if (Vector3.Distance(transform.position, arrayManager.target.transform.position) < 2f ||
                Vector3.Distance(arrayManager.player.transform.position, arrayManager.target.transform.position) < 5f)
            {
                arrayManager.removeArrowFromList(gameObject);
                Destroy(gameObject);
            }

        }


    }
}