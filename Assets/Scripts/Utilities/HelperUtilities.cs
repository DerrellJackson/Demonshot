using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities
{

    public static Camera mainCamera;

    //get the mouse world position
    public static Vector3 GetMouseWorldPosition()
    {

        if(mainCamera == null) mainCamera = Camera.main;

        Vector3 mouseScreenPosition = Input.mousePosition;

        //clamp mouse position to screen size
        mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, 0f, Screen.width);
        mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, 0f, Screen.height);

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        worldPosition.z = 0f;

        return worldPosition;

    }


    //get the camera viewport lower and upper bounds
    public static void CameraWorldPositionBounds(out Vector2Int cameraWorldPositionLowerBounds, out Vector2Int cameraWorldPositionUpperBounds, Camera camera)
    {

        Vector3 worldPositionViewportBottomLeft = camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 worldPositionViewportTopRight = camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        cameraWorldPositionLowerBounds = new Vector2Int((int)worldPositionViewportBottomLeft.x, (int)worldPositionViewportBottomLeft.y);
        cameraWorldPositionUpperBounds = new Vector2Int((int)worldPositionViewportTopRight.x, (int)worldPositionViewportTopRight.y);

    }


    //get the angle in degrees from a direction vector
    public static float GetAngleFromVector(Vector3 vector)
    {

        float radians = Mathf.Atan2(vector.y, vector.x);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;

    }

    
    //get the direction vector from an angle in degrees
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {

        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return directionVector;

    }

    
    //get AimDirection enum value from the angleDegrees, "//" if it doesn't work with the unprofessional drawing way and use the Old Aiming method script parts
    public static AimDirection GetAimDirection(float angleDegrees)
    {

        AimDirection aimDirection;

        //set player direction

        //UpRight
        if(angleDegrees >= 22f && angleDegrees <= 67f)
        {
            aimDirection = AimDirection.UpRight;
        }

        //Up
        else if (angleDegrees > 67f && angleDegrees <= 112f)
        {
            aimDirection = AimDirection.Up;
        }

        //UpLeft
        else if (angleDegrees > 112f && angleDegrees <= 158f)
        {
            aimDirection = AimDirection.UpLeft;
        }

        //Left
        else if ((angleDegrees <= 180f && angleDegrees > 158f) || (angleDegrees > -180 && angleDegrees <= -135f))
        {
            aimDirection = AimDirection.Left;
        }

        //Down
        else if ((angleDegrees > -135f && angleDegrees <= -45f))
        {
            aimDirection = AimDirection.Down;
        }

        //Right
        else if ((angleDegrees > -45f && angleDegrees <= 0f) || (angleDegrees > 0 && angleDegrees < 22f))
        {
            aimDirection = AimDirection.Right;
        }

        //if it does not register correctly
        else 
        {
            aimDirection = AimDirection.Right;
        }
        
        return aimDirection;

    }


    //convert the linear volume scale to decibels
    public static float LinearToDecibels(int linear)
    {

        float linearScaleRange = 20f;

        //formula to convert from the linear scale to the logarithmic decibel scale
        return Mathf.Log10((float)linear / linearScaleRange) * 20f; //math beyond me, but basically makes it go into decibels

    }

    
    //validation method
    //pass an object, string, and checking the string
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
   {

    if(stringToCheck == "")
    {
        Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
        return true;
    }
    return false;
   
   }


    //null value debug check
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, UnityEngine.Object objectToCheck)
    {

        if(objectToCheck == null)
        {
            Debug.Log(fieldName + " is null and must contain a value in object " + thisObject.name.ToString());
            return true;
        }
        return false;

    }


   //passing in the object, field name, and enumerable such as lists/arrays 
   public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
   {
  
    bool error = false;
    int count = 0;

    //Check if null the enumerableObjectToCheck is null (not the item in the object to check)
    if(enumerableObjectToCheck == null)
    {
        Debug.Log(fieldName + " is null in object " + thisObject.name.ToString());
        return true;
    }

    foreach (var item in enumerableObjectToCheck)//check to see if it is null and if it is prints to the console
    {
        if (item == null)
        {
            Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
            error = true;
        }
        else
        {
            count++;
        }
      }
        //checks if we have 0 values and if we do then we have an error
       if(count == 0)
       {
        Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
        error = true;
       }
       return error;
   
   }


    //positive value debug check - if zero is allowed set isZeroAllowed to true, returns true if there is an error
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueToCheck, bool isZeroAllowed)
    {

        bool error = false;

        if(isZeroAllowed)
        {
            if(valueToCheck < 0)
            {
                Debug.Log(fieldName + " must contain a positive value or zero in object " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if(valueToCheck <= 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                error = true;
            }
        }
        return error;

    }


   //positive value debug check - if zero is allowed set isZeroAllowed to true, returns true if there is an error
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, float valueToCheck, bool isZeroAllowed)
    {

        bool error = false;

        if(isZeroAllowed)
        {
            if(valueToCheck < 0)
            {
                Debug.Log(fieldName + " must contain a positive value or zero in object " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if(valueToCheck <= 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                error = true;
            }
        }
        return error;

    }


    //positive range debug check - set isZeroAllowed to true if the min and max range values can both be zero. Returns true if there is an error
    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMinimum, float valueToCheckMinimum, string fieldNameMaximum, float valueToCheckMaximum, bool isZeroAllowed)
    {

        bool error = false;
        if(valueToCheckMinimum > valueToCheckMaximum)
        {
            Debug.Log(fieldNameMinimum + " must be lass than or equal to " + fieldNameMaximum + " in object " + thisObject.name.ToString());
            error = true;
        }
        if(ValidateCheckPositiveValue(thisObject, fieldNameMinimum, valueToCheckMinimum, isZeroAllowed)) error = true;
        if(ValidateCheckPositiveValue(thisObject, fieldNameMaximum, valueToCheckMaximum, isZeroAllowed)) error = true;

        return error;

    }


    //same as above but with INTS
    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMinimum, int valueToCheckMinimum, string fieldNameMaximum, int valueToCheckMaximum, bool isZeroAllowed)
    {

        bool error = false;
        if(valueToCheckMinimum > valueToCheckMaximum)
        {
            Debug.Log(fieldNameMinimum + " must be lass than or equal to " + fieldNameMaximum + " in object " + thisObject.name.ToString());
            error = true;
        }
        if(ValidateCheckPositiveValue(thisObject, fieldNameMinimum, valueToCheckMinimum, isZeroAllowed)) error = true;
        if(ValidateCheckPositiveValue(thisObject, fieldNameMaximum, valueToCheckMaximum, isZeroAllowed)) error = true;

        return error;

    }


    //get the nearest spawn position to the player
    /*public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPosition)
    {

        Room currentRoom = OldGameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0f);

        //loop through all the room spawn positions
        foreach(Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
        {
            //convert the spawn grid to world positions
            Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

            if(Vector3.Distance(spawnPositionWorld, playerPosition) < Vector3.Distance(nearestSpawnPosition, playerPosition))
            {
                nearestSpawnPosition = spawnPositionWorld;
            }
        }

        return nearestSpawnPosition;        

    }*/

}
