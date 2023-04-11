using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CanvasManager : MonoBehaviour
{
    public void CloseCanvas(GameObject Canvas){
        if(Canvas.activeInHierarchy){
            Canvas.SetActive(false);
        }
    }
}
