using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static InventoryManager;

namespace Shopping.Shops
{
    
    public class Shop : MonoBehaviour//, IRaycastable
    {   
        [SerializeField] private string shopName;
        [Range(0, 100)][SerializeField] float sellingDiscountPercentage = 80f;

        [SerializeField] StockItemConfig[] stockConfig;

        [System.Serializable] class StockItemConfig
        {
            public Item item;
            public int itemCode;
            public int itemQuantity;

            [Range(-100, 100)] public float buyingDiscountPercentage; // -100 is 100% more, 100 is 100% less

        }


        Dictionary<ItemDetails, int> transaction = new Dictionary<ItemDetails, int>();
        Dictionary<ItemDetails, int> stock = new Dictionary<ItemDetails, int>();
        Shopper currentShopper = null;
        bool isBuyingMode = true;

        public event Action onChange;


        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ItemDetails> GetFilteredItems()
        {
            return GetAllItems();
        }

        public IEnumerable<ItemDetails> GetAllItems()
        {
          foreach(StockItemConfig config in stockConfig)
            {
                float price = GetPrice(config);
                int quantityInTransaction = 0;
                transaction.TryGetValue(InventoryManager.Instance.GetItemDetails(config.itemCode), out quantityInTransaction);
                yield return new ItemDetails(InventoryManager.Instance.GetItemDetails(config.itemCode), config.itemQuantity, price, quantityInTransaction);
            }  
        }

        private int GetAvailability(StockItemConfig config)
        {
            return CountItemsInInventory(config.item);
        }

        private int CountItemsInInventory(Item item)
        {
            InventoryManager inventory = InventoryManager.Instance.GetComponent<InventoryManager>();
            if(inventory == null) return 0;

            int total = 0;

            for(int i = 0; i < inventory.GetSize(); i++)
            {
                if(inventory.GetItemInSlot(i) == item)
                {
                    total += inventory.GetNumberInSlot(i);
                }
            }
            return total;
        }

        private float GetPrice(StockItemConfig config)
        {
            if(isBuyingMode)
            {
                return config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
            }

            return config.item.GetPrice() * (sellingDiscountPercentage / 100);
        }

        // THIS IS HOW TO HARDCODE NEW ITEMS INTO THE GAME FOR ANY MODDERS 
        // yield return new ShopItem(InventoryManager.Instance.GetItemDetails(<ITEMCODE (int) >),
        //     <QUANTITY IN SHOP (int) >, 
        //     <PRICE (float)> 
        //     --not needed * (1 - <YOUR DISCOUNT AMOUNT TO GIVE> /100) not needed --, 
        //      0);
        // put this in IEnumerable<ShopItem> GetFilteredItems() NOTE: this will add this to every shop. You will need to figure your own way to make them seperate

        // public IEnumerable<ShopItem> GetFilteredItems() 
        // {
        //     yield return new ShopItem(InventoryManager.Instance.GetItemDetails(10000),
        //     10, 13.0f, 0);
        //     yield return new ShopItem(InventoryManager.Instance.GetItemDetails(10001),
        //     8, 9.0f, 0);
        //     yield return new ShopItem(InventoryManager.Instance.GetItemDetails(10002),
        //     100, 8.5f, 0);
        //     yield return new ShopItem(InventoryManager.Instance.GetItemDetails(10003),
        //     2, 100.0f, 0);
            
        // }

        public void SelectFilter(ItemType itemType) {} //found in Enums.cs.. I am not following class fully at all, see section 1 lesson 9 in RPG SHOPS & ABILITIES: INTERMEDIATE C# GAME CODING if it is F'd up.
        public ItemType GetItemType() { return ItemType.none; }
        public void SelectMode(bool isBuying) 
        {
            isBuyingMode = isBuying;
            if(onChange != null)
            {
                onChange();
            }
        }
        public bool IsBuyingMode() { return isBuyingMode; }
        public bool CanTransact() { return true; }
        public void ConfirmTransaction() 
        {   
            InventoryManager shopperInventory = InventoryManager.Instance.GetComponent<InventoryManager>();
            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            if(shopperInventory == null || shopperPurse == null) return;
            var transactionSnapshot = new Dictionary<ItemDetails, int>(transaction);
            if(isBuyingMode)
            {
                foreach(ItemDetails item in transactionSnapshot.Keys)
            {
                int quantity = transactionSnapshot[item];
                float price = item.GetPrice();
                for(int i = 0; i < quantity; i++)
                {
                    if(isBuyingMode)
                    {
                    if(shopperPurse.GetBalance() < price); 

                    shopperInventory.AddItem(InventoryLocation.player, item.itemCode);
                    AddToTransaction(item, -1);
                    shopperPurse.UpdateBalance(-price);
                    }
                    else 
                    {

                        shopperInventory.RemoveItem(InventoryLocation.player, item.itemCode);
                        AddToTransaction(item, -1);
                        shopperPurse.UpdateBalance(price);
                    }
                }
            }
            
            }
        }
        public float TransactionTotal() 
        {
            float total = 0;
            foreach(ItemDetails item in GetAllItems())
            {
                total += item.GetPrice() * item.GetQuantityInTransaction();
            } 
            return total;
        }
        public void AddToTransaction(ItemDetails item, int quantity) 
        {
            if(!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }
            transaction[item] += quantity;

            if(transaction[item] <= 0)
            {
                transaction.Remove(item);
            }
            if(onChange != null)
            {
                onChange();
            }
        }

        private bool canActivate;

   


        /*public CursorType GetCursorType() //THIS IS IF I WANT THE CURSOR TO KNOW WHAT TYPE OF THING THE CURSOR REGISTERS AS
        { 
            return CursorType.Shop;
        }


        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }
            return true;
        }
        */
        private void OnTriggerStay2D(Collider2D collision) 
        {
            if(collision.tag == "Player" )
            {
                canActivate = true;
            }
    
        }


        private void OnTriggerExit2D(Collider2D collision) 
        {
            if(collision.tag == "Player")
            {
                canActivate = false;
            }               
        }

        public string GetShopName()
        {
            return shopName;
        }

        void Update()
        {
  
            if (canActivate && Input.GetKeyDown(KeyCode.E))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>().SetActiveShop(this);
            }
  
        }

    }

}