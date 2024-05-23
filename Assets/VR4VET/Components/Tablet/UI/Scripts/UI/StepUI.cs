using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Task;

public class StepUI : MonoBehaviour
{

    [SerializeField] GameObject checkmark;
    [SerializeField] TextMeshProUGUI txt_name;
    [SerializeField] TextMeshProUGUI txt_description;

    [SerializeField] public Subtask associatedSubTask;
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

    private void FixedUpdate() {
        var Step = associatedSubTask.GetStep(txt_description.text);
        if (Step.Timer != -1) {
            Debug.Log(associatedSubTask.name);
            var Timer = Step.Counter.ToString(@"mm\:ss");
            txt_name.text = Timer;
        }
    }

    /*
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
        ToggleCheck(associatedSubTask.CheckStepCompletion(_name));
    }

    void ToggleCheck(bool b)
    {
        checkmark.SetActive(b);
    }

    */
}
