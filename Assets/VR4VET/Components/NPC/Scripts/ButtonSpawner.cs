using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ButtonSpawner : MonoBehaviour
{

    [SerializeField] GameObject parentInCanvas;
    [SerializeField] GameObject buttonPrefab;
    //private List<GameObject> buttonsSpawned = new List<GameObject>();
    private GameObject[] buttonsSpawned = new GameObject[4];

    private DialogueBoxController dialogueBoxController;
    // private Func<int, int> LambdaAnswerQuestion;
    // public UnityAction<int> DoSomething = (int nr) => {
    //     Debug.Log("function one butoon nr: "+ nr);
    // };

    void Start() {
        dialogueBoxController = GetComponent<DialogueBoxController>();
        if (dialogueBoxController == null ) {
             Debug.LogError("The NPC missing the DialogueBoxController script");
        }
    }


    // Start is called before the first frame update
    // void Start()
    // {
    //     spawnAnswerButtons(4);
    // }

    //void spawnAnswerButtons(int numberOfButtons, String[] answerOptions) {
    void spawnAnswerButtons() {
        Vector3 spawnLocation = new Vector3(0,0,0);
        GameObject button = Instantiate(buttonPrefab, spawnLocation, Quaternion.identity);
        button.transform.SetParent(parentInCanvas.transform, false);
        RectTransform buttonTransfrom = button.GetComponent<RectTransform>();
        Vector3 buttonLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        buttonTransfrom.localPosition = buttonLocation;


        //Destroy(button);
    }

    public void spawnAnswerButtons(int numberOfButtons) {
        for (int i = 0; i < numberOfButtons; i++)
        {
        Vector3 spawnLocation = getSpawnLocation(numberOfButtons, i);
        GameObject button = Instantiate(buttonPrefab, spawnLocation, Quaternion.identity);
        button.transform.SetParent(parentInCanvas.transform, false);
        RectTransform buttonTransfrom = button.GetComponent<RectTransform>();
        Vector3 buttonLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        buttonTransfrom.localPosition = buttonLocation;
        }
    }

    public void spawnAnswerButtons(Answer[] answers) {

        for (int i = 0; i < buttonsSpawned.Count(); i++)
        {
            Debug.Log("Button [ " + i + " ] should not be here");
        }

        Debug.Log("number of buttons: " + answers.Length);
        int nr = 0;
        for (int i = 0; i < answers.Length; i++)
        {
            // spawn button at location
            Debug.Log("Button [ " + i + " ] is created");
            GameObject button;
            Vector3 spawnLocation = getSpawnLocation(answers.Length, i);
            Debug.Log("Spawn location: " + spawnLocation);
            buttonsSpawned[i] = Instantiate(buttonPrefab, spawnLocation, Quaternion.identity);
            button = buttonsSpawned[i];
            button.transform.SetParent(parentInCanvas.transform, false);
            RectTransform buttonTransfrom = button.GetComponent<RectTransform>();
            Vector3 buttonLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
            buttonTransfrom.localPosition = buttonLocation;
            // fill in the text
            button.GetComponentInChildren<TextMeshProUGUI>().text = answers[i].answerLabel;
            // add lister onclick
            // Button buttonComp= button.GetComponent<Button>();
            // Debug.Log("ButtonComp" + buttonComp);
            // Debug.Log("Button [ " + i + " ] is still here?? has the number changed?");
            // // buttonPrefab.GetComponent<Button>().onButtonDown.AddListener(() => {dialogueBoxController.AnswerQuestion(i);});
            // buttonComp.onClick.AddListener(() => {Debug.Log("Script Button [ " + i + " ] was clicked. WOOOOOOW");});
            // Debug.Log("Button [ " + i + " ] is still here?? has the number changed?");
            // buttonComp.onClick.AddListener(() => {dialogueBoxController.AnswerQuestion(0);});
            // Debug.Log("Button [ " + i + " ] is still here?? has the number changed?");
            // nr++;
            // Delegate void LambdaAnswerQuestion = (int nr) => {dialogueBoxController.AnswerQuestion(nr); return 0;};
        
            // buttonsSpawned[i].GetComponent<Button>().onClick.AddListener(DoSomething(i));
        }
        // TODO: figure out this super weird bug. Lambda bullshit. 
        iamsoconfused();
    }

    public void removeAllButtons() {
        Debug.Log("Number of buttons to remove: " + buttonsSpawned.Count());
        for (int i = 0; i < buttonsSpawned.Count(); i++)
        {
            if (buttonsSpawned[i] != null)
            {
                Destroy(buttonsSpawned[i].gameObject);
                buttonsSpawned[i] = null;
            }
        }
    }
    

    private Vector3 getSpawnLocation(int numberOfButtons, int currentNumber) {
        switch (numberOfButtons)
        {
            case 1:
                return new Vector3(0,-38,1);
            case 2:
                return new Vector3(-24 + 48 * currentNumber,-38,0);
            case 3:
                return new Vector3(-46 + 46 * currentNumber,-38,1);
            case 4:
                return new Vector3(-58 + 39 * currentNumber,-38,1);
            default:
                Debug.Log("You have too many buttons/answer options of the dialogue tree. I do not know how to place them");
                return new Vector3(0,0,0);
        }
    }

    public void clickDebug(int i) {
        Debug.Log("Button [ " + i + " ] was clicked");
    }

    void iamsoconfused() {
        Debug.Log("iamsoconfused number of buttons: " + buttonsSpawned.Count());
        for (int i = 0; i < buttonsSpawned.Count(); i++)
        {
            if(buttonsSpawned[i] != null) {
                switch (i)
                {
                    case 0:
                        buttonsSpawned[0].GetComponent<Button>().onClick.AddListener(() => {dialogueBoxController.AnswerQuestion(0);});
                        break;
                    case 1:
                        buttonsSpawned[1].GetComponent<Button>().onClick.AddListener(() => {dialogueBoxController.AnswerQuestion(1);});
                        break;
                    case 2:
                        buttonsSpawned[2].GetComponent<Button>().onClick.AddListener(() => {dialogueBoxController.AnswerQuestion(2);});
                        break;
                    case 3:
                        buttonsSpawned[3].GetComponent<Button>().onClick.AddListener(() => {dialogueBoxController.AnswerQuestion(3);});
                        break;
                    
                    default:
                        buttonsSpawned[0].GetComponent<Button>().onClick.AddListener(() => {dialogueBoxController.AnswerQuestion(0);});
                        break;
                }
            }
        }
    }

}
