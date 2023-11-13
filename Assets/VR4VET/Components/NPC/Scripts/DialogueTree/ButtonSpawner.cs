using System.Linq;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class ButtonSpawner : MonoBehaviour
{

    [SerializeField] GameObject parentInCanvas;
    [SerializeField] GameObject buttonPrefab;
    // max 4 buttons
    private GameObject[] buttonsSpawned = new GameObject[4];
    private DialogueBoxController dialogueBoxController;

    void Start() {
        dialogueBoxController = GetComponent<DialogueBoxController>();
        if (dialogueBoxController == null ) {
            Debug.LogError("The NPC missing the DialogueBoxController script");
        }
    }

    // max 4 buttons
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

    // hard coded workaround for lambda functions 
    void AddAnswerQuestionNumberListener() {
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

    public void spawnAnswerButtons(Answer[] answers) {
        for (int i = 0; i < answers.Length; i++)
        {
            // spawn button at location, and set the canvas as the parent
            Vector3 spawnLocation = getSpawnLocation(answers.Length, i);
            buttonsSpawned[i] = Instantiate(buttonPrefab, spawnLocation, Quaternion.identity);
            GameObject button = buttonsSpawned[i];
            button.transform.SetParent(parentInCanvas.transform, false);
            RectTransform buttonTransfrom = button.GetComponent<RectTransform>();
            Vector3 buttonLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
            buttonTransfrom.localPosition = buttonLocation;
            // fill in the text
            button.GetComponentInChildren<TextMeshProUGUI>().text = answers[i].answerLabel;
            // add lister onclick 
            //(the lambdafunction does not work, because it just sends the maxmum value of i and not the current i, since it is in its own thread)
            // Button buttonComp= button.GetComponent<Button>();
            // buttonComp.onClick.AddListener(() => {dialogueBoxController.AnswerQuestion(0);});
            // buttonComp.onClick.AddListener(() => {Debug.Log("Script Button [ " + i + " ] was clicked.");});
        }
        // Add onclick listeners (lambda workaround)
        AddAnswerQuestionNumberListener();
    }

    /// <summary>
    /// Destroy the button gameobject and remove the button reference in the array
    /// </summary>
    public void removeAllButtons() {
        for (int i = 0; i < buttonsSpawned.Count(); i++)
        {
            if (buttonsSpawned[i] != null)
            {
                Destroy(buttonsSpawned[i].gameObject);
                buttonsSpawned[i] = null;
            }
        }
    }
}
