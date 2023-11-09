using UnityEngine;
using Meta.WitAi.TTS.Utilities;
public class NPCSpawner : MonoBehaviour
{
    public GameObject SpawnNPC(Vector3 position, bool shouldFollow, GameObject npcPrefab)
    {
        // Instantiate the NPC prefab
        Debug.Log("Spawning NPC at " + position);
        GameObject newNPC = Instantiate(npcPrefab, position, Quaternion.identity);

        // Attach the TTS components
        AttachTTSComponents(newNPC);

        // Other NPC setup code (unchanged)
        FollowThePlayerControllerV2 npcController = newNPC.GetComponent<FollowThePlayerControllerV2>();
        if (npcController != null)
        {
            Debug.Log("Setting shouldFollow to " + shouldFollow);
            npcController.shouldFollow = shouldFollow;
        }
        else
        {
            Debug.Log("NPC prefab does not have an NPCController component!");
        }
        return newNPC;
    }

    private void AttachTTSComponents(GameObject npc)
    {
        // Load the TTS prefab from the Resources folder
        GameObject ttsPrefab = Resources.Load<GameObject>("TTS");
        
        if (ttsPrefab != null)
        {
            // Instantiate the TTS prefab and parent it under the NPC
            GameObject ttsObject = Instantiate(ttsPrefab, npc.transform);

            // Assuming the TTSSpeaker component is either on the TTS prefab 
            // or one of its children, we find it and set it on the DialogueBoxController
            TTSSpeaker ttsSpeaker = ttsObject.GetComponentInChildren<TTSSpeaker>();
            DialogueBoxController dialogueController = npc.GetComponent<DialogueBoxController>();

            if (ttsSpeaker != null && dialogueController != null)
            {
                dialogueController.TTSSpeaker = ttsSpeaker.gameObject;
            }
        }
        else
        {
            Debug.LogError("TTS prefab could not be loaded. Ensure it's located in the Resources folder.");
        }
    }
}
