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

        public GameObject taskContextView;
        public GameObject TaskPageSubtaskContent;

        public GameObject TaskPage;
        public GameObject SubtaskPage;




        private Task.TaskHolder th;

    // Start is called before the first frame update
        void Start()
        {
            LoadTaskPage();
            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
        }

        public void LoadTaskPage()
        {
            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
            _tasks = th.GetTaskList();
            //create oppgaver list
            foreach (Task.BTask task in _tasks)
            {
                //task for the list
                GameObject item = Instantiate((GameObject)Resources.Load("UI/TaskItem"), Vector3.zero, Quaternion.identity);
                item.transform.SetParent(taskContextView.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Text caption = item.GetComponentInChildren<Text>(true);
                caption.text = task.name;
                Button button = item.transform.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(() => TaskPageLoader(task));
                //loading it for test
                TaskPageLoader(task);

            }
        }

        public void TaskPageLoader(Task.BTask task)
        {
            //enable go first ------------------

            GameObject name = TaskPage.transform.Find("ListView/Labels/TaskNameLabel").gameObject;
            GameObject descrption = TaskPage.transform.Find("ListView/DescriptionScrollView/Viewport/DescriptionText").gameObject;
            name.GetComponent<Text>().text = task._taskName;
            descrption.GetComponent<Text>().text = task.Description;


            //clean list-------------
            foreach (Task.Subtask sub in task.Subtasks)
            {
                //task for the list
                GameObject item = Instantiate((GameObject)Resources.Load("UI/SubtaskItem"), Vector3.zero, Quaternion.identity);
                item.transform.SetParent(TaskPageSubtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Text caption = item.GetComponentInChildren<Text>(true);
                GameObject points = item.transform.Find("PointText").gameObject;
                points.GetComponent<Text>().text =sub.Points() + "/" + sub.MaxPoints ; 
                caption.text = sub.Name;

                Button button = item.transform.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(() => SubTaskPageLoader(sub));
                //test
                SubTaskPageLoader(sub);
            }

        }


        public void SubTaskPageLoader(Task.Subtask subtask)
        {

            GameObject name = SubtaskPage.transform.Find("ListView/Labels/TaskNameLabel").gameObject;
            GameObject descrption = SubtaskPage.transform.Find("ListView/DescriptionScrollView/Viewport/DescriptionText").gameObject;
            GameObject content = SubtaskPage.transform.Find("ListView/StepsScrollView/StepViewport/Content").gameObject;
            name.GetComponent<Text>().text = subtask.Name;
            descrption.GetComponent<Text>().text = subtask.Description;
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
