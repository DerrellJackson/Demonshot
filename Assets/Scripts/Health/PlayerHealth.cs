using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static PlayerHealthPickup;

public class PlayerHealth : MonoBehaviour
{
    
    public float health;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    public static PlayerHealth playerHealth;
    private void Awake() => playerHealth = this;

    void Start() 
    {
        health = maxHealth;
    }

    public void getStartingHealth()
    {
        health = maxHealth;
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(Random.Range(5,10));
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            RestoreHealth(Random.Range(5,10));
        }

        if(health <= 0)
        {
            //teleport the player to the hospital or player bed and make the hp of them go to half and possibly take out a random inv item :p

            health = maxHealth / 2;
        }
    }

    public void UpdateHealthUI()
    {
        //Debug.Log(health);
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        //taking damage
        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        //restoring hp
        if(fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
        healthText.text = Mathf.Round(health) + "         /         " + Mathf.Round(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        if(maxHealth >= health)
        {
        playerHealthPickup.canAddHealth = true;
        health += healAmount;
        lerpTimer = 0f;
        }

    }


    public void IncreaseHealth(int level)
    {

        maxHealth += (health * 0.01f) * ((100 + level) * 0.01f);
        health = maxHealth;

    }

    public void IncreaseHealthBySkill(int increaseAmount)
    {

        maxHealth += increaseAmount;
        health = maxHealth;

    }

    public void SplitHealth()
    {
        health -= maxHealth / 2f;
    }


}
