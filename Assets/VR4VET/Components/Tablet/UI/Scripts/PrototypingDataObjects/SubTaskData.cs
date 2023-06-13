using System.Collections.Generic;

public class SubTask
{
    string _name = "SubTask #";
    string _desc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam rutrum bibendum ante luctus lobortis. Vestibulum pharetra, mi et euismod posuere, ex nibh venenatis velit, vel eleifend nisi purus ut mi. Vestibulum tempus, ligula ut auctor iaculis, ligula tellus iaculis risus, ac vulputate justo libero in tortor. Etiam ut est nisl. Donec consectetur, dolor at vehicula fermentum, sem tellus pellentesque magna, id viverra nunc risus ac eros. Ut dui lectus, consectetur et varius ut, scelerisque ut metus. Curabitur nec aliquam nunc. Nunc vitae eleifend mi. Pellentesque consectetur felis ut velit fermentum, in lacinia nibh consectetur. In vitae arcu est. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae;";
    List<string> _skills = new List<string>();  //as a string, because they're stored in and referenced from DynamicDataDisplayer to avoid issues with having to update the current proficiency
    List<(string, string, bool)> _steps; //name, description, completion
    private bool _mandatory;
    private bool _fulfilled;







    /// <summary>
    /// Initializes a subtask.
    /// </summary>
    /// <param name="Name">The desired name. Will appear on the label.</param>
    /// <param name="Desc">The desired description. Will appear in the feedback.</param>
    /// <param name="Skills">The skills which are relevant to this particular subtask.</param>
    /// <param name="Steps">Steps in the subtask. No functionality - just a boolean, name and description.</param>
    /// <param name="Mandatory">Wether this subtask is optional or not in fulfilling the task.</param>
    public SubTask(string Name, string Desc, List<Skill> Skills, List<(string, string, bool)> Steps, bool Mandatory)
    {
        _steps = Steps;
        _name = Name;
        _desc = Desc;
        foreach (var item in Skills)
        {
            item.AssociateRelevantSubtasks(new List<SubTask> { this });
            _skills.Add(item.Name);
        }
        _mandatory = Mandatory;
    }



    public bool IsMandatory
    {
        get
        {
            return _mandatory;
        }
    }
    public bool IsFulfilled
    {
        get
        {
            return _fulfilled;
        }
    }


   

    public List<(string, string, bool)> Steps
    {
        get
        {
            return _steps;
        }
    }
    public string Name
    {
        get
        {
            return _name;
        }
    }
    public string Description
    {
        get
        {
            return _desc;
        }
    }


    public List<string> RelevantSkills
    {
        get
        {
            return _skills;
        }
    }

    public bool CheckStepCompletion(string stepName)
    {
        foreach (var item in _steps)
        {
            if (item.Item1 == stepName && item.Item3)
            {
                return true;
            }
        }
        return false;
    }

    public void Fulfill()
    {
        _fulfilled = true;
    }



}
