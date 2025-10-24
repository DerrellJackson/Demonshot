using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class SummonerScript : AllEnemyScript 
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    //where he spawns minions
    private Vector2 targetPosition;
    private Animator anim;

    public float timeBetweenSummons;
    private float summonTime;

    public float meleeattackSpeed;
    public float stopDistance;
    private float timer;
    

    public AllEnemyScript enemyToSummon;
//makes it change the start function as desired
    public override void Start()
// this calls the AllEnemyScript
    {base.Start();
    float randomX = Random.Range(minX, maxX);
    float randomY = Random.Range(minY, maxY);
    targetPosition = new Vector2(randomX, randomY);
    anim = GetComponent<Animator>();}
//if the enemy is alive then we want the enemy to walk to a random spot within our choosing
    public void Update() 
    { if(player == null){return;}
    if (Vector2.Distance(transform.position, targetPosition) > .3f)
    {transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    anim.SetBool("isRunning", true);}
    
    //this is if the enemy reached its location
    else{
    anim.SetBool("isRunning", false);
    if(Time.time >= summonTime)
    {summonTime = Time.time + timeBetweenSummons;
     anim.SetTrigger("Summon");
     }
    }
    if(Vector2.Distance(transform.position, player.position) < stopDistance)
    //we are making it MoveTowards the player and we are doing it by: (stating current position, our target position, and then our speed)
    {if(Time.time > timer)
    {timer = Time.time + timeBetweenAttacks;
     StartCoroutine(MeleeAttack());}}
    }
    //how the enemy spawns in
    public void SummonEnemy(){
    if(player != null){
    Instantiate(enemyToSummon, transform.position, transform.rotation);
    }
    } 
     IEnumerator MeleeAttack()
    //this is saying how the player will take "x" amount of damage
    {player.GetComponent<PlayerHealthController>().TakeDamage(damage);
     //this is how we call back to our position before the enemy attack
     Vector2 originalPosition = transform.position;
     //this is where the enemy goes to attack
     Vector2 targetPosition = player.position;
     //when the animation is just starting it is at 0 but finishes at 1
     float percent = 0;
     while (percent <= 1)
     {percent += Time.deltaTime * meleeattackSpeed;
     //this enables us to go to the target position and come back
     float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
     transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
     yield return null;
     }
     }
}
*/