using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarTest : MonoBehaviour
{
   
    private InstantiatedRoom instantiatedRoom;
    private Grid grid;
    private Tilemap frontTilemap;
    private Tilemap pathTilemap;
    private Vector3Int startGridPosition;
    private Vector3Int endGridPosition;
    private TileBase startPathTile;
    private TileBase finishedPathTile;

    private Vector3Int noValue = new Vector3Int(9999, 9999, 9999);
    private Stack<Vector3> pathStack;

    private void OnEnable() 
    {

        //subscribe to the onRoomChanged event
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;

    }
   

    private void OnDisable() 
    {

        //unsubscribe to the onRoomChanged event
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

    }


    private void Start() 
    {

        startPathTile = GameResources.Instance.preferredEnemyPathTile;
        finishedPathTile = GameResources.Instance.enemyUnwalkableCollisionTilesArray[0];

    }


    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {

        pathStack = null; 
        instantiatedRoom = roomChangedEventArgs.room.instantiatedRoom;
        frontTilemap = instantiatedRoom.transform.Find("Grid/Tilemap4_Front").GetComponent<Tilemap>(); //if this is not correct go to the tilemap with the rooms and rename it correctly
        grid = instantiatedRoom.transform.GetComponentInChildren<Grid>();
        startGridPosition = noValue;
        endGridPosition = noValue;

        SetUpPathTilemap();

    }


    //use a clone of the front tilemap for the path tilemap. if not created then create one, else use the existing one
    private void SetUpPathTilemap()
    {

        Transform tilemapCloneTransform = instantiatedRoom.transform.Find("Grid/Tilemap4_Front(Clone)"); //again this may be the incorrect name so look if it has error

        //if the front tilemap has not been cloned then clone it
        if(tilemapCloneTransform == null)
        {
            pathTilemap = Instantiate(frontTilemap, grid.transform);
            pathTilemap.GetComponent<TilemapRenderer>().sortingOrder = 2;
            pathTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial; //may need to fix the litMaterial unsure though
            pathTilemap.gameObject.tag = "Untagged";
        }
        //else use it
        else 
        {
            pathTilemap = instantiatedRoom.transform.Find("Grid/Tilemap4_Front(Clone)").GetComponent<Tilemap>();
            pathTilemap.ClearAllTiles();
        }

    }


    //updating per frame (duh)
    private void Update() 
    {

        if(instantiatedRoom == null || startPathTile == null || finishedPathTile == null || grid == null || pathTilemap == null) return;
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                ClearPath();
                SetStartPosition();
            }

            if(Input.GetKeyDown(KeyCode.O))
            {
                ClearPath();
                SetEndPosition();
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                DisplayPath();
            }
        }

    }


    //set the start position and the start tiles on the front tilemap
    private void SetStartPosition() 
    {

        if(startGridPosition == noValue)
        {
            startGridPosition = grid.WorldToCell(HelperUtilities.GetMouseWorldPosition());

            if(!IsPositionWithinBounds(startGridPosition))
            {
                startGridPosition = noValue;
                return;
            }

            pathTilemap.SetTile(startGridPosition, startPathTile);
        }
        else 
        {
            pathTilemap.SetTile(startGridPosition, null);
            startGridPosition = noValue;
        }

    }


    //set the end position and the end tiles  on the front tilemap
    private void SetEndPosition() 
    {

        if(endGridPosition == noValue)
        {
            endGridPosition = grid.WorldToCell(HelperUtilities.GetMouseWorldPosition());

            if(!IsPositionWithinBounds(endGridPosition))
            {
                endGridPosition = noValue;
                return;
            }

            pathTilemap.SetTile(endGridPosition, finishedPathTile);
        }
        else 
        {
            pathTilemap.SetTile(endGridPosition, null);
            endGridPosition = noValue;
        }

    }

    
    //check if position is in the bounds
    private bool IsPositionWithinBounds(Vector3Int position)
    {

        //if position is beyond the grid then it will return false
        if(position.x < instantiatedRoom.room.templateLowerBounds.x || position.x > instantiatedRoom.room. templateUpperBounds.x || position.y < instantiatedRoom.room.templateLowerBounds.y 
        || position.y > instantiatedRoom.room.templateUpperBounds.y)
        {
            return false;
        }
        else 
        {
            return true;
        }

    }

    
    //clears path and reset start and finish positions
    private void ClearPath() 
    {

        //clear path
        if(pathStack == null) return; 

        foreach(Vector3 worldPosition in pathStack)
        {
            pathTilemap.SetTile(grid.WorldToCell(worldPosition), null);
        }

        pathStack = null;

        //clear start and finish squares by making them no value
        endGridPosition = noValue;
        startGridPosition = noValue;

    }


    //displays the visible path between the start and end positions
    private void DisplayPath() 
    {

        if(startGridPosition == noValue || endGridPosition == noValue) return;

        pathStack = AStar.BuildPath(instantiatedRoom.room, startGridPosition, endGridPosition);

        if(pathStack == null) return;

        foreach(Vector3 worldPosition in pathStack)
        {
            pathTilemap.SetTile(grid.WorldToCell(worldPosition), startPathTile);
        }

    }


}
