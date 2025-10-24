using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static HealthSkillTree;
using static InteractionSkillTree;
using static FarmingSkillTree;
using UnityEngine.UI;
using static Player;
using static PlayerHealth;

public class HealthSkill : MonoBehaviour
{

    [HideInInspector]public int id;

    public TMP_Text TitleText;
    public TMP_Text DescriptionText;

    [HideInInspector]public int[] ConnectedSkills;

    public static HealthSkill healthSkill;
    private void Awake() => healthSkill = this;

    MovementByVelocity movementByVelocity;

    void Start()
    {
        movementByVelocity = GetComponent<MovementByVelocity>();
    }

    public void UpdateUI()
    {
        TitleText.text = $"{healthSkillTree.SkillLevels[id]}/{healthSkillTree.SkillCaps[id]}\n{healthSkillTree.SkillNames[id]}";
        DescriptionText.text = $"{healthSkillTree.SkillDescriptions[id]} \nYou Have {healthSkillTree.SkillPoint}\n Skill Points";
    

        GetComponent<Image>().color = healthSkillTree.SkillLevels[id] >= healthSkillTree.SkillCaps[id] ? Color.green 
            : healthSkillTree.SkillPoint >= 1 ? Color.yellow : Color.magenta;

            foreach(var connectedSkill in ConnectedSkills)
            {
                healthSkillTree.SkillList[connectedSkill].gameObject.SetActive(healthSkillTree.SkillLevels[id] > 0);
                healthSkillTree.ConnectorList[connectedSkill].SetActive(healthSkillTree.SkillLevels[id] > 0);
            }
    }

    public void Buy()
    {
        if(healthSkillTree.SkillPoint < 1 || healthSkillTree.SkillLevels[id] >= healthSkillTree.SkillCaps[id]) return;
        healthSkillTree.SkillPoint -= 1;
        interactionSkillTree.SkillPoint -= 1;
        farmingSkillTree.SkillPoint -= 1;
       

        healthSkillTree.SkillLevels[id]++;
        healthSkillTree.UpdateAllSkillUI();
        interactionSkillTree.UpdateAllSkillUI(); 
        farmingSkillTree.UpdateAllSkillUI();
        
    }



    //HP REGEN 1\\
    public bool UnlockHealthRegen()
    {
        if(healthSkillTree.SkillPoint < 1 || healthSkillTree.SkillLevels[id] >= healthSkillTree.SkillCaps[id]) return false;
        else   
        return true;
    }
  
    private float hptimerIncrease;

    public void SelfHeal()
    {   
        if(UnlockHealthRegen() == true)
        {
            hptimerIncrease += Time.deltaTime;
            if(hptimerIncrease >= 2f)
            {
            playerHealth.RestoreHealth(1);
            hptimerIncrease = 0f;
            }
        }
        else return;
    
    }
    //HP REGEN 1\\


    //SPEED INCREASE\\
    /*
    public bool UnlockSpeedIncrease()
    {
        if(healthSkillTree.SkillPoint < 1 || healthSkillTree.SkillLevels[id] >= healthSkillTree.SkillCaps[id]) return false;   
        return true;
    }
 
    public void IncreasedSpeed()
    {   
        if(UnlockSpeedIncrease() == true)
        {
        movementByVelocity.increaseSpeed(1000);
        }
        else return;
    }*/
    //SPEED INCREASE\\

    //HP HEAL INCREASE\\
    bool isMax = false;
    public int HealthIncreaseAmount;
    public bool UnlockHealthIncrease()
    {
        if(healthSkillTree.SkillPoint < 1 || healthSkillTree.SkillLevels[id] >= healthSkillTree.SkillCaps[id]) return false;
        else   
        playerHealth.IncreaseHealthBySkill(HealthIncreaseAmount / 2); //dunno why but it for some reason adds double so I had to split it in half
        return true;
    }
    
    public void IncreasedHealth()
    {      

        UnlockHealthIncrease();
        if(!UnlockHealthIncrease() && !isMax)
        {   
        playerHealth.IncreaseHealthBySkill(HealthIncreaseAmount);
        isMax = true;
        }
    
        return;
    
    }
    //HP HEAL INCREASE\\



}
