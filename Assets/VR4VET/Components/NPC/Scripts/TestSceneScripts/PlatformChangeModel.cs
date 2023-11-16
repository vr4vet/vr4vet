using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class PlatformChangeModel : MonoBehaviour
{
    [SerializeField] private GameObject[] ModelPrefabs;

    [SerializeField] private Avatar[] modelAvatar;
    [HideInInspector] private int currentModelNr;

    [SerializeField] private GameObject NPC;
    [HideInInspector] private SetCharacterModelV2 setCharacterModelV2;

    [SerializeField] private GameObject collidingObject;

    void Awake() {
        currentModelNr = 0;
        setCharacterModelV2 = NPC.GetComponent<SetCharacterModelV2>();
    }

    private void ChangeModel() {
        setCharacterModelV2.SetCharacterModel(ModelPrefabs[currentModelNr], modelAvatar[currentModelNr]);
        currentModelNr++;
        if (currentModelNr >= ModelPrefabs.Length || currentModelNr >= modelAvatar.Length)
        {
            currentModelNr = 0;
        }
    }

    void OnTriggerEnter() {

        Debug.Log("WE ARE TRIGGGERD!!! Change skin");
        ChangeModel();
    }

    // void OnTriggerEnter(Collider other) {
    //     if (other.gameObject.Equals(collidingObject)) 
    //     {
    //         Debug.Log("WE ARE TRIGGGERD!!! Change skin");
    //         ChangeModel();
    //     }
    // }

}
