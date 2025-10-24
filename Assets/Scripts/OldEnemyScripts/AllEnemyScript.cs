using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
//Note that this script is used for all other enemies and I use it by changing the 'MonoBehaviour' to 'AllEnemyScript'
//This script it now the holder of all enemy scripts to fix issues with melee!
//ALSO NEED TO REMEMBER TO CHANGE HEAL POPUPS COLOR AND ADD A CRIT POPUP + COLOR

public class AllEnemyScript : MonoBehaviour
{
  public static AllEnemyScript instance;

  public int health;

  public int healAmount;//forvampires
  
  [HideInInspector]
  public Transform player;

  public float speed;

  public float timeBetweenAttacks;

  public int damage;
  
  //the chance for all enemies to drop some sort of loot.. probably want coins or something 0=NONE 100=You will get
  public int pickupChance;
  public int healthPickupChance;
  public GameObject healthPickup;
  //make sure to put only certain enemies getting rare items!!!
  public GameObject[] pickups;

//The deathEffect is a particle while the blood effect is the blood that stays afterwards
  public GameObject deathEffect;
  public GameObject bloodEffect;

  public bool isAMelee;
  public bool isAbleToHitInPlace;
  public bool isAGhost;
  public bool isBlind;
  public bool isANoAimer;
  public bool isAShooter;
  public bool isASummoner;
  public bool isAWanderingSummoner;
  public bool isACharger;
  public bool isARetreater;
  public bool isAnExploder;
  public bool isALeaker;
  public bool isAFarter;
  public bool isATeleporter;
  public bool isAVampire;
  public bool isAbleToDealPoison;
  public bool isAbleToDealFire;
  public bool isAbleToDealBleed;
  public bool isAbleToFreezePlayer;
  public bool isAbleToSlowDownPlayer;
  public bool isAbleToHealWhenNotMoving;
  public bool isValuable;
  public bool isATrick;
  public bool isAminiBoss;
  public bool isABoss;
  public bool isMissingBodyParts;
  public bool hasSecondPhase;
  public bool hasThirdPhase;
  public bool hasASecondLife;
  public bool canPickUpPlayersStuff;
  public bool canPickUpObjects;
  public bool canThrowObjects;
  public bool canFadeOutOfShots;
  public bool canOnlyDieFromMelee;
  public bool canOnlyDieFromProjectiles;
  public bool canKillItselfOnImpact;
  public bool isATestingCharacter;

  //used for melee class
  public float isameleestopDistance;
  [HideInInspector]
  public float isameleeattackTime;
  public float isameleeattackSpeed;

  [HideInInspector]
  public float standingTimer;

  //used for ghost class
  public Rigidbody2D theEnemyRB;
  public float ghostRangeToChasePlayer;
  [HideInInspector]
  public Vector3 ghostMoveDirection;
  [HideInInspector]
  public Animator theAnim;

  //wandering class with stops
  public float wanderingminX;
  public float wanderingmaxX;
  public float wanderingminY;
  public float wanderingmaxY;
  //wandering class without stops
  private bool chooseDir = false;
  private Vector3 randomDir;

  //summons classes
  [HideInInspector]
  public Vector2 targetSummonRandomPosition;
  public float theTimeBetweenSummons;
  [HideInInspector] 
  public float theSummonTime;
  

