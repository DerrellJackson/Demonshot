using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerHealth;

public class PlayerHealthPickup : MonoBehaviour
{

    [HideInInspector]public bool canAddHealth = false;
    public bool canTakeHealth = false;
    public float healAmount;

    public static PlayerHealthPickup playerHealthPickup;
    private void Awake() => playerHealthPickup = this;

    private void OnTriggerEnter2D(Collider2D other) 
    {

        if(other.tag == "Player")
        {

            
            if(canAddHealth == true || canTakeHealth == true)
            {
                playerHealth.RestoreHealth(healAmount);
                Destroy(gameObject);
            }
            else 
            {
                return;
            }
            
        }

    } 

 
}
