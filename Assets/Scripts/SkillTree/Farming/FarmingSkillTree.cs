using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingSkillTree : MonoBehaviour
{
 
    public static FarmingSkillTree farmingSkillTree;
    private void Awake() => farmingSkillTree = this;

    [HideInInspector]public int[] SkillLevels;
    [HideInInspector]public int[] SkillCaps;
    [HideInInspector]public string[] SkillNames;
    [HideInInspector]public string[] SkillDescriptions;

    [HideInInspector]public List<FarmingSkill> SkillList;
    public GameObject SkillHolder;

    [HideInInspector]public List<GameObject> ConnectorList;
    public GameObject ConnectorHolder;

    [HideInInspector]public int SkillPoint;
    

    private void Start()
    {
        SkillPoint = 10;

        SkillLevels = new int[12]; //number of skills in health rn
        SkillCaps = new[] {1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1}; //names go along w the skill cap


        SkillNames = new[] {"Born Farmer", "Seed Finder", "Wild Crops", "Crazy Crops", "Insane Crops", "Better Priced Crops", 
        "Worth it", "Easy Growth", "Fun Farmer", "Experienced Farmer", "Top Tier Farmer", "Workaholic",
        };
        SkillDescriptions = new[] //each "," is for the next name of a skill
        {
            " Farming crops does not lose energy!", //born farmer
            " Have a higher chance of digging up seeds!", //seed finder
            " Add more crops into shops!", //wild crops
            " Add some crazy crops into shops!", //crazy crops
            " Your crops can go an extra day without water!", //insane crops
            " Crops are slightly cheaper to buy!", //better priced crops
            " Crops are worth more when selling!", //worth it
            " Crops take a day less now!", //easy growth
            " All crops provide increased joy to you!",//fun farmer 
            " All crops provide slightly more XP now!",//experienced farmer
            " All crops you eat fill up your hunger more!",//top tier farmer
            " You love to farm. It now provides a lot of joy and energy.",//workaholic
        };

        foreach(var skill in SkillHolder.GetComponentsInChildren<FarmingSkill>()) SkillList.Add(skill);
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
