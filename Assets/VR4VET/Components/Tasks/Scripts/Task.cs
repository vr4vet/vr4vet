/* Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections.Generic;
using UnityEngine;

namespace Task
{
    [CreateAssetMenu(fileName = "New Task", menuName = "Tasks/Task")]
    public class Task : ScriptableObject
    {
        [Header("General information")]
        [SerializeField] private string _taskName;

        [Tooltip("Description of this assignment"), TextArea(5, 20)]
        [SerializeField] private string _description;

        [Tooltip("Mark Subtask as compleate if all its Subtasks are compleated")]
        [SerializeField] private bool _autocompleate = false;

        [Header("Subtask List")]
        [SerializeField] private List<Subtask> _subtasks;

        private bool _compleated = false;
        private string _feedback = "";
        //both should be deleted once the navigation script gets updated or deleted
        [HideInInspector] public GameObject prerequisite;
        [HideInInspector] public GameObject target;

        public string Description { get => _description; set => _description = value; }
        public List<Subtask> Subtasks { get => _subtasks; set => _subtasks = value; }
        public string TaskName { get => _taskName; set => _taskName = value; }
        public string Feedback { get => _feedback; set => _feedback = value; }

        public void Compleated(bool value)
        {
            _compleated = value;
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.updateCheckMarks();
        }

        public bool Compleated()
        {
            if (_autocompleate)
            {
                _compleated = true;
                foreach (Subtask sub in Subtasks)
                {
                    if (!sub.Compleated())
                    {
                        _compleated = false;
                        break;
                    }
                }
            }
            return _compleated;
        }

        public bool CheckSubtaksCompleated()
        {
            bool val = true;
            foreach (Subtask sub in _subtasks)
            {
                if (sub.Compleated() == false)
                {
                    val = false;
                    break;
                }
            }
            return val;
        }

        [Tooltip("look for a subtask given its name (NOT the file name)")]
        public Subtask GetSubtask(string name)
        {
            Subtask returnSub = null;

            foreach (Subtask sub in _subtasks)
            {
                if (sub.SubtaskName == name)
                {
                    returnSub = sub;
                    break;
                }
            }

            return returnSub;
        }
    }
}