using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ActivateRooms : MonoBehaviour
{
     
    #region Header POPULATE WITH MINIMAP CAMERA
    [Header("POPULATE WITH MINIMAP CAMERA")]
    #endregion Header 
    [SerializeField] private Camera miniMapCamera;


    private void Start() 
    {

        InvokeRepeating("EnableRooms", 0.5f, 0.75f);
        
    }


    private void EnableRooms() 
    {   
        //if currently showing the map UI don't process
        if(GameManager.Instance.gameState == GameState.dungeonOverviewMap)
            return;

        //iterate through dungeon rooms
        foreach(KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value; 
            
            HelperUtilities.CameraWorldPositionBounds(out Vector2Int miniMapCameraWorldPositionLowerBounds, out Vector2Int miniMapCameraWorldPositionUpperBounds, miniMapCamera);

            //if room is within minimap view then it will activate the rooms gameobject
            if((room.lowerBounds.x <= miniMapCameraWorldPositionUpperBounds.x && room.lowerBounds.y <= miniMapCameraWorldPositionUpperBounds.y) && (room.upperBounds.x >= miniMapCameraWorldPositionLowerBounds.x 
            && room.upperBounds.y >= miniMapCameraWorldPositionLowerBounds.y))
            {
                room.instantiatedRoom.gameObject.SetActive(true);
            }
            else 
            {
                room.instantiatedRoom.gameObject.SetActive(false);
            }
        }

    }


    #region Validation 
#if UNITY_EDITOR

    private void OnValidate() 
    {
        
        HelperUtilities.ValidateCheckNullValue(this, nameof(miniMapCamera), miniMapCamera);

    }
    
#endif 
    #endregion


}
