using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSkillTree : MonoBehaviour
{
 
    public static HealthSkillTree healthSkillTree;
    private void Awake() => healthSkillTree = this;

    [HideInInspector]public int[] SkillLevels;
    [HideInInspector]public int[] SkillCaps;
    [HideInInspector]public string[] SkillNames;
    [HideInInspector]public string[] SkillDescriptions;

    [HideInInspector]public List<HealthSkill> SkillList;
    public GameObject SkillHolder;

    [HideInInspector]public List<GameObject> ConnectorList;
    public GameObject ConnectorHolder;

    [HideInInspector]public int SkillPoint;
    

    private void Start()
    {
        SkillPoint = 10;

        SkillLevels = new int[12]; //number of skills in health rn
        SkillCaps = new[] {1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1}; //names go along w the skill cap


        SkillNames = new[] {"Self Heal", "Less Hungry", "Health Nut", "Vampiric Healing", "Healing", "Better Blood", "Long Dash", "Fast Dash", "Scary Healing", "Attention Seeker", "Energy Drinker", "The Good Stuff",
        };
        SkillDescriptions = new[] //each "," is for the next name of a skill
        {
            " Heal ONE hp every TWO seconds.",
            " You are not as hungry now.",
            " You can run faster now.",
            " You heal yourself when attacking enemies.",
            " Add Twenty HP.",
            " Vampiric heals more HP now.",
            " Dash for longer distances.",
            " Dash Faster to evade attacks.",
            " Heal while in combat.", 
            " Heal faster for each enemy alive.",
            " Any Energy Drink fills full Energy Bar now.",
            " Better Items are now in shop!",
        };

        foreach(var skill in SkillHolder.GetComponentsInChildren<HealthSkill>()) SkillList.Add(skill);
        foreach(var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>()) ConnectorList.Add(connector.gameObject);

        for(var i = 0; i < SkillList.Count; i++) SkillList[i].id = i;

        SkillList[0].ConnectedSkills = new []{1, 2, 3}; //skill list 3 is connected to 4,5 which is why it says SkillList[3]
        SkillList[3].ConnectedSkills = new []{4,5};
        SkillList[2].ConnectedSkills = new []{6};
        SkillList[6].ConnectedSkills = new []{7};
        SkillList[4].ConnectedSkills = new []{10};
        SkillList[10].ConnectedSkills = new []{11};
        SkillList[5].ConnectedSkills = new []{8, 9};

        UpdateAllSkillUI();
    }


    public void AddSkillPoint()
    {
        SkillPoint++;
        UpdateAllSkillUI();
    }


    public void UpdateAllSkillUI()
    {
        foreach (var skill in SkillList) skill.UpdateUI();
    }

}
