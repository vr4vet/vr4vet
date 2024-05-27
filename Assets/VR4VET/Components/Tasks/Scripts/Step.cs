/* Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System;
using UnityEngine;
using TMPro;

namespace Task
{
    [System.Serializable]

    [CreateAssetMenu(fileName = "New Step", menuName = "Tasks/Step")]
    public class Step : ScriptableObject
    {
        [SerializeField] private string _stepName;
        [SerializeField] [Range(1, 20)] private int _repetionNumber = 1;
        
        [Tooltip(">0 : count down, <0 : no timer, 0 : count up")]
        [SerializeField] private int _timer = -1;

        private bool _compleated = false;
        private int _repetionsCompleated = 0;
        private TimeSpan _counter;

        public int RepetionNumber { get => _repetionNumber; set => _repetionNumber = value; }
        public int RepetionsCompleated { get => _repetionsCompleated; set => _repetionsCompleated = value; }
        public string StepName { get => _stepName; set => _stepName = value; }
        public int Timer { get => _timer; set => _timer = value; }
        public TimeSpan Counter { get => _counter; set => _counter = value; }

        public void CompleateRep()
        {
            if (_repetionsCompleated < _repetionNumber)
            {
                _repetionsCompleated++;
            }
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.updateCheckMarks();
        }

        public float CompleatedPercent()
        {
            float porcent = _repetionsCompleated * 100 / _repetionNumber;
            return porcent;
        }

        public void CompleateAllReps()
        {
            _repetionsCompleated = _repetionNumber;
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.updateCheckMarks();
        }

        public bool IsCompeleted()
        {
            if (_repetionNumber == _repetionsCompleated) _compleated = true;
            return _compleated;
        }

        /// Mark this step as done
        public void SetCompleated(bool value)
        {
            _compleated = value;
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.updateCheckMarks();
        }

        //overload to compleate reps
        public void SetCompleated(bool value, bool compleateReps)
        {
            _compleated = value;
            if (compleateReps)
            {
                RepetionsCompleated = RepetionNumber;
            }
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.updateCheckMarks();
        }
        /// Set the name of this aktivitet (Legacy)

        public void SetAktivitetName(string value)
        {
            _stepName = value;
        }

        
        public void StartTimer(){
            Tablet.TaskListLoader1 taskLoader = GameObject.FindObjectsOfType<Tablet.TaskListLoader1>()[0];
            taskLoader.startTimer(Timer, this);
        }
        
    }
}