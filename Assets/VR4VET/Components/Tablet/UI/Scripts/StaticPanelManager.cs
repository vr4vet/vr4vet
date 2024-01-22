using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Tablet;

public class StaticPanelManager : MonoBehaviour
{

    static StaticPanelManager _instance;
    public static StaticPanelManager Instance
    {
        get
        {
            return _instance;
        }
    }


    GameObject DebugPrefab;
    //references
    [Header("Menu references")]
    [SerializeReference] GameObject MainMenu;
    [SerializeReference] GameObject TaskListMenu;
    [SerializeReference] GameObject TaskAboutMenu;
    [SerializeReference] GameObject SubtaskAboutMenu;
    [SerializeReference] GameObject SkillListMenu;
    [SerializeReference] GameObject SkillAboutMenu;
    [SerializeReference] GameObject ScoreListMenu;
    [SerializeReference] GameObject HelpMenu;
    [SerializeReference] GameObject ScoreMenu;
    private List<GameObject> allMenus = new();




    //the following are for the various submenus
    [Header("task")]
    //for switching around
    [SerializeReference] GameObject PANEL_TASK_ABOUT;
    [SerializeReference] TextMeshProUGUI TXT_TASK; //is the current task
    [SerializeReference] GameObject PANEL_TASK_FEEDBACK;
    [SerializeReference] GameObject PANEL_TASK_SUBTASKLIST;
    [SerializeReference] TextMeshProUGUI TXT_Task_Exp;
    [SerializeReference] TextMeshProUGUI TXT_TaskAbout_Exp;

    [Header("subtask")]
    [SerializeReference] TextMeshProUGUI TXT_Subtask_task; //is the current task shown for the current subtask, as shown in the title

    [SerializeReference] TextMeshProUGUI TXT_Subtask; //is the current Subtask
    [SerializeReference] GameObject PANEL_Subtask_Description; 
    [SerializeReference] GameObject PANEL_Subtask_Steps;
    [SerializeReference] TextMeshProUGUI TXT_Subtask_Exp;


    [Header("skill")]

    [SerializeReference] GameObject PANEL_SKILL_ABOUT_SUBTASKLIST;
    [SerializeReference] GameObject PANEL_Skill_FEEDBACK;
    [SerializeReference] GameObject PANEL_Skill_ABOUT_SKILL;
    [SerializeReference] TextMeshProUGUI TXT_SkillAbout_Exp;

    [SerializeReference] TextMeshProUGUI TXT_Skill; //is the current Skill
    [SerializeReference] TextMeshProUGUI TXT_Skill_Proficiency;
    [SerializeReference] TextMeshProUGUI TXT_Skill_Exp;


    [Header("score")]

    [SerializeReference] TextMeshProUGUI TXT_Score; //is the current score
    [SerializeReference] TextMeshProUGUI TXT_Score_Exp;
    [SerializeReference] TextMeshProUGUI TXT_ScoreInfo_Exp;

    [Header("help")]
    [SerializeReference] TextMeshProUGUI TXT_Help_Exp;

    //methods below, categorized by the screen they are relevant to
    #region UnityMethods
    void Awake()
    {
        if (StaticPanelManager.Instance != null)
        {
            throw new System.Exception(name + " - FloatingUIManager.Awake() - Tried to initialize duplicate singleton.");
        }
        else
        {
            _instance = this;
        }
    }

    #endregion



    #region Navigation Methods

    public void TestFunction()
    {
        Debug.LogWarning("TestFunction got run.");
    }


    public void OnClickToAboutSkill()
    {
        SwitchMenuTo(SkillAboutMenu);
    }

    public void OnClickBackToWorkplace()
    {
        SwitchMenuTo(MainMenu);
    }

    public void OnClickBackToTasks()
    {
        TaskListLoader1 taskListLoader1 = GetComponent<TaskListLoader1>();
        taskListLoader1.UpdateTaskPage();
        SwitchMenuTo(TaskListMenu);
    }

    public void OnClickBackToAboutTask()
    {
        SwitchMenuTo(TaskAboutMenu);
    }
    public void OnClickBackToSkillMenu()
    {
        SwitchMenuTo(SkillListMenu);
    }

    public void OnClickBackToScoreList()
    {
        SwitchMenuTo(ScoreListMenu);
    }

    public void ChangeCurrentTask(string b)
    {
        TXT_TASK.text = b;
        TXT_Subtask_task.text = b;

    }
    public void ChangeCurrentSubTask(string b)
    {
        TXT_Subtask.text = b;

    }
    public void ChangeCurrentSkill(string b)
    {
        TXT_Skill.text = b;

    }

    public void ChangeCurrentScore(string b)
    {
        TXT_Score.text = b;

    }

