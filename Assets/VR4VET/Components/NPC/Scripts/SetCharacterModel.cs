using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetCharacterModel : MonoBehaviour
{

    [SerializeField] private GameObject rig;
    [SerializeField] private GameObject[] meshes;
    [SerializeField] private GameObject parentObject;
    [HideInInspector] private Vector3 spawnLocation;

    [SerializeField] private GameObject characterModelPrefab;

    [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;


    // Start is called before the first frame update
    void Awake()
    {
        VersionTwo();
    }

    // Update is called once per frame
    void VersionOne()
    {
        GameObject bones = Instantiate(rig, spawnLocation, Quaternion.identity);
        bones.transform.SetParent(parentObject.transform);
        Vector3 bonesLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        bones.transform.localPosition = bonesLocation;
        bones.name = rig.name;

        spawnLocation = new Vector3(0,0,0);
        for (int i = 0; i < meshes.Length; i++)
        {
            GameObject skin = Instantiate(meshes[i], spawnLocation, Quaternion.identity);
            skin.transform.SetParent(parentObject.transform);
            Vector3 skinLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
            skin.transform.localPosition = skinLocation;
            skin.name = meshes[i].name;
            skin.SetActive(false);
            skin.SetActive(true);
        }
    }

    void VersionTwo() {
        Animator ani = characterModelPrefab.GetComponent<Animator>();
        ani.runtimeAnimatorController = runtimeAnimatorController;
        ani.avatar = null;
        GameObject bonesAndSkin = Instantiate(characterModelPrefab, spawnLocation, Quaternion.identity);
        bonesAndSkin.transform.SetParent(parentObject.transform);
        Vector3 bonesAndSkinLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        bonesAndSkin.transform.localPosition = bonesAndSkinLocation;


    }
}
