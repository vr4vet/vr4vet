using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tablet {
    public class TaskListLoader : MonoBehaviour
    {

        private List<Task.BTask> _tasks = new List<Task.BTask>();

        public GameObject contentViewParent;
        private List<Task.Subtask> _subtasklist;
        [SerializeField] private GameObject _SubtaskList;

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
                item.transform.SetParent(contentViewParent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Button button = item.transform.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(() => SubTaskListLoader());



            }
        }

        public void SubTaskListLoader()
        {
           

        }



    }
}
