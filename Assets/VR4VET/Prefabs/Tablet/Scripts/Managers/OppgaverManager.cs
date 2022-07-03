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
    public class OppgaverManager : MonoBehaviour
    {
        public static OppgaverManager oppgaverManager;

        [Header("ContentView til oppgave og aktivitet sider")]
        public GameObject oppgaverContentView;
        public GameObject AktiviteterContentView;

        [HideInInspector]
        public Oppgave[] oppgaver;

        //uses to destroy the created elements each time we close the ferdighet page
        private List<GameObject> aktivitetItems = new List<GameObject>();


        /// <summary>
        /// Unity start method
        /// </summary>
        private void Start()
        {
            if (oppgaverManager == null)
                oppgaverManager = this;
            else if (oppgaverManager != this)
                Destroy(gameObject);

            oppgaver = GameObject.FindObjectsOfType<Oppgave>();

            //create oppgaver list
            foreach (Oppgave oppgave in oppgaver)
            {
                GameObject item = Instantiate((GameObject)Resources.Load("UI/OppgaveItem"), Vector3.zero, Quaternion.identity);
                item.transform.SetParent(oppgaverContentView.transform);
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

                string oppgaveName = oppgave.oppgaveName;

                if (oppgaveName.Length > 25)
                {
                    string oppgaveNameResized = oppgaveName.Substring(0, 25) + " ...";
                    item.transform.Find("OppgaveButton").GetComponentInChildren<Text>().text = oppgaveNameResized;
                }
                else
                {
                    item.transform.Find("OppgaveButton").GetComponentInChildren<Text>().text = oppgaveName;
                }

                item.transform.Find("LesButton").GetComponent<Button>().onClick.AddListener(() => CreateOppgavePage(oppgave));
            }
        }



        /// <summary>
        /// Create the aktivitet page for hver oppgave
        /// </summary>
        /// <param name="oppgave"></param>
        private void CreateOppgavePage(Oppgave oppgave)
        {
            TabletManager.tabletManager.ShowCanvas(TabletManager.tabletManager.aktiviteterPageCanvas);
            FindDeepChild(TabletManager.tabletManager.aktiviteterPageCanvas.gameObject, "OppgaveNameLabel").GetComponent<Text>().text = oppgave.oppgaveName;
            FindDeepChild(TabletManager.tabletManager.aktiviteterPageCanvas.gameObject, "BeskrivelseText").GetComponent<Text>().text = oppgave.beskrivelse;

            foreach (Aktivitet aktivitetObject in oppgave.GetAktivitetList())
            {
                //Create aktivitet in the list
                GameObject aktivitet = Instantiate((GameObject)Resources.Load("UI/AktivitetItem"), Vector3.zero, Quaternion.identity);
                aktivitet.transform.SetParent(AktiviteterContentView.transform);
                aktivitet.transform.localPosition = Vector3.zero;
                aktivitet.transform.localScale = Vector3.one;
                aktivitet.transform.localRotation = Quaternion.Euler(0, 0, 0);
                aktivitet.transform.Find("AktivitetButton").GetComponentInChildren<Text>().text = aktivitetObject.GetAktivitetName(); //aktivitet name
                aktivitetItems.Add(aktivitet);

                //Show poeng line 5/10
                int totalAchievedPoeng = 0;

                //find out sum of all poeng that aktivitet hat fått from a ferdighet
                foreach (Ferdighet ferdighet in FerdighetManager.ferdighetManager.ferdigheterList)
                {
                    foreach (KeyValuePair<Aktivitet, int> aktivitetObj in ferdighet.GetFerdighetAktiviteter())
                    {
                        if (aktivitetObj.Key.GetAktivitetName() == aktivitetObject.GetAktivitetName())
                        {
                            totalAchievedPoeng += aktivitetObj.Value;
                        }
                    }
                }


                //calculate total poeng of each aktivitet
                string ferdighetPoenText = LoadTextFile();
                List<string> aktiviteter = FerdighetManager.ferdighetManager.ExtractFromBody(ferdighetPoenText, "<", ">");
                int totalPoeng = 0;

                foreach (string aktvitet in aktiviteter)
                {
                    string[] aktivitetWord = aktvitet.Split(':');
                    if (aktivitetWord[0] == aktivitetObject.GetAktivitetName())
                    {
                        int num = 0;
                        if (Int32.TryParse(aktivitetWord[1], out num))
                            totalPoeng += num;
                    }
                }


                string myPoeng = totalAchievedPoeng.ToString();
                string totalPoengStr = totalPoeng.ToString();

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
        /// Load the generated text file as a string
        /// </summary>
        /// <returns></returns>
        public string LoadTextFile()
        {
            string text = "";
            string line = "";
            TextAsset theList = (TextAsset)Resources.Load("FerdighetPoenger", typeof(TextAsset));
            if (theList != null)
            {
                using (StreamReader sr = new StreamReader(new MemoryStream(theList.bytes)))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        text += line;
                    }
                }
            }
            return text;
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