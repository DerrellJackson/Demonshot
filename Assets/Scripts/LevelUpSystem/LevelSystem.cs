using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static HealthSkillTree;
using static InteractionSkillTree;
using static FarmingSkillTree;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem levelSystem;
    private void Awake() => levelSystem = this;
    
    public int level;
    public float currentXp;
    public float requiredXp;

    private float lerpTimer;
    private float delayTimer;

    [Header("UI")] 
    public Image frontXpBar;
    public Image backXpBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    public TextMeshProUGUI levelUpText;
    public TextMeshProUGUI skillPointText;
    public Image levelUpImage;
    public Image skillPointImage;

    [Header("Multipliers")]
    [Range (1f, 300f)] public float additionMultiplier = 300;
    [Range(2f, 4f)] public float powerMultiplier = 2;
    [Range (7f, 14f)]public float divisionMultiplier = 7;

    void Start() 
    {
        levelUpImage.fillAmount = 0f;
        skillPointImage.fillAmount = 0f;
        frontXpBar.fillAmount = currentXp / requiredXp;
        backXpBar.fillAmount = currentXp / requiredXp;
        backXpBar.color = Color.yellow;
        requiredXp = CalculateRequiredXp();
        levelText.text = "Level " + level;
        levelUpText.text = " ";
        skillPointText.text = " ";

    }


    void Update() 
    {

        UpdateXpUI();
        if(Input.GetKeyDown(KeyCode.Equals))
        GainExperienceFlatRate(1000);
        if(currentXp > requiredXp)
        {
            LevelUp();
        }

    }


    public void UpdateXpUI()
    {

        float xpFraction = currentXp / requiredXp;
        float FXP = frontXpBar.fillAmount; 
        if(FXP < xpFraction)
        {
            delayTimer += Time.deltaTime;
            backXpBar.fillAmount = xpFraction;
            if(delayTimer > 5)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4;
                frontXpBar.fillAmount = Mathf.Lerp(FXP, backXpBar.fillAmount, percentComplete);
                RemoveLevelUpText();
            }
        }
        xpText.text = currentXp + "/" + requiredXp;
        if(level >= 100)
        {
        xpText.text = "MAX"; 
        delayTimer += Time.deltaTime;
        if(delayTimer > 8)
        {
        RemoveLevelUpText();
        }
        }

    }


    public void GainExperienceFlatRate(float xpGained)
    {

        currentXp += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;

    }

    
    public void GainExperienceScalable(float xpGained, int passedLevel)
    {

        if(passedLevel < level)
        {
            float multiplier = 1 + (level - passedLevel) * 0.1f;
            currentXp += xpGained * multiplier;
        }
        else 
        {
            currentXp += xpGained;
        }
        lerpTimer = 0f;
        delayTimer = 0f;

    }

    bool isMaxLevel = false;
    public void LevelUp()
    {
        if(!isMaxLevel)
        {
        healthSkillTree.AddSkillPoint();
        interactionSkillTree.AddSkillPoint();
        farmingSkillTree.AddSkillPoint();

        level++;
        ShowLevelUpText();
        frontXpBar.fillAmount = 0f;
        backXpBar.fillAmount = 0f;
        currentXp = Mathf.RoundToInt(currentXp - requiredXp);
        GetComponent<PlayerHealth>().IncreaseHealth(level);
        requiredXp = CalculateRequiredXp();
        levelText.text = "Level " + level;
        }
        if(level >= 100f)
        {
            levelText.text = "MAX";
            frontXpBar.fillAmount = 100f;
            backXpBar.fillAmount = 100f;
            isMaxLevel = true;
        }

    }


    private int CalculateRequiredXp()
    {

        int solveForRequiredXp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 5;

    }


    private void ShowLevelUpText()
    {

        if(level < 100)
        {
           levelUpText.text = "You have just leveled up to " + level + ". " + "\nYou need\n " + requiredXp + " xp" + "\nto level up to the next level!";
           levelUpImage.fillAmount = 100f;
           skillPointText.text = "You gained a skill point";
           skillPointImage.fillAmount = 100f;
        }
        else if(level >= 100)
        {
            levelUpText.text = "You have just reached MAX level. Gain more skill points through quests and other ways! \nLEVEL 100 REACHED";
            levelUpImage.fillAmount = 100f;
            skillPointText.text = "Last Level Up Point.";
            skillPointImage.fillAmount = 100f;
        }

    }

    private void RemoveLevelUpText()
    {
        
        levelUpText.text = " ";
        levelUpImage.fillAmount = 0f;
        skillPointText.text = " ";
        skillPointImage.fillAmount = 0f;

    }

    public void LoseXP(float xpLost)
    {   
        lerpTimer = 5f;
        if(currentXp >= xpLost)
        {
        currentXp -= xpLost;
        frontXpBar.fillAmount = currentXp / requiredXp;
        StartCoroutine(DecreaseYellow());
        }
        else if (currentXp <= xpLost)
        {
            currentXp = 0f;
        frontXpBar.fillAmount = 0f;
        StartCoroutine(DecreaseYellow());
        }
        UpdateXpUI();
    
    }

    IEnumerator DecreaseYellow()
    {
        backXpBar.color = Color.red;
        yield return new WaitForSeconds(0.75f);
        backXpBar.fillAmount = currentXp / requiredXp;
        backXpBar.color = Color.yellow;
        //this remove level up text goes here to fix a bug of it not ever getting removed
        RemoveLevelUpText();

    }

    
}
