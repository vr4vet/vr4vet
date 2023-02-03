/* Copyright (C) 2023 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Tablet
{
    public class TaskHolder : MonoBehaviour
    {
        [SerializeField]
        public  List<TaskxTarget> _taskAndTargerts = new List<TaskxTarget>()   ;


        void Start()
        {
            
            foreach (TaskxTarget convo in _taskAndTargerts)
            {

                convo.task.taskTarget = convo.target;
            }

        }

        public List<Task> GetTasks()
            {
            List<Task> returntasks = new List<Task>();
            foreach (TaskxTarget convo in _taskAndTargerts)
            {
                
                returntasks.Add(convo.task);
            }

            return returntasks;
        }

    }

    [System.Serializable]
    public class TaskxTarget
    {
        public Tablet.Task task;
        public GameObject target;



    }

}
