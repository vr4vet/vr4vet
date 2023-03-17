/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollow : MonoBehaviour
{


    // When savig the entire transform some data gets lost. So this Struct saves what we need from the camara transform
    private struct PastPosition
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Foward; 

    }


    [SerializeField] private GameObject player;
    [SerializeField] private int _delay=30;
    [SerializeField] private float _distanceToCamera;
    private Camera _cam;
    private float _height;
    private Quaternion delayCamRotation;
    private Queue<PastPosition> lapins = new Queue<PastPosition>();

 


    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        _height = this.GetComponent<RectTransform>().rect.height * this.GetComponent<RectTransform>().localScale.y / 2;
        _cam = Camera.main;

        transform.position = player.transform.position;//set origin of parent object 

        //set the menu to be on front of the player and looking toward them
        transform.position = _cam.transform.position + _cam.transform.forward * _distanceToCamera;
        transform.LookAt(transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);

        //change the Y position to fix one (height) , and the X,Z rotation to 0 )
        transform.position = new Vector3(transform.position.x, _height, transform.position.z);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


    }

    // Update is called once per frame
    void Update()
    {


    
        lapins.Enqueue(new PastPosition() { Position = _cam.transform.position, Rotation = _cam.transform.rotation, Foward = _cam.transform.forward });

        if (lapins.Count > _delay) //temporal solution  
        {

            PastPosition chad = lapins.Dequeue();

            //set the menu to be on front of the player and looking toward them
            transform.position = chad.Position + chad.Foward * _distanceToCamera;
            transform.LookAt(transform.position + chad.Rotation * Vector3.forward, chad.Rotation * Vector3.up);

            //change the Y position to fix one (height) , and the X,Z rotation to 0 )
            transform.position = new Vector3(transform.position.x, _height, transform.position.z);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        }

       
    }



    IEnumerator UpdatePositionQuee() 
    {
        while (true)
        {
            lapins.Enqueue(new PastPosition() { Position = _cam.transform.position, Rotation = _cam.transform.rotation, Foward = _cam.transform.forward });
            yield return new WaitForEndOfFrame();
        }
    }







}
