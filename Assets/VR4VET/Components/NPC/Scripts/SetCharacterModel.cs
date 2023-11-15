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
    void Start()
    {
        VersionOne();
    }

    // Update is called once per frame
    void VersionOne()
    {
        spawnLocation = new Vector3(0,0,0);
        for (int i = 0; i < meshes.Length; i++)
        {
            GameObject skin = Instantiate(meshes[i], spawnLocation, Quaternion.identity);
            skin.transform.SetParent(parentObject.transform);
            Vector3 skinLocation = new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.y);
            skin.transform.localPosition = skinLocation;
            //GameObject skin = Instantiate(meshes[i], parentObject.transform, true);
            skin.name = meshes[i].name;
        }

        GameObject bones = Instantiate(rig, spawnLocation, Quaternion.identity);
        bones.transform.SetParent(parentObject.transform);
        //Vector3 bonesLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        Vector3 bonesLocation =  new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.y);
        bones.transform.localPosition = bonesLocation;
        //GameObject bones = Instantiate(rig, parentObject.transform, true);
        bones.name = rig.name;
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
