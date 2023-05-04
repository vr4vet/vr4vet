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
            SetMaxValues();
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


        public void SetMaxValues()
        {
            foreach (BTask task in _tasks)
            {
                foreach (Subtask sub in task.Subtasks)
                {
                    sub.addSubToSkill();
                    sub.UpdateStepsReps();
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

        // to think the points should just be a multiplier of the points. 






        public void CompleateSubtask(string taskName, string SubtaskName, bool compleateStepReps)
        {
            foreach (BTask task in _tasks)
            {
                if (task.TaskName == taskName)
                {
                    foreach (Subtask subtask in task.Subtasks)
                    {
                        if (subtask.SubtaskName == SubtaskName)
                        {
                            foreach (Step step in subtask.StepList)
                            {
                               step.SetCompleated(true);                                                           
                            }
                            Debug.LogError("Step not found in Subtask");
                        }
                    }
                    Debug.LogError("Subtask not found in Tasks");
                }
            }
        }


        public void AddPointsToSkill(string stepName, string skillName, int points)
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
