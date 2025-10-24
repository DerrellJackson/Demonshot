using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static InteractionSkillTree;
using static HealthSkillTree;
using static FarmingSkillTree;
using UnityEngine.UI;

public class InteractionSkill : MonoBehaviour
{
    public int id;

    public TMP_Text TitleText;
    public TMP_Text DescriptionText;

    public int[] ConnectedSkills;

    public void UpdateUI()
    {
        TitleText.text = $"{interactionSkillTree.SkillLevels[id]}/{interactionSkillTree.SkillCaps[id]}\n{interactionSkillTree.SkillNames[id]}";
        DescriptionText.text = $"{interactionSkillTree.SkillDescriptions[id]} \nYou Have {interactionSkillTree.SkillPoint}\n Skill Points";
    

        GetComponent<Image>().color = interactionSkillTree.SkillLevels[id] >= interactionSkillTree.SkillCaps[id] ? Color.green 
            : interactionSkillTree.SkillPoint >= 1 ? Color.yellow : Color.magenta;

            foreach(var connectedSkill in ConnectedSkills)
            {
                interactionSkillTree.SkillList[connectedSkill].gameObject.SetActive(interactionSkillTree.SkillLevels[id] > 0);
                interactionSkillTree.ConnectorList[connectedSkill].SetActive(interactionSkillTree.SkillLevels[id] > 0);
            }
    }

    public void Buy()
    {
        if(interactionSkillTree.SkillPoint < 1 || interactionSkillTree.SkillLevels[id] >= interactionSkillTree.SkillCaps[id]) return;
        interactionSkillTree.SkillPoint -= 1;
        healthSkillTree.SkillPoint -= 1;
        farmingSkillTree.SkillPoint -= 1;

        interactionSkillTree.SkillLevels[id]++;
        healthSkillTree.UpdateAllSkillUI();
        interactionSkillTree.UpdateAllSkillUI(); 
        
    }

}
