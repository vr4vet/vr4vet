/*
 * Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tablet {
    public class TaskListLoader : MonoBehaviour
    {

        private List<Task.BTask> _tasks = new List<Task.BTask>();
        private List<Task.Skill> _skills = new List<Task.Skill>();

        //main pages
        public GameObject tasksListCanvas;
        public GameObject subtaskPageCanvas;
        public GameObject TaskPageCanvas;
        public GameObject skillsListPageCanvas;
        public GameObject skillPageCanvas;
        
        
        //parents objects to load the buttons in
        public GameObject taskContent;
        public GameObject TaskSubtaskContent;

        void Start()
        {
            //setting loading the scriptable objects 
            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
            _tasks = th.GetTaskList();
            _skills = th.getSkillList();

            //load info in the tablet
            LoadTaskPage();
            LoadSkillsPage();


        }

        public void LoadSkillsPage()
        {   

            GameObject content = skillsListPageCanvas.transform.Find("TasksScrollListBG/ScrollListViewport/TaksContent").gameObject;

            //loads each task on the parent object
            foreach (Task.Skill skill in _skills)
            {
                //task for the list
                GameObject item = Instantiate((GameObject)Resources.Load("UI/SkillItem"), Vector3.zero, Quaternion.identity);
                
                item.transform.SetParent(content.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Text caption = item.GetComponentInChildren<Text>(true);
                caption.text = skill.Name;
                Button button = item.transform.Find("Button").GetComponent<Button>();

                button.onClick.AddListener(() => SkillPageLoader(skill));
            }
        }

        public void SkillPageLoader(Task.Skill skill)
        {
            //hide previos pagee
            skillsListPageCanvas.SetActive(false);
            skillPageCanvas.SetActive(true);
            // hide the subtask list view
            GameObject name = skillPageCanvas.transform.Find("ListView/Labels/SkillNameLabel").gameObject;
            GameObject descrption = skillPageCanvas.transform.Find("ListView/DescriptionScrollView/Viewport/BeskrivelseContent/DescriptionText").gameObject;
            GameObject content = skillPageCanvas.transform.Find("ListView/ProgressScrollView/ProgressScrollListViewport/ContentSkill").gameObject;
            name.GetComponent<Text>().text = skill.Name;
            descrption.GetComponent<Text>().text = skill.Description;


            //cleaning list before loading the new subtasks
            foreach (Transform child in content.transform)
            {
                GameObject.Destroy(child.gameObject);
            }


            foreach (Task.Subtask sub in skill.Subtasks)
            {
                GameObject item = Instantiate((GameObject)Resources.Load("UI/StepItem"), Vector3.zero, Quaternion.identity);
                item.transform.SetParent(content.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Text caption = item.transform.Find("Text").gameObject.GetComponent<Text>();
                Text reps = item.transform.Find("RepText").gameObject.GetComponent<Text>();
                caption.text = sub.Name;
                reps.text = sub.Points() + "/" + sub.MaxPoints;

            }


        }


        //gets called on Start since the list of task is always the same
        public void LoadTaskPage()
        {
            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
            _tasks = th.GetTaskList();

            //loads each task on the parent object
            foreach (Task.BTask task in _tasks)
            {
                //task for the list
                GameObject item = Instantiate((GameObject)Resources.Load("UI/TaskItem"), Vector3.zero, Quaternion.identity);
                item.transform.SetParent(taskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Text caption = item.GetComponentInChildren<Text>(true);
                caption.text = task.name;
                Button button = item.transform.Find("Button").GetComponent<Button>();

                button.onClick.AddListener(() => TaskPageCanvas.SetActive(true));
                button.onClick.AddListener(() => TaskPageLoader(task));
            }
        }

        public void TaskPageLoader(Task.BTask task)
        {
            //hide previos pagee
            tasksListCanvas.SetActive(false);
            GameObject name = TaskPageCanvas.transform.Find("ListView/Labels/TaskNameLabel").gameObject;
            GameObject descrption = TaskPageCanvas.transform.Find("ListView/DescriptionScrollView/Viewport/DescriptionText").gameObject;
            name.GetComponent<Text>().text = task._taskName;
            descrption.GetComponent<Text>().text = task.Description;


            //cleaning list before loading the new subtasks
            foreach (Transform child in TaskSubtaskContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }


            foreach (Task.Subtask sub in task.Subtasks)
            {
                //task for the list
                GameObject item = Instantiate((GameObject)Resources.Load("UI/SubtaskItem"), Vector3.zero, Quaternion.identity);
                item.transform.SetParent(TaskSubtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Text caption = item.GetComponentInChildren<Text>(true);
                GameObject points = item.transform.Find("PointText").gameObject;
                points.GetComponent<Text>().text =sub.Points() + "/" + sub.MaxPoints ; 
                caption.text = sub.Name;

                Button button = item.transform.Find("Button").GetComponent<Button>();                              
                button.onClick.AddListener(() => SubTaskPageLoader(sub));
            }

        }


        public void SubTaskPageLoader(Task.Subtask subtask)
        {
            //hide previos pagee
            TaskPageCanvas.SetActive(false);
            subtaskPageCanvas.SetActive(true);
            // hide the subtask list view
            GameObject name = subtaskPageCanvas.transform.Find("ListView/Labels/SubtaskNameLabel").gameObject;
            GameObject descrption = subtaskPageCanvas.transform.Find("ListView/DescriptionScrollView/Viewport/DescriptionText").gameObject;
            GameObject content = subtaskPageCanvas.transform.Find("ListView/StepsScrollView/StepViewport/ContentSteps").gameObject;
            name.GetComponent<Text>().text = subtask.Name;
            descrption.GetComponent<Text>().text = subtask.Description;


            //cleaning list before loading the new subtasks
            foreach (Transform child in content.transform)
            {
                GameObject.Destroy(child.gameObject);
            }


            foreach (Task.Step step in subtask.StepList)
            {
                GameObject item = Instantiate((GameObject)Resources.Load("UI/StepItem"), Vector3.zero, Quaternion.identity);
                item.transform.SetParent(content.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Text caption = item.transform.Find("Text").gameObject.GetComponent<Text>();
                Text reps = item.transform.Find("RepText").gameObject.GetComponent<Text>();
                caption.text = step.GetName();
                reps.text = step.RepetionsCompleated + "/" + step.RepetionNumber;

            }


        }


    }
}
