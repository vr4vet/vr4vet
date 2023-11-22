using System.Linq;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _parentInCanvas;
    [SerializeField] private GameObject _buttonPrefab;
    // max 4 buttons
    [HideInInspector] private GameObject[] _buttonsSpawned = new GameObject[4];
    [HideInInspector] private DialogueBoxController _dialogueBoxController;

    void Start() {
        _dialogueBoxController = GetComponent<DialogueBoxController>();
        if (_dialogueBoxController == null ) {
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
                Debug.LogError("You have too many buttons/answer options of the dialogue tree. I do not know how to place them");
                return new Vector3(0,0,0);
        }
    }
 
    // max 4 buttons
    void AddAnswerQuestionNumberListener() {
        for (int i = 0; i < _buttonsSpawned.Count(); i++)
        {
            if(_buttonsSpawned[i] != null) {
                switch (i)
                {
                    case 0:
                        _buttonsSpawned[0].GetComponent<Button>().onClick.AddListener(() => {_dialogueBoxController.AnswerQuestion(0);});
                        break;
                    case 1:
                        _buttonsSpawned[1].GetComponent<Button>().onClick.AddListener(() => {_dialogueBoxController.AnswerQuestion(1);});
                        break;
                    case 2:
                        _buttonsSpawned[2].GetComponent<Button>().onClick.AddListener(() => {_dialogueBoxController.AnswerQuestion(2);});
                        break;
                    case 3:
                        _buttonsSpawned[3].GetComponent<Button>().onClick.AddListener(() => {_dialogueBoxController.AnswerQuestion(3);});
                        break;
                    
                    default:
                        _buttonsSpawned[0].GetComponent<Button>().onClick.AddListener(() => {_dialogueBoxController.AnswerQuestion(0);});
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
            _buttonsSpawned[i] = Instantiate(_buttonPrefab, spawnLocation, Quaternion.identity);
            GameObject button = _buttonsSpawned[i];
            button.transform.SetParent(_parentInCanvas.transform, false);
            RectTransform buttonTransfrom = button.GetComponent<RectTransform>();
            Vector3 buttonLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
            buttonTransfrom.localPosition = buttonLocation;
            // fill in the text
            button.GetComponentInChildren<TextMeshProUGUI>().text = answers[i].answerLabel;
        }
        // Add onclick listeners
        AddAnswerQuestionNumberListener();
    }

    /// <summary>
    /// Destroy the button gameobject and remove the button reference in the array
    /// </summary>
    public void removeAllButtons() {
        for (int i = 0; i < _buttonsSpawned.Count(); i++)
        {
            if (_buttonsSpawned[i] != null)
            {
                Destroy(_buttonsSpawned[i].gameObject);
                _buttonsSpawned[i] = null;
            }
        }
    }
}
