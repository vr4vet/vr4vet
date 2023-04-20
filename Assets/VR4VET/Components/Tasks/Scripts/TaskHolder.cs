/*
 * Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Task
{
    public class TaskHolder : MonoBehaviour
    {
        public static TaskHolder Instance;
        private List<TaskxTarget> _taskAndTargerts = new List<TaskxTarget>();

        [SerializeField]
        private List<Skill> _skillList = new List<Skill>();

        [SerializeField]
        private List<BTask> _tasks = new List<BTask>();
        private List<Task> _taskRetro = new List<Task>();



        //making the task holder a singleton
        void Awake()
        {
            SetValues();
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public void SetValues()
        {
            foreach (BTask task in _tasks)
            {
                foreach (Subtask sub in task.Subtasks)
                {
                    sub.addSubToSkill();
                    sub.UpdateMaxPoints();
                }
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
