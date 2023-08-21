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
    [CreateAssetMenu(fileName ="Old Task", menuName = "Tasks/OldTask")]
    public class TaskDeprecated : ScriptableObject
    {

        private Transform _taskPosition;
        private bool _compleated;

        [Tooltip("Task Name")]
        public string taskName;

        [Tooltip("Description of this assignment"), TextArea(5, 20)]
        public string description;
        [Tooltip("Activities List")]
        [SerializeField]
        public List<Step> activities = new List<Step>();

        [Header("Navigation")]
        [HideInInspector]
        public GameObject taskTarget;
        public TaskDeprecated prerequisite;

        [HideInInspector]
        public Button button;

        [HideInInspector]
        public Image checkedIcon;
        [HideInInspector]
        public Image unCheckedIcon;

     


        [HideInInspector] private bool is_task_Completed;



        private void Update()
        {
            CheckTaskCompeletion();
            if (is_task_Completed)
            {
                if (button)
                    button.interactable = false;

                if (checkedIcon && checkedIcon)
                {
                    checkedIcon.enabled = true;
                    unCheckedIcon.enabled = false;
                }
            }

        }




        /// Mark this task as compeleted if its all activities are done
        public void CheckTaskCompeletion()
        {
            //the task is not completed if even one aktivitet is not done
            foreach (Step step in activities)
            {
                if (step.IsCompeleted() == false)
                {
                    is_task_Completed = false;
                    return;
                }
            }
            is_task_Completed = true;
        }

        public bool IsTaskCompeleted()
        {
            return is_task_Completed;
        }


     
        public List<Step> GetAktivitetList()
        {

            return activities;
        }
    }
}