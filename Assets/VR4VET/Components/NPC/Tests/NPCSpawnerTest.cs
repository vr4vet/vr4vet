using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class NPCSpawnerTest
{
    private GameObject NPCSpawnerObject;
    private ScriptableObject _npc;

    [SetUp]
    public void SetUp()
    {
        SceneManager.CreateScene("NPCSpawnerTest");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("NPCSpawnerTest"));

        // Load the NPCSpawner and the example NPC
        var NPCSpawnerPrefab = Resources.Load<GameObject>("Prefabs/NPCSpawner");
        _npc = Resources.Load<ScriptableObject>("TestNPC/TestNPC");
        // Assign the NPC to the spawners NPC list
        NPCSpawnerObject = UnityEngine.Object.Instantiate(NPCSpawnerPrefab);
        //NPCSpawnerObject.GetComponent<NPCSpawner>();
    }
    
    [UnityTest]
    public IEnumerator TestSpawnerSimplePasses()
    {
        yield return null;
        // Use the Assert class to test conditions
        Assert.AreNotEqual(NPCSpawnerObject, null);
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.Destroy(NPCSpawnerObject);
        SceneManager.UnloadSceneAsync("NPCSpawnerTest");
    }
}
