using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StepUI : MonoBehaviour
{

    [SerializeField] Image checkmark;
    [SerializeField] TextMeshProUGUI txt_name;
    [SerializeField] TextMeshProUGUI txt_description;

    private SubTask associatedSubTask;
    private string _name;
    private string _desc;
    public string Name
    {
        get
        {
            return _name;
        }
    }
    public string Description
    {
        get
        {
            return _desc;
        }
    }

    public void InitializeButton(string stepName, string description, SubTask s)
    {//initialized when a subtask is chosen
        associatedSubTask = s;
        _name = stepName;
        _desc = description;
        txt_name.text = _name;
        txt_description.text = _desc;
        Debug.Log("Initialized new Step with text" + _name + " and description " + _desc);
    }


    public void RefreshCompletion()
    {
        bool b = associatedSubTask.CheckStepCompletion(_name);
        checkmark.color = (b ? Color.green : Color.white);
    }
}
