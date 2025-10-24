using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
 
    public float current_mana;
    [HideInInspector] public float mana_drain_amount;
    public float starting_mana = 50f;
    public float mana_max = 100f;
    public float drainTest = 40f;
    public float regenTimer = 0f;
    public float regenResetTimer = 4f;

    public float mana_regen_amount = 20f;

    // void Start()
    // {

    //     current_mana = starting_mana;

    // }

    // void Update()
    // {
    //     UseMana();
    // }

    // void UseMana() //it will need to be changed here with an int of drain amount
    // {

    //     if(current_mana != 0 && Input.GetMouseButtonDown(1)) //&& isUsingAManaWeapon(true))
    //     {
    //         DrainMana(drainTest);
    //     }

    // }

    // public void DrainMana(float drainAmount)
    // {
    //     if(mana_drain_amount != null)
    //     {
    //     Debug.Log("Current Mana: " + (current_mana - drainAmount));
    //     current_mana -= drainAmount; //place current_mana by the 1, this is for testing rn
    //     }
    //     else 
    //     {
    //         Debug.Log("Mana Drain Is Not Functioning Properly!!! Check Mana.cs or look for a script which uses it!!!");
    //     }
    // }

    //     public void IncreaseMana(float increaseAmount)
    // {
    //     if(mana_drain_amount != null)
    //     {
    //     Debug.Log("Current Mana: " + (current_mana + increaseAmount));
    //     current_mana += increaseAmount; //place current_mana by the 1, this is for testing rn
    //     }
    //     else 
    //     {
    //         Debug.Log("Mana Increase Is Not Functioning Properly!!! Check Mana.cs or look for a script which uses it!!!");
    //     }
    // }


    public bool isUsingAManaWeapon()
    {
        //FIND THE PLAYERS CURRENTLY USED WEAPON HERE!!!
        return true;
    }


}
