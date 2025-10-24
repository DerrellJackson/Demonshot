using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

//THIS WILL BE ACTIVATED LATER BUT FOR OTHER LEVELS
[RequireComponent(typeof(InstantiatedRoom))]
[DisallowMultipleComponent]
public class RoomLightingControl : MonoBehaviour
{
    
    private InstantiatedRoom instantiatedRoom;


    private void Awake() 
    {

        //load components 
        instantiatedRoom = GetComponent<InstantiatedRoom>();

    }


    //subscribe to event
    private void OnEnable() 
    {

        //subscribe to room changed event
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;

    }


    //unsubscribe from event
    private void OnDisable() 
    {

        //unsubcribe from room changed event
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

    }


    //handle room changed event
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {

        //if this is the room entered and not the room that is already lit, then it will fade in the room lighting
        if(roomChangedEventArgs.room == instantiatedRoom.room && !instantiatedRoom.room.isLit)
        {
            //fade in room
            FadeInRoomLighting();

            //fade in the room doors lighting
            FadeInDoors();

            instantiatedRoom.room.isLit = true;
        }

    }


    //fade in the room lighting
    private void FadeInRoomLighting() 
    {

        //fade in the lighting for the room tilemaps
        StartCoroutine(FadeInRoomLightingRoutine(instantiatedRoom));

    }


    //fade in the room lighting coroutine
    private IEnumerator FadeInRoomLightingRoutine(InstantiatedRoom instantiatedRoom)
    {

        //creates the new material that needs to be faded in 
        Material material = new Material(GameResources.Instance.variableLitShader);

        /*instantiatedRoom.groundTilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.decoration1Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.decoration2Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.frontTilemap.GetComponent<TilemapRenderer>().material = material;*/
        instantiatedRoom.minimapTilemap.GetComponent<TilemapRenderer>().material = material;

        for(float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }


        //set material back to lit material
        /*instantiatedRoom.groundTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.decoration1Tilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.decoration2Tilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.frontTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;*/
        instantiatedRoom.minimapTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;

    }


    //fade in the doors
    private void FadeInDoors()
    {

        Door[] doorArray = GetComponentsInChildren<Door>();

        foreach(Door door in doorArray)
        {
            DoorLightingControl doorLightingControl = door.GetComponentInChildren<DoorLightingControl>();

            doorLightingControl.FadeInDoor(door);
        }

    }


}
