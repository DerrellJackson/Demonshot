using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
   
    #region Tooltip
    [Tooltip("MovementDetailsSO containing the movement details obviously")]
    #endregion Tooltip
    [SerializeField] private MovementDetailsSO movementDetails;
    
    private Enemy enemy;
    private Stack<Vector3> movementSteps = new Stack<Vector3>();
    private Vector3 playerReferencePosition;
    private Coroutine moveEnemyRoutine;
    private float currentEnemyPathRebuildCooldown;
    private WaitForFixedUpdate waitForFixedUpdate;
    [HideInInspector] public float moveSpeed;
    private bool chasePlayer = false;
    [HideInInspector] public int updateFrameNumber = 1; //default value. This is set by the enemy spawner and is used to reduce the frame stagger
    private List<Vector2Int> surroundingPositionList = new List<Vector2Int>();


    private void Awake() 
    {

        //load components
        enemy = GetComponent<Enemy>();

        moveSpeed =  movementDetails.GetMoveSpeed();

    }


    private void Start() 
    {

        //create wait for fixed update to be used in the coroutine
        waitForFixedUpdate = new WaitForFixedUpdate();

        //reset player reference position
        playerReferencePosition = OldGameManager.Instance.GetPlayer().GetPlayerPosition();

    }


    private void Update() 
    {

        MoveEnemy();

    }
    

    //use the AStar pathfinding to build a path to the player and then move the enemy to each grid on the path until it reaches the player
    private void MoveEnemy()
    {

        //movement cooldown timer 
        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        //check distance to player to see if enemy should start chasing them
        if(!chasePlayer && Vector3.Distance(transform.position, OldGameManager.Instance.GetPlayer().GetPlayerPosition()) < enemy.enemyDetails.chaseDistance) //checks if it is less than chase distance 
        {
            chasePlayer = true;
        }

        //if not close enough to chase player then it will return 
        if(!chasePlayer)
        return;

        //process A Star path rebuild on certain frames to spread the load of frame sputtering between enemies
        if(Time.frameCount % Settings.targetFrameRateToSpreadPathfindingOver != updateFrameNumber) return;

        //if cooldown reached or player has moved more than the three squares distance then it will rebuild the enemy path and move the enemy
        if(currentEnemyPathRebuildCooldown <= 0f || (Vector3.Distance(playerReferencePosition, OldGameManager.Instance.GetPlayer().GetPlayerPosition()) > Settings.playerMoveDistanceToRebuildPath))
        {

            //reset path rebuild timer
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebuildCooldown;

            //reset player reference position
            playerReferencePosition = OldGameManager.Instance.GetPlayer().GetPlayerPosition();

            //move the enemy using the AStar pathfinding and trigger rebuild of path to player
            CreatePath();

            //if a path has been built move the enemy towards path (and player)
            if(movementSteps != null)
            {
                if(moveEnemyRoutine != null)
                {
                    //trigger idle event
                    enemy.idleEvent.CallIdleEvent();
                    StopCoroutine(moveEnemyRoutine);
                }
                //move enemy along the path using a coroutine
                moveEnemyRoutine = StartCoroutine(MoveEnemyRoutine(movementSteps));
            }

        }

    }


    //coroutine to move the enemy to the next location on the path
    private IEnumerator MoveEnemyRoutine(Stack<Vector3> movementSteps)
    {

        while(movementSteps.Count > 0)
        {
            Vector3 nextPosition = movementSteps.Pop();

            //while not very close continue to move, when close move onto the next step 
            while(Vector3.Distance(nextPosition, transform.position) > 0.2f)
            {
                //trigger movement event 
                enemy.movementToPositionEvent.CallMovementToPositionEvent(nextPosition, transform.position, moveSpeed, (nextPosition - transform.position).normalized);

                yield return waitForFixedUpdate; //moving the enemy using 2D physics so wait until the next fixed update
            }

            yield return waitForFixedUpdate;
        }

        //end of path steps, trigger the enemy idle event
        enemy.idleEvent.CallIdleEvent();
    }


    //use the AStar static class to build a path for the enemy to move on
    private void CreatePath() 
    {

        Room currentRoom = OldGameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        //get players position on the grid
        Vector3Int playerGridPosition = GetNearestNonObstaclePlayerPosition(currentRoom);

        //get enemy position on the grid
        Vector3Int enemyGridPosition = grid.WorldToCell(transform.position);

        //build a path for the enemy to move on
        movementSteps = AStar.BuildPath(currentRoom, enemyGridPosition, playerGridPosition);

        //take off first step on path - this is the grid square the enemy is currently on
        if(movementSteps != null)
        {
            movementSteps.Pop(); //pop method removes an item on the stack
        }
        else 
        {
            //trigger idle event - no path 
            enemy.idleEvent.CallIdleEvent();
        }

    }


    //set the frame number that the enemy path will be recalculated on - to avoid the lag spikes
    public void SetUpdateFrameNumber(int updateFrameNumber)
    {

        this.updateFrameNumber = updateFrameNumber;

    }


    //get the nearest position to the player that is not an obstacle (a player can access a collision tile so this is needed which without this the enemy will not be able to path the player properly)
    private Vector3Int GetNearestNonObstaclePlayerPosition(Room currentRoom)
    {

        Vector3 playerPosition = OldGameManager.Instance.GetPlayer().GetPlayerPosition();

        Vector3Int playerCellPosition = currentRoom.instantiatedRoom.grid.WorldToCell(playerPosition);

        Vector2Int adjustedPlayerCellPosition = new Vector2Int(playerCellPosition.x - currentRoom.templateLowerBounds.x, playerCellPosition.y - currentRoom.templateLowerBounds.y); //the half tiles are only on lower bounds

        int obstacle = Mathf.Min(currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x, adjustedPlayerCellPosition.y],
        currentRoom.instantiatedRoom.aStarItemObstacles[adjustedPlayerCellPosition.x, adjustedPlayerCellPosition.y]); 

        //if the player is not on a cell square marked as an obstacle then return that position
        if(obstacle != 0)
        {
            return playerCellPosition;
        }
        //find a cell that is not an obstacle, required for the half tiles. basically the player is making me add code because they want to hide on collision tiles so code is needed for the enemy to walk there.
        else 
        {
            //empty surrounding position list
            surroundingPositionList.Clear();

            //populate surrounding position list, this will hold eight possible vector locations surrounding a (0,0) grid square
            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if(j == 0 && i == 0) continue;

                    surroundingPositionList.Add(new Vector2Int(i, j));
                }
            }

            //loop through all positions
            for(int l = 0; l < 8; l++)
            {
                //generate a random index for the list
                int index = Random.Range(0, surroundingPositionList.Count);

                //see if there is an obstacle in the selected surrounding position
                try 
                {
                    obstacle = Mathf.Min(currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x + surroundingPositionList[index].x, 
                    adjustedPlayerCellPosition.y + surroundingPositionList[index].y], 
                    currentRoom.instantiatedRoom.aStarItemObstacles[adjustedPlayerCellPosition.x + surroundingPositionList[index].x, 
                    adjustedPlayerCellPosition.y + surroundingPositionList[index].y]);

                    //if no obstacle return the cell position to navigate to
                    if(obstacle != 0)
                    {
                        return new Vector3Int(playerCellPosition.x + surroundingPositionList[index].x, playerCellPosition.y + surroundingPositionList[index].y, 0);
                    }
                }
                //catch errors where the surrounding position is outside the grid
                catch 
                {

                }

                //remove the surrounding position with the obstacle so we can try again
                surroundingPositionList.RemoveAt(index);
            }

            //if no non obstacle cells are found around the player then the enemy will go to the enemy spawn position
            return (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];

        }

    }


    #region Validation
#if UNITY_EDITOR 

    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }

#endif 
    #endregion Validation

}
