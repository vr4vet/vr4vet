using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipScript : MonoBehaviour, IPointerClickHandler
{
    public Transform Player;
    public bool FacePlayer = true;
    public bool AlwaysAboveParent = true;
    public bool StartOpen = true;
    public bool StartMinimized = true;
    public bool CloseWhenDistant = false;
    public float CloseThreshold = 10;
    public string Header;
    [TextArea(10,15)]
    public string TextContent;
    Transform Parent;
    LineRenderer Line;
    Transform Panel;
    Button CloseButton;
    Animator animator;
    bool isOpen;

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

        Panel = transform.Find("Card");
            CloseButton = Panel.Find("Button").GetComponent<Button>();
            CloseButton.onClick.AddListener(Deactivate);
            animator = Panel.GetComponent<Animator>();
            animator.SetBool("open", !StartMinimized);
            if (!StartOpen)
            {
                Deactivate();
            }

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
        }
        Line.SetPosition(1, Parent.position);
        Line.SetPosition(0, transform.position);
        Panel.Find("header").Find("Text").GetComponent<Text>().text = Header;
        Panel.Find("TextField").Find("Text").GetComponent<Text>().text = TextContent;
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Panel != null || animator != null)
        {
        isOpen = animator.GetBool("open");
        animator.SetBool("open", !isOpen);
        Debug.Log("The animation is " + animator.GetBool("open"));
        }
    }
}
