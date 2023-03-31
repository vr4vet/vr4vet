/*
 * Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Task
{
   
    [CreateAssetMenu(fileName ="New Task", menuName = "Tasks/Task")]
    public class BTask : ScriptableObject
    {

        private bool _compleated;

        public string _taskName;

        [Tooltip("Description of this assignment"), TextArea(5, 20)]
        [SerializeField] private string _description;
        [SerializeField] private List<Subtask> _subtasks = new();
        [SerializeField] private int _repetitions = 1;
        [Tooltip("Select None if you don't need it ")]
        public GameObject target;

    
        private int  _numberComplated =0;
        [HideInInspector]public GameObject prerequisite;

        public string Description { get => _description; set => _description = value; }
        public List<Subtask> Subtasks { get => _subtasks; set => _subtasks = value; }

        /// Mark this task as compeleted if its all activities are done
        public bool CheckTaskCompeletion()
        {
            bool val = true;
            foreach (Subtask sub in _subtasks)
            {
                if (sub.IsCompeleted() == false)
                {
                    val = false;
                    break ;
                }
            }
            return val;
        }

        public bool IsTaskCompeleted()
        {
            return (_numberComplated>= _repetitions);
        }
 
  
    }
}