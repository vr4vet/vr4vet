/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System.Collections.Generic;
using System.Linq;
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
        private List<Subtask> _subtasks = new List<Subtask>();
        private Dictionary<Step, int> ferdighetAktiviteter = new Dictionary<Step, int>();

        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public List<Subtask> Subtasks { get => _subtasks; set => _subtasks = value; }
        public int TotalPoints { get => _totalPoints; set => _totalPoints = value; }

        public void AddSubtask(Subtask sub)
        {
             if (! _subtasks.Contains(sub))
            {
                _subtasks.Add(sub);
            }
        }


   
  
     


        /// Get the poeng that the player has gotten from this ferdighet
   
        public int GetAchievedPoeng()
        {
            achievedPoints = 0;
            foreach (KeyValuePair<Step, int> aktivitet in ferdighetAktiviteter)
            {
                achievedPoints += aktivitet.Value;
            }
            return achievedPoints;
        }


     
        /// set the poeng to this ferdighet
 
        public void SetAchievedPoeng(int value)
        {
            achievedPoints = value;
        }


        
        /// Change the description of this ferdighet. can be used for feedback
       
        public void SetFerdighetBeskrivelse(string value)
        {
            _description = value;
        }
        

      
        ///Get the ferdighet description 
        
        public string GetFerdighetBeskrivelse()
        {
            return _description;
        }


        /// Add more poneg to this ferdighet
    


        public void AddArchivedPoints(int value)
        {
            if (achievedPoints + value < _totalPoints)
                achievedPoints += value;
            else
                achievedPoints = _totalPoints;
        }

    }
}