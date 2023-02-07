using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipScript : MonoBehaviour
{
    public Transform Player;
    Transform Parent;
    LineRenderer Line;
    // Start is called before the first frame update
    void Start()
    {
        Parent = transform.parent;
        Line = gameObject.GetComponent<LineRenderer>();
        Line.widthMultiplier = 0.005f;
        Line.SetPosition(1, Parent.position);
        Line.SetPosition(0, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player);
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
