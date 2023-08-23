using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreChartUI : MonoBehaviour
{
    [SerializeField] private GameObject ChartTaskPrefab;
    [SerializeField] private GameObject ChartSubtaskPrefab;
    [SerializeField] private GameObject ChartSkillTextPrefab;
    [SerializeField] private Transform taskRowParent;
    [SerializeField] private Transform skillRowParent;

    private int currentPage = 0;
    private int pageSize = 1;
    private List<Transform> skillColumns = new List<Transform>();
    private List<Task.Task> allTasks = new List<Task.Task>();
    private List<Task.Skill> allSkills = new List<Task.Skill>();

    private RectTransform rectTransform;
    private float leftLimit = -1000;
    private float rightLimit = 0;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        allTasks = Task.TaskHolder.Instance.taskList;
        allSkills = Task.TaskHolder.Instance.skillList;
        Generate(allTasks);
    }

    /// <summary>
    /// call this after task data initialization in Dynamic Data Displayer
    /// </summary>
    /// <param name="tasks"> all tasks </param>
    private void Generate(List<Task.Task> tasks)
    {
        ClearUI();

        int totalTaskCount = tasks.Sum(task => task.Subtasks.Count) + tasks.Count;
        int pageCount = Mathf.CeilToInt((float)totalTaskCount / pageSize);

        currentPage = Mathf.Clamp(currentPage, 0, pageCount - 1);
        int startIndex = currentPage * pageSize;
        int endIndex = Mathf.Min(startIndex + pageSize, totalTaskCount);

        int currentTaskIndex = 0;
        int currentSubtaskIndex = 0;

        foreach (var task in tasks)
        {
            if (currentTaskIndex >= startIndex && currentTaskIndex < endIndex)
            {
                Instantiate(ChartTaskPrefab, taskRowParent).name = "T" + currentTaskIndex;
            }

            currentTaskIndex++;

            foreach (var subtask in task.Subtasks)
            {
                if (currentSubtaskIndex >= startIndex && currentSubtaskIndex < endIndex)
                {
                    Instantiate(ChartSubtaskPrefab, taskRowParent).name = "S" + currentSubtaskIndex;
                }

                currentSubtaskIndex++;
                if (currentSubtaskIndex >= endIndex)
                    break;
            }

            if (currentTaskIndex >= endIndex)
                break;
        }


        int skillStartIndex = currentPage * pageSize;
        int skillEndIndex = Mathf.Min(skillStartIndex + pageSize, allSkills.Count);

        for (int i = skillStartIndex; i < skillEndIndex; i++)
        {
            Instantiate(ChartSkillTextPrefab, skillRowParent).name = i.ToString();
        }
    }


    private void ClearUI()
    {
        foreach (Transform child in taskRowParent)
            Destroy(child.gameObject);

        foreach (Transform child in skillRowParent)
            Destroy(child.gameObject);

        skillColumns.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) PageUp();
        if (Input.GetKeyDown(KeyCode.S)) PageDown();
        if (Input.GetKeyDown(KeyCode.A)) PageLeft();
        if (Input.GetKeyDown(KeyCode.D)) PageRight();
    }

    public void PageDown()
    {
        currentPage++;
        Generate(allTasks);
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

    public void PageUp()
    {
        currentPage--;
        Generate(allTasks);
    }
}
