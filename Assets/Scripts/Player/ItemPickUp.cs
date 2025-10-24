using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
 
    private void OnTriggerEnter2D(Collider2D collision) 
    {

        Item item = collision.GetComponent<Item>(); 

        if (item != null) 
        {   
            //get item details
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);

            //if item can be picked up
            if(itemDetails.canBePickedUp == true) 
            {
                //add item to inv
                InventoryManager.Instance.AddItem(InventoryLocation.player, item, collision.gameObject);
            }

        }

    }


        private void OnTriggerStay2D(Collider2D collision) 
        {
            
        Crop crop = collision.GetComponent<Crop>();
 
        if (Input.GetKeyDown(KeyCode.E) && crop != null)
        {
            GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(crop.cropGridPosition.x, crop.cropGridPosition.y);
            if (gridPropertyDetails == null)
                return;
 
            // Get seed item details
            ItemDetails seedItemDetails = InventoryManager.Instance.GetItemDetails(gridPropertyDetails.seedItemCode);
            if (seedItemDetails == null)
                return;
 
            CropDetails cropDetails = GridPropertiesManager.Instance.GetCropDetails(seedItemDetails.itemCode);
            if (cropDetails == null)
                return;
 
            if (gridPropertyDetails != null && seedItemDetails != null && cropDetails != null && gridPropertyDetails.growthDays >= cropDetails.growthDays[cropDetails.growthDays.Length - 1])
            {
                Debug.Log("harvesting with e");
                crop.HarvestCrop(cropDetails, gridPropertyDetails);
 
            }
 
        }
    }


}
