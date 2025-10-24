using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerDetailsSO", menuName = "Scriptable Objects/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject 
{  

    #region Header PLAYER BASE DETAILS
  [Space(10)]
  [Header("PLAYER BASE DETAILS")]
  #endregion


    #region Tooltip
  [Tooltip("Player character name.")]
  #endregion
    public string playerCharacterName;

    #region Tooltip
    [Tooltip("Prefab gameobject for the player")]
    #endregion
    public GameObject playerPrefab;

    #region Tooltip
    [Tooltip("Player Runtime animator controller")]
    #endregion
    public RuntimeAnimatorController runtimeAnimatorController;

    #region Header PLAYER HEALTH
    [Space(10)]
    [Header("PLAYER HEALTH")]
    #endregion

    #region Tooltip
    [Tooltip("The health amount that the player starts with")]
    #endregion
    public int playerHealthAmount;

    #region Tooltip
    [Tooltip("Select if the GameObject has an immunity period immediately after being hit. If so specify the immunity time in seconds in the other field")]
    #endregion
    public bool isImmuneAfterHit = false;

    #region Tooltip
    [Tooltip("Immunity time in seconds after being hit")]
    #endregion 
    public float hitImmunityTime;

    #region Header WEAPON
    [Space(10)]
    [Header("WEAPON")]
    #endregion


    #region Tooltip 
    [Tooltip("Player starting weapon (default weapon but can be changed by pickups)")]
    #endregion
    public WeaponDetailsSO startingWeapon;

    #region Tooltip 
    [Tooltip("Populate the list of starting weapons")]
    #endregion
    public List<WeaponDetailsSO> startingWeaponList;
    

    #region Header OTHER
    [Space(10)]
    [Header("OTHER")]
    #endregion


    #region Tooltip
    [Tooltip("Player icone sprite to be used in the minimap")]
    #endregion
    public Sprite playerMinimapIcon;

    #region Tooltip
    [Tooltip("Player hand sprite")]
    #endregion
    public Sprite playerHandSprite;


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {

        HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmount), playerHealthAmount, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(startingWeapon), startingWeapon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMinimapIcon), playerMinimapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
        HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(startingWeaponList), startingWeaponList);

        if(isImmuneAfterHit)
        {
          HelperUtilities.ValidateCheckPositiveValue(this, nameof(hitImmunityTime), hitImmunityTime, false);
        }

    }   
#endif
    #endregion

}
