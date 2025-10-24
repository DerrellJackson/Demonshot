using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//this script is being used for Doors: ITEMS(NOT WEAPONS) : CHESTS etc.
public class Interactables : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    
    private void Update() {
        if(isInRange)//if we are in the collision range
        {
            if(Input.GetKeyDown(interactKey))//the player presses the key
            {
                interactAction.Invoke();//makes it do the function
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
       if(collision.gameObject.CompareTag("Player")) 
       {
        isInRange = true;
        Debug.Log("Is In Range");
       }  
    }
    private void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag("Player")) 
       {
        isInRange = false;
       }  
    }
}
