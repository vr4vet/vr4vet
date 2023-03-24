using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Task
{

    [System.Serializable]
    public class Step 
    {
        [HideInInspector]
        public int archivedPoints;
 
        private int _maxPosiblePoints;
    
        [SerializeField]
        private string _StepName;
        [SerializeField]   [Range(1, 20)]
        private int _repetionNumber = 1;
        private bool _compleated;
        private List<Skill> _relatedSkills = new List<Skill>();
        private int _repetionsCompleated = 0;
        [SerializeField]
        private int _pointValue;


        public int _maxPsiblePoint { get; set; }

        public void CompleateStep()
        {
            if (_repetionsCompleated < _repetionNumber)
            {
                _repetionsCompleated++;
            }

        }

        public void CompleateAll()
        {
            _repetionsCompleated = _repetionNumber;
        }


        public List<Skill> getRelatedSkills()
        {
            return _relatedSkills;
        }


        public bool IsCompeleted()
        {
            return _compleated;
        }


     
        /// Mark this aktivitet as done
       
        public void SetCompleated(bool value)
        {
            _compleated = value;
        }



        public string GetName()
        {
            return _StepName;
        }


        /// Set the name of this aktivitet
  
        public void SetAktivitetName(string value)
        {
            _StepName = value;
        }



        public int GetAchievedPoeng()
        {
            return archivedPoints;
        }


     
        public void SetAchievedPoeng(int givenPoen)
        {
            archivedPoints = givenPoen;
        }



        public void AddToAchievedPoeng(int value)
        {
            foreach (Skill ferdighet in _relatedSkills)
            {
                foreach (Step aktivitet in ferdighet.GetFerdighetAktiviteter().Keys.ToList())
                {
                    if (aktivitet.GetName() == _StepName)
                    {
                        //if (achievedPoeng + value < ferdighet.GetAchievedPoeng())
                            archivedPoints += value;
                        return;
                        //else
                        //    achievedPoeng = ferdighet.GetAchievedPoeng();
                    }
                }
            }
        }

    }
}