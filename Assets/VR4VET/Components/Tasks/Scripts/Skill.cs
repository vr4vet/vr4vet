/* Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System.Collections.Generic;
using UnityEngine;

namespace Task
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "Tasks/Skill")]
    public class Skill : ScriptableObject
    {
        private int _maxPoints = 100;
        private int achievedPoints;
        [SerializeField] private string _name;

        [Tooltip("Description of this skill"), TextArea(5, 20)]
        [SerializeField] private string _description;
        [ TextArea(5, 20)]
        [SerializeField] private string _feedback;


        [Header("Related Subtask")]
        [SerializeField] private List<Subtask> _subtasks = new List<Subtask>();
        //public Dictionary<Subtask, int> _pointsPerSubtask = new Dictionary<Subtask, int>();

        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public List<Subtask> Subtasks { get => _subtasks; set => _subtasks = value; }
        public int TotalPoints { get => _maxPoints; set => _maxPoints = value; }
        public int MaxPoints { get => achievedPoints; set => achievedPoints = value; }
        public string Feedback { get => _feedback; set => _feedback = value; }

        void Awake()
        {
            foreach (Subtask sub in _subtasks)
            {
             //   _pointsPerSubtask.Add(sub, 0);
                sub.RelatedSkills.Add(this);
            }
        }


        private int GetArchivedPoints()
        {
            achievedPoints = 0;
            foreach ( Subtask sub in _subtasks )
            {
                achievedPoints += sub.Points;
            }
            return achievedPoints;
        }
    }
}