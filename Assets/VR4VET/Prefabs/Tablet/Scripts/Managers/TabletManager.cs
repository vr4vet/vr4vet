/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using UnityEngine;
using UnityEngine.UI;

namespace Tablet
{
    /// <summary>
    /// This class is the main class for the tablet
    /// </summary>
    public class TabletManager : MonoBehaviour
    {
        private TabletPosition tabletPos;

        [Header("strings")]
        public string YrkesNavn = "Change this";

        #region Public Variables
        [Header("Canvases")]
        public Canvas mainPageCanvas;
        public Canvas oppgaverPageCanvas;
        public Canvas aktiviteterPageCanvas;
        public Canvas ferdigheterPageCanvas;
        public Canvas ferdighetPageCanvas;
        public Canvas helpPageCanvas;
        #endregion

        private GameObject[] yrkesTitles;
        public static TabletManager tabletManager;


        Camera cam;

        /// <summary>
        /// Runs at the start
        /// </summary>
        private void Start()
        {
            if (tabletManager == null)
                tabletManager = this;
            else if (tabletManager != this)
                Destroy(gameObject);

            if (!Camera.main)
                cam = GameObject.FindObjectOfType<Camera>();
            else
                cam = Camera.main;

            Debug.Log("All managers can be found under Tablet -> Managers");
            //set Camera.main as all canvases camera in world space if it's not assigned yet
            if (cam)
            {
                if(mainPageCanvas.worldCamera == null)
                    mainPageCanvas.worldCamera = cam;

                if (oppgaverPageCanvas.worldCamera == null)
                    oppgaverPageCanvas.worldCamera = cam;

                if (aktiviteterPageCanvas.worldCamera == null)
                    aktiviteterPageCanvas.worldCamera = cam;

                if (ferdigheterPageCanvas.worldCamera == null)
                    ferdigheterPageCanvas.worldCamera = cam;

                if (ferdighetPageCanvas.worldCamera == null)
                    ferdighetPageCanvas.worldCamera = cam;

                if (helpPageCanvas.worldCamera == null)
                    helpPageCanvas.worldCamera = cam;
            }

            tabletPos = transform.parent.transform.parent.gameObject.GetComponent<TabletPosition>();

            //Find all yerkesTitles gameobjects in the scene and set it up
            yrkesTitles = GameObject.FindGameObjectsWithTag("YrkesTitle");
            foreach (GameObject text in yrkesTitles)
                text.GetComponent<Text>().text = YrkesNavn;

            //restart the tablet
            ShowCanvas(mainPageCanvas);
        }


        /// <summary>
        /// Open the tablet
        /// </summary>
        /// <param name="status"></param>
        public void OpenTablet(bool status)
        {
            tabletPos.SelectTablet(status);
        }

        /// <summary>
        /// Deactive all canvases
        /// </summary>
        private void HideAllCanvases()
        {
            oppgaverPageCanvas.gameObject.SetActive(false);
            mainPageCanvas.gameObject.SetActive(false);
            aktiviteterPageCanvas.gameObject.SetActive(false);
            ferdigheterPageCanvas.gameObject.SetActive(false);
            ferdighetPageCanvas.gameObject.SetActive(false);
            helpPageCanvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// Active his canvas and deactive the other canvases
        /// </summary>
        /// <param name="canvas"></param>
        public void ShowCanvas(Canvas canvas)
        {

            HideAllCanvases();
            canvas.gameObject.SetActive(true);
        }

        /// <summary>
        /// This method vil close the tablet
        /// </summary>
        public void CloseTablet()
        {
            ShowCanvas(mainPageCanvas);
            OppgaverManager.oppgaverManager.DestroyAktiviterList();
            FerdighetManager.ferdighetManager.DestroyTheFerdigheterList();
            FerdighetManager.ferdighetManager.DestroyTheOppgaveInFerdighetList();
            OpenTablet(false);
        }



        /// <summary>
        /// this methid is only for testing and uses in  the test scene
        /// </summary>
        public void TestMethod1(GameObject testObject)
        {
            //FerdighetManager.ferdighetManager.GiveFeedback("Accuracy", "Good Job!");
            FerdighetManager.ferdighetManager.AddPoeng("Find the helmet", "Accuracy", 20);
            testObject.GetComponentInChildren<Text>().text = "Done";
            testObject.GetComponent<Collider>().enabled = false;
            testObject.GetComponent<Renderer>().material.color = Color.green;
            testObject.GetComponent<Rigidbody>().isKinematic = true;
            ;
        }


        /// <summary>
        /// this methid is only for testing and uses in  the test scene
        /// </summary>
        public void TestMethod2(GameObject testObject)
        {
            //FerdighetManager.ferdighetManager.GiveFeedback("Utholdenhet", "Good Job!");
            FerdighetManager.ferdighetManager.AddPoeng("Hit the pointer to the hammer", "Speed", 10);
            testObject.GetComponentInChildren<Text>().text = "Done";
            testObject.GetComponent<Collider>().enabled = false;
            testObject.GetComponent<Renderer>().material.color = Color.green;
            testObject.GetComponent<Rigidbody>().isKinematic = true;

            //do this only when all aktivitet of this oppgave is done
            Destroy(testObject.transform.Find("target").gameObject); //or inactive in scenet
            NavigationManager.navigationManager.ResetNavigation();
        }

    }
}