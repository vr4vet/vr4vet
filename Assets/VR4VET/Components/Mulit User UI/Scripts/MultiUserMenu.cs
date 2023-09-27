using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
