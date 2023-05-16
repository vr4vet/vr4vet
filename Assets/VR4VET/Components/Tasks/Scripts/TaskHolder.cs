/* Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections.Generic;
using UnityEngine;

namespace Task
{
    public class TaskHolder : MonoBehaviour
    {
        public static TaskHolder Instance;
        private List<TaskxTarget> _taskAndTargerts = new List<TaskxTarget>();

        [Header("Profession Tasks")]
        [SerializeField] public List<Skill> skillList = new List<Skill>();

        [SerializeField] public List<Task> taskList = new List<Task>();

        //making the task holder a singleton
        private void Awake()
        {
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

        public void CompleateSubtask(string taskName, string SubtaskName, bool compleateStepReps)
        {
            foreach (Task task in taskList)
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
    }

    [System.Serializable]
    public class TaskxTarget
    {
        public Subtask subtask;
        public GameObject target;
    }
}