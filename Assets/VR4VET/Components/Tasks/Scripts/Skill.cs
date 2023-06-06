/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tablet
{
    /// <summary>
    /// ferdighet object
    /// </summary>
    [CreateAssetMenu(fileName = "New Skill", menuName = "Tasks/Skill")]
    public class Skill : ScriptableObject
    {
        public int totalPoints;
        private int achievedPoints;
        public string skillName;
        [Tooltip("Description of this skill"), TextArea(5, 20)]
        public string skillDescription;

        //hvor mye til hver aktivitet har gitt denne ferdighet (aktivitet,poeng)
        private Dictionary<Activity, int> ferdighetAktiviteter = new Dictionary<Activity, int>();


        /// <summary>
        /// get the dictionary of all aktivitet that are testet by this ferdighet
        /// </summary>
        /// <returns></returns>
        public Dictionary<Activity, int> GetFerdighetAktiviteter()
        {
            return ferdighetAktiviteter;
        }


        /// <summary>
        /// Add an aktivitet to this ferdighet
        /// </summary>
        /// <param name="aktivitet"></param>
        /// <param name="value"></param>
        public void AddToFerdighetAktiviteter(Activity aktivitet, int value)
        {
            if (!ferdighetAktiviteter.Keys.Contains(aktivitet))
            {
                if (achievedPoints + value < totalPoints)
                {
                    ferdighetAktiviteter.Add(aktivitet, achievedPoints + value);
                }
                else
                {
                    ferdighetAktiviteter.Add(aktivitet, value);
                }
            }
        }


        /// <summary>
        /// Get the name of this ferdighet
        /// </summary>
        /// <returns></returns>
        public string GetFerdighetName()
        {
            return skillName;
        }


        /// <summary>
        /// Set the name of this ferdighet
        /// </summary>
        /// <param name="value"></param>
        public void SetFerdighetName(string value)
        {
            skillName = value;
        }

        /// <summary>
        /// Get the total poeng that this ferdighet has gotten by all activities
        /// </summary>
        /// <returns></returns>
        public int GetTotalPoeng()
        {
            return totalPoints;
        }


        /// <summary>
        /// set the total poeng that this ferdighet can achieved to
        /// </summary>
        /// <param name="value"></param>
        public void SetTotalPoeng(int value)
        {
            totalPoints = value;
        }


        /// <summary>
        /// Get the poeng that the player has gotten from this ferdighet
        /// </summary>
        /// <returns></returns>
        public int GetAchievedPoeng()
        {
            achievedPoints = 0;
            foreach (KeyValuePair<Activity, int> aktivitet in ferdighetAktiviteter)
            {
                achievedPoints += aktivitet.Value;
            }
            return achievedPoints;
        }


        /// <summary>
        /// set the poeng to this ferdighet
        /// </summary>
        /// <param name="value"></param>
        public void SetAchievedPoeng(int value)
        {
            achievedPoints = value;
        }


        /// <summary>
        /// Change the description of this ferdighet. can be used for feedback
        /// </summary>
        /// <param name="value"></param>
        public void SetFerdighetBeskrivelse(string value)
        {
            skillDescription = value;
        }
        

        /// <summary>
        ///Get the ferdighet description 
        /// </summary>
        /// <returns></returns>
        public string GetFerdighetBeskrivelse()
        {
            return skillDescription;
        }


        /// <summary>
        /// Add more poneg to this ferdighet
        /// </summary>
        /// <param name="value"></param>
        public void AddAchievedPoeng(int value)
        {
            if (achievedPoints + value < totalPoints)
                achievedPoints += value;
            else
                achievedPoints = totalPoints;
        }

    }
}