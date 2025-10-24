[System.Serializable]

public class GridPropertyDetails 
{

    public int gridX;
    public int gridY;
    public bool isDiggable = false;
    public bool canDropItem = false;
    public bool canPlaceFurniture = false;
    public bool isPath = false;
    public bool isNPCObstacle = false;
    public int daysSinceDug = -1; //for crops
    public int daysSinceWatered = -1; //for crops
    public int seedItemCode = -1; //if seed planted 
    public int growthDays = -1; //number of days plant has grown
    public int daysSinceLastHarvest = -1; //for plants that get many days of harvest

    public GridPropertyDetails()
    {

    }

}
