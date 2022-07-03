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
    /// Aktivitet object
    /// </summary>
    public class Aktivitet : MonoBehaviour
    {
        private int achievedPoeng;
        private string aktivitetName;
        private bool AktivitetIsCompeleted;
        private List<Ferdighet> aktivitetFerdigheter = new List<Ferdighet>();


        /// <summary>
        /// Get the list of all ferdigheter that this aktivitet has gotten
        /// </summary>
        /// <returns></returns>
        public List<Ferdighet> GetAktivitetFerdigheter()
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
        /// <param name="ferdighet"></param>
        public void AddToAktivitetFerdigheter(Ferdighet ferdighet)
        {
            if (!aktivitetFerdigheter.Contains(ferdighet))
                aktivitetFerdigheter.Add(ferdighet);
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
            foreach (Ferdighet ferdighet in aktivitetFerdigheter)
            {
                foreach (Aktivitet aktivitet in ferdighet.GetFerdighetAktiviteter().Keys.ToList())
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