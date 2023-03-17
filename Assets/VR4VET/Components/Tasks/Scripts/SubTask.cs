using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Task
{

    [CreateAssetMenu(fileName = "New Subtask", menuName = "Tasks/SubTask")]

    public class Subtask : ScriptableObject
    {

        [SerializeField]
        private string _subtaskName;

        [SerializeField]
        private int _pointValue;


        [Header("Repetions")]
        [Tooltip("Select Same value for no randomization")]
        [SerializeField] private int _repetitionMin = 1;
        [SerializeField] private int _repetitionMax = 1;
    
        private int _repetitions=1;


        [SerializeField]
        private List<Step> _stepList = new List<Step>();



        private int _points;
        private Transform _taskPosition;
        private int _repetionsCompleated = 0;


        [Tooltip("Description of this assignment"), TextArea(5, 20)]
        private string description;
        [Tooltip("Activities List")]



        [Header("Navigation")]
        [HideInInspector]
        public GameObject taskTarget;


        [HideInInspector] private bool is_task_Completed;



        public void AddPoints(int value)
        {
            _points += value;
        }


        public void Start()
        {
            if (_repetitionMin != _repetitionMax)
            {
                RandomizeReps();
            }

        }

        public void RandomizeReps()
        {
            _repetitions = (Random.Range(_repetitionMin, _repetitionMax));
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
