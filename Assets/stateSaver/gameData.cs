using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public string saveDate;
    public string saveName;
    public GameData(int score, string saveDate, string saveName)
    {
        this.score = score;
        this.saveDate = saveDate;
        this.saveName = saveName;
    }
}