using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRectPanningLimiter : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private float minXLimit;
    [SerializeField] private float maxXLimit;
    [SerializeField] private float minYLimit;
    [SerializeField] private float maxYLimit;


    private void LateUpdate()
    {
        Vector2 anchoredPosition = content.anchoredPosition;
        float clampedX = Mathf.Clamp(anchoredPosition.x, minXLimit, maxXLimit);
        float clampedY = Mathf.Clamp(anchoredPosition.y, minYLimit, maxYLimit);
        content.anchoredPosition = new Vector2(clampedX, clampedY);
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
