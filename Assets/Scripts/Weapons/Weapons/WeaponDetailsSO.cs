using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapons/Weapon Details")]
public class WeaponDetailsSO : ScriptableObject 
{
    
    #region Header WEAPON BASE DETAILS
    [Space(10)]
    [Header("WEAPON BASE DETAILS")]
    #endregion Header WEAPON BASE DETAILS
    
    #region Tooltip
    [Tooltip("Weapon name")]
    #endregion Tooltip
    public string weaponName;
   
    #region Tooltip
    [Tooltip("The sprite for the weapon - the sprite should have the 'generate physics shape' option selected ")]
    #endregion Tooltip
    public Sprite weaponSprite;

  
    #region Header WEAPON CONFIGURATION
    [Space(10)]
    [Header("WEAPON CONFIGURATION")]
    #endregion Header WEAPON CONFIGURATION
    
    #region Tooltip
    [Tooltip("Weapon Shoot Position - the offset position for the end of the weapon from the sprite pivot point")]
    #endregion Tooltip
    public Vector3 weaponShootPosition;
    
    #region Tooltip
    [Tooltip("Weapon current ammo")]
    #endregion Tooltip
    public AmmoDetailsSO weaponCurrentAmmo;

    #region Tooltip
    [Tooltip("Weapon shoot effect SO, it contains the particle effect parameters used in conjunction with the weaponShootEffectPrefab")]
    #endregion Tooltip
    public WeaponShootEffectSO weaponShootEffect;

    #region Tooltip
    [Tooltip("The firing sound effect of the weapon")] //optional, not in validation
    #endregion Tooltip
    public SoundEffectSO weaponFiringSoundEffect;

    #region Tooltip
    [Tooltip("The reloading sound effect of the weapon")]
    #endregion Tooltip
    public SoundEffectSO weaponReloadingSoundEffect; //optional, not in validation

    #region Header WEAPON OPERATING VALUES
    [Space(10)]
    [Header("WEAPON OPERATING VALUES")]
    #endregion Header WEAPON OPERATING VALUES
    #region Tooltip
    [Tooltip("Select if the weapon has infinite ammo")]
    #endregion Tooltip
    public bool hasInfiniteAmmo = false;
    //NOTE: I AM PLANNING MOST WEAPONS TO HAVE THIS CHECKED BUT I WILL HAVE CERTAIN ITEMS W AN AMMO LIMIT AS WELL
    //THERE WILL BE THE STARTING WEAPON THAT A PLAYER GOES TO ONCE THEY DO RUN OUT OF AMMO

    public float weaponReloadTime; 
    //Note: the weaponReloadTime was added without watching the videos so if errors then WATCH THEM.

    #region Tooltip 
    [Tooltip("Select if the weapon has infinite clip capacity")]
    #endregion Tooltip 
    public bool hasInfiniteClipCapacity;
    //MOST WILL BE TICKED WITH UNLIMITED AMMO EXCEPT OVERPOWERED WEAPONS

    #region Tooltip
    [Tooltip("The weapon capacity - shots before a reload")]
    #endregion Tooltip
    public int weaponClipAmmoCapacity = 1;
    //CHANGE THE INT TO THE NUMBER OF AMMO BEFORE RELOAD
    #region Tooltip
    [Tooltip("Weapon ammo capacity - the maximum number of rounds that can be held for the weapon (total capacity till 0 bullets left)")]
    #endregion Tooltip
    public int weaponAmmoCapacity = 100;
    //CHANGE THE INT TO THE NUMBER OF BULLETS TILL YOU ARE EMPTY
    #region Tooltip
    [Tooltip("Weapon fire rate - weapon fire speed - 0.2 means 5 shots a second")]
    #endregion Tooltip
    public float weaponFireRate = 0.2f;
    #region Tooltip
    [Tooltip("Weapon precharge time - the time in seconds that needs to be held till weapon fires")]
    #endregion Tooltip
    public float weaponPrechargeTime = 0f;
    //MOST WEAPONS WILL BE SET AT DEFUALT BUT STRONG WEAPONS MAY NEED TO CHARGE BEFORE FIRING


    #region Validation
#if UNITY_EDITOR
    private void OnValidate() 
    {

        HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if(!hasInfiniteAmmo)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        }
        if(!hasInfiniteClipCapacity)
        {
            HelperUtilities.ValidateCheckPositiveValue(this,nameof(weaponClipAmmoCapacity), weaponClipAmmoCapacity, false);
        }

    }
#endif
    #endregion


}
