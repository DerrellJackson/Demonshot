using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
   
   #region Header References
   [Space(10)]
   [Header("References")]
   #endregion

   #region Tooltip
   [Tooltip("Populate with the HealthBarComponent on the HealthBar gameObject")]
   #endregion
   [SerializeField] private HealthBar healthBar;

   #region Tooltip
   [Tooltip("Check if the health component is meant for an object with a health bar")]
   #endregion
   [SerializeField] private bool isAnObject;

    private int playerHealthCap = 100;
    private int startingHealth;
    private int currentHealth;
    private HealthEvent healthEvent;
    private Player player;
    private Coroutine immunityCoroutine;
    private bool isImmuneAfterHit = false;
    private float immunityTime = 0f;
    private SpriteRenderer spriteRenderer = null;
    private const float spriteFlashInterval = 0.2f;
    private WaitForSeconds WaitForSecondsSpriteFlashInterval = new WaitForSeconds(spriteFlashInterval);

    [HideInInspector] public bool isDamageable = true;
    [HideInInspector] public Enemy enemy;


    private void Awake() 
    {

        //load components
        healthEvent = GetComponent<HealthEvent>();

    }


    private void Start() 
    {

        //trigger a health event for UI update
        CallHealthEvent(0);

        GetStartingHealth();
        
        //attempt to load enemy / player components
        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();

        //get player / enemy hit immunity details
        if (player != null)
        {
           
                isImmuneAfterHit = true;
              //  immunityTime = player.hitImmunityTime;
                spriteRenderer = player.spriteRenderer;
            
        }
        else if (enemy != null)
        {   
          
                isImmuneAfterHit = true;
                //immunityTime = enemy.enemyDetails.hitImmunityTime;
                spriteRenderer = enemy.spriteRendererArray[0];
           
        } 


        //Enable the health bar if required and object
        if(enemy != null && healthBar != null /*&& enemy.enemyDetails.isHealthBarDisplayed == true && healthBar != null */|| (isAnObject = true  && healthBar != null) )
        {
            healthBar.EnableHealthBar();
            
        }
        else if (healthBar != null)
        {
            healthBar.DisableHealthBar();
        }

    }


    //public method called when damage is taken
    public void TakeDamage(int damageAmount)
    {
    
        bool isDashing = false;

        if(player != null)
        isDashing = player.playerControl.isPlayerDashing;

        if(isDamageable && !isDashing)
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);

            PostHitImmunity();

            //set health had as the percentage remaining
            if(healthBar != null)
            {
                healthBar.SetHealthBarValue((float)currentHealth / (float)startingHealth);
            }
        }

    }


    //indicate a hit and give some post hit immunity
    private void PostHitImmunity()
    {

        //check if gameobject is active and if it is not then returns
        if(gameObject.activeSelf == false) 
        return;

        //check if there even is post hit immunity
        if(isImmuneAfterHit)
        {
            if(immunityCoroutine != null)
                StopCoroutine(immunityCoroutine);

            //flash red and give a period of immunity
            immunityCoroutine = StartCoroutine(PostHitImmunityRoutine(immunityTime, spriteRenderer));
        }

    }

    
    //coroutine to indicate a hit and give some post hit immunity
    private IEnumerator PostHitImmunityRoutine(float immunityTime, SpriteRenderer spriteRenderer)
    {

        int iterations = Mathf.RoundToInt(immunityTime / spriteFlashInterval / 2f);

        isDamageable = false;

        while(iterations > 0)
        {
        
            //need to add code to make player flash when hit. will prob need to edit the playerdetailsSO first

            yield return WaitForSecondsSpriteFlashInterval;

            //need to add code to make player flash when hit. will prob need to edit the playerdetailsSO first

            yield return WaitForSecondsSpriteFlashInterval;

            iterations--;

            yield return null;

        }

        isDamageable = true;

    }


    private void CallHealthEvent(int damageAmount)
    {

        //trigger health event
        healthEvent.CallHealthChangedEvent(((float)currentHealth / (float)startingHealth), currentHealth, damageAmount);

    }


    //set starting health
    public void SetStartingHealth(int startingHealth)
    {
      
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
      
    }


    //get starting health
    public int GetStartingHealth()
    {
        return startingHealth;
    }


    //increase health by specified amount 
    public void IncreaseTotalHealth(int healthAmount)
    {
       
        int healthIncrease = Mathf.RoundToInt((startingHealth + healthAmount));

        int totalHealth = currentHealth + healthIncrease;

        CallHealthEvent(0);

        if(totalHealth > playerHealthCap) //just change starting health to a number that it should be capped at
        { 
            currentHealth = playerHealthCap; 
        }
        else
        { 
            currentHealth = totalHealth;
        }

    }


    //this is to add health back to the full amount, the above lines are for increase total health
    public void AddHealth(int healthPercent)
    {

        int healthIncrease = Mathf.RoundToInt((startingHealth * healthPercent) / 100f);

        int totalHealth = currentHealth + healthIncrease;

        CallHealthEvent(0);

        //if I want to cap the health 
        if(totalHealth > startingHealth)
        { 
            currentHealth = startingHealth; 
        }
        else
        { 
            currentHealth = totalHealth;
        }

    }

}
