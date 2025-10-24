using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMap : SingletonMonobehaviour<DungeonMap>
{
   
    #region Header GameObject References 
    [Space(10)]
    [Header("GameObject References")]
    #endregion 

    #region Tooltip
    [Tooltip("Put the Minimap UI gameobject here")]
    #endregion
    [SerializeField] private GameObject minimapUI;

    private Camera dungeonMapCamera;
    private Camera cameraMain;

    private void Start() 
    {

        //cache the main cam 
        cameraMain = Camera.main;

        //get the players transform
        Transform playerTransform = GameManager.Instance.GetPlayer().transform;

        //populate player as the cinemachines target
        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = playerTransform;

        //get the dungeon maps camera
        dungeonMapCamera = GetComponentInChildren<Camera>();
        dungeonMapCamera.gameObject.SetActive(false);

    }


    private void Update()
    {

        //if mouse button pressed and the gamestate is dungeon map then get the room clicked
        if(Input.GetMouseButtonDown(0) && GameManager.Instance.gameState == GameState.dungeonOverviewMap)
        {
            GetRoomClicked();
        }

    }


    //get the room clicked on
    private void GetRoomClicked() 
    {

        //convert screen position to the world position
        Vector3 worldPosition = dungeonMapCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);

        //check for collisions at cursor point
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(new Vector2(worldPosition.x, worldPosition.y), 1f);

        //check if any of the colliders are a room
        foreach(Collider2D collider2D in collider2DArray)
        {
            if(collider2D.GetComponent<InstantiatedRoom>() != null)
            {
                InstantiatedRoom instantiatedRoom = collider2D.GetComponent<InstantiatedRoom>();

                //if clicked room is clear of enemies and was already visited at least once
                if(instantiatedRoom.room.isClearedOfEnemies && instantiatedRoom.room.isPreviouslyVisited)
                {   
                    //teleportation only works in corridors
                     if( instantiatedRoom.room.isPreviouslyVisited && instantiatedRoom.room.roomNodeType.isCorridorEW ||
                    instantiatedRoom.room.isPreviouslyVisited && instantiatedRoom.room.roomNodeType.isCorridorNS) //REMEMBER TO ADD CORRIDOR TP LOCATIONS FOR PLAYER
                    {
                    //move player to room
                    StartCoroutine(MovePlayerToRoom(worldPosition, instantiatedRoom.room));
                    }
                }
            }
        }

    }


    //move the player to the selected room
    private IEnumerator MovePlayerToRoom(Vector3 worldPosition, Room room)
    {

        //call room changed event
        StaticEventHandler.CallRoomChangedEvent(room);

        //fade out screen to black immediately
        yield return StartCoroutine(GameManager.Instance.Fade(0f, 1f, 0f, Color.black));

        //clear dungeon map
        ClearDungeonOverViewMap();

        //disable the player during the teleport
        GameManager.Instance.GetPlayer().playerControl.DisablePlayer();

        ///get nearest spawn point loaction to teleport to
        //Vector3 spawnPosition = HelperUtilities.GetSpawnPositionNearestToPlayer(worldPosition);

        //move the player to the new location (at the closest point that the player clicked)
        //GameManager.Instance.GetPlayer().transform.position = spawnPosition;

        //fade the screen back in
        yield return StartCoroutine(GameManager.Instance.Fade(1f, 0f, 1f, Color.black));

        //enable player again
        GameManager.Instance.GetPlayer().playerControl.EnablePlayer();

    }


    //display the overview map ui
    public void DisplayDungeonOverViewMap() 
    {

        //set the game state
        GameManager.Instance.previousGameState = GameManager.Instance.gameState;
        GameManager.Instance.gameState = GameState.dungeonOverviewMap;

        //disable the player
        GameManager.Instance.GetPlayer().playerControl.DisablePlayer();

        //disable main camera and enable the dungeons camera
        cameraMain.gameObject.SetActive(false);
        dungeonMapCamera.gameObject.SetActive(true);

        //ensure all rooms are active so they can be displayed
        ActivateRoomsForDisplay();

        //disable small minimap ui 
        minimapUI.SetActive(false);

    }


    //clear dungeon overview map ui
    public void ClearDungeonOverViewMap()
    {

        //set game state
        GameManager.Instance.gameState = GameManager.Instance.previousGameState;
        GameManager.Instance.previousGameState = GameState.dungeonOverviewMap;


        //enable player
        GameManager.Instance.GetPlayer().playerControl.EnablePlayer();

        //enable main camera and disable the dungeons camera
        cameraMain.gameObject.SetActive(true);
        dungeonMapCamera.gameObject.SetActive(false);

        //enable small minimap Ui
        minimapUI.SetActive(true);

    }


    //make sure all rooms are active so they can be displayed
    private void ActivateRoomsForDisplay()
    {

        //go through all dungeon rooms
        foreach(KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {

            Room room = keyValuePair.Value;

            room.instantiatedRoom.gameObject.SetActive(true);
            
        }

    }

}
