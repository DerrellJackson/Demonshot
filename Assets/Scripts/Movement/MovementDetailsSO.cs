using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails", menuName = "Scriptable Objects/Movement/Movement Details")]
public class MovementDetailsSO : ScriptableObject
{
    public static MovementDetailsSO movementDetailsSO;
    private void Awake() => movementDetailsSO = this;
    
    #region Header MOVEMENT DETAILS
    [Space(10)]
    [Header("MOVEMENT DETAILS")]
    #endregion Header 

    #region Tooltip
    [Tooltip("The minimum move speed. The GetMoveSpeed method calculates a random value between the minimum and maximum")]
    #endregion Tooltip 
    public float minMoveSpeed = 8f;
    #region Tooltip
    [Tooltip("The maximum move speed. The GetMoveSpeed method calculates a random valuse between the minimum and maximum")]
    #endregion Tooltip
    public float maxMoveSpeed = 8f;
    #region Tooltip
    [Tooltip("If there is a dash movement - this is the dash speed")]
    #endregion
    public float dashSpeed; //for player
    #region Tooltip
    [Tooltip("If there is a dash movement - this is the dash distance")]
    #endregion
    public float dashDistance; //for player
    #region Tooltip
    [Tooltip("If there is a dash movement - this is the cooldown time in seconds between dash actions")]
    #endregion
    public float dashCooldownTime; //for player


    //get a random movement speed between the minimum and maximum values
    public float GetMoveSpeed()
    {

        if (minMoveSpeed == maxMoveSpeed)
        {
            return minMoveSpeed;
        }
        else 
        {
            return Random.Range(minMoveSpeed, maxMoveSpeed);
        }

    }

    public void AddSpeed()
    {
       
        minMoveSpeed ++;
        maxMoveSpeed ++;

    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate() 
    {

        HelperUtilities.ValidateCheckPositiveRange(this, nameof(minMoveSpeed), minMoveSpeed, nameof(maxMoveSpeed), maxMoveSpeed, false);

        if (dashDistance != 0f || dashSpeed != 0 || dashCooldownTime != 0)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(dashDistance), dashDistance, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(dashSpeed), dashSpeed, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(dashCooldownTime), dashCooldownTime, false);
        }

    }
#endif
    #endregion Validation

}
