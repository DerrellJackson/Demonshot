using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBarUI : MonoBehaviour
{
   
   [SerializeField] private Sprite blank1x1sprite = null;
   [SerializeField] private InventorySlotUI[] inventorySlot = null;
   public GameObject inventoryBarDraggedItem;
   [HideInInspector] public GameObject inventoryTextBoxGameobject;

    private RectTransform rectTransform;

    private bool _isInventoryBarPositionBottom = true;

    public bool IsInventoryBarPositionBottom { get => _isInventoryBarPositionBottom; set => _isInventoryBarPositionBottom = value; }

    private void Awake() 
    {

        rectTransform = GetComponent<RectTransform>();

    }


    private void OnEnable() 
    {

        StaticEventHandler.InventoryUpdatedEvent += InventoryUpdated;

    }


    private void OnDisable() 
    {

        StaticEventHandler.InventoryUpdatedEvent -= InventoryUpdated;

    }


    private void Update() 
    {

        //switch inventory bar position depending on player position
        SwitchInventoryBarPosition();

    }


    //clear all highlights from the inventory bar
    public void ClearHighlightOnInventorySlots() 
    {

        if(inventorySlot.Length > 0) 
        {
            //loop through the inv slots and clear highlighted sprites
            for(int i = 0; i < inventorySlot.Length; i++)
            {
                if(inventorySlot[i].isSelected)
                {
                    inventorySlot[i].isSelected = false;
                    inventorySlot[i].inventorySlotHighlight.color = new Color(0f, 0f, 0f, 0f);
                    
                    //update inv to show item as not selected 
                    InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
                }
            }
        }

    }


    //set the selected highligh if set on all inventory item positions
    public void SetHighlightedInventorySlots()
    {

        if(inventorySlot.Length > 0)
        {
            //loop through inventory slots and clear highlight sprites 
            for(int i = 0; i < inventorySlot.Length; i++)
            {
                SetHighlightedInventorySlots(i);
            }
        }

    }


    //set the highlight if set on an inventory item for a given slot item position
    public void SetHighlightedInventorySlots(int itemPosition)
    {

        if(inventorySlot.Length > 0 && inventorySlot[itemPosition].itemDetails != null)
        {
        if(inventorySlot[itemPosition].isSelected)
        {
            inventorySlot[itemPosition].inventorySlotHighlight.color = new Color(1f, 1f, 1f, 1f); 

            //update inventory to show item as selected 
            InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, inventorySlot[itemPosition].itemDetails.itemCode);
        }
        }

    }

    
    private void ClearInventorySlots() 
    {
        
        if(inventorySlot.Length > 0)
        {
            //loop through inventory slots and update wit the blank sprite
            for(int i = 0; i < inventorySlot.Length; i++)
            {
            inventorySlot[i].inventorySlotImage.sprite = blank1x1sprite;
            inventorySlot[i].textMeshProUGUI.text = "";
            inventorySlot[i].itemDetails = null;
            inventorySlot[i].itemQuantity = 0;
            SetHighlightedInventorySlots(i);
            }
        }

    }


    private void InventoryUpdated(InventoryLocation inventoryLocation, Dictionary<int, InventoryItem> inventoryDict)
    {

        if(inventoryLocation == InventoryLocation.player) 
        {
            ClearInventorySlots(); 

            if(inventorySlot.Length > 0 && inventoryDict.Count > 0)
            {
                //loop through inventory slots and update with corresponding inventory list items
                for(int i = 0; i < inventorySlot.Length; i++)
                {
                    if(i < inventoryDict.Count)
                    {
                        int itemCode = inventoryDict[i].itemCode; 

                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                        if(itemDetails != null)
                        {
                            //add images and details to inv item slot
                            inventorySlot[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                            inventorySlot[i].textMeshProUGUI.text = inventoryDict[i].itemQuantity.ToString();
                            inventorySlot[i].itemDetails = itemDetails;
                            inventorySlot[i].itemQuantity = inventoryDict[i].itemQuantity;
                            SetHighlightedInventorySlots(i);
                        }
                    }
                    else 
                    {
                        break;
                    }
                }
            }
        }

    }


    private void SwitchInventoryBarPosition() 
    {

        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition();

        if(playerViewportPosition.y > 0.3f && IsInventoryBarPositionBottom == false) 
        {
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
             rectTransform.anchoredPosition = new Vector2(0f, 7f);

            IsInventoryBarPositionBottom = true;
        }
        else if (playerViewportPosition.y <= 0.3f && IsInventoryBarPositionBottom == true) 
        {
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -7f);

            IsInventoryBarPositionBottom = false;
        }

    }


    public void DestroyCurrentlyDraggedItems() 
    {

        for(int i = 0; i < inventorySlot.Length; i++)
        {
            if(inventorySlot[i].draggedItem != null)
            {
                Destroy(inventorySlot[i].draggedItem);
            }
        }

    }


    public void ClearCurrentlySelectedItems()
    {

        for(int i = 0; i < inventorySlot.Length; i++)
        {
            inventorySlot[i].ClearSelectedItem();
        }

    }


}
