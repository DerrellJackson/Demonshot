using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Purse : MonoBehaviour
{

    [SerializeField] float startingMoney = 100f;

    float balance = 0;

    public event Action onChange;

    private void Awake()
    {
        balance = startingMoney;
    }

    public float GetBalance()
    {
        return balance;
    }

    public void UpdateBalance(float amount)
    {
        balance += amount;
        if(onChange != null)
        {
            onChange();
        }
    }

}
