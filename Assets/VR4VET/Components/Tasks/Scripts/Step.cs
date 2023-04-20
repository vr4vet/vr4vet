/*
 * Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */


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

        [SerializeField]   private string _StepName;
        [SerializeField]   [Range(1, 20)] private int _repetionNumber = 1;    
        [SerializeField] private int _valuePerRep;

        private bool _compleated;
        private List<Skill> _relatedSkills = new List<Skill>();
        private int _repetionsCompleated = 0;

        public int ValuePerRep { get => _valuePerRep; set => _valuePerRep = value; }
        public int RepetionNumber { get => _repetionNumber; set => _repetionNumber = value; }
        public int RepetionsCompleated { get => _repetionsCompleated; set => _repetionsCompleated = value; }

        public void CompleateRep()
        {
            if (_repetionsCompleated < _repetionNumber)
            {
                _repetionsCompleated++;
            }

        }

        public int MaxPoints()
        {
            return _repetionNumber * _valuePerRep;
        }

        public int Points()
        {
            return _repetionsCompleated * _valuePerRep;
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


        /*
        public void AddArchivedPoints(int value)
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


        */

    }
}