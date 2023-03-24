/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia, Abbas Jafari
 * Ask your questions by email: jorgeega@ntnu.no
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Task
{
    /// <summary>
    /// Task object
    /// </summary>
    [CreateAssetMenu(fileName ="New Task", menuName = "Tasks/BTask")]
    public class BTask : ScriptableObject
    {

        private bool _compleated;

        public string _taskName;

        [Tooltip("Description of this assignment"), TextArea(5, 20)]
        [SerializeField] private string _description;
        [SerializeField] private List<Subtask> _subTasks = new List<Subtask>();
        [SerializeField] private int _repetitions = 1;


        private int  _numberComplated;


        /// Mark this task as compeleted if its all activities are done
        public bool CheckTaskCompeletion()
        {
            bool val = true;
            foreach (Subtask sub in _subTasks)
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