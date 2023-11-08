using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public GameObject npcPrefab; // Drag and drop your NPC prefab here in the inspector.
    public Vector3 npcSpawnPosition; // Position where the NPC will be spawned

    private NPCSpawner npcSpawner; // Reference to the NPCSpawner
    private GameObject spawnedNPC; // Reference to the spawned NPC

    private float spawnDistance = 15.0f;
    private float despawnDistance = 20.0f;

    private void Start()
    {
        npcSpawner = FindObjectOfType<NPCSpawner>();
        if (npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found in the scene!");
        }
        
        if (PlayerManager.instance == null)
        {
            Debug.LogError("PlayerManager instance not set!");
        }
        else if (PlayerManager.instance.player == null)
        {
            Debug.LogError("PlayerManager's player GameObject not set!");
        }
    }

    private void Update()
    {
            if (PlayerManager.instance == null || PlayerManager.instance.player == null)
        {
            Debug.LogError("PlayerManager or player is not set.");
            return;
        }
        if (spawnedNPC == null)
        {
            float distanceToPlayer = Vector3.Distance(PlayerManager.instance.player.transform.position, npcSpawnPosition);
            if (distanceToPlayer <= spawnDistance)
            {
                SpawnNPC();
            }
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(PlayerManager.instance.player.transform.position, spawnedNPC.transform.position);
            if (distanceToPlayer > despawnDistance)
            {
                DespawnNPC();
            }
        }

        // Check if 'B' key is pressed to toggle NPC follow
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleFollow();
        }
    }

    private void SpawnNPC()
    {
        if (spawnedNPC == null && npcSpawner != null)
        {
            spawnedNPC = npcSpawner.SpawnNPC(npcSpawnPosition, false, npcPrefab);
        }
    }

    private void DespawnNPC()
    {
        if (spawnedNPC != null)
        {
            Destroy(spawnedNPC);
        }
    }

    private void ToggleFollow()
    {
        if (spawnedNPC != null)
        {
            NPCController npcController = spawnedNPC.GetComponent<NPCController>();
            if (npcController != null)
            {
                npcController.shouldFollow = !npcController.shouldFollow;
            }
        }
    }
}
