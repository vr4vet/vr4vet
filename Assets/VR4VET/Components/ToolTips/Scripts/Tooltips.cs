using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



/// <summary>
/// Tool tip behaviour class which handles geric tooltip behaviours such as animation and close conditions. 
/// Donot rename the object names in the tooltip prefab.
/// </summary>

public class Tooltips : MonoBehaviour, IPointerClickHandler
{
    GameObject Panel;
    Button CloseButton;
    // Start is called before the first frame update
    void Start()
    {
        // try to find the panel object in the scene and assign it to the Panel variable or else throw an error
        try
        {
            Panel = gameObject.transform.Find("Card").gameObject;
            CloseButton = Panel.transform.Find("Button").gameObject.GetComponent<Button>();
            CloseButton.onClick.AddListener(TaskOnClick);
        }
        catch (Exception e)
        {
            Debug.Log("Failed to find the panel object in the scene. Please check the name of the panel object in the scene and try again.");
            Debug.Log(e);
        }

    }

    // Update is called once per frame
    void Update()
    {
        ///For debug purposes
        //if (Input.GetKeyDown("space"))
        //{
           // print("space key was pressed");
            //OpenPanel();
        //}

    }

    /// <summary>
    /// When the panel is clicked, open the panel
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer clicked!");
        OpenPanel();
    }

    /// <summary>
    /// When the close button is clicked, deactivate the panel
    /// </summary>
    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        DeactivatePanel();
    }

    /// <summary>
    /// Open or close the panel with animation
    /// </summary>
    public void OpenPanel()
    {
        
        if (Panel != null)
        {
            //Panel.SetActive=true;
            Animator animator = Panel.GetComponent<Animator>();
            
            if (animator != null)
            {
                bool isOpen = animator.GetBool("open");
                animator.SetBool("open", !isOpen);
                Debug.Log("The animation is " + animator.GetBool("open"));
            } else
            {
                Debug.Log("Failed to find Animator component");
            }
        }
    }

    /// <summary>
    /// Deactivates the Panel
    /// </summary>
    public void DeactivatePanel()
    {
        Panel.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// activates the Panel
    /// </summary>
    public void ActivatePanel()
    {
        Panel.transform.parent.gameObject.SetActive(true);
    }

}
