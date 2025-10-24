using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//attach this script to all the prefab rooms 
[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public class InstantiatedRoom : MonoBehaviour
{
   
    [HideInInspector] public Room room;
    [HideInInspector] public Grid grid;
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap decoration1Tilemap;
    [HideInInspector] public Tilemap decoration2Tilemap;
    [HideInInspector] public Tilemap frontTilemap;
    [HideInInspector] public Tilemap collisionTilemap;
    [HideInInspector] public Tilemap minimapTilemap;
    [HideInInspector] public int[,] aStarMovementPenalty; //use this 2D array to store the movement penalties from the tilemaps that will be used in the AStar pathfinding
    [HideInInspector] public int[,] aStarItemObstacles; //use to store position of moveable items that are obstacles
    [HideInInspector] public Bounds roomColliderBounds;
    [HideInInspector] public List<MoveItem> moveableItemsList = new List<MoveItem>();

    [SerializeField] private GameObject environmentGameObject;

    private BoxCollider2D boxCollider2D;


    private void Awake() 
    {

        boxCollider2D = GetComponent<BoxCollider2D>();

        //save room collider bounds
        roomColliderBounds = boxCollider2D.bounds;

    }


    private void Start() 
    {

        //update moveable item obstacles array 
        UpdateMoveableObstacles();

    }


    //NOTE: I may wanna change my BoxCollider in PREFABS for the dungeon Entrance area 
    //trigger room changed event when player enters a room
    private void OnTriggerEnter2D(Collider2D collision) 
    {

        //if the player triggered the collider
        if(collision.tag == Settings.playerTag && room != OldGameManager.Instance.GetCurrentRoom())
        {
            //set the room as visited
            this.room.isPreviouslyVisited = true;

            //call room changed event
            StaticEventHandler.CallRoomChangedEvent(room);
        }

    }

    
    //initialise the instantiated room
    public void Initialise(GameObject roomGameobject)
    {

        PopulateTilemapMemberVariables(roomGameobject);

        BlockOffUnusedDoorWays();

        AddObstaclesAndPreferredPaths();

        CreateItemObstaclesArray();

        AddDoorsToRooms();

        DisableCollisionTilemapRenderer();

    }

    
    //populate the tilemap and grid member variables
    private void PopulateTilemapMemberVariables(GameObject roomGameobject)
    {

        //get the grid component
        grid = roomGameobject.GetComponentInChildren<Grid>();

        //get tilemaps in children
        Tilemap[] tilemaps = roomGameobject.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)
        {
            if(tilemap.gameObject.tag == "Ground_Tilemap")
            {
                groundTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "Decoration1_Tilemap")
            {
                decoration1Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "Decoration2_Tilemap")
            {
                decoration2Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "Front_Tilemap")
            {
                frontTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "Collision_Tilemap")
            {
                collisionTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "Minimap_Tilemap")
            {
               minimapTilemap = tilemap;
            }
        }

    }


    //block off unused doorways in the room
    private void BlockOffUnusedDoorWays()
    {

        //loop through all doorways
        foreach(Doorway doorway in room.doorWayList)
        {
            if(doorway.isConnected)
                continue;

            //block unconnected doorways using tiles on tilemaps
            if(collisionTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(collisionTilemap, doorway);
            }
             if(minimapTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(minimapTilemap, doorway);
            }
             if(groundTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(groundTilemap, doorway);
            }
             if(decoration1Tilemap != null)
            {
                BlockADoorwayOnTilemapLayer(decoration1Tilemap, doorway);
            }
             if(decoration2Tilemap != null)
            {
                BlockADoorwayOnTilemapLayer(decoration2Tilemap, doorway);
            }
             if(frontTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(frontTilemap, doorway);
            }
        }

    }

    
    //block a doorway on a tilemap layer
    private void BlockADoorwayOnTilemapLayer(Tilemap tilemap, Doorway doorway)
    {

        switch (doorway.orientation)
        {
            case Orientation.north:
            case Orientation.south:
                BlockDoorwayHorizontally(tilemap, doorway);
                break;

            case Orientation.east:
            case Orientation.west:
                BlockDoorwayVertically(tilemap, doorway);
                break;

            case Orientation.none:
                break;
        }

    }


    //blocks the doorway horizontally
    private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
    {

        Vector2Int startPosition = doorway.doorwayStartCopyPosition;

        //loop through all tiles to copy
        for(int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for(int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                //get rotation of tile being copied
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                //copy tile
                tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));    

                //set rotation of tile copied
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), transformMatrix);
            }
        }

    }


    //blocks the doorway vertically
    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {

        Vector2Int startPosition = doorway.doorwayStartCopyPosition;

        //loop through all tiles to copy
        for(int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
        {
            for(int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
            {
                //get rotation of tile being copied
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                //copy tile
                tilemap.SetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));
                
                //set rotation of tile copied
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), transformMatrix);
            }
        }        

    }


    //update obstacles used by AStar pathfinding
    private void AddObstaclesAndPreferredPaths()
    {

        //this array will be populated with wall obstacles
        aStarMovementPenalty = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1, room.templateUpperBounds.y - room.templateLowerBounds.y + 1];

        //loop through all grid squares
        for(int x = 0; x < (room.templateUpperBounds.x - room.templateLowerBounds.x + 1); x++)
        {
            for(int y = 0; y < (room.templateUpperBounds.y - room.templateLowerBounds.y + 1); y++)
            {
                //set default movement penalty for grid squares
                aStarMovementPenalty[x, y] = Settings.defaultAStarMovementPenalty;

                //add obstacles for collision tiles the enemy cannot walk on
                TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + room.templateLowerBounds.x, y + room.templateLowerBounds.y, 0));

                foreach(TileBase collisionTile in GameResources.Instance.enemyUnwalkableCollisionTilesArray)
                {
                    if(tile == collisionTile)
                    {
                        aStarMovementPenalty[x, y] = 0;
                        break;
                    }
                }

                //add preferred path for enemies
                if(tile == GameResources.Instance.preferredEnemyPathTile)
                {
                    aStarMovementPenalty[x, y] = Settings.preferredPathAStarMovementPenalty;
                }
            }
        }

    }


    //add opening doors if this is not a corridor room
    private void AddDoorsToRooms()
    {

        //test if the room is a corridor and if so then return
        if(room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNS) return;

        //loop through all the doorways in the room to check if it is connected or not and instantiate them
        foreach(Doorway doorway in room.doorWayList)
        {
            //check if the doorway prefab is not null annd the doorway is connected
            if(doorway.doorPrefab != null && doorway.isConnected)
            {
                float tileDistance = Settings.tileSizePixels / Settings.pixelsPerUnit;

                GameObject door = null;

                if(doorway.orientation == Orientation.north)
                {
                    //create door with parent as the room
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y + tileDistance - 1f, 0f);//THE Y POSITION EFFECTS HOW CLOSE OR FAR THE DOOR IS
                }

                else if(doorway.orientation == Orientation.south)
                {
                    //create door with parent as the room
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y + 1f, 0f);//THE Y POSITION EFFECTS HOW CLOSE OR FAR THE DOOR IS
                }
                else if(doorway.orientation == Orientation.east)
                {
                    //create door with parent as the room
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance - 1f, doorway.position.y + tileDistance * .50f, 0f);//CHANGE THE DECIMAL POINT BASED ON SIZE OF THE DOOR
                }
                else if(doorway.orientation == Orientation.west)
                {
                    //create door with parent as the room
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + 1f, doorway.position.y + tileDistance * .50f, 0f);//CHANGE THE DECIMAL POINT BASED ON SIZE OF THE DOOR
                }

                //get door component
                Door doorComponent = door.GetComponent<Door>();

                //instantiate skull icon for minimap by boss door
                if(room.roomNodeType.isBossRoom)
                {
                GameObject skullIcon = Instantiate(GameResources.Instance.minimapSkullPrefab, gameObject.transform);
                skullIcon.transform.localPosition = door.transform.localPosition;
                }
                //set if the door is part of a boss room
               /* if(room.roomNodeType.isBossRoom)
                {
                    doorComponent.isBossRoomDoor = true;

                    //lock the door to prevent access BUT I MAY CHANGE THIS CAUSE THIS SETTING WILL MAKE IT TILL ALL ENEMIES ARE DEFEATED IN _E V E R Y_ ROOM
                    doorComponent.LockDoor();
                }
*/
            }
        }

    }


    //disable collision tilemap renderer
    private void DisableCollisionTilemapRenderer()
    {

        //disable collision tilemap renderer
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;

    }


    //disable the room trigger collider that is used to trigger when the player enters a room
    public void DisableRoomCollider() 
    {

        boxCollider2D.enabled = false;

    }


    //enable the room trigger collider that is used to trigger when the player enters a room
    public void EnableRoomCollider()
    {

        boxCollider2D.enabled = true;

    }


    //lock the room doors
    public void LockDoors() 
    {

        Door[] doorArray = GetComponentsInChildren<Door>();

        //trigger lock doors
        foreach(Door door in doorArray)
        {
            door.LockDoor();
        }

        //disable room trigger collider
        DisableRoomCollider();

    }

    
    //unlock the room doors
    public void UnlockDoors(float doorUnlockDelay)
    {

        StartCoroutine(UnlockDoorsRoutine(doorUnlockDelay));

    }


    //unlock the room doors routine
    private IEnumerator UnlockDoorsRoutine(float doorUnlockDelay)
    {

        if(doorUnlockDelay > 0f)
        yield return new WaitForSeconds(doorUnlockDelay);

        Door[] doorArray = GetComponentsInChildren<Door>();

        //trigger open doors
        foreach(Door door in doorArray)
        {

            door.UnlockDoor();

        }
        
        //enable room trigger collider
        EnableRoomCollider();

    }


    //create the items obstacle array
    private void CreateItemObstaclesArray() 
    {

        //this array will be populated during gameplay with any moveable obstacles
        aStarItemObstacles = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1, room.templateUpperBounds.y - room.templateLowerBounds.y + 1];

    }


    //initialize item array with the default AStar movement plenalty values
    private void InitializeItemObstaclesArray() 
    {

        for(int x = 0; x < (room.templateUpperBounds.x - room.templateLowerBounds.x + 1); x++)
        {
            for (int y = 0; y < (room.templateLowerBounds.y - room.templateLowerBounds.y + 1); y++)
            {
                //set default movement penalty for grid squares
                aStarItemObstacles[x, y] = Settings.defaultAStarMovementPenalty;
            }
        }

    }


    //update the array of moveable obstacles
    public void UpdateMoveableObstacles() 
    {

        InitializeItemObstaclesArray();

        foreach(MoveItem moveItem in moveableItemsList)
        {
            Vector3Int colliderBoundsMin = grid.WorldToCell(moveItem.boxCollider2D.bounds.min);
            Vector3Int colliderBoundsMax = grid.WorldToCell(moveItem.boxCollider2D.bounds.max);

            //loop through and add the moveable item collider bounds to the obstacle array
            for(int i = colliderBoundsMin.x; i <= colliderBoundsMax.x; i++)
            {
                for(int j = colliderBoundsMin.y; j <= colliderBoundsMax.y; j++)
                {
                    aStarItemObstacles[i - room.templateLowerBounds.x, j - room.templateLowerBounds.y] = 0;
                }
            }
        }

    }


    //this is for DEBUGGING, it shows the position of the table obstacles, so I gotta comment this out when I am done with it
    // private void OnDrawGizmos() 
    // {

    //     for(int i = 0; i < (room.templateUpperBounds.x - room.templateLowerBounds.x + 1); i++)
    //     {
    //         for(int j = 0; j < (room.templateUpperBounds.y - room.templateLowerBounds.y + 1); j++)
    //        {
    //             if(aStarItemObstacles[i, j] == 0)
    //             {
    //                 Vector3 worldCellPos = grid.CellToWorld(new Vector3Int(i + room.templateLowerBounds.x, j + room.templateLowerBounds.y, 0));

    //                 Gizmos.DrawWireCube(new Vector3(worldCellPos.x + 0.5f, worldCellPos.y + 0.5f, 0), Vector3.one);
    //             }
    //         }
    //     }

    // }


    #region Validation
#if UNITY_EDITOR

    private void OnValidate() 
    {

        HelperUtilities.ValidateCheckNullValue(this, nameof(environmentGameObject), environmentGameObject);

    }

#endif 
    #endregion Validation

}
