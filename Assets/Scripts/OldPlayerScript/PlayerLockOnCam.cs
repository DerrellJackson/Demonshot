using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLockOnCam : MonoBehaviour
{
   
   public Transform playerTransform;
   public float speed;

//this is locking the camera is a spot
 //  public float minX;     
 //  public float maxX;
  // public float minY;
  // public float maxY;

   private void Start()
   {//curent position of camera is the players position
        transform.position = playerTransform.position;
   }
   private void Update() 
   {//if the player is dead it wont follow
        if(playerTransform != null)
      { //float clampedX = Mathf.Clamp(playerTransform.position.x, minX, maxX);
     //   float clampedY = Mathf.Clamp(playerTransform.position.y, minY, maxY);

    //'lerp' means that it smoothly moves on one point based on the speed of it. so we get the cam/player position and the speed of it
        transform.position = Vector2.Lerp(transform.position, playerTransform.position, speed); 
    //change new Vector2() into just playerTransform.position to make if follow limitless and hide the MinX,MaxX,MinY,MaxY variables at the top of the script. Hide the clampedX, clampedY also
       }
   }
}
