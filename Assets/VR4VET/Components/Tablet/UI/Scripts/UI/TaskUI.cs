using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{

    [SerializeField] Color completedColor;
    //ui counterpart for Task class
    TaskData _task; //the task
    public TaskData Task
    {
        get
        {
            return _task;
        }
    } 

    [SerializeField] Image checkmark;
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] Button btn;

    List<SubTaskUI> _subtasks; //this is so we can check for completion and open these

    public void InitializeInterface(TaskData t)
    {
        _task = t;
        Refresh();
        Debug.Log("Initialized task " + t.Name);

    }


    public void OnClickTaskUI()
    {
        //reference DynamicDataManager, set current Task to this, then refresh children and run StaticPanelManager view method.

        DynamicDataDisplayer.Instance.OnClickTask(this);
    }



    public void ToggleChildVisibility()
    {
        foreach (var item in _subtasks)
        {
            item.gameObject.SetActive(true);
        }
    }

    public void Refresh()
    {
        ToggleCheck(_task.IsFulfilled());
    }


    void ToggleCheck(bool b)
    {
        if (!b)
        {
            checkmark.color = Color.white;

        }
        else
        {
            checkmark.color = completedColor;

        }
    }


}
