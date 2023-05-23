/* Copyright (C) 2023 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */



using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tablet
{
    /// <summary>
    /// Activity object
    /// </summary>
    [System.Serializable]
    public class Activity
    {
        public int achievedPoeng;
        public int maxPosiblePoints;
        public string aktivitetName;
        public bool AktivitetIsCompeleted;
        public List<Skill> aktivitetFerdigheter = new List<Skill>();


        /// <summary>
        /// Get the list of all ferdigheter that this aktivitet has gotten
        /// </summary>
        /// <returns></returns>
        public List<Skill> GetAktivitetFerdigheter()
        {
            return aktivitetFerdigheter;
        }


        /// <summary>
        /// It will returns true if the aktivitet is done
        /// </summary>
        /// <returns></returns>
        public bool IsAktivitetCompeleted()
        {
            return AktivitetIsCompeleted;
        }


        /// <summary>
        /// Mark this aktivitet as done
        /// </summary>
        /// <param name="value"></param>
        public void AktivitetIsDone(bool value)
        {
            AktivitetIsCompeleted = value;
        }


        /// <summary>
        /// This will add a ferdighet to this aktivitet
        /// </summary>
        /// <param name="skill"></param>
        public void AddToAktivitetFerdigheter(Skill skill)
        {
            if (!aktivitetFerdigheter.Contains(skill))
                aktivitetFerdigheter.Add(skill);
        }


        /// <summary>
        /// Get the name of this aktivitet
        /// </summary>
        /// <returns></returns>
        public string GetAktivitetName()
        {
            return aktivitetName;
        }


        /// <summary>
        /// Set the name of this aktivitet
        /// </summary>
        /// <param name="value"></param>
        public void SetAktivitetName(string value)
        {
            aktivitetName = value;
        }


        /// <summary>
        /// Get AchievedPoeng
        /// </summary>
        /// <returns></returns>
        public int GetAchievedPoeng()
        {
            return achievedPoeng;
        }


        /// <summary>
        /// Change AchievedPoeng
        /// </summary>
        /// <param name="givenPoen"></param>
        public void SetAchievedPoeng(int givenPoen)
        {
            achievedPoeng = givenPoen;
        }


        /// <summary>
        /// Add the poing to this aktivitet
        /// </summary>
        /// <param name="value"></param>
        public void AddToAchievedPoeng(int value)
        {
            foreach (Skill ferdighet in aktivitetFerdigheter)
            {
                foreach (Activity aktivitet in ferdighet.GetFerdighetAktiviteter().Keys.ToList())
                {
                    if (aktivitet.GetAktivitetName() == aktivitetName)
                    {
                        //if (achievedPoeng + value < ferdighet.GetAchievedPoeng())
                            achievedPoeng += value;
                        return;
                        //else
                        //    achievedPoeng = ferdighet.GetAchievedPoeng();
                    }
                }
            }
        }

    }
}