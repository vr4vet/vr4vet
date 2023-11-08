using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This keeps the instance alive across different scenes.
        }
        else if (instance != this)
        {
            Destroy(gameObject); // This destroys any duplicate instances that might arise.
        }
    }

    #endregion

    public GameObject player;
}
