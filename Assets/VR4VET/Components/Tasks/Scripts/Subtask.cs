/* Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections.Generic;
using UnityEngine;

namespace Task
{
    [CreateAssetMenu(fileName = "New Subtask", menuName = "Tasks/Subtask")]
    public class Subtask : ScriptableObject
    {
        [Header("General information")]
        [SerializeField] private string _subtaskName;

        [Tooltip("Description of this SubTask"), TextArea(5, 20)]
        [SerializeField] private string _description;

        //Randomizer needs to better define

        private int _repetitionMin = 1;
        private int _repetitionMax = 1;
        private int _repetitions = 1;

        [Header("Steps")]
        [SerializeField]
        [Tooltip("Mark Subtask as compleate if all step's repetions are compleated")]
        private bool _autocompleate = false;

        [SerializeField] private List<Step> _stepList = new List<Step>();

        private List<Skill> _relatedSkills = new List<Skill>();
        private int _points = 0;
        private bool _compleated = false;

        //for navigation system
        private Transform _taskPosition;

        public string Description { get => _description; set => _description = value; }
        public List<Step> StepList { get => _stepList; set => _stepList = value; }
        public string SubtaskName { get => _subtaskName; set => _subtaskName = value; }
        public int Points { get => _points; set => _points = value; }
        public List<Skill> RelatedSkills { get => _relatedSkills; set => _relatedSkills = value; }

        //it returns the completed status, if the autocomplete box is selected it will return true when all its steps are completed
        public bool Compleated()
        {
            if (_autocompleate)
            {
                _compleated = true;
                foreach (Step step in StepList)
                {
                    if (! step.IsCompeleted())
                    {
                        _compleated = false;
                        break;
                    }
                }
            }
            return _compleated;
        }

        public void SetCompleated(bool isCompleated)
        {
            _compleated = isCompleated;
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.updateCheckMarks();
        }

        public float GeneralPercent()
        {
            float percent = 0;
            foreach (Step step in StepList)
            {
                percent += step.CompleatedPercent();
            }
            return percent / StepList.Count;
        }

        public void AddPoints(int value)
        {
            _points += value;
            Tablet.TaskListLoader1.Ins.UpdateSkillPoints();
        }

        public Step GetStep(string stepname)
        {
            Step returnStep = null;
            foreach (Step step in _stepList)
            {
                if (step.StepName == stepname)
                {
                    return step;
                }
            }
            return returnStep;
        }

        public void RandomizeReps()
        {
            if (_repetitionMin != _repetitionMax)
            {
                _repetitions = (Random.Range(_repetitionMin, _repetitionMax));
            }
            else _repetitions = _repetitionMax;
        }
    }
}