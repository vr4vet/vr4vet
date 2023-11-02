using UnityEngine;
using Meta.WitAi.TTS.Utilities;
public class NPCSpawner : MonoBehaviour
{
    public GameObject SpawnNPC(Vector3 position, bool shouldFollow, GameObject npcPrefab)
    {
        // Instantiate the NPC prefab
        GameObject newNPC = Instantiate(npcPrefab, position, Quaternion.identity);

        // Attach the TTS components
        AttachTTSComponents(newNPC);

        // Other NPC setup code (unchanged)
        NPCController npcController = newNPC.GetComponent<NPCController>();
        if (npcController != null)
        {
            npcController.shouldFollow = shouldFollow;
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
