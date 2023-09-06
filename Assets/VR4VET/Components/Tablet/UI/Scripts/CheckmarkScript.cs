using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckmarkScript : MonoBehaviour
{
    public Image Checkmark;


    void Start()
    {
         Checkmark.enabled = false;
    }


    public void OnClick()
    {
        Checkmark.enabled = !Checkmark.enabled;
    }
}
