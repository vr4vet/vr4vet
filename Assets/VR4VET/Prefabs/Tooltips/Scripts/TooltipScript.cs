using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipScript : MonoBehaviour
{
    public Transform Player;
    public bool FacePlayer = true;
    public bool AlwaysAboveParent = true;
    public bool CloseWhenDistant = false;
    public float CloseThreshold = 10;
    Transform Parent;
    LineRenderer Line;

    // Start is called before the first frame update
    void Start()
    {
        Parent = transform.parent;
        if(AlwaysAboveParent)
        {
            transform.position = Parent.position + new Vector3(0, 0.5f, 0);
        }
        Line = gameObject.GetComponent<LineRenderer>();
        Line.widthMultiplier = 0.005f;
        Line.SetPosition(1, Parent.position);
        Line.SetPosition(0, transform.position);
        if(!Player)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            if (!Player)
            {
                Debug.LogError("Assign the player under the tooltip or with a Player tag");
            }
        }
        Player = Player.Find("HeadCollision");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, Player.position) >= CloseThreshold && CloseWhenDistant)
        {
            Deactivate();
        }
        if(AlwaysAboveParent)
        {
            transform.position = Parent.position + new Vector3(0, 0.5f, 0);
        }
        if(FacePlayer)
        {
            transform.LookAt(Player);
            Debug.Log("Looking At Player");
        }
        Line.SetPosition(1, Parent.position);
        Line.SetPosition(0, transform.position);
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
