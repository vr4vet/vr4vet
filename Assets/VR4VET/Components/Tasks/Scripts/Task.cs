/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia, Abbas Jafari
 * Ask your questions by email: jorgeega@ntnu.no
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tablet
{
    /// <summary>
    /// Task object
    /// </summary>
    [CreateAssetMenu(fileName ="New Task", menuName = "Tasks/Task")]
    public class Task : ScriptableObject
    {

        private Transform taskPosition;


        [Tooltip("Task Name")]
        public string _taskName;

        [Tooltip("Description of this assignment"), TextArea(5, 20)]
        public string description;
        [Tooltip("Activities List")]
        [SerializeField]
        public List<Activity> activities = new List<Activity>();

        [Header("Navigation")]
        [HideInInspector]
        public GameObject taskTarget;
        public Task prerequisite;

        [HideInInspector]
        public Button button;

        [HideInInspector]
        public Image checkedIcon;
        [HideInInspector]
        public Image unCheckedIcon;


        [HideInInspector] private bool is_task_Completed;


        /// <summary>
        /// Unity Update method
        /// </summary>
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



        /// <summary>
        /// This method calls in editor
        /// </summary>
        void OnValidate()
        {
         
            /**
            //set the size of activities and poeng as same as totalAktiviter amount
            Array.Resize(ref activities, totalActivities);


            //not allow to be more that 15 char
            if (_taskName.Length > 15)
                _taskName = _taskName.Substring(0,14);



            //not allow to be more that 30 char
            for (int i = 0; i < activities.Length; i++)
            {
                if (activities[i].Length > 30)
                {
                    activities[i] = activities[i].Substring(0, 29);
                }
            }
        
        **/
        }


        /// <summary>
        /// Mark this task as compeleted if its all activities are done
        /// </summary>
        public void CheckTaskCompeletion()
        {
            //the task is not completed if even one aktivitet is not done
            foreach (Activity aktivitet in activities)
            {
                if (aktivitet.IsAktivitetCompeleted() == false)
                {
                    is_task_Completed = false;
                    return;
                }
            }
            is_task_Completed = true;
        }

        /// <summary>
        /// Ask about this task is compeletet or not
        /// </summary>
        /// <returns></returns>
        public bool IsTaskCompeleted()
        {
            return is_task_Completed;
        }


        /// <summary>
        /// Unity Start method
        /// </summary>
        private void Start()
        {
            
        }


        /// <summary>
        /// Get the list of all aktivitet that this oppgave has
        /// </summary>
        /// <returns></returns>
        public List<Activity> GetAktivitetList()
        {

            return activities;
        }
    }
}