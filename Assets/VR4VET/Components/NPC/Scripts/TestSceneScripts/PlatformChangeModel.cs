using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class PlatformChangeModel : MonoBehaviour
{
    [SerializeField] private GameObject[] ModelPrefabs;
    [HideInInspector] private int currentModelNr;

    [SerializeField] private GameObject NPC;
    [HideInInspector] private SetCharacterModelV2 setCharacterModelV2;

    void Awake() {
        currentModelNr = 0;
        setCharacterModelV2 = NPC.GetComponent<SetCharacterModelV2>();
    }

    private void ChangeModel() {
        setCharacterModelV2.SetCharacterModel(ModelPrefabs[currentModelNr]);
        currentModelNr++;
    }
}
