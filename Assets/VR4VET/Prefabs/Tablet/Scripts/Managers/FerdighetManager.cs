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
    public class FerdighetManager : MonoBehaviour
    {
        public static FerdighetManager ferdighetManager;

        [Tooltip("Hvormange ferdigheter har du?"), Range(0, 15)]
        public int antallFerdigheter = 0;

        [Tooltip("Ferdigheter av denne yrke, ferdighet navn kan ikke være mer en 15 bokstaver")]
        public string[] ferdighter = new string[0];

        [Tooltip("Beskrivelse av denne ferdighet"), TextArea(5, 20)]
        public string[] beskrivelser;

        //[Space(10)]
        //[Tooltip("Total poeng ferdigheter")]
        //public int totalPoeng = 100; 

        //all ferdigheter will save in this list
        [HideInInspector]
        public List<Ferdighet> ferdigheterList = new List<Ferdighet>();

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
            //set the size of beskrivelser as same as ferdigheter amount

            if (ferdighter.Length != antallFerdigheter || beskrivelser.Length != antallFerdigheter)
            {
                Array.Resize(ref ferdighter, antallFerdigheter);
                Array.Resize(ref beskrivelser, antallFerdigheter);
            }


            //not allow to be more that 15 char
            for (int i = 0; i < ferdighter.Length; i++)
            {
                if (ferdighter[i].Length > 15)
                {
                    ferdighter[i] = ferdighter[i].Substring(0, 14);
                }
            }

        }

        /// <summary>
        /// Unity MonoBehaviour Start method 
        /// </summary>
        private void Start()
        {
            if (ferdighetManager == null)
                ferdighetManager = this;
            else if (ferdighetManager != this)
                Destroy(gameObject);

            //if there is no ferdigheter yet, create a test one and send an error message
            if (ferdighter.Length == 0)
            {
                Debug.LogError("You have not created any ferdigheter yet. Create new ferdigheter under FerdighetManager gameobject");
                Array.Resize(ref ferdighter, 1);
                Array.Resize(ref beskrivelser, 1);
                ferdighter[0] = "Test Ferdighet";
                beskrivelser[0] = "You have not created any ferdigheter yet. Create new ferdigheter under FerdighetManager gameobject";
            }

            Invoke("CreateFerdighetObjects", 0.1f);
        }




        /// <summary>
        /// Create the detail page for this ferdighet
        /// </summary>
        private void CreateFerdighetObjects()
        {
            int i = 0;
            foreach (string ferfighet in ferdighter)
            {
                Ferdighet f = new GameObject(ferfighet).AddComponent<Ferdighet>();
                f.transform.SetParent(gameObject.transform);
                f.SetTotalPoeng(GetFerdighetTotalPoeng(ferfighet));
                f.SetAchievedPoeng(0);
                f.SetFerdighetName(ferfighet);
                f.SetFerdighetBeskrivelse(beskrivelser[i]);
                ferdigheterList.Add(f);
                i++;
            }
        }

        /// <summary>
        /// this will calculate the maz poeng each ferdighet can take using the text file
        /// </summary>
        /// <param name="ferdighet"></param>
        /// <returns></returns>
        private int GetFerdighetTotalPoeng(string ferdighet)
        {

            List<string> ferdighetPart = ExtractFromBody(OppgaverManager.oppgaverManager.LoadTextFile(), "{", "}");
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



        /// <summary>
        /// This method will add poeng to ferdigheter
        /// </summary>
        /// <param name="oppgaveName"></param>
        /// <param name="ferdighetName"></param>
        /// <param name="poeng"></param>
        public void AddPoeng(string aktivitetName, string ferdighetName,int poeng)
        {
            foreach (Ferdighet ferdighet in ferdigheterList)
            {
                foreach (Oppgave oppgave in OppgaverManager.oppgaverManager.oppgaver)
                {
                    foreach (Aktivitet aktivitet in oppgave.GetAktivitetList())
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

                            //add the aktivitet to ferdighter dic som holds aktiviteter that need this ferdighet and its poeng
                            ferdighet.AddToFerdighetAktiviteter(aktivitet, poeng);
                        }
                    }
                }
            }

        }


        /// <summary>
        /// This method will change the ferdighet beskrivelse and will use to giving feedback to the user
        /// </summary>
        /// <param name="ferdighetName"></param>
        /// <param name="tilbakeMelding"></param>
        public void GiveFeedback(string ferdighetName, string tilbakeMelding)
        {
            foreach (Ferdighet ferdighet in ferdigheterList)
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
        private void GenerateFerdighetPage(Ferdighet ferdighet)
        {

            FindDeepChild(TabletManager.tabletManager.ferdighetPageCanvas.gameObject, "FerdighetNameLabel").GetComponent<Text>().text = ferdighet.GetFerdighetName();
            FindDeepChild(TabletManager.tabletManager.ferdighetPageCanvas.gameObject, "BeskrivelseText").GetComponent<Text>().text = ferdighet.GetFerdighetBeskrivelse();
            FindDeepChild(TabletManager.tabletManager.ferdighetPageCanvas.gameObject, "OppgaverLabel").GetComponent<Text>().text = ferdighet.GetFerdighetName() + " testes i disse oppgavene";

            Dictionary<string, List<string>> oppgaverTotalPoengDic = new Dictionary<string, List<string>>();



            //get every ferdighet and its oppgaver and aktiviteter
            List<string> ferdighetPart = ExtractFromBody(OppgaverManager.oppgaverManager.LoadTextFile(), "{", "}");
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
                        foreach (KeyValuePair<Aktivitet, int> ferdighetAktiviteter in ferdighet.GetFerdighetAktiviteter())
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
                        ferdighetItem.transform.SetParent(FindDeepChild(TabletManager.tabletManager.ferdighetPageCanvas.gameObject, "Content").transform);
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

            TabletManager.tabletManager.ShowCanvas(TabletManager.tabletManager.ferdighetPageCanvas);
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
        public void CreateFerdigheterPage(Canvas FerdigheterListContent)
        {
            foreach (Ferdighet ferdighet in ferdigheterList)
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
                foreach (Oppgave oppgave in OppgaverManager.oppgaverManager.oppgaver)
                {
                    foreach (Aktivitet aktivitet in oppgave.GetAktivitetList())
                    {
                        foreach (Ferdighet ferdighetObj in aktivitet.GetAktivitetFerdigheter())
                        {
                            if (ferdighetObj.GetFerdighetName() == ferdighet.GetFerdighetName())
                            {
                                foreach (KeyValuePair<Aktivitet, int> item in ferdighetObj.GetFerdighetAktiviteter())
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
            TabletManager.tabletManager.ShowCanvas(FerdigheterListContent);
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