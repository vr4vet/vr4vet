using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public GameObject thisGameObject;
    public GameObject collidingObject;
    private DialogueBoxController dialogueBoxController;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(collidingObject)) 
        {
            Debug.Log(dialogueTree.sections[0].dialogue[0]);
            Destroy(thisGameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
