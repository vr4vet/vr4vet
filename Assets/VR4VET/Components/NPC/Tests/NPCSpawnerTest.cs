using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class NPCSpawnerTest
{
    private GameObject NPCSpawner;

    [SetUp]
    public void SetUp()
    {
        SceneManager.CreateScene("NPCSpawnerTest");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("NPCSpawnerTest"));

        var NPCSpawnerPrefab = Resources.Load<GameObject>("Prefabs/NPCSpawner");
        NPCSpawner = UnityEngine.Object.Instantiate(NPCSpawnerPrefab);
    }
    
    [UnityTest]
    public IEnumerator TestSpawnerSimplePasses()
    {
        yield return null;
        // Use the Assert class to test conditions
        Assert.AreNotEqual(NPCSpawner, null);
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.Destroy(NPCSpawner);
        SceneManager.UnloadSceneAsync("NPCSpawnerTest");
    }
}