    public void SetExperienceName(string b)
    {
        TXT_Task_Exp.text = "Task - " + b;
        TXT_TaskAbout_Exp.text = "Task - " + b;
        TXT_Subtask_Exp.text = "Task - " + b;
        TXT_Skill_Exp.text = "Skills - " + b;
        TXT_SkillAbout_Exp.text = "Skills - " + b;
        TXT_Score_Exp.text = "Score - " + b;
        TXT_Help_Exp.text = "Help - " + b;
    }

    void SwitchMenuTo(GameObject b)
    {
        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        //run whatever graphical resetting methods here if we do things like expanding/collapsing categories | clear text areas of text here as well
        b.SetActive(true);
        Debug.Log("Now displaying: " + b);
    }

    void Start()
    {
        allMenus.AddRange(new List<GameObject>() { MainMenu, TaskListMenu, TaskAboutMenu, SubtaskAboutMenu, SkillListMenu, SkillAboutMenu, ScoreListMenu, ScoreMenu, HelpMenu });

        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        MainMenu.SetActive(true);
    }

    void Update()
    {

    }
    #endregion
    #region MainMenu
    public void OnClickMenuTask()
    {
        TaskListLoader1 taskListLoader1 = GetComponent<TaskListLoader1>();
        taskListLoader1.UpdateTaskPage();
        SwitchMenuTo(TaskListMenu);
    }
    public void OnClickMenuSkills()
    {
        SwitchMenuTo(SkillListMenu);
    }

    public void OnClickMenuHelp()
    {
        SwitchMenuTo(HelpMenu);
    }

    public void OnClickMenuScore()
    {
        // SwitchMenuTo(ScoreListMenu);
        SwitchMenuTo(ScoreMenu);
    }


    #endregion
    #region TaskListMenu

    public void OnClickTask()
    {

        SwitchMenuTo(TaskAboutMenu);

    }



    #endregion
    #region TaskAboutMenu

    public void OnClickTaskAbout()
    {
        PANEL_TASK_ABOUT.SetActive(true);
        PANEL_TASK_FEEDBACK.SetActive(false);
        PANEL_TASK_SUBTASKLIST.SetActive(false);
    }
    public void OnClickTaskFeedback()
    {
        PANEL_TASK_ABOUT.SetActive(false);
        PANEL_TASK_FEEDBACK.SetActive(true);
        PANEL_TASK_SUBTASKLIST.SetActive(false);
    }
    public void OnClickTaskSubtaskList()
    {
        PANEL_TASK_ABOUT.SetActive(false);
        PANEL_TASK_FEEDBACK.SetActive(false);
        PANEL_TASK_SUBTASKLIST.SetActive(true);
    }


    public void OnClickTaskLocation()
    {
        Debug.Log("Clicked Location button.");
    }
    public void OnClickTaskSound()
    {
        Debug.Log("Clicked Sound button.");


    }


    #endregion
    #region SubtaskAboutMenu



    public void OnClickSubTaskViewSteps()
    {
        PANEL_Subtask_Description.SetActive(false);
        PANEL_Subtask_Steps.SetActive(true);
      
    }


    //return from viewing steps
    public void OnClickBackFromSubTaskStepList()
    {
        PANEL_Subtask_Description.SetActive(true);
        PANEL_Subtask_Steps.SetActive(false);
        
    }

    public void OnClickSubTaskFeedBack()
    {
        PANEL_Subtask_Description.SetActive(false);
        PANEL_Subtask_Steps.SetActive(false);
      
    }



    #endregion
    #region SkillListMenu
    public void OnClickSkill(Skill skill)
    {
        SwitchMenuTo(SkillAboutMenu);
    }


    #endregion
    #region SkillAboutMenu

    public void OnClickSkillAbout()
    {
        PANEL_Skill_ABOUT_SKILL.SetActive(true);
        PANEL_Skill_FEEDBACK.SetActive(false);
        PANEL_SKILL_ABOUT_SUBTASKLIST.SetActive(false);
    }

    public void OnClickSkillFeedback()
    {
        PANEL_Skill_ABOUT_SKILL.SetActive(false);
        PANEL_Skill_FEEDBACK.SetActive(true);
        PANEL_SKILL_ABOUT_SUBTASKLIST.SetActive(false);

    }

    public void OnClickSkillSubtasks()
    {
        PANEL_Skill_ABOUT_SKILL.SetActive(false);
        PANEL_Skill_FEEDBACK.SetActive(false);
        PANEL_SKILL_ABOUT_SUBTASKLIST.SetActive(true);
    }



    #endregion
    #region ScoreListMenu

    public void OnClickScoreIndividualEntry()
    {
        SwitchMenuTo(ScoreMenu);

    }

    #endregion
    #region ScoreMenu



    #endregion










    public void SelectTask(TaskData b)
    {

    }

    public void SelectSubTask(SubTask b)
    {


        SwitchMenuTo(SubtaskAboutMenu);
    }

    public void DisplaySkill(string name, int proficiency)
    {

    }

   
}
