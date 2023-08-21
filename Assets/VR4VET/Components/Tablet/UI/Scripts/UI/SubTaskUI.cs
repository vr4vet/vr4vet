using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SubTaskUI : MonoBehaviour
{
    //this exists so we can store tasks, subtasks and skills without having to attach them to an object

    [SerializeField] GameObject checkmark;
    [SerializeField] TextMeshProUGUI texty;
    [SerializeField] Button btn;

    Task.Subtask _subTask;

    List<Task.Step> Steps
    {
        get
        {
            return _subTask.StepList;
        }
    }





    public void InitializeButton(Task.Subtask subtask)
    {
        _subTask = subtask;
        texty.text = _subTask.SubtaskName;
        Refresh();
        Debug.Log("Initialized new SubTask Button for SubTask => " + texty.text);
    }


    public void ToggleChildVisibility()
    {
        //foreach (var item in _subtasks)
        //{
        //    item.gameObject.SetActive(true);
        //}
    }


    public void Refresh()
    {
        ToggleCheck(_subTask.Compleated());
    }

    void ToggleCheck(bool b)
    {
        checkmark.SetActive(b);
    }

    public void Fulfill()
    {
        _subTask.SetCompleated(true);
    }


   


    //selects this particular subtask, navigates to this subtask's interface representation, and refreshes the UI to display the related skills
    public void OnClickSubTaskUI()
    {

        DynamicDataDisplayer.Instance.OnClickSubTask(this._subTask);
    }
}
