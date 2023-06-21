using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SubTaskUI : MonoBehaviour
{
    //this exists so we can store tasks, subtasks and skills without having to attach them to an object

    [SerializeField] Image checkmark;
    [SerializeField] TextMeshProUGUI texty;
    [SerializeField] Button btn;

    SubTask _subTask;

    List<(string, string, bool)> Steps
    {
        get
        {
            return _subTask.Steps;
        }
    }





    public void InitializeButton(SubTask subtask)
    {
        _subTask = subtask;
        texty.text = _subTask.Name;
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
        if (!_subTask.IsFulfilled)
        {
            checkmark.color = Color.white;

        }
        else
        {
            checkmark.color = Color.green;

        }
    }



    public void Fulfill()
    {
        _subTask.Fulfill();
    }


   


    //selects this particular subtask, navigates to this subtask's interface representation, and refreshes the UI to display the related skills
    public void OnClickSubTaskUI()
    {

        DynamicDataDisplayer.Instance.OnClickSubTask(this._subTask);
    }
}
