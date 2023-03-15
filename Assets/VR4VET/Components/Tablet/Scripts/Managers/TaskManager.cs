/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace Tablet
{
    /// <summary>
    /// This call will controll anything about the oppgaver and their relation with the ferdigheter
    /// </summary>
    public class TaskManager : MonoBehaviour
    {

        [SerializeField] private TabletManager _tabletManager;
        [SerializeField] private SkillManager _skillManager;



        [Header("ContentView for task and activity pages")]
        public GameObject tasksContentView;
        public GameObject ActivitiesContentView;

        [HideInInspector]
        public List<Task> tasks = new List<Task>();

        //uses to destroy the created elements each time we close the ferdighet page
        private List<GameObject> aktivitetItems = new List<GameObject>();


        /// <summary>
        /// Unity start method
        /// </summary>




        private void Start()
        {

            TaskHolder th = GameObject.FindObjectsOfType<TaskHolder>()[0];
            tasks = th.GetTaskList();
            //create oppgaver list
            foreach (Task oppgave in tasks)
            {
                GameObject item = Instantiate((GameObject)Resources.Load("UI/OppgaveItem"), Vector3.zero, Quaternion.identity);
                item.transform.SetParent(tasksContentView.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                oppgave.unCheckedIcon = FindDeepChild(item, "unchecked").GetComponent<Image>();
                oppgave.unCheckedIcon.enabled = true; //show uncheck
                oppgave.checkedIcon = FindDeepChild(item, "checked").GetComponent<Image>();
                oppgave.checkedIcon.enabled = false; //Hide check
                oppgave.button = item.transform.Find("OppgaveButton").GetComponent<Button>();
                oppgave.button.onClick.AddListener(() => NavigationManager.navigationManager.SetTarget(oppgave, oppgave.button));


                //if this oppgave is already done
                if (oppgave.IsTaskCompeleted())
                    oppgave.button.interactable = false; //deactive the button
                //

                string oppgaveName = oppgave._taskName;

                if (oppgaveName.Length > 25)
                {
                    string oppgaveNameResized = oppgaveName.Substring(0, 25) + " ...";
                    item.transform.Find("OppgaveButton").GetComponentInChildren<Text>().text = oppgaveNameResized;
                }
                else
                {
                    item.transform.Find("OppgaveButton").GetComponentInChildren<Text>().text = oppgaveName;
                }

                item.transform.Find("LesButton").GetComponent<Button>().onClick.AddListener(() => CreateTaskPage(oppgave));
            }
        }



       
        
        private void CreateTaskPage(Task task)
        {

            _tabletManager.ShowCanvas(_tabletManager.aktiviteterPageCanvas);
            FindDeepChild(_tabletManager.aktiviteterPageCanvas.gameObject, "TaskNameLabel").GetComponent<Text>().text = task._taskName;
            FindDeepChild(_tabletManager.aktiviteterPageCanvas.gameObject, "DescriptionText").GetComponent<Text>().text = task.description;

            Debug.Log(task.GetAktivitetList());

            foreach (Activity aktivitetObject in task.GetAktivitetList())
            {
                //Create aktivitet in the list
                GameObject aktivitet = Instantiate((GameObject)Resources.Load("UI/AktivitetItem"), Vector3.zero, Quaternion.identity);
                aktivitet.transform.SetParent(ActivitiesContentView.transform);
                aktivitet.transform.localPosition = Vector3.zero;
                aktivitet.transform.localScale = Vector3.one;
                aktivitet.transform.localRotation = Quaternion.Euler(0, 0, 0);
                aktivitet.transform.Find("AktivitetButton").GetComponentInChildren<Text>().text = aktivitetObject.GetAktivitetName(); //aktivitet name
                aktivitetItems.Add(aktivitet);

                //Show poeng line 5/10
                int totalAchievedPoeng = 0;

                //find out sum of all poeng that aktivitet hat fått from a ferdighet
                foreach (Skill ferdighet in _skillManager._skillList)
                {
                    foreach (KeyValuePair<Activity, int> aktivitetObj in ferdighet.GetFerdighetAktiviteter())
                    {
                        if (aktivitetObj.Key.GetAktivitetName() == aktivitetObject.GetAktivitetName())
                        {
                            totalAchievedPoeng += aktivitetObj.Value;
                        }
                    }
                }


           

                string myPoeng =  aktivitetObject.achievedPoeng.ToString() ;    //totalAchievedPoeng.ToString();
                string totalPoengStr = aktivitetObject.maxPosiblePoints.ToString();

                aktivitet.transform.Find("Poeng").GetComponent<Text>().text = myPoeng + "/" + totalPoengStr; //aktivitet name

                //Manage the checkbox
                GameObject checkbox = aktivitet.transform.Find("Checkbox").gameObject;
                if (totalAchievedPoeng == 0)
                {
                    checkbox.transform.Find("Checked").gameObject.SetActive(false);
                    checkbox.transform.Find("UnChecked").gameObject.SetActive(true);
                }
                else
                {
                    checkbox.transform.Find("Checked").gameObject.SetActive(true);
                    checkbox.transform.Find("UnChecked").gameObject.SetActive(false);
                }

            }

        }



        /// <summary>
        /// Destroy the aktivitet page when coming back to main menu
        /// </summary>
        public void DestroyAktiviterList()
        {
            foreach (GameObject element in aktivitetItems)
            {
                Destroy(element);
            }
            aktivitetItems.Clear();

        }


        


        /// <summary>
        /// Find Grandchild of an gameobject
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public GameObject FindDeepChild(GameObject parent, string childName)
        {
            foreach (Transform myChild in parent.GetComponentsInChildren<Transform>())
            {
                if (myChild.name == childName)
                    return myChild.gameObject;
            }
            return null;
        }






    }

}