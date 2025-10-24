using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
   
    [System.Serializable]

    private struct RangeByLevel 
    {
        public DungeonLevelSO dungeonLevel;
        [Range(0, 100)] public int min;
        [Range(0, 100)] public int max;
    }


    #region Header CHEST PREFAB 
    [Space(10)]
    [Header("CHEST PREFAB")]
    #endregion Header CHEST PREFAB

    #region Tooltip
    [Tooltip("Populate with the chest prefab")]
    #endregion Tooltip 
    [SerializeField] private GameObject chestPrefab;


    #region Header CHEST SPAWN CHANCE 
    [Space(10)]
    [Header("CHEST SPAWN CHANCE")]
    #endregion Header CHEST SPAWN CHANCE 

    #region Tooltip
    [Tooltip("The minimum probability for spawning a chest")]
    #endregion Tooltip 
    [SerializeField] [Range(0, 100)] private int chestSpawnChanceMin;

    #region Tooltip
    [Tooltip("The maximum probability for spawning a chest")]
    #endregion Tooltip 
    [SerializeField] [Range(0, 100)] private int chestSpawnChanceMax;

    #region Tooltip
    [Tooltip("You can override the chest spawn chance by dungeon level")]
    #endregion Tooltip
    [SerializeField] private List<RangeByLevel> chestSpawnChanceByLevelList;


    #region Header CHEST SPAWN DETAILS 
    [Space(10)]
    [Header("CHEST SPAWN DETAILS")]
    #endregion Header CHEST SPAWN DETAILS

    [SerializeField] private ChestSpawnEvent chestSpawnEvent;
    [SerializeField] private ChestSpawnPosition chestSpawnPosition;

    #region Tooltip
    [Tooltip("The minimum number of items to spawn (but max is one of each type)")]
    #endregion Tooltip 
    [SerializeField] [Range(0, 3)] private int numberOfItemsToSpawnMin; //CHANGE THIS LATER WHEN I ADD MORE ITEM TYPES

    #region Tooltip
    [Tooltip("The maximum number of items to spawn (but max is one of each type)")]
    #endregion Tooltip 
    [SerializeField] [Range(0, 3)] private int numberOfItemsToSpawnMax; //CHANGE THIS LATER WHEN I ADD MORE ITEM TYPES


    #region Header CHEST CONTENT DETAILS
    [Space(10)]
    [Header("CHEST CONTENT DETAILS")]
    #endregion Header CHEST CONTENT DETAILS 

    #region Tooltip 
    [Tooltip("The weapons to spawn in for each dungeon level and the spawn chance")]
    #endregion Tooltip
    [SerializeField] private List<SpawnableObjectsByLevel<WeaponDetailsSO>> weaponSpawnByLevelList;

    //Add item powerups thing here and make it just like the above tooltip!!!

    #region Tooltip
    [Tooltip("The range of health to spawn for each level")]
    #endregion Tooltip
    [SerializeField] private List<RangeByLevel> healthSpawnByLevelList;

    #region Tooltip
    [Tooltip("The range of ammo to spawn for each level")]
    #endregion Tooltip 
    [SerializeField] private List<RangeByLevel> ammoSpawnByLevelList;

    private bool chestSpawned = false;
    private Room chestRoom;


    private void OnEnable() 
    {

        //sub to room changed event 
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;

        //sub to room enemies defeated event
        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;

    }


    private void OnDisable() 
    {

        //unsub from the room changed event 
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

        //unsub from the room enemies defeated event 
        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;

    }


    //handle room changed event 
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {

        //get the room that the chest is in if it isn't already known
        if(chestRoom == null)
        {
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;
        }

        //if the chest is spawned on room entry then spawn the chest
        if(!chestSpawned && chestSpawnEvent == ChestSpawnEvent.onRoomEntry && chestRoom == roomChangedEventArgs.room)
        {
            SpawnChest();
        }

    }

    //handle room enemies defeated event 
    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {

        //get the room that the chest is in if  not already known
        if(chestRoom == null)
        {
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;
        }    

        //if the chest is spawned when enemies are defeated and the chest is in the room that the enemies have been defeated in
        if(!chestSpawned && chestSpawnEvent == ChestSpawnEvent.onEnemiesDefeated && chestRoom == roomEnemiesDefeatedArgs.room)
        {
            SpawnChest();
        }

    }


    //spawn in chest
    private void SpawnChest() 
    {

        chestSpawned = true; 

        //should chest be spawned based on specified chance, and if not return true
        if(!RandomSpawnChest()) return; 

        //get number of everything to spawn
        GetItemsToSpawn(out int ammoNum, out int healthNum, out int weaponNum);///will add itemPowerNum and take out ammo/health

        //instantiate chest 
        GameObject chestGameObject = Instantiate(chestPrefab, this.transform);

        //position the chest 
        if(chestSpawnPosition == ChestSpawnPosition.atSpawnerPosition)
        {
            chestGameObject.transform.position = this.transform.position;
        }
        else if (chestSpawnPosition == ChestSpawnPosition.atPlayerPosition)
        {
            //get nearest point to player
           // Vector3 spawnPosition = HelperUtilities.GetSpawnPositionNearestToPlayer(OldGameManager.Instance.GetPlayer().transform.position);

            //calc some random variation 
            Vector3 variation = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            //chestGameObject.transform.position = spawnPosition + variation;
        }

        //get chest component
        Chest chest = chestGameObject.GetComponent<Chest>();

        //initialize chest 
        if(chestSpawnEvent == ChestSpawnEvent.onRoomEntry)
        {
            //don't use the material effect 
            chest.Initialize(false, GetHealthPercentToSpawn(healthNum), GetWeaponDetailsToSpawn(weaponNum), GetAmmoPercentToSpawn(ammoNum));//change ammoNum, healthNum and add the itemPowerNum
        }
        else 
        {
            //use material effect 
            chest.Initialize(true, GetHealthPercentToSpawn(healthNum), GetWeaponDetailsToSpawn(weaponNum), GetAmmoPercentToSpawn(ammoNum));//change ammoNum, healthNum and add the itemPowerNum
        }

    }


    //random spawn of chest
    private bool RandomSpawnChest()
    {

        int chancePercent = Random.Range(chestSpawnChanceMin, chestSpawnChanceMax + 1);

        //check if an override chance percent has been set for the current level
        foreach(RangeByLevel rangeByLevel in chestSpawnChanceByLevelList)
        {
        if(rangeByLevel.dungeonLevel == OldGameManager.Instance.GetCurrentDungeonLevel())
            {
                chancePercent = Random.Range(rangeByLevel.min, rangeByLevel.max + 1);
                break;
            }
        }

        //get random value between 1 to 100
        int randomPercent = Random.Range(1, 100 + 1);

        if(randomPercent <= chancePercent)
        {
            return true;
        }
        else 
        {
            return false;
        }

    }


    //get the number of items to spawn, will need to remove ammo, health, and add itemPower
    private void GetItemsToSpawn(out int ammo, out int health, out int weapons)
    {

        ammo = 0; //remove
        health = 0; //remove 
        weapons = 0; 

        int numberOfItemsToSpawn = Random.Range(numberOfItemsToSpawnMin, numberOfItemsToSpawnMax + 1);

        int choice;

        if(numberOfItemsToSpawn == 1)
        {
            choice = Random.Range(0, 3);
            if(choice == 0) { weapons ++; return; }
            if(choice == 1) { ammo++; return; } //remove
            if(choice == 2) { health++; return; }// remove + add itemPower
        }
        else if (numberOfItemsToSpawn == 2)
        {
            choice = Random.Range(0, 3);
            if(choice == 0) { weapons ++; ammo++; return; }
            if(choice == 1) { ammo++; health++; return; } //remove
            if(choice == 2) { health++; weapons++; return; }// remove + add itemPower
        }
        else if(numberOfItemsToSpawn >= 3)
        {
            weapons++;
            ammo++;
            health++;
            return; // remove + add itemPower
        }
    }


    //get ammo percent to spawn
    private int GetAmmoPercentToSpawn(int ammoNumber)//may remove 
    {
        if(ammoNumber == 0) return 0;

        //get ammo spawn percent range for each level
        foreach(RangeByLevel spawnPercentByLevel in ammoSpawnByLevelList)
        {
            if(spawnPercentByLevel.dungeonLevel == OldGameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;

    }


    //get health percent to spawn
    private int GetHealthPercentToSpawn(int healthNumber)
    {
        if(healthNumber == 0) return 0; 

        //get health spawn percent range for each level
        foreach(RangeByLevel spawnPercentByLevel in healthSpawnByLevelList)
        {
            if(spawnPercentByLevel.dungeonLevel == OldGameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;

    } 


    //get the weapon details to spawn and it returns null if no weapon is to be spawned or if the player already had the weapon
    private WeaponDetailsSO GetWeaponDetailsToSpawn(int weaponNumber)
    {
        if (weaponNumber == 0) return null;

        //create an instance of the class used to select a random item from a list based one the relative ' ratios ' of the specified item
        RandomSpawnableObject<WeaponDetailsSO> weaponRandom = new RandomSpawnableObject<WeaponDetailsSO>(weaponSpawnByLevelList);

        WeaponDetailsSO weaponDetails = weaponRandom.GetItem(); 

        return weaponDetails;
    }
    

    #region Validation
#if UNITY_EDITOR

    //validate prefab details entered
    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(chestPrefab), chestPrefab);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(chestSpawnChanceMin), chestSpawnChanceMin, nameof(chestSpawnChanceMax), chestSpawnChanceMax, true);

        if(chestSpawnChanceByLevelList != null && chestSpawnChanceByLevelList.Count > 0)
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(chestSpawnChanceByLevelList), chestSpawnChanceByLevelList);

            foreach(RangeByLevel rangeByLevel in chestSpawnChanceByLevelList)
            {
                HelperUtilities.ValidateCheckNullValue(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                HelperUtilities.ValidateCheckPositiveRange(this, nameof(rangeByLevel.min), rangeByLevel.min, nameof(rangeByLevel.max), rangeByLevel.max, true);
            }
        }

        HelperUtilities.ValidateCheckPositiveRange(this, nameof(numberOfItemsToSpawnMin), numberOfItemsToSpawnMin, nameof(numberOfItemsToSpawnMax), numberOfItemsToSpawnMax, true);

        if(weaponSpawnByLevelList != null && weaponSpawnByLevelList.Count > 0)
        {
            foreach(SpawnableObjectsByLevel<WeaponDetailsSO> weaponDetailsByLevel in weaponSpawnByLevelList)
            {
                HelperUtilities.ValidateCheckNullValue(this, nameof(weaponDetailsByLevel.dungeonLevel), weaponDetailsByLevel.dungeonLevel);

                foreach(SpawnableObjectRatio<WeaponDetailsSO> weaponRatio in weaponDetailsByLevel.spawnableObjectRatioList)
                {
                    HelperUtilities.ValidateCheckNullValue(this, nameof(weaponRatio.dungeonObject), weaponRatio.dungeonObject);

                    HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponRatio.ratio), weaponRatio.ratio, true);
                }
            }
        }
        if(healthSpawnByLevelList != null && healthSpawnByLevelList.Count > 0)
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(healthSpawnByLevelList), healthSpawnByLevelList);

            foreach(RangeByLevel rangeByLevel in healthSpawnByLevelList)
            {
                HelperUtilities.ValidateCheckNullValue(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                HelperUtilities.ValidateCheckPositiveRange(this, nameof(rangeByLevel.min), rangeByLevel.min, nameof(rangeByLevel.max), rangeByLevel.max, true);
            }
        }
        if(ammoSpawnByLevelList != null && ammoSpawnByLevelList.Count > 0)
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(ammoSpawnByLevelList), ammoSpawnByLevelList);
            foreach(RangeByLevel rangeByLevel in ammoSpawnByLevelList)
            {
                HelperUtilities.ValidateCheckNullValue(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                HelperUtilities.ValidateCheckPositiveRange(this, nameof(rangeByLevel.min), rangeByLevel.min, nameof(rangeByLevel.max), rangeByLevel.max, true);
            }
        }

    }

#endif 
    #endregion Validation

}
