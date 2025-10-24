using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class RangedScript : AllEnemyScript
{
  public float stopDistance;
  private float attackTime;
  //Remove meleeattackSpeed if I make new script for ranged w/out melee attack
  public float meleeattackSpeed;
  //Remove timer if I make new script for ranged w/out melee attack

   
  private float timer;
  private Animator anim;

  public Transform shotPoint;

  public GameObject enemyBullet;
  
  //override is so it overrides the main enemy start function
  public override void Start() {
    anim = GetComponent<Animator>();
    base.Start();
  }

  private void Update() {
    if(PlayerOldScript.instance.gameObject.activeInHierarchy){
    //if the distance in greater than the stop distance to the player we want the enemy to move towards the player
    {if (Vector2.Distance(transform.position, player.position) > stopDistance)
    {transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);}
    if (Time.time >= attackTime)
    {attackTime = Time.time + timeBetweenAttacks;
     anim.SetTrigger("Attack");}
  }   //THIS IS FOR RANGED UNITS WITH ABILITY TO USE MELEE SO IF I WANT IT DISABLED EITHER PUT STOP DISTANCE AT 0 OR ADD NEW SCRIPT W/OUT BELOW LINES
    if (player !=null){
   if(Vector2.Distance(transform.position, player.position) < stopDistance)
    {if(Time.time > timer)
    {timer = Time.time + timeBetweenAttacks;
     StartCoroutine(MeleeAttack());}}}
}}
  public void RangedAttack()
  {  if(PlayerOldScript.instance.gameObject.activeInHierarchy){
    Vector2 direction = player.position - shotPoint.position;  
        //Rad2Deg converts it to Degrees and this statement is the angle the weapon must rotate around the face
        //of the cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;   
       //Quaternion is a rotation
       //So our quaternion rotation function is the .AngleAxis which we use the (angle) from the float above and on
       //the Z axis or: (Vecotr3.forward) this is used for 2D Games
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //this transform.rotation = rotation means we are just saying the rotation is to the above statement
        shotPoint.rotation = rotation;
        
        Instantiate(enemyBullet, shotPoint.position, shotPoint.rotation);
          }
  }

          //THIS IS FOR RANGED UNITS WITH ABILITY TO USE MELEE SO IF I WANT IT DISABLED EITHER PUT STOP DISTANCE AT 0 OR ADD NEW SCRIPT W/OUT BELOW LINES
           IEnumerator MeleeAttack()
    //this is saying how the player will take "x" amount of damage
    { if (player !=null){
      player.GetComponent<PlayerHealthController>().TakeDamage(damage);
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
}
*/