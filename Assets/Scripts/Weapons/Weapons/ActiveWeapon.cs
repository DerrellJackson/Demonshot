using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
    
    #region Tooltip
    [Tooltip("Populate with the SpriteRenderer on the child Weapon gameobject")]
    #endregion
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

    #region Tooltip
    [Tooltip("Populate with the Collider on the child weapon gameobject")]
    #endregion
    [SerializeField] private PolygonCollider2D weaponPolygonCollider2D;

    #region Tooltip 
    [Tooltip("Populate with the transform on the WeaponShootPosition gameobject")]
    #endregion
    [SerializeField] private Transform weaponShootPositionTransform;

    #region Tooltip
    [Tooltip("Populate with the transform on the WeaponEffectPosition gameobject")]
    #endregion
    [SerializeField] private Transform weaponEffectPositionTransform;

    private SetActiveWeaponEvent setWeaponEvent;
    private Weapon currentWeapon;


    private void Awake() 
    {

        //load components
        setWeaponEvent = GetComponent<SetActiveWeaponEvent>();

    }


    private void OnEnable() 
    {

        //subscribe to event
        setWeaponEvent.OnSetActiveWeapon += SetWeaponEvent_OnSetActiveWeapon;

    }


    private void OnDisable() 
    {

        //unsubscribe to event
        setWeaponEvent.OnSetActiveWeapon  -= SetWeaponEvent_OnSetActiveWeapon;

    }


    private void SetWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent setActiveWeaponEvent, SetActiveWeaponEventArgs setActiveWeaponEventArgs)
    {

        SetWeapon(setActiveWeaponEventArgs.weapon);

    }


    private void SetWeapon(Weapon weapon)
    {

        currentWeapon = weapon;

        //set the current weapons sprite
        weaponSpriteRenderer.sprite = currentWeapon.weaponDetails.weaponSprite;

        //if the weapon has a polygon collider and a sprite then set it to the weapon sprite physics
        if(weaponPolygonCollider2D != null && weaponSpriteRenderer.sprite != null)
        {
            //get sprite physics shape - this returns the sprite physics shape points as a list of vector2's
            List<Vector2> spritePhysicsShapePointsList = new List<Vector2>();
            weaponSpriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList);

            //set polygon collider on weapon to pick up the physics shape for the sprite - set collider points to sprite physics shape points
            weaponPolygonCollider2D.points = spritePhysicsShapePointsList.ToArray();
        }

        //set the weapons shoot position
        weaponShootPositionTransform.localPosition = currentWeapon.weaponDetails.weaponShootPosition; //currentWeapon.weaponDetails.weaponShootPosition was what it was

    }


    //get the current ammo in the weapon
    public AmmoDetailsSO GetCurrentAmmo()
    {

        return currentWeapon.weaponDetails.weaponCurrentAmmo; //unfinished till later lesson

    }


    //get the current weapon
    public Weapon GetCurrentWeapon()
    {
        
        return currentWeapon; //unfinished till later lesson
    
    }


    //get the shoot position
    public Vector3 GetShootPosition() 
    {

        return weaponShootPositionTransform.position; //unfinished till later lesson
        
    }


    //get the shoot effects position
    public Vector3 GetShootEffectPosition()
    {

        return weaponEffectPositionTransform.position;

    }


    public void RemoveCurrentWeapon()
    {

        currentWeapon = null;

    }


    #region  Validation
#if UNITY_EDITOR

    private void OnValidate()
    {

        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponSpriteRenderer), weaponSpriteRenderer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponPolygonCollider2D), weaponPolygonCollider2D);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponShootPositionTransform), weaponShootPositionTransform);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponEffectPositionTransform), weaponEffectPositionTransform);

    }

#endif
    #endregion

}