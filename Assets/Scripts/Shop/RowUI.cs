using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Shopping.Shops 
{
    public class RowUI : MonoBehaviour
    {
        
        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] Image itemImage;
       // [SerializeField] TextMeshProUGUI quantityInShop;
        [SerializeField] TextMeshProUGUI itemPrice;
        [SerializeField] TextMeshProUGUI amountBeingPurchased;

        Shop currentShop = null;
        ItemDetails item = null;

        public void Setup(Shop currentShop, ItemDetails item)
        {
            this.currentShop = currentShop;
            this.item = item;
            itemName.text = item.GetName();
            itemImage.sprite = item.GetSprite();
        //    quantityInShop.text = $"{item.GetQuantity()} remaining";
            itemPrice.text = $"${item.GetPrice():N2}";
            amountBeingPurchased.text = $"{item.GetQuantityInTransaction()}";
        }

        public void Add()
        {
            currentShop.AddToTransaction(item.AddItem(), 1);
        }

        public void Remove()
        {
            currentShop.AddToTransaction(item.AddItem(), -1);
        }

        
        

    }
}