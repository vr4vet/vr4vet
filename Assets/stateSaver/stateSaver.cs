using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;
using Unity.XR.CoreUtils;

public class stateSaver: MonoBehaviour
{
    public List<GameObject> unityGameObjects = new List<GameObject>();
    public GameObject loadMenu;
    public GameObject mainMenu;
    [HideInInspector]
    public GameData gameData;

    public void Start()
    {
        string[] saveStates = Directory.GetFiles(Application.persistentDataPath + "\\gameSaves\\gameData", "*")
                                     .Select(Path.GetFileName)
                                     .ToArray();
        loadMenu.SetActive(true);
        for (int i= 0; i < 5 && i < saveStates.Length; i++)
        {
            GameObject gameObjectReference = loadMenu.transform.GetChild(i).gameObject;
            string saveName = saveStates[i].Substring(0, saveStates[i].Length - 4);
            gameObjectReference.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = saveStates[i];
            gameObjectReference.SetActive(true);
            string stateName = saveStates[i];
            gameObjectReference.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { loadObjects(stateName); });
        }
        loadMenu.SetActive(false);
}

    public void saveObjects()
    {
        if (gameData == null || gameData.saveName == "")
        {
            gameData = new GameData(0, DateTime.Now.ToString("dd-MMM-yyy-HH-mm-ss"), DateTime.Now.ToString("dd-MMM-yyy-HH-mm-ss"));
        } else
        {
            gameData.saveDate = DateTime.Now.ToString("dd-MMM-yyy-HH-mm-ss");
        }
        BinaryFormatter formatter = new BinaryFormatter();
        if (!Directory.Exists(Application.persistentDataPath + "\\gameSaves\\gameData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "\\gameSaves\\gameData");
        }
        string gameDataPath = Application.persistentDataPath + "\\gameSaves\\gameData\\" + gameData.saveName + ".dat";
        FileStream gameDatastream = new FileStream(gameDataPath, FileMode.Create);

        formatter.Serialize(gameDatastream, gameData);
        gameDatastream.Close();

        // Save every gameObject in the list of gameObjects that need to be saved
        // This only saves the rotation and position
        foreach (GameObject gameObject in unityGameObjects)
        {
            if (!Directory.Exists(Application.persistentDataPath + "\\gameSaves\\" + gameData.saveName))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "\\gameSaves\\" + gameData.saveName);
            }
            string path = Application.persistentDataPath + "\\gameSaves\\" + gameData.saveName + "\\" + gameObject.GetComponent<saveObject>().id + ".dat";
            FileStream stream = new FileStream(path, FileMode.Create);

            GameObjectData data = new GameObjectData(gameObject);

            formatter.Serialize(stream, data);
            stream.Close();
        }


    }

    public void loadObjects(string fileName)
    {
        string gameDataPath = Application.persistentDataPath + "\\gameSaves\\gameData\\" + fileName;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream gameDataStream = new FileStream(gameDataPath, FileMode.Open);
        GameData gameDataLoaded = formatter.Deserialize(gameDataStream) as GameData;
        gameData = gameDataLoaded;
            
        // If game data needs to be used on retrieving it can be done here.
        

        foreach (GameObject gameObject in unityGameObjects)
        {
            string path = Application.persistentDataPath + "\\gameSaves\\" + gameData.saveName + "\\" + gameObject.GetComponent<saveObject>().id + ".dat";
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);

                GameObjectData data = formatter.Deserialize(stream) as GameObjectData;
                gameObject.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]));

                stream.Close();
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
            }
        }
        loadMenu.SetActive(false);
        mainMenu.GetComponent<NewMenuManger>().ResumeGame();
    }
}
