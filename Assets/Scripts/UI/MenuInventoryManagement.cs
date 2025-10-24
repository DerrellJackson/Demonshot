using System.Collections.Generic;
using UnityEngine;

public class MenuInventoryManagement : MonoBehaviour
{
   
    [SerializeField] private MenuInventoryManagementSlot[] inventoryManagementSlot = null;

    public GameObject inventoryManagementDraggedItemPrefab;

    [SerializeField] private Sprite transparent1x1 = null;

    [HideInInspector] public GameObject inventoryTextBoxGameobject;


    private void OnEnable() 
    {

        StaticEventHandler.InventoryUpdatedEvent += PopulatePlayerInventory;

        //populate players inventory
        if(InventoryManager.Instance != null)
        {
            PopulatePlayerInventory(InventoryLocation.player, InventoryManager.Instance.inventoryDictionaries[(int)InventoryLocation.player]);
        }

    }


    private void OnDisable() 
    {

        StaticEventHandler.InventoryUpdatedEvent -= PopulatePlayerInventory;

        DestroyInventoryTextBoxGameobject();

    }


    public void DestroyInventoryTextBoxGameobject() 
    {

        //destroy inventory text box if created 
        if(inventoryTextBoxGameobject != null)
        {
            Destroy(inventoryTextBoxGameobject);
        }

    }


    public void DestroyCurrentlyDraggedItems() 
    {

        //loop through all player inventory items 
        for(int i = 0; i < InventoryManager.Instance.inventoryDictionaries[(int)InventoryLocation.player].Count; i++)
        {
            Destroy(inventoryManagementSlot[i].draggedItem);
        }

    }


    private void PopulatePlayerInventory(InventoryLocation inventoryLocation, Dictionary<int, InventoryItem> inventoryDict)
    {

        if(inventoryLocation == InventoryLocation.player) 
        {
            InitialiseInventoryManagementSlots();

            //loop through all player inventory items 
            for(int i = 0; i < InventoryManager.Instance.inventoryDictionaries[(int)InventoryLocation.player].Count; i++)
            {

                //get inventory item details
                inventoryManagementSlot[i].itemDetails = InventoryManager.Instance.GetItemDetails(inventoryDict[i].itemCode);
                inventoryManagementSlot[i].itemQuantity = inventoryDict[i].itemQuantity;

                if(inventoryManagementSlot[i].itemDetails != null)
                {
                    //update inventory management slot with image and quality
                    inventoryManagementSlot[i].inventoryManagementSlotImage.sprite = inventoryManagementSlot[i].itemDetails.itemSprite;
                    inventoryManagementSlot[i].textMeshProUGUI.text = inventoryManagementSlot[i].itemQuantity.ToString();
                }

            }
        }

    }


    private void InitialiseInventoryManagementSlots() 
    {

        //clear inventory slots 
        for(int i = 0; i < Settings.playerMaximumInventoryCapacity; i++)
        {
            inventoryManagementSlot[i].greyedOutImageGO.SetActive(false);
            inventoryManagementSlot[i].itemDetails = null;
            inventoryManagementSlot[i].itemQuantity = 0;
            inventoryManagementSlot[i].inventoryManagementSlotImage.sprite = transparent1x1;
            inventoryManagementSlot[i].textMeshProUGUI.text = "";
        }

        //grey out unavailable slots
        for(int i = InventoryManager.Instance.inventoryListCapacityIntArray[(int)InventoryLocation.player]; i < Settings.playerMaximumInventoryCapacity; i++)
        {
            inventoryManagementSlot[i].greyedOutImageGO.SetActive(true);
        }
        
    }


}
