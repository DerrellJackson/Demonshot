using UnityEngine;

[System.Serializable]
public class CropDetails 
{

    [ItemCodeDescription] public int seedItemCode; //this is the item code for the corresponding seed
    public int[] growthDays; //days growth for each stage 
    //public int totalGrowthDays;//total days until harvestable
    public GameObject[] growthPrefab;//prefab to use when instantiating growth stages 
    public Sprite[] growthSprite;
    public Season[] seasons; //growth seasons
    public Sprite harvestedSprite;//sprite used when it is harvested
    
    [ItemCodeDescription] public int harvestedTransformItemCode;//if the item transforms into another item when harvested this item code will be populated
    public bool hideCropBeforeHarvestedAnimation;//if the crop should be disabled before the harvested animation
    public bool disableCropColliderBeforeHarvestedAnimation;//if colliders on crop should be disabled to avoid the harvested animation effecting the game objects
    public bool isHarvestedAnimation;//if true harvested animation to be played on final growth stage prefab
    public bool isHarvestActionEffect = false;//flag to determine whether there ia a harvest action effect
    public bool spawnCropProducedAtPlayerPosition;
    //public HarvestActionEffect harvestActionEffect;//the harvest action effect for the crop
    //need to enable above after script is made

    [ItemCodeDescription] public int[] harvestToolItemCode;//array of item codes for the tools that can harvest or 0 array if no tool is required for harvesting crop
    public int[] requiredHarvestActions;//number of harvest actions required for corresponding tool in harvest tool item code array

    [ItemCodeDescription] public int[] cropProducedItemCode;//array of item codes produced for the harvested crop
    public int[] cropProducedMinQuantity;//array of min quantities produced for the harvested crop
    public int[] cropProducedMaxQuantity;//array of max quantities produced for the harvested crop
    public int daysToRegrow;//days to regrow next crop or -1 if a single crop


    //returns true if the tool item code can be used to harvest this crop, else returns false
    public bool CanUseToolToHarvestCrop(int toolItemCode) 
    {

        if(RequiredHarvestActionsForTool(toolItemCode) == -1)
        {
            return false;
        }
        else 
        {
            return true;
        }

    }


    //returns -1 if the tool cannot be used to harvest this crop, else it returns the number of harvest actions required by this tool
    public int RequiredHarvestActionsForTool(int toolItemCode)
    {
        for(int i = 0; i < harvestToolItemCode.Length; i++)
        {
            if(harvestToolItemCode[i] == toolItemCode)
            {
                return requiredHarvestActions[i];
            }
        }
        return -1;
    }

}
