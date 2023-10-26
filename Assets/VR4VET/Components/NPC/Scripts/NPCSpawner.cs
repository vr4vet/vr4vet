using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject SpawnNPC(Vector3 position, bool shouldFollow, GameObject npcPrefab)
	{
		GameObject newNPC = Instantiate(npcPrefab, position, Quaternion.identity);
		NPCController npcController = newNPC.GetComponent<NPCController>();
		
		if(npcController != null)
		{
			npcController.shouldFollow = shouldFollow;
			// Set any other parameters here.
		}

		return newNPC;
	}

}
