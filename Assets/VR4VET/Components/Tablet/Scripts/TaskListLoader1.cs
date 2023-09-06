/*
 * Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tablet
{
    public class TaskListLoader1 : MonoBehaviour
    {
        private List<Task.Task> _tasks = new List<Task.Task>();
        private List<Task.Skill> _skills = new List<Task.Skill>();

        private StaticPanelManager panelManager;

        //main pages

        [Header("Main Page Canvas")]
        public GameObject tasksListCanvas;

        public GameObject subtaskPageCanvas;
        public GameObject TaskPageCanvas;
        public GameObject skillsListPageCanvas;
        public GameObject skillPageCanvas;

        //parents objects to load the buttons in
        [Header("Content Spaces")]
        public GameObject taskContent;

        public GameObject TaskSubtaskContent;
        public GameObject skillContent;
        [SerializeField] private GameObject _subtaskContent;
        [SerializeField] private GameObject _skillSubtaskContent;

        [Header("skill other")]
        [SerializeField] private TMP_Text _skillTab;

        [SerializeField] private TMP_Text _skillFeedback;
        [SerializeField] private TMP_Text _skillabout;
        [SerializeField] private TMP_Text _skillPoints;

        [Header("task other")]
        [SerializeField] private TMP_Text _taskNameTab;

        [SerializeField] private TMP_Text _taskFeedback;
        [SerializeField] private TMP_Text _taskAboutTab;

        [Header("sutask other")]
        [SerializeField] private TMP_Text _subtaskNameTab;

        [SerializeField] private TMP_Text _subtaskAboutTab;

        [Header("Experience Name")]
        [SerializeField] private string Exp_Name;

        [Header("UI Prefabs")]
        [SerializeField] private GameObject _skillEntryList;

        [SerializeField] private GameObject _stepListEntry;
        [SerializeField] private GameObject _subtaskListEntry;
        [SerializeField] private GameObject _taskListEntry;

        [Header("Aditional Events")]
        [SerializeField] private UnityEvent _skillPageOpen;

        [SerializeField] private UnityEvent _tasksListOpen;
        [SerializeField] private UnityEvent _taskPageOpen;
        [SerializeField] private UnityEvent _subtaskPageOpen;

        private void Start()
        {
            //setting loading the scriptable objects
            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
            _tasks = th.taskList;
            _skills = th.skillList;

            panelManager = this.gameObject.GetComponent<StaticPanelManager>();
            //load info in the tablet
            StartCoroutine(LoadTabletInfo());
            panelManager.SetExperienceName(Exp_Name);
        }

        //since task and skill won't change in the experience we can load them from the beginning
        private IEnumerator LoadTabletInfo()
        {
            yield return new WaitForSeconds(2);
            LoadTaskPage();
            LoadSkillsPage();
        }

        public void LoadSkillsPage()
        {
            //loads each skill on the parent object
            foreach (Task.Skill skill in _skills)
            {
                //task for the list
                GameObject item = Instantiate(_skillEntryList, Vector3.zero, Quaternion.identity);
                Debug.Log("skill item added, " + item.name + " skill:" + skill.Name);
                item.transform.SetParent(skillContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                // we find the button first and then its text component
                Button button = item.transform.Find("btn_SubTask").GetComponent<Button>();
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true);
                buttonText.text = skill.Name;

                button.onClick.AddListener(() => SkillPageLoader(skill));
            }
            // refreshing after adding the new elements for the Page loader to set the pages correctly
            skillContent.GetComponent<ContentPageChanger>().Refresh();
        }

        public void SkillPageLoader(Task.Skill skill)
        {
            if (_skillPageOpen != null) _skillPageOpen.Invoke();

            // hide the subtask list view
            panelManager.OnClickToAboutSkill();

            _skillTab.text = skill.Name;
            _skillabout.text = skill.Description;
            _skillFeedback.text = skill.Feedback;
            _skillPoints.text = skill.GetArchivedPoints() + "/" + skill.MaxPossiblePoints;

            //cleaning list before loading the new subtasks
            foreach (Transform child in _skillSubtaskContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (Task.Subtask sub in skill.Subtasks)
            {
                //task for the list
                GameObject item = Instantiate(_subtaskListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(_skillSubtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                TMP_Text caption = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
                // GameObject points = item.transform.Find("PointText").gameObject; points for later
                caption.text = sub.SubtaskName;

                Button button = item.transform.Find("btn_SubTask").GetComponent<Button>();
                GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
                if (sub.Compleated()) checkmark.SetActive(true);
                button.onClick.AddListener(() => SubTaskPageLoader(sub));
            }
            // refreshing after adding the new elements for the Page loader to set the pages correctly
        }

        //gets called on Start since the list of task is always the same
        public void LoadTaskPage()
        {
            if (_tasksListOpen != null) _tasksListOpen.Invoke();

            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
            _tasks = th.taskList;

            //loads each task on the parent object
            // will add the task
            foreach (Task.Task task in _tasks)
            {
                //task for the list
                GameObject item = Instantiate(_taskListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(taskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                TMP_Text caption = item.transform.Find("txt_TaskNr").GetComponent<TMP_Text>();
                caption.text = task.TaskName;
                Button button = item.transform.Find("btn_Task").GetComponent<Button>();
                GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
                if (task.Compleated()) checkmark.SetActive(true);

                button.onClick.AddListener(() => panelManager.OnClickBackToAboutTask());
                button.onClick.AddListener(() => TaskPageLoader(task));
            }
            // refreshing after adding the new elements for the Page loader to set the pages correctly
            taskContent.GetComponent<ContentPageChanger>().Refresh();
        }

        public void TaskPageLoader(Task.Task task)
        {
            //for extra events
            if (_taskPageOpen != null) _taskPageOpen.Invoke();

            panelManager.OnClickBackToAboutTask();

            //hide previos pagee
            _taskNameTab.text = task.TaskName;
            _taskAboutTab.text = task.Description;
            _taskFeedback.text = task.Feedback;

            //cleaning list before loading the new subtasks
            foreach (Transform child in TaskSubtaskContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (Task.Subtask sub in task.Subtasks)
            {
                //task for the list
                GameObject item = Instantiate(_subtaskListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(TaskSubtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                TMP_Text caption = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
                // GameObject points = item.transform.Find("PointText").gameObject; points for later
                caption.text = sub.SubtaskName;

                Button button = item.transform.Find("btn_SubTask").GetComponent<Button>();
                GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
                if (sub.Compleated()) checkmark.SetActive(true);
                button.onClick.AddListener(() => SubTaskPageLoader(sub));
            }
        }

        public void SubTaskPageLoader(Task.Subtask subtask)
        {
            if (_subtaskPageOpen != null) _subtaskPageOpen.Invoke();

            //hide previos pagee
            panelManager.OnClickSkillSubtasks();

            TaskPageCanvas.SetActive(false);
            subtaskPageCanvas.SetActive(true);

            _subtaskNameTab.GetComponent<TMP_Text>().text = subtask.SubtaskName;
            _subtaskAboutTab.GetComponent<TMP_Text>().text = subtask.Description;

            //cleaning list before loading the new subtasks
            foreach (Transform child in _subtaskContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (Task.Step step in subtask.StepList)
            {
                GameObject item = Instantiate(_stepListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(_subtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                TMP_Text caption = item.transform.Find("txt_Desc").GetComponent<TMP_Text>();
                GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
                if (step.IsCompeleted()) checkmark.SetActive(true);

                TMP_Text reps = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();

                caption.text = step.StepName;
                reps.text = step.RepetionsCompleated + "/" + step.RepetionNumber;
            }
        }
    }
}