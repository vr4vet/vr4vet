/* Copyright (C) 2023 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Task
{
    public class TaskHolder : MonoBehaviour
    {
       
        private List<TaskxTarget> _taskAndTargerts = new List<TaskxTarget>();

        [SerializeField]
        private List<Skill> _skillList = new List<Skill>();

        [SerializeField]
        private List<BTask> _tasks = new List<BTask>();
        private List<Task> _taskRetro = new List<Task>();


        public List<Skill> getSkillList()
        {
            List<Skill> returnskills = new List<Skill>();
            foreach (Skill skill in _skillList)
            {

                returnskills.Add(skill);
            }

            return returnskills;
        }


        public List<BTask> GetTaskList()
        {
           

            return _tasks;
        }

        public List<Task> GetRetroTaskList()
        {


            return _taskRetro;
        }



        public void AddPoints(string stepName, string skillName, int points)
        {


            
        }


     

    }






    [System.Serializable]
    public class TaskxTarget
    {
        public Subtask subtask;
        public GameObject target;

    }

}
