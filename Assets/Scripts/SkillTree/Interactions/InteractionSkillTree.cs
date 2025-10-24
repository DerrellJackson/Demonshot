using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSkillTree : MonoBehaviour
{

    public static InteractionSkillTree interactionSkillTree;
    private void Awake() => interactionSkillTree = this;

    [HideInInspector]public int[] SkillLevels;
    [HideInInspector]public int[] SkillCaps;
    [HideInInspector]public string[] SkillNames;
    [HideInInspector]public string[] SkillDescriptions;

    [HideInInspector]public List<InteractionSkill> SkillList;
    public GameObject SkillHolder;

    [HideInInspector]public List<GameObject> ConnectorList;
    public GameObject ConnectorHolder;

    [HideInInspector]public int SkillPoint;
    

    private void Start()
    {
        SkillPoint = 10;

        SkillLevels = new int[12]; //number of skills in health rn
        SkillCaps = new[] {1, 1, 3, 1, 1, 3, 1, 1, 1, 1, 1, 1}; //names go along w the skill cap


        SkillNames = new[] {"Good Impression", "Overheard", "Poor But Not", "Long Rod", "Happy Drinker", "Thrift Store",
          "Feels Bad", "Lightweight", "Regenerating Potions", "Smooth Talker", "Extremely Rich", "Happy",
        };
        SkillDescriptions = new[] //each "," is for the next name of a skill
        {
            " Talking to others provides hearts once a day.",//good impression
            " Everyone likes you by one heart because they thought you said something nice.",//overheard
            " Shops think you are poor so have decreased the price of things for just you.",//poor but not
            " You catch new fish as well as large fish now.", //long rod
            " Drinking before talking to people provides an extra heart.",//happy drinker
            " Adds New Cheap Clothes into the shop.", //thrift store 
            " Shops will pay you more for fish now because they think you are poor.",//feels bad 
            " Drinking potions provides a small time boost now.",//lightweight
            " Potions will now provide a slight regen temporarily!",//regenerating potions
            " Talking provides XP now!",//smooth talker
            " People pay you more for the items you sell.",//extemely rich
            " Joy never drains.",//happy
        };

        foreach(var skill in SkillHolder.GetComponentsInChildren<InteractionSkill>()) SkillList.Add(skill);
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
