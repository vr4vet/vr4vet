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

        [SerializeField] private string _name;
        [Tooltip("Description of this SubTask"), TextArea(5, 20)]
        [SerializeField] private string _description;



        [Header("Repetions")]
        [Tooltip("Select Same value for no randomization")]
        [SerializeField] private int _repetitionMin = 1;
        [SerializeField] private int _repetitionMax = 1;

        private int _repetitions = 1;


        [SerializeField] private List<Step> _stepList = new List<Step>();
        [SerializeField] private List<Skill> _relatedSkills = new List<Skill>();

        private int _maxPoints = 0;
        private int _points=0;
        private Transform _taskPosition;
        private int _repetionsCompleated = 0;

      



        [Header("Navigation")]
        [HideInInspector]
        public Subtask prerequisite;

        public string Description { get => _description; set => _description = value; }
        public int MaxPoints { get => _maxPoints; set => _maxPoints = value; }
        public List<Step> StepList { get => _stepList; set => _stepList = value; }
        public string Name { get => _name; set => _name = value; }

        public void AddPoints(int value)
        {
            _points += value;
        }

        public void addSubToSkill()
        {
            foreach (Skill skill in _relatedSkills)
            {
                skill.AddSubtask(this);
            }
        }

        public void UpdateMaxPoints()
        {
            _maxPoints = 0;
            foreach (Step step in _stepList)
            {
                _maxPoints += step.MaxPoints();
            }
        }

        public int Points()
        {
            int val = 0;
            foreach (Step step in _stepList)
            {
                val += step.Points();
            }
            return val;
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
                Debug.Log("all " + _name + " already compleated");
            }
        }


    }
}