  public GameObject theEnemyToSummon;


private void Awake() 
{
  instance = this;
}
//allows it to be changed
  public virtual void Start() 
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;  
    theAnim = GetComponent<Animator>();
    float randomX = Random.Range(wanderingminX, wanderingmaxX);
    float randomY = Random.Range(wanderingminY, wanderingmaxY);
    targetSummonRandomPosition = new Vector2(randomX, randomY);
  }

  public void TakeDamage(int damageAmount) 
  { 
    if(player != null){
    health -= damageAmount;
    DamagePopUp.Create(transform.position, damageAmount);
    if (health <= 0)
    { 
      //how the item drop percent works:  
      int randomNumber = Random.Range(0, 101);
      if (randomNumber < pickupChance)  
      {GameObject randomPickup = pickups[Random.Range(0, pickups.Length)];
       Instantiate (randomPickup, transform.position, transform.rotation);}
       
       int randHealth = Random.Range(0, 101);
       if (randHealth < healthPickupChance)
       {Instantiate(healthPickup, transform.position, transform.rotation);}
        Instantiate(deathEffect, transform.position, Quaternion.identity);
         Instantiate(bloodEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }}
    else {return;}
  }
  private void Update() 
  {
   
   if(isAMelee)//hits player then goes back to the spot it started hit from, mixable with many
   {
    if(PlayerOldScript.instance.gameObject.activeInHierarchy){
    if(player !=null)
    //if we are too far away from the player it will chase
    {if(Vector2.Distance(transform.position, player.position) > isameleestopDistance)
    //we are making it MoveTowards the player and we are doing it by: (stating current position, our target position, and then our speed)
    {transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);}
    else
    {
     if(Time.time >= isameleeattackTime)  
     {  StartCoroutine(isaMeleeAttack());
        isameleeattackTime = Time.time + timeBetweenAttacks;
        //may want to test this line bc enemies can heal more than max health and they may be being healed twice
         if(isAVampire)
    { health += healAmount;
    DamagePopUp.Create(transform.position, healAmount);}
     }  
    }
    }
    }
   }
  

  if(isAbleToHitInPlace)
  {
    if(Vector2.Distance(transform.position, player.position) < isameleestopDistance)
    //we are making it MoveTowards the player and we are doing it by: (stating current position, our target position, and then our speed)
    {if(Time.time > standingTimer)
    {standingTimer = Time.time + timeBetweenAttacks;
     StartCoroutine(isaMeleeAttack());}}
    
  }

  if(isAGhost)//can travel past anything, mixable
  {
    if(player != null)
        {
        if(Vector3.Distance(transform.position, player.transform.position) < ghostRangeToChasePlayer)
        {
        ghostMoveDirection = player.transform.position - transform.position;
        }
        else
        {// may wanna make a script without this line so the enemy continues to move off in space
        ghostMoveDirection = Vector3.zero;
        }
        ghostMoveDirection.Normalize();

        theEnemyRB.velocity = ghostMoveDirection * speed;

        if(ghostMoveDirection != Vector3.zero)
        {
        theAnim.SetBool("isMoving", true);
        }
        else
        {
        theAnim.SetBool("isMoving", false);
        }
        }

  }
  
  if(isBlind)//travels randomly, do not mix with melee
  {
      if(player != null)
    { Wander();}
  }

  if(isANoAimer)//shoots random directions, mixable with all except other shooter classes
  {

  }

  if(isAShooter)//fires at player, mixable
  {

  }
  
  if(isASummoner)//summons weaker enemies, not mixable with melee, may wanna make a melee summoner class
  {

  }
  
  if(isAWanderingSummoner)
  {
     if(player == null){return;}
    if (Vector2.Distance(transform.position, targetSummonRandomPosition) > .3f)
    {transform.position = Vector2.MoveTowards(transform.position, targetSummonRandomPosition, speed * Time.deltaTime);
    theAnim.SetBool("isRunning", true);}
    
    //this is if the enemy reached its location
    else{
    theAnim.SetBool("isRunning", false);
    if(Time.time >= theSummonTime)
    {theSummonTime = Time.time + theTimeBetweenSummons;
     theAnim.SetTrigger("Summon");
     }
    }
  }


  if(isACharger)//charges at player, do not mix with melee
  {

  }
  
  if(isARetreater)//runs from player, do not mix with melee or targeting classes except for shooter classes
  {

  }

  if(isAnExploder)//explodes on death, mixable
  {

  }

  if(isALeaker)//drops substance while it walks which can kill player, mixable
  {

  }
  
  if(isAFarter)//deals knockback when the player gets close, mixable
  {

  }

  if(isATeleporter)//teleports in seperate sections on the map, mixable
  {

  }
  
  if(isAVampire)//heals self when it damages player, mixable
  {
    //this line is put in melee class, needs to be tested more + needs to go into other melee classes still
  }
  
  if(isAbleToDealPoison)//deals a poison damage overtime on player, mixable
  {

  }

  if(isAbleToDealFire)//deals a burn effect on the player, mixable
  {

  }

  if(isAbleToDealBleed)//deals a bleeding effect on the player, mixable
  {

  }

  if(isAbleToFreezePlayer)//freezes the player on hits, mixable
  {

  }

  if(isAbleToSlowDownPlayer)//slows down the player on hits, mixable
  {

  }
  
  if(isAbleToHealWhenNotMoving)//heals when not in combat, mixable but will be iffy with scripts that head towards player
  {

  }
  
  if(isValuable)//drops currency upon being hit, mixable
  {

  }
  
  if(isATrick)//will vanish after spawned in to scare the player with a strong enemy, no need to mix
  {

  }
  
  if(isAminiBoss)//has a miniboss bar, mixable
  {

  }

  if(isABoss)//has a boss bar, mixable
  {

  }
  
  if(isMissingBodyParts)//occasionally body parts can fall off, mixable
  {

  }
  
  if(hasSecondPhase)//second phase at half health, mixable
  {

  }

  if(hasThirdPhase)//has three phases that swithc every third of health, mixable
  {

  }

  if(hasASecondLife)//will respawn a second time, mixable
  {

  }

  if(canPickUpPlayersStuff)//can pick up weapons and items the player does not pick up, mixable
  {

  }

  if(canPickUpObjects)//picks up objects that are usually in the way, mixable
  {

  }

  if(canThrowObjects)//can throw picked up objects at the player, mixable but requires canPickUpObjects to be true OR needs to spawn in holding an object
  {

  }

  if(canFadeOutOfShots)//can vanish randomly so they do not take damage, mixable
  {

  }

  if(canOnlyDieFromMelee)//projectiles or any other form of damage do not work, mixable
  {

  }

  if(canOnlyDieFromProjectiles)//melee or any other form of damage do not work, mixable
  {

  }

  if(canKillItselfOnImpact)//runs at the player and dies, only mixable with shooter classes
  {

  }

  if(isATestingCharacter)//respawns on death - deals no damage - can not fully die, mixable
  {

  }

  }

   IEnumerator isaMeleeAttack()
    //this is saying how the player will take "x" amount of damage
    {player.GetComponent<PlayerHealthController>().TakeDamage(damage);
    if(isAVampire)
    { health += healAmount;
    DamagePopUp.Create(transform.position, healAmount);}
     //this is how we call back to our position before the enemy attack
     Vector2 originalPosition = transform.position;
     //this is where the enemy goes to attack
     Vector2 targetPosition = player.position;
     //when the animation is just starting it is at 0 but finishes at 1
     float percent = 0;
     while (percent <= 1)
     {percent += Time.deltaTime * isameleeattackSpeed;
     //this enables us to go to the target position and come back
     float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
     transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
     yield return null;
     }
     }


    private void OnTriggerStay2D(Collider2D collision) 
 
    {  
      if(collision.tag == "Player"){
      StartCoroutine(ghostAttack()); }
    }   
    
  
    IEnumerator ghostAttack()
    {player.GetComponent<PlayerHealthController>().TakeDamage(damage);
    yield return null;
  }
  

  private void OnTriggerEnter2D(Collider2D collision) {
    if(collision.tag == "Player"){
    collision.GetComponent<PlayerHealthController>().TakeDamage(damage);
   }
   }
  
  public void Summon(){
    if(player != null){
    Instantiate(theEnemyToSummon, transform.position, transform.rotation);
    }
    } 

  private IEnumerator ChooseDirection()
  {
    chooseDir = true;
    yield return new WaitForSeconds(Random.Range(2f, 8f));
    randomDir = new Vector3(0, 0, Random.Range(0, 360));
    Quaternion nextRotation = Quaternion.Euler(randomDir);
    transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
    chooseDir = false;
  } 

  void Wander()
    {
      if(player != null)
      {
        if(!chooseDir)
        {
          StartCoroutine(ChooseDirection());
        }
        
        transform.position += -transform.right * speed * Time.deltaTime;
      }
    }
}
*/