using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public TooltipManager TTM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown("space"))
        {
            TTM.InstantiateTooltip(transform, "TestHeader", "TestContent", "Minimized");
        }
    }
}
