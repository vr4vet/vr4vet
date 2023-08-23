using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class deals exclusively with populating the pages and lists of tasks, subtasks and skills, based on the currently picked element or refreshing when changing the contents of said lists.
/// </summary>
public class DynamicDataDisplayer : MonoBehaviour
{
    static DynamicDataDisplayer _instance;
    public static DynamicDataDisplayer Instance
    {
        get
        {
            return _instance;
        }
    }


    #region References
    [Header("Prefabs")]
    [SerializeField] GameObject prefabTask;
    [SerializeField] GameObject prefabSubTask;
    [SerializeField] GameObject prefabSkill;
    [SerializeField] GameObject prefabStep;




    [Header("Refs")]
    [SerializeField] ContentPageChanger taskPaginator;
    [SerializeField] ContentPageChanger subtaskPaginator;
    [SerializeField] ContentPageChanger skillPaginator;
    [SerializeField] ContentPageChanger stepPaginator;

    #endregion
    #region Variables
    List<Task.Task> _tasks = new(); //all tasks.
    List<TaskData> _subTasks = new(); //all subtasks.
    Dictionary<string, Skill> _skills; //all skills.




    Task.Task CurrentTask;
    Task.Subtask CurrentSubTask;
    #endregion
    #region Unity Methods
    void Awake()
    {
        if (DynamicDataDisplayer.Instance != null)
        {
            throw new Exception(name + " - ElementInitializer.Awake() - Tried to initialize duplicate singleton.");
            //Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        ProcessDebugInput();
    }

    void Update()
    {

       


    }
    #endregion

    void ProcessDebugInput()
    {



        Debug.LogWarning("Added debug task.");

        //we need tasks, subtasks, steps, skills.


        //skills - SubTask
        Skill s1 = new Skill("test", "debugdesc", 1);
        Skill s2 = new Skill("test", "debugdesc", 1);
        Skill s3 = new Skill("test", "debugdesc", 1);
        Skill s4 = new Skill("test", "debugdesc", 1);
        Skill s5 = new Skill("test", "debugdesc", 1);
        Skill s6 = new Skill("test", "debugdesc", 1);
        Skill s7 = new Skill("test", "debugdesc", 1);

        //steps -> SubTask
        List<(string, string, bool)> steps1 = new List<(string, string, bool)>() { ("Change a tire.", "Test Step Description", false), ("Change a tire.", "Test Step Description", false), ("Change a tire.", "Test Step Description", false), ("Change a tire.", "Test Step Description", false), ("Change a tire.", "Test Step Description", false), ("Change a tire.", "Test Step Description", false), ("Change a tire.", "Test Step Description", false) };

        //SubTasks
        SubTask b1 = new SubTask("test", "debugdesc", new List<Skill>() { s1, s1, s1, s1 }, steps1, false);
        SubTask b2 = new SubTask("test", "debugdesc", new List<Skill>() { s1, s1, s1, s1 }, steps1, false);
        SubTask b3 = new SubTask("test", "debugdesc", new List<Skill>() { s1, s1, s1, s1 }, steps1, false);
        SubTask b4 = new SubTask("test", "debugdesc", new List<Skill>() { s1, s1, s1, s1 }, steps1, false);

        //Task
        TaskData d1 = new("test", "debugdesc", new List<SubTask>() { b1, b2, b3, b4 });


        for (int i = 0; i < 9; i++)
        {
        //    AddTask(d1);
        }
    }

    /// <summary>
    ///  Adds the Task, as well as the entirety of the data related to the task. However not sure if this matches the existing VR4VET Task data structure perfectly.
    /// </summary>
    /// <param name="desiredTask">Task to be added to the tablet task list.</param>
    void AddTask(Task.Task desiredTask)
    {
        _tasks.Add(desiredTask);
        TaskUI t = Instantiate(prefabTask, taskPaginator.transform).GetComponent<TaskUI>(); //
        t.InitializeInterface(desiredTask);
        taskPaginator.AddChild(t.gameObject);
    }



    /// <summary>
    /// runs when clicking on a task, displays subtasks for the current task
    /// </summary>
    void RefreshSubtasks()
    {//shows the subtasks for current task

        subtaskPaginator.Clear();
        List<Task.Subtask> newSubTasks = CurrentTask.Subtasks;
        if (newSubTasks.Count > 0)
        {
            for (int i = 0; i < newSubTasks.Count; i++)
            {
                SubTaskUI subtaskInterface = Instantiate(prefabSubTask, subtaskPaginator.transform).GetComponent<SubTaskUI>();

                //add UnityAction to the button to allow navigating to the respective skill menu 
                subtaskInterface.InitializeButton(newSubTasks[i]);
                SubTaskUI g = Instantiate(prefabSubTask, subtaskPaginator.transform).GetComponent<SubTaskUI>();
                g.InitializeButton(newSubTasks[i]);


            }

        }
        subtaskPaginator.Refresh();
    }
    /// <summary>
    /// runs when clicking on a subtask, displays skills for the current task
    /// </summary>
    void RefreshSkills()
    {
        skillPaginator.Clear();

        foreach (var item in _skills)
        {
            SkillUI b = Instantiate(prefabSkill, skillPaginator.transform).GetComponent<SkillUI>();
            skillPaginator.AddChild(b.gameObject); //initialize new skills UI.
            b.UpdateValue();

        }



        List<Task.Skill> newSkills = CurrentSubTask.RelatedSkills;

        if (newSkills != null)
        {
            for (int i = 0; i < newSkills.Count; i++)
            {
                SkillUI skillInterface = Instantiate(prefabSkill, skillPaginator.transform).GetComponent<SkillUI>();

                skillInterface.InitializeButton(newSkills[i].Name);


            }
        }
    }
    void RefreshSteps()
    {
        //1 get steps of current SubTask
        //2 instantiate a prefab for each
        //parent it to the step content paginator

        StepUI b = Instantiate(prefabStep, skillPaginator.transform).GetComponent<StepUI>();
        stepPaginator.AddChild(b.gameObject); //initialize new skills UI.
        //add the given Task's skills we do this on subtask selection
        List<Task.Skill> newSkills = CurrentSubTask.RelatedSkills;

        if (newSkills != null)
        {
            for (int i = 0; i < newSkills.Count; i++)
            {
                SkillUI skillUI = Instantiate(prefabSkill, skillPaginator.transform).GetComponent<SkillUI>();
                skillUI.InitializeButton(newSkills[i].Name);
            }
        }




    }



    //clears preexisting subtasks, if any, and instantiates SubTaskUI for the current task. if performance due to Instantiate() and Destroy() calls is an issue pooling could solve it but i seriously doubt it's anywhere measurably detrimental
    public void OnClickTask(TaskUI task)
    {

        CurrentTask = task.Task;
        RefreshSubtasks();
        StaticPanelManager.Instance.OnClickTask();
    }
    public void OnClickSubTask(Task.Subtask subtask)
    {


        CurrentSubTask = subtask;
        RefreshSteps();
    }
    public void OnClickSkill(string skillName)
    {



    }

    void ClearTasks()
    {
        _tasks.Clear();
    }

    public int GetSkillProficiency(string b)
    {
        Skill prof;
        if (!_skills.TryGetValue(b, out prof))
        {
            throw new System.Exception("DynamicDataDisplayer @ GetSkillProficiency - attempted to get skill proficiency but skill doesn't exist in _skills dictionary.");
        }
        return prof.Proficiency;
    }

    public Skill GetSkill(string b)
    {
        Skill prof;
        if (!_skills.TryGetValue(b, out prof))
        {
            throw new System.Exception("DynamicDataDisplayer @ GetSkillProficiency - attempted to get skill but skill " + b + " doesn't exist in _skills dictionary.");
        }
        return prof;
    }
}
