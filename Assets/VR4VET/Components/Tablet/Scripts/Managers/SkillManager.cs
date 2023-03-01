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
    /// This class will handle everything about ferdighet objects
    /// </summary>
    public class SkillManager : MonoBehaviour
    {
        public TabletManager _tabletManager;
        public TaskManager _taskmanager;

        [SerializeField]
        private GameObject _skillNameLabel;
        [SerializeField]
        private GameObject _descriptionText;
        [SerializeField]
        private GameObject _skillTaskLabel;
        [SerializeField]
        private GameObject _contentContainer;

        //all ferdigheter will save in this list
        [HideInInspector]
        public List<Skill> _skillList = new List<Skill>();

        //This list holds list rows of ferdighet page.
        //uses to destroy the created elements every time we close the ferdigheter page
        private List<GameObject> ferdighetItemsObj = new List<GameObject>();

        

        private void Start()
        {
            TaskHolder th = GameObject.FindObjectsOfType<TaskHolder>()[0];
            _skillList = th.getSkillList();


            //if there is no ferdigheter yet, create a test one and send an error message


            // Invoke("CreateFerdighetObjects", 0.1f);
        }




        /// This method will change the skill description
      
        public void GiveFeedback(string skillName, string feedback)
        {
            foreach (Skill ferdighet in _skillList)
            {
                if (ferdighet.GetFerdighetName() == skillName)
                {
                    ferdighet.SetFerdighetBeskrivelse(feedback);
                }
            }
        }

       /// Extract string from body
        public List<string> ExtractFromBody(string body, string start, string end)
        {
            List<string> matched = new List<string>();

            int indexStart = 0;
            int indexEnd = 0;

            bool exit = false;
            while (!exit)
            {
                indexStart = body.IndexOf(start);

                if (indexStart != -1)
                {
                    indexEnd = indexStart + body.Substring(indexStart).IndexOf(end);

                    matched.Add(body.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length));

                    body = body.Substring(indexEnd + end.Length);
                }
                else
                {
                    exit = true;
                }
            }

            return matched;
        }


        /// Create the ferdighet detail page
      
        private void GenerateFerdighetPage(Skill skill)
        {

            _skillNameLabel.GetComponent<Text>().text = skill.GetFerdighetName();
            _descriptionText.GetComponent<Text>().text = skill.GetFerdighetBeskrivelse();
            _skillTaskLabel.GetComponent<Text>().text = skill.GetFerdighetName() + "Is tested in the following tasks:";

            Dictionary<string, List<string>> oppgaverTotalPoengDic = new Dictionary<string, List<string>>();



            //get every ferdighet and its oppgaver and aktiviteter
         

            _tabletManager.ShowCanvas(_tabletManager.ferdighetPageCanvas);
        }



        // Calls every time the go back to main menu
        public void DestroyTheFerdigheterList()
        {
            foreach (GameObject element in ferdighetItemsObj)
            {
                Destroy(element);
            }
            ferdighetItemsObj.Clear();

        }




        //  this method will create the ferdighet page with the last information and show the page
         
        public void CreateFerdigheterPage(Canvas skillListName)
        {
            foreach (Skill ferdighet in _skillList)
            {
                //Create aktivitet in the list
                GameObject ferdighetItem = Instantiate((GameObject)Resources.Load("UI/FerdighetItem"), Vector3.zero, Quaternion.identity);
                ferdighetItem.transform.SetParent(_contentContainer.transform);
                ferdighetItem.transform.localPosition = Vector3.zero;
                ferdighetItem.transform.localScale = Vector3.one;
                ferdighetItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
                ferdighetItem.transform.Find("FerdighetButton").GetComponentInChildren<Text>().text = ferdighet.GetFerdighetName(); //aktivitet name
                ferdighetItem.transform.Find("FerdighetButton").GetComponent<Button>().onClick.AddListener(() => GenerateFerdighetPage(ferdighet));
                ferdighetItemsObj.Add(ferdighetItem);

                int poeng = 0;
                foreach (Task oppgave in _taskmanager.tasks)
                {
                    foreach (Activity aktivitet in oppgave.GetAktivitetList())
                    {
                        foreach (Skill ferdighetObj in aktivitet.GetAktivitetFerdigheter())
                        {
                            if (ferdighetObj.GetFerdighetName() == ferdighet.GetFerdighetName())
                            {
                                foreach (KeyValuePair<Activity, int> item in ferdighetObj.GetFerdighetAktiviteter())
                                {
                                    if(item.Key.GetAktivitetName() == aktivitet.GetAktivitetName())
                                        poeng += item.Value;
                                }
                            }
                        }
                    }
                }

                //Show poeng line 5/10
                string myPoeng = poeng.ToString();  
                //total poeng
                string totalPoeng = ferdighet.GetTotalPoeng().ToString();

                ferdighetItem.transform.Find("Poeng").GetComponent<Text>().text = myPoeng + "/" + totalPoeng; //aktivitet name

            }
            _tabletManager.ShowCanvas(skillListName);
        }
        

       

    }
}