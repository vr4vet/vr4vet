using System.Collections.Generic;
using UnityEngine;

public class PointingController : MonoBehaviour
{
    private List<NpcData> npcs;
    private GameObject _objectToLookAt;
    
    void Start()
    {
        // This list will store necessary data for each NPC in the scene
        npcs = new List<NpcData>();
        HashSet<Transform> parentSet = new HashSet<Transform>();
        
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        // Loop through all the game objects in the scene
        foreach (GameObject obj in allGameObjects)
        {
            // Check if the name contains "PBR", which means it is an NPC model
            if (obj.name.Contains("PBR"))
            {
                Transform parentTransform = obj.transform.parent;
                if (parentTransform != null && !parentSet.Contains(parentTransform))
                {
                    // Store the initial position and rotation of the NPC which would be used to reset it position and add it to the list
                    float initialPosition = obj.transform.position.y;
                    Quaternion initialRotation = obj.transform.rotation;
                    npcs.Add(new NpcData(obj, initialPosition, initialRotation));
                    
                    // This set is used to avoid adding the same NPC model multiple times, which happens for some unknown reason
                    parentSet.Add(parentTransform);
                }
            }
        }

        // Loop through the list of NPCs
        foreach (NpcData npcData in npcs)
        {
            /* Instead of keeping its generic model name it will be changed to the actual name of the NPC which the parent has.
            This is done to make it easier to identify the NPC later in the script */
            Transform parentTransform = npcData.Npc.transform.parent;
            if (parentTransform != null)
            {
                npcData.Npc.name = parentTransform.gameObject.name;
            }
            else
            {
                Debug.Log(npcData.Npc.name + " has no parent.");
            }
        }
    }
    
    public void ChangeDirection(int section, GameObject talkingNpc)
    {
        // Find the correct NPC based on the currently speaking NPC in the scene
        NpcData npcData = npcs.Find(npc => npc.Npc.name == talkingNpc.name);
        
        // Set the direction the NPC should look at
        if (npcData != null)
        {
            // Reset the position of the NPC to make sure it stays in the same place
            var vector3 = npcData.Npc.transform.position;
            vector3.y = npcData.InitialPosition;
            npcData.Npc.transform.position = vector3;
            // The object to look at is stored in the dialogue tree
            _objectToLookAt = talkingNpc.GetComponent<DialogueBoxController>().dialogueTreeRestart.sections[section].objectToLookAt;
            // Find the object in the scene which corresponds to the prefab that is set in the dialogue tree
            _objectToLookAt = GameObject.Find(_objectToLookAt.name);
            
            if (_objectToLookAt == null)
            {
                Debug.Log("Object to look at not found");
            } 
            else
            {
                // Look at the correct object based on the dialogue text
                Vector3 direction = _objectToLookAt.transform.position - npcData.Npc.transform.position;
                direction.y = 0;
                if (direction != Vector3.zero)
                {
                    npcData.Npc.transform.rotation = Quaternion.LookRotation(direction);
                }
            }
        } 
    }

    // Reset the direction of the NPC
    public void ResetDirection(GameObject talkingNpc)
    {
        NpcData npcData = npcs.Find(npc => npc.Npc.name == talkingNpc.name);
        
        if (npcData != null)
        { 
            // The NPC is reset to it's initial rotation. The initial rotation always updated in case the NPC has moved 
            Vector3 rotation = npcData.InitialRotation.eulerAngles; 
            rotation.y = npcData.Npc.transform.parent.rotation.eulerAngles.y;
            npcData.InitialRotation = Quaternion.Euler(rotation);
            npcData.Npc.transform.rotation = npcData.InitialRotation;
        }
        else
        {
            Debug.Log("NPC not found");
        }
    }
}


