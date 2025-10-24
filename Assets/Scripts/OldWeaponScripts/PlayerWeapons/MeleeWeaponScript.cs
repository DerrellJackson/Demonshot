using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class MeleeWeaponScript : MonoBehaviour
{
   private float timeBetweenAttacks;
   public float startTimeBetweenAttack;

   public Transform attackPos;
   public float attackRange;
   public LayerMask whatIsEnemies;
   public int damageAmount;

  Animator cameraAnim;
  Animator playerAnim;

  private void Start() 
  {
     cameraAnim = Camera.main.GetComponent<Animator>();
     playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>(); 
  }

   private void Update() {
    if(timeBetweenAttacks <= 0)
    {
      //then attack
      if(Input.GetMouseButtonDown(1))
      { 
         playerAnim.SetTrigger("isMelee");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
          
          cameraAnim.SetTrigger("shake");
          enemiesToDamage[i].GetComponent<AllEnemyScript>().TakeDamage(damageAmount);
        }
      
      timeBetweenAttacks = startTimeBetweenAttack;
      
      } 
    } 
    else 
    {
      timeBetweenAttacks -= Time.deltaTime;
    }
   }
   
   private void OnDrawGizmosSelected() {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(attackPos.position, attackRange);
   }
   }
   
  */  