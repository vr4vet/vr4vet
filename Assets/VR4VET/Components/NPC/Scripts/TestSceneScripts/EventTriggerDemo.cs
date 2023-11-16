using UnityEngine;

public class EventTriggerDemo : MonoBehaviour
{

    // TODO: change from public to [SerializeField] private, and see what need to be public
    [SerializeField] private GameObject npcPrefab; // Assign your NPC prefab in the inspector.
    
    // Hardcoded spawn positions
    private Vector3 greetingNPCSpawnPosition = new Vector3(-15, 0, -4);
    private Vector3 proximityNPCSpawnPosition = new Vector3(6, 0, 3);

    private Vector3 taskNPCSpawnPosition = new Vector3(-1, 0, -9);

    private NPCSpawner npcSpawner;
    private GameObject greetingNPC;
    private GameObject proximityNPC;
    private FollowThePlayerControllerV2 proximityNPCController;
    private GameObject taskNPC;

    private float proximityRadius = 12.0f; // Radius for checking proximity to the player.
    private bool taskNPCSpawned = false; // To ensure task NPC is only spawned once.

    private void Start()
    {
        npcSpawner = FindObjectOfType<NPCSpawner>();
        if (npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found in the scene!");
            return;
        }

        // Spawn the greeting NPC at the hardcoded position
        greetingNPC = npcSpawner.SpawnNPC(greetingNPCSpawnPosition, false, npcPrefab);
        // Configure the greeting NPC here with dialogue or other components.

        // Spawn the proximity NPC at the hardcoded position but deactivate it until the player is close enough
        proximityNPC = npcSpawner.SpawnNPC(proximityNPCSpawnPosition, false, npcPrefab);
        // Configure the proximity NPC here with dialogue or other components.
        proximityNPC.SetActive(true);
        HandleProximityNPC();
    }

    private void Update()
    {
        UpdateProximityNPC();

    }

    private void HandleProximityNPC()
    {
        if (proximityNPC != null) {proximityNPCController = proximityNPC.GetComponent<FollowThePlayerControllerV2>();}

    }
    private void UpdateProximityNPC()
    {
        // Check the distance between the player and the proximity NPC spawn position
        float distanceToPlayer = Vector3.Distance(PlayerManager.instance.player.transform.position, proximityNPCSpawnPosition);
        if (distanceToPlayer <= proximityRadius)
        {
            Debug.Log("Player is close enough to the proximity NPC");
            
                // Activate the proximity NPC and make it follow the player
            if (proximityNPCController != null)
            {
                proximityNPCController.shouldFollow = true;
                // Trigger the initial dialogue for proximity NPC here
            }
            
        }
        else if (proximityNPCController != null)   // Deactivate the proximity NPC's follow behavior if the player is too far away
        {
            proximityNPCController.shouldFollow = false;
        }
    }


    public GameObject SpawnTaskNPC()
    {
        // Spawn the task NPC at the hardcoded spawn position
		Debug.Log("Spawning task NPC");
        taskNPC = npcSpawner.SpawnNPC(taskNPCSpawnPosition, false, npcPrefab);
        taskNPCSpawned = true;
        return taskNPC;
        // Configure the task NPC here with dialogue or other components.
    }

    // private void OnTriggerEnter(Collider other)
    // {           // Check if the colliding object is the player and if the wall is "Wall (2)"
        
    //     if (!taskNPCSpawned) 
    //     {   
    //         Debug.Log("Player entered the trigger");
    //         SpawnTaskNPC();
    //     }

        
    // }
}
