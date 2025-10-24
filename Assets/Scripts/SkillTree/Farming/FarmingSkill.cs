using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static HealthSkillTree;
using static InteractionSkillTree;
using static FarmingSkillTree;
using UnityEngine.UI;
using static Player;

public class FarmingSkill : MonoBehaviour
{
  
    [HideInInspector]public int id;

    public TMP_Text TitleText;
    public TMP_Text DescriptionText;

    [HideInInspector]public int[] ConnectedSkills;

    public static FarmingSkill farmingSkill;
    private void Awake() => farmingSkill = this;

    MovementByVelocity movementByVelocity;

    void Start()
    {
        movementByVelocity = GetComponent<MovementByVelocity>();
    }

    public void UpdateUI()
    {
        TitleText.text = $"{farmingSkillTree.SkillLevels[id]}/{farmingSkillTree.SkillCaps[id]}\n{farmingSkillTree.SkillNames[id]}";
        DescriptionText.text = $"{farmingSkillTree.SkillDescriptions[id]} \nYou Have {farmingSkillTree.SkillPoint}\n Skill Points";
    

        GetComponent<Image>().color = farmingSkillTree.SkillLevels[id] >= farmingSkillTree.SkillCaps[id] ? Color.green 
            : farmingSkillTree.SkillPoint >= 1 ? Color.yellow : Color.magenta;

            foreach(var connectedSkill in ConnectedSkills)
            {
                farmingSkillTree.SkillList[connectedSkill].gameObject.SetActive(farmingSkillTree.SkillLevels[id] > 0);
                farmingSkillTree.ConnectorList[connectedSkill].SetActive(farmingSkillTree.SkillLevels[id] > 0);
            }
    }

    public void Buy()
    {
        if(farmingSkillTree.SkillPoint < 1 || farmingSkillTree.SkillLevels[id] >= farmingSkillTree.SkillCaps[id]) return;
        healthSkillTree.SkillPoint -= 1;
        interactionSkillTree.SkillPoint -= 1;
        farmingSkillTree.SkillPoint -= 1;
       

        farmingSkillTree.SkillLevels[id]++;
        healthSkillTree.UpdateAllSkillUI();
        interactionSkillTree.UpdateAllSkillUI(); 
        farmingSkillTree.UpdateAllSkillUI();
        
    }


}
