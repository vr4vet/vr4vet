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

        [Header("Subtask List")]
        [SerializeField] private List<Subtask> _subtasks;

        private bool _compleated = false;
        private string _feedback = "";
        [HideInInspector] public GameObject prerequisite;
        [HideInInspector] public GameObject target;

        public string Description { get => _description; set => _description = value; }
        public List<Subtask> Subtasks { get => _subtasks; set => _subtasks = value; }
        public string TaskName { get => _taskName; set => _taskName = value; }
        public bool Compleated { get => _compleated; set => _compleated = value; }
        public string Feedback { get => _feedback; set => _feedback = value; }

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