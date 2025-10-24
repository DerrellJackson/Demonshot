using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private int harvestActionCount = 0;

    [HideInInspector] public Vector2Int cropGridPosition;

    public void ProcessToolAction(ItemDetails equippedItemDetails)
    {

        //get grid property details 
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cropGridPosition.x, cropGridPosition.y);

        if(gridPropertyDetails == null)
        return; 

        //get seed item details 
        ItemDetails seedItemDetails = InventoryManager.Instance.GetItemDetails(gridPropertyDetails.seedItemCode);
        if(seedItemDetails == null)
        return; 

        //get crop details 
        CropDetails cropDetails = GridPropertiesManager.Instance.GetCropDetails(seedItemDetails.itemCode);
        if(cropDetails == null)
        return;

        //increment harvest action count 
        harvestActionCount += 1;

        //get required harvest actions for tool
        int requiredHarvestActions = cropDetails.RequiredHarvestActionsForTool(equippedItemDetails.itemCode);
        if(requiredHarvestActions == -1)
        return; //this tool cannot be used to harvest this crop basically

        //increment harvest action count
        harvestActionCount += 1;

        //check is required harvest actions are made
        if(harvestActionCount >= requiredHarvestActions)
        HarvestCrop(cropDetails, gridPropertyDetails);

    }


    public void HarvestCrop(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails)
    {

        //delete crop from grid properties
        gridPropertyDetails.seedItemCode = -1;
        gridPropertyDetails.growthDays = -1;
        gridPropertyDetails.daysSinceLastHarvest = -1;
        gridPropertyDetails.daysSinceWatered = -1;

        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);

        HarvestActions(cropDetails, gridPropertyDetails);

    }


    private void HarvestActions(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails)
    {

        SpawnHarvestedItems(cropDetails);

        Destroy(gameObject);

    }


    private void SpawnHarvestedItems(CropDetails cropDetails)
    {

        //spawn the item to be produced
        for(int i = 0; i < cropDetails.cropProducedItemCode.Length; i++)
        {
            int cropsToProduce;

            //calc how many crops are needed to be produced
            if(cropDetails.cropProducedMinQuantity[i] == cropDetails.cropProducedMaxQuantity[i] || cropDetails.cropProducedMaxQuantity[i] < cropDetails.cropProducedMinQuantity[i]) 
            {
                cropsToProduce = cropDetails.cropProducedMinQuantity[i];
            }
            else 
            {
                cropsToProduce = Random.Range(cropDetails.cropProducedMinQuantity[i], cropDetails.cropProducedMaxQuantity[i] + 1);
            }
            for(int j = 0; j < cropsToProduce; j++)
            {
                Vector3 spawnPosition;
                if(cropDetails.spawnCropProducedAtPlayerPosition)
                {
                    //add item to the players inventory
                    InventoryManager.Instance.AddItem(InventoryLocation.player, cropDetails.cropProducedItemCode[i]);
                }
                else 
                {
                    //random position
                    spawnPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), 0f);
                    SceneItemsManager.Instance.InstantiateSceneItem(cropDetails.cropProducedItemCode[i], spawnPosition);
                }
            }
        }

    }


}
