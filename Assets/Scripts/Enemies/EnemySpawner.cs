using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : SingletonMonobehaviour<EnemySpawner>
{
   
    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private Room currentRoom;
    private RoomEnemySpawnParameters roomEnemySpawnParameters;


    private void OnEnable() 
    {

    //subscribe to room changed event 
    StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged; 

    }


    private void OnDisable() 
    {

    //unsubscribe to room changed event 
    StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged; 

    }


    //process a change in the room
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {

        enemiesSpawnedSoFar = 0;
        currentEnemyCount = 0;

        currentRoom = roomChangedEventArgs.room;

        //update music for room
        MusicManager.Instance.PlayMusic(currentRoom.ambientMusic, 0.2f, 2f);

        //if the room is an entrance or a corridor do not spawn in anything
        if(currentRoom.roomNodeType.isCorridorEW || currentRoom.roomNodeType.isCorridorNS || currentRoom.roomNodeType.isEntrance)
        return;

        //if the room has already been defeated then it will return
        if(currentRoom.isClearedOfEnemies)
        return;

        //get a random number of enmies to spawn inside of each room
        enemiesToSpawn = currentRoom.GetNumberOfEnemiesToSpawn(OldGameManager.Instance.GetCurrentDungeonLevel());

        //get room enemy spawn parameters
        roomEnemySpawnParameters = currentRoom.GetRoomEnemySpawnParameters(OldGameManager.Instance.GetCurrentDungeonLevel());

        //if no enemies to spawn return
        if(enemiesToSpawn == 0)
        {
            //marks the room as cleared
            currentRoom.isClearedOfEnemies = true;

            return;
        }


        //get concurrent number of enemies to spawn
        enemyMaxConcurrentSpawnNumber = GetConcurrentEnemies();

        //update music for room
        MusicManager.Instance.PlayMusic(currentRoom.battleMusic, 0.2f, 0.5f);

        //lock doors
        currentRoom.instantiatedRoom.LockDoors();

        //spawn enemies
        SpawnEnemies();

    }


    //spawn the enemies
    private void SpawnEnemies()
    {

        //set gamestate engaging boss
        if(OldGameManager.Instance.gameState == GameState.bossStage)
        {
            OldGameManager.Instance.previousGameState = GameState.bossStage;
            OldGameManager.Instance.gameState = GameState.engagingBoss;
        }

        //set game state engaging enemies
        else if(OldGameManager.Instance.gameState == GameState.playingLevel)
        {
            OldGameManager.Instance.previousGameState = GameState.playingLevel;
            OldGameManager.Instance.gameState = GameState.engagingEnemies;
        }

        StartCoroutine(SpawnEnemiesRoutine());

    }


    //spawn the enemies coroutine
    private IEnumerator SpawnEnemiesRoutine() 
    {

        Grid grid = currentRoom.instantiatedRoom.grid;

        //create an instance of the helper class used to select a random enemy
        RandomSpawnableObject<EnemyDetailsSO> randomEnemyHelperClass = new RandomSpawnableObject<EnemyDetailsSO>(currentRoom.enemiesByLevelList);

        //check we have somewhere that the enemies can spawn
        if(currentRoom.spawnPositionArray.Length > 0)
        {
            //loop through to create all the enemies 
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                //wait until current enemy count is less than max concurrent enemies
                while (currentEnemyCount >= enemyMaxConcurrentSpawnNumber)
                {
                    yield return null;
                }

                Vector3Int cellPosition = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];

                //create enemy and get next enemy type to spawn
                CreateEnemy(randomEnemyHelperClass.GetItem(), grid.CellToWorld(cellPosition));

                yield return new WaitForSeconds(GetEnemySpawnInterval());

            }
        }

    }


    //get a random spawn interval between the minimum and maximum values
    private float GetEnemySpawnInterval()
    {

        return(Random.Range(roomEnemySpawnParameters.minSpawnInterval, roomEnemySpawnParameters.maxSpawnInterval));

    }


    //get a random number of concurrent enemies between the minimum and maximum values
    private int GetConcurrentEnemies() 
    {

        return(Random.Range(roomEnemySpawnParameters.minConcurrentEnemies, roomEnemySpawnParameters.maxConcurrentEnemies));

    }


    //create an enemy in the specified position
    private void CreateEnemy(EnemyDetailsSO enemyDetails, Vector3 position)
    {

        //keep track of the number of enemies spawned so far
        enemiesSpawnedSoFar++;

        //add one to the current enemy count - this is reduced when an enemy is destroyed
        currentEnemyCount++;

        //get current dungeon level
        DungeonLevelSO dungeonLevel = OldGameManager.Instance.GetCurrentDungeonLevel();

        //instantiate the enemy
        GameObject enemy = Instantiate(enemyDetails.enemyPrefab, position, Quaternion.identity, transform);

        //initialize the enemy
        enemy.GetComponent<Enemy>().EnemyInitialization(enemyDetails, enemiesSpawnedSoFar, dungeonLevel);

        //subscribe to the enemy destroyed event
        enemy.GetComponent<DestroyedEvent>().OnDestroyed += Enemy_OnDestroyed;

    }


    //process enemy destroyed
    private void Enemy_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {

        //unsubscribe from the destroyed event
        destroyedEvent.OnDestroyed -= Enemy_OnDestroyed;

        //reduce the current enemy count
        currentEnemyCount--;

        //score points - call points scored event
        StaticEventHandler.CallPointsScoredEvent(destroyedEventArgs.points);

        if(currentEnemyCount <= 0 && enemiesSpawnedSoFar == enemiesToSpawn)
        {
            currentRoom.isClearedOfEnemies = true;

            //set the game state
            if(OldGameManager.Instance.gameState == GameState.engagingEnemies)
            {
                OldGameManager.Instance.gameState = GameState.playingLevel;
                OldGameManager.Instance.previousGameState = GameState.engagingEnemies;
            }
            else if (OldGameManager.Instance.gameState == GameState.engagingBoss)
            {
                OldGameManager.Instance.gameState = GameState.bossStage;
                OldGameManager.Instance.previousGameState = GameState.engagingBoss;
            }

            //unlock doors
            currentRoom.instantiatedRoom.UnlockDoors(Settings.doorUnlockDelay);

            //update music for room
            MusicManager.Instance.PlayMusic(currentRoom.ambientMusic, 0.2f, 2f);

            //trigger room enemies defeated event
            StaticEventHandler.CallRoomEnemiesDefeatedEvent(currentRoom);
        }

    }


}
