/*
 * Developer: Jorge Garcia
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



        [Header("Repetions")]
        [Tooltip("Select Same value for no randomization")]
        [SerializeField] private int _repetitionMin = 1;
        [SerializeField] private int _repetitionMax = 1;
        [Tooltip("0 means no timer")]
        [SerializeField] private float _timerSecods = 0.0f;



        private int _repetitions = 1;


        [SerializeField] private List<Step> _stepList = new List<Step>();
        [SerializeField] private List<Skill> _relatedSkills = new List<Skill>();

        private int _stepsreps = 0;
        private int _points=0;
        private Transform _taskPosition;
        private int _repetionsCompleated = 0;


        [Header("Navigation")]
        [HideInInspector]
        public Subtask prerequisite;

        public string Description { get => _description; set => _description = value; }
        public int StepsReps { get => _stepsreps; set => _stepsreps = value; }
        public List<Step> StepList { get => _stepList; set => _stepList = value; }
        public string SubtaskName { get => _subtaskName; set => _subtaskName = value; }
        public int Points { get => _points; set => _points = value; }

        public void AddPoints(int value)
        {
            _points += value;
        }


        public Step GetStep(string stepname)
        {
            Step returnStep = null;
            foreach ( Step step in _stepList)
            {
                if (step.StepName== stepname)
                {
                    return step;
                }
            }
            return returnStep;
        }

        public void addSubToSkill()
        {
            foreach (Skill skill in _relatedSkills)
            {
                skill.AddSubtask(this);
            }
        }

        //update the value of max ammount of points from the steps
        public void UpdateStepsReps()
        {
            _stepsreps = 0;
            foreach (Step step in _stepList)
            {
                _stepsreps += step.RepetionNumber;
            }
        }


        public void RandomizeReps()
        {
            if (_repetitionMin != _repetitionMax)
            {
                _repetitions = (Random.Range(_repetitionMin, _repetitionMax));
            }
            else _repetitions = _repetitionMax;
                
        }


        public bool IsCompeleted()
        {
            return (_repetionsCompleated >= _repetitions);

        }

        public void CompleateSubTask()
        {
            if (_repetionsCompleated < _repetitions)
            {
                _repetionsCompleated++;
            }
            else
            {
                Debug.Log("all " + _subtaskName + " already compleated");
            }
        }


    }
}
