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
    public class Ferdighet : MonoBehaviour
    {
        private int totalPoeng;
        private int achievedPoeng;
        private string ferdighetName;
        private string ferdighetBeskrivelse;

        //hvor mye til hver aktivitet har gitt denne ferdighet (aktivitet,poeng)
        private Dictionary<Aktivitet, int> ferdighetAktiviteter = new Dictionary<Aktivitet, int>();


        /// <summary>
        /// get the dictionary of all aktivitet that are testet by this ferdighet
        /// </summary>
        /// <returns></returns>
        public Dictionary<Aktivitet, int> GetFerdighetAktiviteter()
        {
            return ferdighetAktiviteter;
        }


        /// <summary>
        /// Add an aktivitet to this ferdighet
        /// </summary>
        /// <param name="aktivitet"></param>
        /// <param name="value"></param>
        public void AddToFerdighetAktiviteter(Aktivitet aktivitet, int value)
        {
            if (!ferdighetAktiviteter.Keys.Contains(aktivitet))
            {
                if (achievedPoeng + value < totalPoeng)
                {
                    ferdighetAktiviteter.Add(aktivitet, achievedPoeng + value);
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
            return ferdighetName;
        }


        /// <summary>
        /// Set the name of this ferdighet
        /// </summary>
        /// <param name="value"></param>
        public void SetFerdighetName(string value)
        {
            ferdighetName = value;
        }

        /// <summary>
        /// Get the total poeng that this ferdighet has gotten by all aktiviteter
        /// </summary>
        /// <returns></returns>
        public int GetTotalPoeng()
        {
            return totalPoeng;
        }


        /// <summary>
        /// set the total poeng that this ferdighet can achieved to
        /// </summary>
        /// <param name="value"></param>
        public void SetTotalPoeng(int value)
        {
            totalPoeng = value;
        }


        /// <summary>
        /// Get the poeng that the player has gotten from this ferdighet
        /// </summary>
        /// <returns></returns>
        public int GetAchievedPoeng()
        {
            achievedPoeng = 0;
            foreach (KeyValuePair<Aktivitet, int> aktivitet in ferdighetAktiviteter)
            {
                achievedPoeng += aktivitet.Value;
            }
            return achievedPoeng;
        }


        /// <summary>
        /// set the poeng to this ferdighet
        /// </summary>
        /// <param name="value"></param>
        public void SetAchievedPoeng(int value)
        {
            achievedPoeng = value;
        }


        /// <summary>
        /// Change the beskrivelse of this ferdighet. can be used for feedback
        /// </summary>
        /// <param name="value"></param>
        public void SetFerdighetBeskrivelse(string value)
        {
            ferdighetBeskrivelse = value;
        }
        

        /// <summary>
        ///Get the ferdighet beskrivelse 
        /// </summary>
        /// <returns></returns>
        public string GetFerdighetBeskrivelse()
        {
            return ferdighetBeskrivelse;
        }


        /// <summary>
        /// Add more poneg to this ferdighet
        /// </summary>
        /// <param name="value"></param>
        public void AddAchievedPoeng(int value)
        {
            if (achievedPoeng + value < totalPoeng)
                achievedPoeng += value;
            else
                achievedPoeng = totalPoeng;
        }

    }
}