using UnityEngine;

public class EventTriggerDemo : MonoBehaviour
{

    // TODO: change from public to [SerializeField] private, and see what need to be public
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private GameObject npcPrefabV1; // Assign your NPC prefab in the inspector.
    
    [SerializeField] private GameObject npcPrefabV2; 
    // Hardcoded spawn positions
    private Vector3 greetingNPCSpawnPosition = new Vector3(0, 0, 0);
    private Vector3 proximityNPCSpawnPosition = new Vector3(1, 0, 1);
    private Vector3 taskNPCSpawnPosition = new Vector3(4, 0, 4);

    private Vector3 characterModelNPCV1SpawnPosition = new Vector3(5, 0, 5);
    private Vector3 characterModelNPCV2SpawnPosition = new Vector3(6, 0, 6);

    private NPCSpawner npcSpawner;
    private GameObject greetingNPC;
    private GameObject proximityNPC;
    private GameObject taskNPC;

    private GameObject characterModelNPCV1;
    private GameObject characterModelNPCV2;

    private float proximityRadius = 100.0f; // Radius for checking proximity to the player.
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
        proximityNPC = npcSpawner.SpawnNPC(proximityNPCSpawnPosition, true, npcPrefab);
        // Configure the proximity NPC here with dialogue or other components.
        proximityNPC.SetActive(true);

        // characterModelNPCV1 = npcSpawner.SpawnNPC(characterModelNPCV1SpawnPosition, true, npcPrefabV1);
        // characterModelNPCV2 = npcSpawner.SpawnNPC(characterModelNPCV2SpawnPosition, true, npcPrefabV2);
    }

    private void Update()
    {
        HandleProximityNPC();

        // Check for the 'B' key to spawn the taskNPC
        if (Input.GetKeyDown(KeyCode.B) && !taskNPCSpawned)
        {	
			Debug.Log("B key pressed");
            SpawnTaskNPC();
        }
    }

    private void HandleProximityNPC()
    {
        // Check the distance between the player and the proximity NPC spawn position
        if (proximityNPC != null && !proximityNPC.activeSelf)
        {
            float distanceToPlayer = Vector3.Distance(PlayerManager.instance.player.transform.position, proximityNPCSpawnPosition);
            if (distanceToPlayer <= proximityRadius)
            {
                // Activate the proximity NPC and make it follow the player
                proximityNPC.SetActive(true);
                FollowThePlayerControllerV2 proximityNPCController = proximityNPC.GetComponent<FollowThePlayerControllerV2>();
                Debug.Log("proximityNPCController" + proximityNPCController); 
                if (proximityNPCController != null)
                {
                    proximityNPCController.shouldFollow = true;
                    // Trigger the initial dialogue for proximity NPC here
                }
            }
        }
    }

    private void SpawnTaskNPC()
    {
        // Spawn the task NPC at the hardcoded spawn position
		Debug.Log("Spawning task NPC");
        taskNPC = npcSpawner.SpawnNPC(taskNPCSpawnPosition, false, npcPrefab);
        taskNPCSpawned = true;
        // Configure the task NPC here with dialogue or other components.
    }

    private void OnTriggerEnter(Collider other)
    {
        // This can be left empty if you only want the B key to trigger the taskNPC spawn
    }
}
