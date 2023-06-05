using System.Collections.Generic;
using UnityEngine;

public class ScoreChartUI : MonoBehaviour
{

    [SerializeReference] GameObject ChartTaskPrefab;
    [SerializeReference] GameObject ChartSubtaskPrefab;
    [SerializeReference] GameObject ChartSkillTextPrefab;


    [SerializeReference] Transform taskRowParent;
    [SerializeReference] Transform skillRowParent; //we dont assign to this, we assign to children of this
    List<Transform> SkillColumns = new(); //each existing skill has it's own column

    // Start is called before the first frame update
    void Start()
    {

    }







    /// <summary>
    /// call this after task data initialization in Dynamic Data Displayer
    /// </summary>
    /// <param name="tasks"> all tasks </param>
    void Generate(List<TaskData> tasks)
    {
        List<object> tasksAndSubtasks = new();
        //get put tasks and subtasks into it

        foreach (var item in tasksAndSubtasks)
        {
            if (item.GetType().FullName == "TaskData")
            {
                GameObject b = Instantiate(ChartTaskPrefab);
                //add TaskUI here and let the player click it to goto the desired task/subtask

                //instantiate task
            }
            else if (item.GetType().FullName == "SubTaskData")
            {
                //instantiate task
                //add TaskUI here and let the player click it to goto the desired task/subtask

            }
            else
            {
                throw new System.Exception("ScoreChartUI @ Generate() - added unknown object to tasks&subtasks to instantiate ");
            }
        }



        //get tasks, instantiate task-subtask(1,2,3,4...etc) row
        //get skills, instantiate skill row (Skill name + skill text for each task/subtask)





    }




    // Update is called once per frame
    void Update()
    {

    }











}
