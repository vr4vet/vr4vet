using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CollapsibleTable : MonoBehaviour
{
    [SerializeField] private GameObject taskPrefab;
    [SerializeField] private GameObject subtaskPrefab;
    [SerializeField] private GameObject skillTextPrefab;
    [SerializeField] private Transform taskRowParent;
    [SerializeField] private Transform skillRowParent;

    private List<Transform> taskRows = new List<Transform>();
    private List<Transform> skillColumns = new List<Transform>();
    private List<Task.Task> allTasks = new List<Task.Task>();
    private List<Task.Skill> allSkills = new List<Task.Skill>();

    private Dictionary<Transform, List<Transform>> taskSubtaskMap = new Dictionary<Transform, List<Transform>>();
    private RectTransform rectTransform;
    private float leftLimit = -1000;
    private float rightLimit = 0;
    private float upLimit = 1000;
    private float downLimit = 0;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        allTasks = Task.TaskHolder.Instance.taskList;
        allSkills = Task.TaskHolder.Instance.skillList;
        GenerateTable(allTasks);
    }

    private void GenerateTable(List<Task.Task> tasks)
    {
        ClearTable();

        foreach (var task in tasks)
        {
            Transform taskRow = Instantiate(taskPrefab, taskRowParent).transform;
            taskRow.Find("txt_TaskNr").GetComponent<TMP_Text>().text = task.TaskName;
            taskRows.Add(taskRow);

            List<Transform> subtaskRows = new List<Transform>();

            foreach (var subtask in task.Subtasks)
            {
                Transform subtaskRow = Instantiate(subtaskPrefab, taskRowParent).transform;
                subtaskRow.Find("txt_SubTaskNr").GetComponent<TMP_Text>().text = subtask.SubtaskName;
                subtaskRow.gameObject.SetActive(false);
                subtaskRows.Add(subtaskRow);
            }

            // Create a button to toggle subtasks
            Button taskButton = taskRow.Find("btn_Task").GetComponent<Button>();
            taskButton.onClick.AddListener(() => ToggleSubtasks(subtaskRows));

           // ToggleSubtasks(subtaskRows); 

            taskSubtaskMap.Add(taskRow, subtaskRows);
        }

        CreateSkillColumns();
    }

    private void CreateSkillColumns()
    {
        foreach (var skill in allSkills)
        {
            Transform skillColumn = Instantiate(skillTextPrefab, skillRowParent).transform;
            skillColumn.Find("txt_SkillProficiency").GetComponent<TMP_Text>().text = skill.Name;
            skillColumns.Add(skillColumn);
        }
    }

    private void ClearTable()
    {
        foreach (Transform child in taskRowParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in skillRowParent)
        {
            Destroy(child.gameObject);
        }

        taskRows.Clear();
        skillColumns.Clear();
         taskSubtaskMap.Clear();
    
    }

    public void ToggleSubtasks(List<Transform> subtaskRows)
    {
        bool areSubtasksActive = subtaskRows[0].gameObject.activeSelf; // Check the state of the first subtask

        foreach (Transform subtaskRow in subtaskRows)
        {
            subtaskRow.gameObject.SetActive(!areSubtasksActive);
        }
    }

    public void PageUp()
    {
        float newPosition = rectTransform.anchoredPosition.y - 50;
        newPosition = Mathf.Clamp(newPosition, downLimit, upLimit);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newPosition);
    }
    public void PageDown()
    {
        float newPosition = rectTransform.anchoredPosition.y + 50;
        newPosition = Mathf.Clamp(newPosition, downLimit, upLimit);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newPosition);
    }

    public void PageRight()
    {
        float newPosition = rectTransform.anchoredPosition.x - 150;
        newPosition = Mathf.Clamp(newPosition, leftLimit, rightLimit);
        rectTransform.anchoredPosition = new Vector2(newPosition, rectTransform.anchoredPosition.y);
    }

    public void PageLeft()
    {
        float newPosition = rectTransform.anchoredPosition.x + 150;
        newPosition = Mathf.Clamp(newPosition, leftLimit, rightLimit);
        rectTransform.anchoredPosition = new Vector2(newPosition, rectTransform.anchoredPosition.y);
    }
   
}
