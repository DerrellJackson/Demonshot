using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{

    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    public Dictionary<int, InventoryItem>[] inventoryDictionaries;


    Item[] slots;

    private int[] selectedInventoryItem; //the index of the array is the inventory list and the value is the items code

    [HideInInspector] public int[] inventoryListCapacityIntArray; //the index of the array is the inventory list (from the InventoryLocation enum), and the value is the cap of the inventory list

    [SerializeField] private ItemListSO itemList = null;


    protected override void Awake() 
    {

        base.Awake();

        //create inventory lists 
        CreateInventoryLists();

        //create the item details dictionary
        CreateItemDetailsDictionary();

        //initialise selected inv item array
        selectedInventoryItem = new int[(int)InventoryLocation.count];

        for (int i = 0; i < selectedInventoryItem.Length; i++)
        {
            selectedInventoryItem[i] = -1;
        }

    }


    //create the inventory list
    private void CreateInventoryLists() 
    {
        //initialize capacity array
        inventoryListCapacityIntArray = new int[(int)InventoryLocation.count];

        //player inv capacity
        inventoryListCapacityIntArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;

        //chest inv capacity
        //inventoryListCapacityIntArray[(int)InventoryLocation.chest] = Settings.chestInventoryCapacity;

        inventoryDictionaries = new Dictionary<int, InventoryItem>[(int)InventoryLocation.count];

        //create dict for player inv
        Dictionary<int, InventoryItem> playerDict = new Dictionary<int, InventoryItem>();

        for (int i = 0; i < inventoryListCapacityIntArray[(int)InventoryLocation.player]; i++)
        {
            InventoryItem invItem;
            invItem.itemCode = 0;
            invItem.itemQuantity = 0;
            playerDict.Add(i, invItem);
        }

        inventoryDictionaries[(int)InventoryLocation.player] = playerDict;

        //create dict for chest inv
        Dictionary<int, InventoryItem> chestDict = new Dictionary<int, InventoryItem>();

        for (int i = 0; i < inventoryListCapacityIntArray[(int)InventoryLocation.chest]; i++)
        {
            InventoryItem invItem;
            invItem.itemCode = 0;
            invItem.itemQuantity = 0;
            chestDict.Add(i, invItem);
        }

        inventoryDictionaries[(int)InventoryLocation.chest] = chestDict;

    }

        public int GetSize()
    {
        return 48;
    }




    //add an item to the inventory list for the inv location and then destroy the game object to delete
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete)
    {
       
        AddItem(inventoryLocation, item);
        
        Destroy(gameObjectToDelete); 

    }


    //add an item to the inventory list for the inv loaction
   public void AddItem(InventoryLocation inventoryLocation, Item item)

    {

        int itemCode = item.ItemCode;

        AddItem(inventoryLocation, itemCode);

    }
  
   

    //add item of type item code to the inventory list for the inventorylocation
    public void AddItem(InventoryLocation inventoryLocation, int itemCode)
     {

        Dictionary<int, InventoryItem> inventoryDict = inventoryDictionaries[(int)inventoryLocation];

        //check if inventory already contains the item
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            AddItemAtPosition(inventoryDict, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryDict, itemCode);
        }
        StaticEventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryDictionaries[(int)inventoryLocation]);

    }

    //add item of type item code to the inventory list for the inventorylocation
    public void AddItem(InventoryLocation inventoryLocation, int itemCode, int numberOfItems)
     {

        Dictionary<int, InventoryItem> inventoryDict = inventoryDictionaries[(int)inventoryLocation];

        //check if inventory already contains the item
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {   
            while(numberOfItems >= 1)
        {
            numberOfItems--;
            AddItemAtPosition(inventoryDict, itemCode, itemPosition);
        }
        }
        else
        {    
            AddItemAtPosition(inventoryDict, itemCode);
            numberOfItems--;
            while(numberOfItems >= 1)
            {
                numberOfItems--;
                int newItemPosition = FindItemInInventory(inventoryLocation, itemCode);
                AddItemAtPosition(inventoryDict, itemCode, newItemPosition);
            }
            
        }
        StaticEventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryDictionaries[(int)inventoryLocation]);

    }



    private int GetFirstEmptyItemSlot(Dictionary<int, InventoryItem> inventoryDict)

    {

        foreach (KeyValuePair<int, InventoryItem> item in inventoryDict)

        {

            if (item.Value.itemCode == 0) return item.Key;

        }

        return -1;

    }

    public Item GetItemInSlot(int slot)
    {
        return slots[slot];
    }

    public int GetNumberInSlot(int itemquantity)
    {
        return itemquantity;
    }


    //adds the item to the end of the inventory
    private void AddItemAtPosition(Dictionary<int, InventoryItem> inventoryDict, int itemCode)
    {

        InventoryItem inventoryItem = new InventoryItem();
        int itemSlot = GetFirstEmptyItemSlot(inventoryDict);

        if (itemSlot != -1)
        {

            inventoryItem.itemCode = itemCode;
            inventoryItem.itemQuantity = 1;
            inventoryDict[itemSlot] = inventoryItem;

        }

     //   DebugPrintInventoryList(inventoryList);

    }


    //swap item at from item index with item at to item index in inv location inv list
    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem)

    {
       
        //if from index and to index are within the bounds and not the same and >= zero
        if (fromItem != toItem && fromItem >= 0)
        {
            if (inventoryDictionaries[(int)inventoryLocation].ContainsKey(toItem))
            {

                InventoryItem fromInvItem = inventoryDictionaries[(int)inventoryLocation][fromItem];
                InventoryItem toInvItem = inventoryDictionaries[(int)inventoryLocation][toItem];
                inventoryDictionaries[(int)inventoryLocation][toItem] = fromInvItem;
                inventoryDictionaries[(int)inventoryLocation][fromItem] = toInvItem;
                //send Event that inv was updated
                StaticEventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryDictionaries[(int)inventoryLocation]);

            }

        }

    }


    //clear the selected inventory item for inventorylocation
    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        
        selectedInventoryItem[(int)inventoryLocation] = -1;

    }


    //add item to position in the inventory
    private void AddItemAtPosition(Dictionary<int, InventoryItem> inventoryDict, int itemCode, int itemPosition)
 {

        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryDict[itemPosition].itemQuantity + 1;
        inventoryItem.itemQuantity = quantity;
        inventoryItem.itemCode = itemCode;
        inventoryDict[itemPosition] = inventoryItem;
        //Debug.ClearDeveloperConsole();
        //DebugPrintInventoryList(inventoryList);

    }


    //find if an item code is already in the inventory, rtuens the item position in the inventory list, or -1 if the item is not in the inventory
    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
    {

        Dictionary<int, InventoryItem> inventoryDict = inventoryDictionaries[(int)inventoryLocation];

        foreach (KeyValuePair<int, InventoryItem> item in inventoryDict)
        {
            if (item.Value.itemCode == itemCode) return item.Key;
        }

        return -1;

    }


    //populates the item details dictionary from the scriptable object items list
    private void CreateItemDetailsDictionary() 
    {

        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemList.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }

    }


    //get item details for any item passed in as a parameter
    public ItemDetails GetItemDetails(int itemCode)
    {

        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            return itemDetails;
        }
        else 
        {
            return null;
        }

    }
    

    //returns the item details from the itemlistSO for the currently selected item in the inventory location, or null if an item is not selected
    public ItemDetails GetSelectedInventoryItemDetails(InventoryLocation inventoryLocation)
    {

        int itemCode = GetSelectedInventoryItem(inventoryLocation);

        if(itemCode == -1)
        {
            return null;
        }
        else 
        {
            return GetItemDetails(itemCode);
        }

    }


    //get the selected item for inventory location and returns item code or -1 if nothing is selected
    private int GetSelectedInventoryItem(InventoryLocation inventoryLocation)
    {

        return selectedInventoryItem[(int)inventoryLocation];

    }


    //get the item type decription for an item type and returns the item type description as a srting for a given item type
    public string GetItemTypeDescription(ItemType itemType)
    {

        string itemTypeDescription;
        switch (itemType)
        {
            case ItemType.Seed:
            itemTypeDescription = Settings.Seed;
            break;

            case ItemType.Egg:
            itemTypeDescription = Settings.Egg;
            break;

            case ItemType.Ore:
            itemTypeDescription = Settings.Ore;
            break;

            case ItemType.Furniture:
            itemTypeDescription = Settings.Furniture;
            break;

            case ItemType.Decor:
            itemTypeDescription = Settings.Decor;
            break;

            case ItemType.Food:
            itemTypeDescription = Settings.Food;
            break;

            case ItemType.Raw_food:
            itemTypeDescription = Settings.Rawfood;
            break;

            case ItemType.Breaking_tool:
            itemTypeDescription = Settings.BreakingTool;
            break;

            case ItemType.Fishing_tool:
            itemTypeDescription = Settings.FishingTool;
            break;

            case ItemType.Chopping_tool:
            itemTypeDescription = Settings.ChoppingTool;
            break;

            case ItemType.Hoeing_tool:
            itemTypeDescription = Settings.HoeingTool;
            break;

            case ItemType.Reaping_tool:
            itemTypeDescription = Settings.ReapingTool;
            break; 

            case ItemType.Watering_tool:
            itemTypeDescription = Settings.WateringTool;
            break;

            case ItemType.Collecting_tool:
            itemTypeDescription = Settings.CollectingTool;
            break;

            case ItemType.Melee_weapon: 
            itemTypeDescription = Settings.MeleeWeapon;
            break;

            case ItemType.Projectile_weapon:
            itemTypeDescription = Settings.ProjectileWeapon;
            break;

            case ItemType.Explosive_weapon:
            itemTypeDescription = Settings.ExplosiveWeapon;
            break;

            case ItemType.Craftable:
            itemTypeDescription = Settings.Craftable;
            break;

            case ItemType.Drink:
            itemTypeDescription = Settings.Drink;
            break;

            case ItemType.Potion:
            itemTypeDescription = Settings.Potion;
            break;
            
            case ItemType.Honey_tool:
            itemTypeDescription = Settings.HoneyTool;
            break;

            case ItemType.Flower:
            itemTypeDescription = Settings.Flower;
            break;

            case ItemType.Raw_Ore:
            itemTypeDescription = Settings.RawOre;
            break;

            case ItemType.Multi_Tool:
            itemTypeDescription = Settings.MultiTool;
            break;

            default:
            itemTypeDescription = itemType.ToString();
            break;
        }
        return itemTypeDescription;

    }


    //remove an item from the inventory, and create a game object at the position it was dropped
    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)

    {

        Dictionary<int, InventoryItem> inventoryDict = inventoryDictionaries[(int)inventoryLocation];

        //check if the inventory contains the item
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryDict, itemCode, itemPosition);
        }
        StaticEventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryDictionaries[(int)inventoryLocation]);

    }

    private void RemoveItemAtPosition(Dictionary<int, InventoryItem> inventoryDict, int itemCode, int itemPosition)
    {
        
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryDict[itemPosition].itemQuantity - 1;

        if(quantity > 0)
        {
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
        }
        else 
        {
            inventoryItem.itemQuantity = 0;
            inventoryItem.itemCode = 0;
        }
        inventoryDict[itemPosition] = inventoryItem;

    }


    //set the selected inventory item for inventory location to itemcode 
    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation, int itemCode)
    {

        selectedInventoryItem[(int)inventoryLocation] = itemCode;

    }


    //debug log 
   /* private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    {
        
        foreach(InventoryItem inventoryItem in inventoryList)
        {
            Debug.Log("Item Description:" + InventoryManager.Instance.GetItemDetails(inventoryItem.itemCode).itemDescription + "  Item Quantity: " + inventoryItem.itemQuantity);
        }
        Debug.Log("**************************************");

    }
    */

}
