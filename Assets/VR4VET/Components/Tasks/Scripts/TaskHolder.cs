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
        public List<TaskxTarget> _taskAndTargerts = new List<TaskxTarget>();

        [SerializeField]
        private List<Skill> _skillList = new List<Skill>();

        private List<Task> _tasks = new List<Task>();

        void Start()
        {
            //asing the target value to each task ( the targets exist only on the scene 
            //when no target is elected the value is just None
            foreach (TaskxTarget convo in _taskAndTargerts)
            {

                convo.task.taskTarget = convo.target;
                _tasks.Add(convo.task);
            }



        }

        public List<Skill> getSkillList()
        {
            List<Skill> returnskills = new List<Skill>();
            foreach (Skill skill in _skillList)
            {

                returnskills.Add(skill);
            }

            return returnskills;
        }


        public List<Task> GetTaskList()
        {
            List<Task> returntasks = new List<Task>();
            foreach (TaskxTarget convo in _taskAndTargerts)
            {

                returntasks.Add(convo.task);
            }

            return returntasks;
        }



        public void AddPoints(string activityName, string skillName, int points)
        {


            foreach (Skill ferdighet in _skillList)
            {
                foreach (Task task in _tasks)
                {
                    foreach (Activity aktivitet in task.GetAktivitetList())
                    {
                        //add poeng to aktivitet
                        if (aktivitet.GetAktivitetName() == activityName && ferdighet.GetFerdighetName() == skillName)
                        {
                            //add ferdighet to this aktivitet
                            aktivitet.AddToAktivitetFerdigheter(ferdighet);

                            //add poeng to aktivitet object
                            aktivitet.AddToAchievedPoeng(points);

                            //mark this aktivitet as compeleted
                            aktivitet.AktivitetIsDone(true);

                            //add the aktivitet to skills dic som holds aktiviteter that need this ferdighet and its poeng
                            ferdighet.AddToFerdighetAktiviteter(aktivitet, points);
                        }
                    }
                }
            }

        }


     

    }






    [System.Serializable]
    public class TaskxTarget
    {
        public Task task;
        public GameObject target;

    }

}
