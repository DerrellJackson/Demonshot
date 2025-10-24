using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class MeleeScript : AllEnemyScript
{
  public float stopDistance;

  private float attackTime;

  public float attackSpeed;

  private void Update() 
  {//the 'player' written down below is from the "AllEnemyScript"
  if(PlayerOldScript.instance.gameObject.activeInHierarchy){
    if(player !=null)
    //if we are too far away from the player it will chase
    {if(Vector2.Distance(transform.position, player.position) > stopDistance)
    //we are making it MoveTowards the player and we are doing it by: (stating current position, our target position, and then our speed)
    {transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);}
    else
    {
     if(Time.time >= attackTime)  
     {  StartCoroutine(Attack());
        attackTime = Time.time + timeBetweenAttacks;
     }  
    }
    }
    }
    }
    IEnumerator Attack()
    //this is saying how the player will take "x" amount of damage
    {player.GetComponent<PlayerHealthController>().TakeDamage(damage);
     //this is how we call back to our position before the enemy attack
     Vector2 originalPosition = transform.position;
     //this is where the enemy goes to attack
     Vector2 targetPosition = player.position;
     //when the animation is just starting it is at 0 but finishes at 1
     float percent = 0;
     while (percent <= 1)
     {percent += Time.deltaTime * attackSpeed;
     //this enables us to go to the target position and come back
     float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
     transform.position = Vector2.Lerp(originalPosition, targetPosition, formula);
     yield return null;
     }
     }
}

*/