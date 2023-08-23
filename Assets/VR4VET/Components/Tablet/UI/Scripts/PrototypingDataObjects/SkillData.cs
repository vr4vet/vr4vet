using System.Collections.Generic;

public class Skill
{
    List<SubTask> _relevantSubTasks = new List<SubTask>();
    private string _name;

    private string _desc;
    private int _proficiency; //0-100

    public Skill(string n, string d, int prof)
    {
        
        _name = n;
        _desc = d;
        _proficiency = prof;
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
    public int Proficiency
    {
        get
        {
            return _proficiency;
        }
    }

public void AssociateRelevantSubtasks(List<SubTask> s)
    {
        _relevantSubTasks.AddRange(s);
    }

    public void ChangeProficiency(int i)
    {
        _proficiency += i;
        //update graphics either here or when running this method
    }
}
