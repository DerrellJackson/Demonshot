using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerHealth;
using static LevelSystem;
using UnityEngine.SceneManagement;


public class NeedsSystemUI : MonoBehaviour
{

 /*   private NeedVisualUI coreImageEnergy;
    private NeedVisualUI coreImageFood;
    private NeedVisualUI coreImageFun;

    private void Awake() 
    {
        coreImageEnergy = new NeedVisualUI (transform.Find("coreImageEnergy"), NeedsSystem.NeedType.Energy);
        coreImageFood = new NeedVisualUI (transform.Find("coreImageHunger"), NeedsSystem.NeedType.Hunger);
        coreImageFun = new NeedVisualUI (transform.Find("coreImageHappiness"), NeedsSystem.NeedType.Happiness);
    }

    public void SetNeedsSystem(NeedsSystem needsSystem)
    {

        coreImageEnergy.SetNeedsSystem(needsSystem);
        coreImageFood.SetNeedsSystem(needsSystem);
        coreImageFun.SetNeedsSystem(needsSystem);

    }*/

    public Image hungerFullImage;
    public Image energyFullImage;
    public Image happinessFullImage;
   

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Image fadeImage;
    public SceneName respawnSceneName;
    public float drainHungerAmount = 1f;
    public float drainEnergyAmount = 1f;
    public float drainHappinessAmount = 1f;
    public float healthDrainBetweenTime = 1f;
    public float drainHealthAmount = 1f;
    private float healthTimer;
    public float drainXPAmount = 1000f;
    private float loseXPTimer;
    public float loseXPResetTimer;
    public GameObject player;

    private bool isHungry = true;
    private bool isTired = true;
    private bool isSad = true;

    private void Start() 
    {
        hungerFullImage.fillAmount = 1f;
        energyFullImage.fillAmount = 1f;
        happinessFullImage.fillAmount = 1f;
        CalculateDrainAmountSpeed();
    }

    private void CalculateDrainAmountSpeed()
    {
        drainHungerAmount = (drainHungerAmount / 100) * 0.001f;
        drainEnergyAmount = (drainEnergyAmount / 100) * 0.001f;
        drainHappinessAmount = (drainHappinessAmount / 100) * 0.001f;
    }

    public void Update()
    {
        DrainNeeds();
    }


    public void DrainNeeds()
    {
        DrainHunger();
        DrainEnergy();
        DrainHappiness();
    }
    

    public void DrainHunger()
    {
        if(isHungry == true)
        {
        hungerFullImage.fillAmount -= drainHungerAmount;
        if(hungerFullImage.fillAmount <= 0f)
        {
            healthTimer += Time.deltaTime;
            if(healthTimer >= healthDrainBetweenTime)
            {
                DrainHealth();
                healthTimer = 0f;
            }
        }
        }
    }

    public void DrainEnergy()
    {
        if(isTired == true)
        {
        energyFullImage.fillAmount -= drainEnergyAmount;
        if(energyFullImage.fillAmount <= 0f)
        {
            PlayerPassedOut();
        }
        }
    }

    public void DrainHappiness()
    {
        if(isSad == true)
        {
        happinessFullImage.fillAmount -= drainHappinessAmount;
        if(happinessFullImage.fillAmount <= 0f)
        {
            PlayerDepressed();
        }
        }
    }

    bool healthCanDrain = false;

    private void DrainHealth()
    {
        
        playerHealth.TakeDamage(drainHealthAmount);
        healthTimer = 0f;
            
    }



    public void PlayerPassedOut()
    {
        hungerFullImage.fillAmount = 0.5f;
        energyFullImage.fillAmount = 0.5f;
        happinessFullImage.fillAmount = 0.5f;
        playerHealth.SplitHealth();
        RespawnPlayer();
    }

    public void PlayerDepressed()
    {
        loseXPTimer += Time.deltaTime;
        if(loseXPTimer >= loseXPResetTimer)
        {
            levelSystem.LoseXP(drainXPAmount);
            loseXPTimer = 0f;
            happinessFullImage.fillAmount = 0.25f;
        }
    }

    public void SetFull()
    {
        isHungry = false;
    }

    public void SetAwake()
    {
        isTired = false;
    }

    public void SetHappy()
    {
        isSad = false;
    }

    private void RespawnPlayer()
    {
        
    }

}
