using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Shopping.Shops;

public class PurseUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI balanceInShop;
    [SerializeField] TextMeshProUGUI balanceInInventory;

    Purse playerPurse = null;

    void Start()
    {
        
        playerPurse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();

        if(playerPurse != null)
        {
            playerPurse.onChange += RefreshUI;
        }

        RefreshUI();
    }

    private void RefreshUI()
    {

        balanceInInventory.text = $"${playerPurse.GetBalance():N2}";
        balanceInShop.text = $"${playerPurse.GetBalance():N2}";

    }

}
