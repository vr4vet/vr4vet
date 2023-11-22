using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

/// <summary>
/// Configure if the NPC will follow the player or not, 
/// at what distance away from the player should the NPC begin to follow the player, 
/// and at what distance should the NPC stop away from the player.
/// shouldFollow: if false, the NPC will stand still. If true, the NPC will follow the player around at a distance
/// startFollowingRadius: If the player is further away than this radius, then the NPC will follow after them
/// personalSpaceFactor: The NPC will always keep at least this follow distance from the Player. The player can go closer to the NPC if needed
/// </summary>
public class FollowThePlayerController : MonoBehaviour
{

    [SerializeField] public float StartFollowingRadius = 8f;
    [SerializeField] public float PersonalSpaceFactor = 4f;
    [SerializeField] public bool ShouldFollow = false;

    [HideInInspector] private Transform _target;
    [HideInInspector] private NavMeshAgent _agent;
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _velocityYHash;

    void Start()
    {
        if (NPCToPlayerReferenceManager.Instance == null)
        {
            Debug.LogError("NPCManager instance was not found. Please ensure it exists in the scene.");
            return;
        }
        // Find an target-object to follow on the player
        _target = NPCToPlayerReferenceManager.Instance.PlayerTarget.transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _velocityYHash = Animator.StringToHash("VelocityY");
        if (_agent == null)
        {
            Debug.LogError("THe NPC is missing the NavMeshAgent");
            return;
        }
        if (_animator == null)
        {
            Debug.LogError("THe NPC is missing the Animator");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(_target.position, transform.position);
        if (_animator != null)
        {
            if (ShouldFollow)
            {
                // If the player is far away, go closer, but let them have some personal space. 
                if (distance >= StartFollowingRadius)
                {
                    Vector3 direction = (_target.position - transform.position).normalized;
                    _agent.SetDestination(_target.position - direction * PersonalSpaceFactor);
                    _animator.SetFloat(_velocityYHash, _agent.velocity.magnitude);
                }
                else
                {
                    _animator.SetFloat(_velocityYHash, _agent.velocity.magnitude);
                }
            }
            else
            {
                // NPC should stand still
                _animator.SetFloat(_velocityYHash, 0); // Set the animation state to idle
                _agent.SetDestination(transform.position); // Stop the agent from moving
            }
        } else {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    public void UpdateAnimator(Animator animator)
    {
        this._animator = animator;
    }
}
