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
      //  [Tooltip("How many skills do you have?"), Range(0, 15)]
       // public int _skillQuantity = 0;

        
       // public string[] descriptions;

        //[Space(10)]
        //[Tooltip("Total poeng ferdigheter")]
        //public int totalPoeng = 100; 

        //all ferdigheter will save in this list
        [HideInInspector]
        public List<Skill> _skillList = new List<Skill>();

        //This list holds list rows of ferdighet page.
        //uses to destroy the created elements every time we close the ferdigheter page
        private List<GameObject> ferdighetItemsObj = new List<GameObject>();

        //This list holds list rows of oppgaver in ferdighet page.
        //uses to destroy the created elements every time we close the ferdighet page
        private List<GameObject> oppgaveItemsInFerdighetPage = new List<GameObject>();

        /// <summary>
        /// This method calls in editor every time any of the variables are changed
        /// </summary>
        void OnValidate()
        {
            

        }

        /// <summary>
        /// Unity MonoBehaviour Start method 
        /// </summary>
        private void Start()
        {
            TaskHolder th = GameObject.FindObjectsOfType<TaskHolder>()[0];
            _skillList = th.getSkillList();


            //if there is no ferdigheter yet, create a test one and send an error message


            // Invoke("CreateFerdighetObjects", 0.1f);
        }




        /// <summary>
        /// Create the detail page for this ferdighet
        /// </summary>
        /// 
        /**
        private void CreateFerdighetObjects()
        {
            int i = 0;
            foreach (string ferfighet in skills)
            {
                Skill f = new GameObject(ferfighet).AddComponent<Skill>();
                f.transform.SetParent(gameObject.transform);
                f.SetTotalPoeng(GetFerdighetTotalPoeng(ferfighet));
                f.SetAchievedPoeng(0);
                f.SetFerdighetName(ferfighet);
                f.SetFerdighetBeskrivelse(descriptions[i]);
                ferdigheterList.Add(f);
                i++;
            }
        }
        **/

        /// <summary>
        /// this will calculate the maz poeng each ferdighet can take using the text file
        /// </summary>
        /// <param name="ferdighet"></param>
        /// <returns></returns>
       
        /*
        private int GetFerdighetTotalPoeng(string ferdighet)
        {

            List<string> ferdighetPart = ExtractFromBody(TaskManager.oppgaverManager.LoadTextFile(), "{", "}");
            int poeng = 0;
            foreach (string part in ferdighetPart)
            {
                //get ferdighet name
                string ferdighetName = ExtractFromBody(part, "**.", ".**")[0];

                if (ferdighetName == ferdighet)
                {
                    List<string> aktivitetLines = ExtractFromBody(part, "<", ">");
                    foreach (string aktivitet in aktivitetLines)
                    {
                        string[] aktivitetWord = aktivitet.Split(':');
                        string aktivitetName = aktivitetWord[0].Substring(1);
                        int temp = 0;
                        if (Int32.TryParse(aktivitetWord[1], out temp))
                        {
                            poeng += temp;
                        }
                    }
                }
           
            }
            return poeng;
        }

        */

        /// <summary>
        /// This method will add poeng to ferdigheter
        /// </summary>
        /// <param name="oppgaveName"></param>
        /// <param name="ferdighetName"></param>
        /// <param name="poeng"></param>
        
        /*
        public void AddPoeng(string aktivitetName, string ferdighetName,int poeng)
        {
            foreach (Skill ferdighet in _skillList)
            {
                foreach (Task oppgave in TaskManager.oppgaverManager.tasks)
                {
                    foreach (Activity aktivitet in oppgave.GetAktivitetList())
                    {
                        //add poeng to aktivitet
                        if (aktivitet.GetAktivitetName() == aktivitetName && ferdighet.GetFerdighetName() == ferdighetName)
                        {
                            //add ferdighet to this aktivitet
                            aktivitet.AddToAktivitetFerdigheter(ferdighet);

                            //add poeng to aktivitet object
                            aktivitet.AddToAchievedPoeng(poeng);

                            //mark this aktivitet as compeleted
                            aktivitet.AktivitetIsDone(true);

                            //add the aktivitet to skills dic som holds aktiviteter that need this ferdighet and its poeng
                            ferdighet.AddToFerdighetAktiviteter(aktivitet, poeng);
                        }
                    }
                }
            }

        }
    
        */

        /// <summary>
        /// This method will change the ferdighet beskrivelse and will use to giving feedback to the user
        /// </summary>
        /// <param name="ferdighetName"></param>
        /// <param name="tilbakeMelding"></param>
        public void GiveFeedback(string ferdighetName, string tilbakeMelding)
        {
            foreach (Skill ferdighet in _skillList)
            {
                if (ferdighet.GetFerdighetName() == ferdighetName)
                {
                    ferdighet.SetFerdighetBeskrivelse(tilbakeMelding);
                }
            }
        }

       /// <summary>
       /// Extract string from body
       /// </summary>
       /// <param name="body"></param>
       /// <param name="start"></param>
       /// <param name="end"></param>
       /// <returns></returns>
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


        /// <summary>
        /// Create the ferdighet detail page
        /// </summary>
        /// <param name="ferdighet"></param>
        private void GenerateFerdighetPage(Skill ferdighet)
        {

            FindDeepChild(_tabletManager.ferdighetPageCanvas.gameObject, "FerdighetNameLabel").GetComponent<Text>().text = ferdighet.GetFerdighetName();
            FindDeepChild(_tabletManager.ferdighetPageCanvas.gameObject, "BeskrivelseText").GetComponent<Text>().text = ferdighet.GetFerdighetBeskrivelse();
            FindDeepChild(_tabletManager.ferdighetPageCanvas.gameObject, "OppgaverLabel").GetComponent<Text>().text = ferdighet.GetFerdighetName() + " testes i disse oppgavene";

            Dictionary<string, List<string>> oppgaverTotalPoengDic = new Dictionary<string, List<string>>();



            //get every ferdighet and its oppgaver and aktiviteter
            List<string> ferdighetPart = ExtractFromBody(_taskmanager.LoadTextFile(), "{", "}");
            foreach (string part in ferdighetPart)
            {
                //get ferdighet name
                string ferdighetName = ExtractFromBody(part, "**.", ".**")[0];


                if (ferdighetName == ferdighet.GetFerdighetName())
                {
                    //get oppgave name
                    List<string> oppgaveNames = ExtractFromBody(part, "(", ")");

                    //get the aktiviteter  of each oppgave and save to a new list
                    foreach (string oppN in oppgaveNames)
                    {
                        if (!oppgaverTotalPoengDic.ContainsKey(ExtractFromBody(oppN, "--.", ".--")[0]))
                            oppgaverTotalPoengDic.Add(ExtractFromBody(oppN, "--.", ".--")[0], ExtractFromBody(oppN, "<", ">"));
                    }



                    foreach (KeyValuePair<string, List<string>> akDic in oppgaverTotalPoengDic)
                    {

                        //calculate max poeng
                        int poeng = 0;
                        foreach (string aktivitetLine in akDic.Value)
                        {
                            string[] aktivitetWord = aktivitetLine.Split(':');
                            string aktivitetName = aktivitetWord[0].Substring(1);
                            int temp = 0;
                            if (Int32.TryParse(aktivitetWord[1], out temp))
                            {
                                poeng += temp;
                            }

                        }

                        int totalAchievedPoeng = 0;
                        //calcaulate achived poeng
                        foreach (KeyValuePair<Activity, int> ferdighetAktiviteter in ferdighet.GetFerdighetAktiviteter())
                        {
                            //same ferdighet
                            if(ferdighet.GetFerdighetName() == ferdighetName)
                            {
                                foreach (string aktivitetLine in akDic.Value)
                                {
                                    //ferdighet has these aktiviteter
                                    if(aktivitetLine.Contains(ferdighetAktiviteter.Key.GetAktivitetName()))
                                        totalAchievedPoeng += ferdighetAktiviteter.Value;
                                }
                            }
                        }

                        //create the list item
                        GameObject ferdighetItem = Instantiate((GameObject)Resources.Load("UI/FerdighetPageItem"), Vector3.zero, Quaternion.identity);
                        ferdighetItem.transform.SetParent(FindDeepChild(_tabletManager.ferdighetPageCanvas.gameObject, "Content").transform);
                        ferdighetItem.transform.localPosition = Vector3.zero;
                        ferdighetItem.transform.localScale = Vector3.one;
                        ferdighetItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        ferdighetItem.transform.Find("OppgaveButton").GetComponentInChildren<Text>().text = akDic.Key;
                        oppgaveItemsInFerdighetPage.Add(ferdighetItem);



                        string totalPoengStr = poeng.ToString();
                        string myPoeng = totalAchievedPoeng.ToString();

                        //show poeng
                        ferdighetItem.transform.Find("Poeng").GetComponent<Text>().text = myPoeng + "/" + totalPoengStr; //aktivitet name

                    }

                }

            }

            _tabletManager.ShowCanvas(_tabletManager.ferdighetPageCanvas);
        }



        /// <summary>
        /// Calls every time the go back to main menu
        /// </summary>
        public void DestroyTheFerdigheterList()
        {
            foreach (GameObject element in ferdighetItemsObj)
            {
                Destroy(element);
            }
            ferdighetItemsObj.Clear();

        }

        /// <summary>
        /// Calls every time the go back to main menu
        /// </summary>
        public void DestroyTheOppgaveInFerdighetList()
        {
            foreach (GameObject element in oppgaveItemsInFerdighetPage)
            {
                Destroy(element);
            }
            oppgaveItemsInFerdighetPage.Clear();

        }

        /// <summary>
        ///  this method will create the ferdighet page with the last information and show the page
        /// </summary>
        /// <param name="FerdigheterListContent"></param>
        /// 
        
        public void CreateFerdigheterPage(Canvas FerdigheterListContent)
        {
            foreach (Skill ferdighet in _skillList)
            {
                //Create aktivitet in the list
                GameObject ferdighetItem = Instantiate((GameObject)Resources.Load("UI/FerdighetItem"), Vector3.zero, Quaternion.identity);
                ferdighetItem.transform.SetParent(FindDeepChild(FerdigheterListContent.gameObject, "Content").transform);
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
            _tabletManager.ShowCanvas(FerdigheterListContent);
        }
        

        /// <summary>
        /// Find Grandchild of an gameobject
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        private GameObject FindDeepChild(GameObject parent, string childName)
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