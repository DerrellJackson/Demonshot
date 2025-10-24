using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class AllBossScript : MonoBehaviour
{
   public int health;
   public AllEnemyScript[] enemies;
   public float spawnOffset;
   public int damage;
  public float timeBetweenSummonsAfterHit;
    private float summonTime;

   private int halfHealth;
   private Animator anim;
   public int pickupChance;
  public int healthPickupChance;
  public GameObject healthPickup;
  private Slider bossHealthBar;
  public GameObject[] pickups;

  public GameObject deathEffect;
  public GameObject bloodEffect;

   private void Start() {
      //may have to add more to this line if I add enemies with own health idk
      bossHealthBar = FindObjectOfType<Slider>();
      bossHealthBar.maxValue = health;
      bossHealthBar.value = health;
      halfHealth = health / 2;
      anim = GetComponent<Animator>();
   }
   public void TakeDamage(int damageAmount) 
  {
    health -= damageAmount;
    bossHealthBar.value = health;
    if (health <= 0)
    {
        int randomNumber = Random.Range(0, 101);
      if (randomNumber < pickupChance)  
      {GameObject randomPickup = pickups[Random.Range(0, pickups.Length)];
       Instantiate (randomPickup, transform.position, transform.rotation);}
       
       int randHealth = Random.Range(0, 101);
       if (randHealth < healthPickupChance)
       {Instantiate(healthPickup, transform.position, transform.rotation);}
        Destroy(gameObject);
        bossHealthBar.gameObject.SetActive(false);
         Instantiate(deathEffect, transform.position, Quaternion.identity);
         Instantiate(bloodEffect, transform.position, Quaternion.identity);
    }
    if (health <= halfHealth)
    {
      anim.SetTrigger("stage2");
    }
    if(Time.time >= summonTime)
    {summonTime = Time.time + timeBetweenSummonsAfterHit;
    AllEnemyScript randomEnemy = enemies[Random.Range(0, enemies.Length)];
    Instantiate(randomEnemy, transform.position + new Vector3(spawnOffset, spawnOffset, 0), transform.rotation);}
  }
   private void OnTriggerEnter2D(Collider2D collision) {
    if(collision.tag == "Player"){
    collision.GetComponent<PlayerHealthController>().TakeDamage(damage);
   }
   }
}
*/
