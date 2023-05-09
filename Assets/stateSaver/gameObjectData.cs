using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class GameObjectData
{
    public float[] position;
    public float[] rotation;
    public GameObjectData(GameObject gameObject)
    {
        position = new float[3];
        rotation = new float[3];
        position[0] = gameObject.transform.position.x;
        position[1] = gameObject.transform.position.y;
        position[2] = gameObject.transform.position.z;
        rotation[0] = gameObject.transform.eulerAngles.x;
        rotation[1] = gameObject.transform.eulerAngles.y;
        rotation[2] = gameObject.transform.eulerAngles.z;
    }
}