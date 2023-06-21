using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskData
{
    string _taskName;
    string defaultDescription = "Praesent ac efficitur neque, non pretium leo. Proin sed felis tempus, consectetur sem vel, scelerisque mi. Ut placerat dolor id est viverra, quis fringilla tellus placerat. Vestibulum nec risus dui. Suspendisse tempus mi eu odio lobortis varius. Interdum et malesuada fames ac ante ipsum primis in faucibus. Quisque tincidunt diam consectetur, feugiat ante ut, laoreet quam.";
    string _description = "";
    List<SubTask> _subtasks;


    public string Name
    {
        get
        {
            return _taskName;
        }
    }

    public string Description
    {
        get
        {
            return _description;
        }
    }

    public List<SubTask> SubTasks
    {
        get
        {
            return _subtasks;
        }
    }


    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="n">Desired name.</param>
    /// <param name="d">Desired description.</param>
    /// <param name="s">Desired task subtasks.</param>
    public TaskData(string n, string d , List<SubTask> s = null)
    {
        _taskName = n;
        _description = d;
        _subtasks = new List<SubTask>(s);
    }


    public bool IsFulfilled()
    {
        if (_subtasks == null)
        {
            return true;
        }
        bool complete = true;
        for (int i = 0; i < _subtasks.Count; i++)
        {
            if (_subtasks[i].IsMandatory && !_subtasks[i].IsFulfilled)
            {
                complete = false;
            }
        }
        //train skill after completed task? vs after subtask completion
        //foreach (var item in GetRelevantSkills())
        //{
        //    item.
        //}
        return complete;
    }


    public List<string> GetRelevantSkills()
    {
        if (_subtasks == null || _subtasks.Count == 0)
        {
            return null;
        }
        List<string> l = new();
        foreach (var item in _subtasks)
        {
            foreach (var s in item.RelevantSkills)
            {
                l.Add(s);
            }
        }
        return (l.Count > 0) ? l : null;
    }


}
