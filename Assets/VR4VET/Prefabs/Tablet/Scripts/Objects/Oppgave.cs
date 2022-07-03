/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tablet
{
    /// <summary>
    /// Oppgave object
    /// </summary>
    public class Oppgave : MonoBehaviour
    {

        [Tooltip("Skriv oppgave navnet her")]
        public string oppgaveName;

        [Tooltip("Beskrivelse av denne oppgaven"), TextArea(5, 20)]
        public string beskrivelse;

        [Tooltip("Hvormange oppgaver har du?"), Range(0, 15)]
        [SerializeField]
        public int totalAktiviteter = 0;

        [Tooltip("Aktiviteter Liste")]
        [SerializeField]
        public string[] aktiviteter = new string[0];

        [Header("Navigasjon")]
        public GameObject oppgaveTarget;
        public Oppgave prerequisite;

        [HideInInspector]
        public Button button;

        [HideInInspector]
        public Image checkedIcon;
        [HideInInspector]
        public Image unCheckedIcon;

        private List<Aktivitet> aktivitetList = new List<Aktivitet>();


        [HideInInspector] private bool is_task_Completed;


        /// <summary>
        /// Unity Update method
        /// </summary>
        private void Update()
        {
            CheckTaskCompeletion();
            if (is_task_Completed)
            {
                if (button)
                    button.interactable = false;

                if (checkedIcon && checkedIcon)
                {
                    checkedIcon.enabled = true;
                    unCheckedIcon.enabled = false;
                }
            }

        }



        /// <summary>
        /// This method calls in editor
        /// </summary>
        void OnValidate()
        {
            //set the size of aktiviteter and poeng as same as totalAktiviter amount
            Array.Resize(ref aktiviteter, totalAktiviteter);


            //not allow to be more that 15 char
            if (oppgaveName.Length > 15)
                oppgaveName = oppgaveName.Substring(0,14);



            //not allow to be more that 30 char
            for (int i = 0; i < aktiviteter.Length; i++)
            {
                if (aktiviteter[i].Length > 30)
                {
                    aktiviteter[i] = aktiviteter[i].Substring(0, 29);
                }
            }
        }


        /// <summary>
        /// Mark this task as compeleted if its all aktiviteter are done
        /// </summary>
        public void CheckTaskCompeletion()
        {
            //the task is not completed if even one aktivitet is not done
            foreach (Aktivitet aktivitet in aktivitetList)
            {
                if (aktivitet.IsAktivitetCompeleted() == false)
                {
                    is_task_Completed = false;
                    return;
                }
            }
            is_task_Completed = true;
        }

        /// <summary>
        /// Ask about this task is compeletet or not
        /// </summary>
        /// <returns></returns>
        public bool IsTaskCompeleted()
        {
            return is_task_Completed;
        }


        /// <summary>
        /// Unity Start method
        /// </summary>
        private void Start()
        {
            foreach (string aktivitet in aktiviteter)
            {
                Aktivitet a = new GameObject(aktivitet).AddComponent<Aktivitet>();
                a.transform.SetParent(gameObject.transform);
                a.SetAktivitetName(aktivitet);
                a.SetAchievedPoeng(0);
                a.AktivitetIsDone(false);//not completed at the start
                aktivitetList.Add(a); // fill the dictionary with aktiviteter and poenger
            }
        }


        /// <summary>
        /// Get the list of all aktivitet that this oppgave has
        /// </summary>
        /// <returns></returns>
        public List<Aktivitet> GetAktivitetList()
        {
            return aktivitetList;
        }
    }
}