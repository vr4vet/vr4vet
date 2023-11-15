using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCharacterModelV2 : MonoBehaviour
{
    // [SerializeField] private GameObject rig;
    // [SerializeField] private GameObject[] meshes;
    [SerializeField] private GameObject parentObject;
    [HideInInspector] private Vector3 spawnLocation;

    [SerializeField] private GameObject characterModelPrefab;

    [SerializeField] private Avatar avatar;

    [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;


    // Start is called before the first frame update
    void Awake()
    {
        spawnLocation = new Vector3(0,0,0);
        VersionTwo();
    }

    // Update is called once per frame
    // void VersionOne()
    // {
    //     GameObject bones = Instantiate(rig, spawnLocation, Quaternion.identity);
    //     bones.transform.SetParent(parentObject.transform);
    //     Vector3 bonesLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
    //     bones.transform.localPosition = bonesLocation;
    //     bones.name = rig.name;

    //     spawnLocation = new Vector3(0,0,0);
    //     for (int i = 0; i < meshes.Length; i++)
    //     {
    //         GameObject skin = Instantiate(meshes[i], spawnLocation, Quaternion.identity);
    //         skin.transform.SetParent(parentObject.transform);
    //         Vector3 skinLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
    //         skin.transform.localPosition = skinLocation;
    //         skin.name = meshes[i].name;
    //         skin.SetActive(false);
    //         skin.SetActive(true);
    //     }
    // }

    private void VersionTwo() {
        // parentObject.GetComponent<Animator>().runtimeAnimatorController = runtimeAnimatorController;
        GameObject bonesAndSkin = Instantiate(characterModelPrefab, spawnLocation, Quaternion.identity);
        bonesAndSkin.transform.SetParent(parentObject.transform);
        Vector3 bonesAndSkinLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        bonesAndSkin.transform.localPosition = bonesAndSkinLocation;

        Animator ani = bonesAndSkin.GetComponent<Animator>();
        if (ani == null) {
            Debug.Log("Adding new Animator");
            bonesAndSkin.AddComponent<Animator>();
            ani = bonesAndSkin.GetComponent<Animator>();
        }
        ani.runtimeAnimatorController = runtimeAnimatorController;
        ani.avatar = avatar;
    }


    public void SetCharacterModel(GameObject characterModelPrefab)  {
        this.characterModelPrefab = characterModelPrefab;
        VersionTwo();
    }
}
