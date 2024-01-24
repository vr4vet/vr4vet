using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    //UI counterpart to skill - skill is a (string, float) tuple, list in DynamicDataDisplayer singleton

    [SerializeField] TextMeshProUGUI text_proficiency;
    [SerializeField] TextMeshProUGUI text_name;
    [SerializeField] Image checkmark;
    [SerializeField] Button btn;
    string skill;

    public void InitializeButton(string s)
    {

        text_name.text = s;
        skill = s;
        UpdateValue();
        Debug.Log("Initialized new SkillUI with name " + skill);
    }

    //when clicking a singular skill button, it should switch to the AboutSkill screen and refresh skill subtasks, with this as the currentSkill
    public void OnClickSkillUI()
    {//switch to individual list panel in DDD and update values withthat
        StaticPanelManager.Instance.OnClickSkill(DynamicDataDisplayer.Instance.GetSkill(skill));
    }

    public void UpdateValue()
    {
        int p = DynamicDataDisplayer.Instance.GetSkillProficiency(skill);

        checkmark.color = (p == 100 ? Color.green : Color.white);
        text_proficiency.color = p == 100 ? Color.green : new Color(28, 69, 110);
        text_proficiency.text = p.ToString();
    }

}
