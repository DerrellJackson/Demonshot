using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
  public class Key : MonoBehaviour


  { [SerializeField] int keysPickup = 1;

//This Fixes Double Pickups
  bool wasCollected = false;
   
     void OnTriggerEnter2D(Collider2D other)
    { if(other.tag == "Player" && !wasCollected)
        { 
          wasCollected = true;
           FindObjectOfType<UI_Scripts>().AddToKeys(keysPickup);
          gameObject.SetActive(false);
          Destroy(gameObject); }
              
    }
    
  }
