using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeedsSystem : MonoBehaviour
{

    private const float MAX = 100f;

    public enum NeedType 
    {

        Hunger,
        Energy, 
        Happiness,

    }
    
    private float fillAmount;
    private float totalFillAmount;
    private float currentAmount;


    public void startNeed()
    {

        totalFillAmount = 250f;
        fillAmount = 100f;
        currentAmount = fillAmount;
        //CalculateRegenTimerMax();
        SetTotalFillAmount(totalFillAmount);

    }

    public void RefillAllNeeds()
    {
        fillAmount = totalFillAmount;
    }


    public void DrainNeed(float useAmount)
    {

            fillAmount -= useAmount;
            //lastUseTime = Time.time;
            Debug.Log("fillAmount: " + fillAmount);
            //OnValuesChanged?.Invoke(this, EventArgs.Empty);
        
    }

    public void DrainHunger(float useAmount)
    {
        useAmount = 1f;
        DrainNeed(useAmount);
    }

    public void FillNeed(float useAmount)
    {
        currentAmount += useAmount + currentAmount;
    }

    public float GetRingNormalizedValue()
    {

        return fillAmount / MAX;

    }


    public float GetTotalFillNormalizedValue()
    {

        return totalFillAmount / MAX;

    }


    public float GetCurrentAmountValue()
    {

        return currentAmount / MAX;

    }


    /*private void CalculateRegenTimerMax()
    {

        float baseRespawnTimer = 1f;
        float penaltyRespawnTimer = 1f;
        regenTimerMax = baseRespawnTimer + penaltyRespawnTimer * (1f - GetCoreNormalizedValue());

    }


    public void RegenTick() 
    {

        float regenSpeed = 5f;
        fillAmount += regenSpeed;
        fillAmount = Mathf.Clamp(fillAmount, 0f, totalFillAmount);
        OnValuesChanged?.Invoke(this, EventArgs.Empty);

    }


    private void HandleRegen()
    {

        if(CanRegen())
        {
            regenTimer -= Time.deltaTime;
            while (regenTimer < 0f)
            {
                regenTimer += regenTimerMax;
                RegenTick();
            }
        }

    }*/


    public void Update()
    {

        //HandleRegen();

    }


    /*private bool CanRegen()
    {
    
        float regenUseDelay = .2f;
        return Time.time >= lastUseTime + .2f;

    }*/


    public void SetTotalFillAmount(float totalFillAmount)
    {

        this.totalFillAmount = totalFillAmount;
        fillAmount = Mathf.Clamp(fillAmount, 0f, totalFillAmount);
        //OnValuesChanged?.Invoke(this, EventArgs.Empty);

    }


    public float GetTotalFillAmount()
    {

        return totalFillAmount;

    }

}



