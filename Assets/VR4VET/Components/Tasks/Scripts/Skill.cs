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
        private int _totalPoints;
        private int achievedPoints;
        [SerializeField] private string _name;

        [Tooltip("Description of this skill"), TextArea(5, 20)]
        [SerializeField] private string _description;

        [SerializeField] private List<Subtask> _subtasks = new List<Subtask>();
        public Dictionary<Subtask, int> _pointsPerSubtask = new Dictionary<Subtask, int>();

        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public List<Subtask> Subtasks { get => _subtasks; set => _subtasks = value; }
        public int TotalPoints { get => _totalPoints; set => _totalPoints = value; }
        public int AchievedPoints { get => achievedPoints; set => achievedPoints = value; }

        public void Awake()
        {
            foreach (Subtask sub in _subtasks)
            {
                _pointsPerSubtask.Add(sub, 0);
            }
        }

        private int GetArchivedPoints()
        {
            achievedPoints = 0;
            foreach (KeyValuePair<Subtask, int> dic in _pointsPerSubtask)
            {
                achievedPoints += dic.Value;
            }
            return achievedPoints;
        }
    }
}