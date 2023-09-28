using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MultiUserMenu : MonoBehaviour
{
    public GameObject SingleModeCanvas;
    public GameObject MultiUserCanvas;
    public GameObject MultiUser_JoinCanvas;
    public GameObject MultiUser_CreateCanvas;
    public GameObject MultiModeCanvas;

    // Start is called before the first frame update
    void Start()
    {
        SingleModeCanvas.SetActive(false);
        MultiUserCanvas.SetActive(false);
        MultiUser_JoinCanvas.SetActive(false);
        MultiUser_CreateCanvas.SetActive(false);
        MultiModeCanvas.SetActive(false);
    }

    public void LoadSceneByIndex(int sceneIndexToLoad)
    {
        // Load the scene using the specified index.
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}
