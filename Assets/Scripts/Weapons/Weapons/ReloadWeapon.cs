using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ReloadWeapon : MonoBehaviour
{
   
    private ReloadWeaponEvent reloadWeaponEvent;
    private WeaponReloadedEvent weaponReloadedEvent;
    private SetActiveWeaponEvent setActiveWeaponEvent;
    private Coroutine reloadWeaponCoroutine;

    private void Awake() 
    {

        //load components
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>(); 

    }


    private void OnEnable() 
    {

        //subscribe to the reload weapon event
        reloadWeaponEvent.OnReloadWeapon += ReloadWeaponEvent_OnReloadWeapon;

        //subscribe to the active weapon event
        setActiveWeaponEvent.OnSetActiveWeapon += SetActiveWeaponEvent_OnSetActiveWeapon;

    }


    private void OnDisable() 
    {

        //unsubscribe to the reload weapon event
        reloadWeaponEvent.OnReloadWeapon -= ReloadWeaponEvent_OnReloadWeapon;

        //unsubscribe to the active weapon event
        setActiveWeaponEvent.OnSetActiveWeapon -= SetActiveWeaponEvent_OnSetActiveWeapon;

    }


    //handle weapon reload event
    private void ReloadWeaponEvent_OnReloadWeapon(ReloadWeaponEvent reloadWeaponEvent, ReloadWeaponEventArgs reloadWeaponEventArgs)
    {

        StartReloadWeapon(reloadWeaponEventArgs);

    }


    //start reloading the weapon
    private void StartReloadWeapon(ReloadWeaponEventArgs reloadWeaponEventArgs)
    {

        if(reloadWeaponCoroutine != null)
        {
            StopCoroutine(reloadWeaponCoroutine);
        }

        reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(reloadWeaponEventArgs.weapon, reloadWeaponEventArgs.topUpAmmoPercent));

    }


    //reload weapon coroutine
    private IEnumerator ReloadWeaponRoutine(Weapon weapon, int topUpAmmoPercent)
    {

        //play reload sound if there is one to be players
        if(weapon.weaponDetails.weaponReloadingSoundEffect != null)
        {
            SoundEffectManager.Instance.PlaySoundEffect(weapon.weaponDetails.weaponReloadingSoundEffect);
        }

        //set weapon as reloading
        weapon.isWeaponReloading = true;

        //update reload progress timer
        while(weapon.weaponReloadTimer < weapon.weaponDetails.weaponReloadTime)
        {
            weapon.weaponReloadTimer += Time.deltaTime;
            yield return null;
        }

        //if total ammo is to be increased then update
        if(topUpAmmoPercent != 0)
        {
            int ammoIncrease = Mathf.RoundToInt((weapon.weaponDetails.weaponAmmoCapacity * topUpAmmoPercent) / 100f);

            int totalAmmo = weapon.weaponRemainingAmmo + ammoIncrease;

            //checking if the total ammo is greater than the capacity before adding it in
            if(totalAmmo > weapon.weaponDetails.weaponAmmoCapacity)
            {
                weapon.weaponRemainingAmmo = weapon.weaponDetails.weaponAmmoCapacity;
            }
            else 
            {
                weapon.weaponRemainingAmmo = totalAmmo;
            }
        }

        //if the weapon has infinite ammo then it will just refill the clip everytime
        if(weapon.weaponDetails.hasInfiniteAmmo)
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponDetails.weaponClipAmmoCapacity; //fills up ammo
        }

        //if it is not infinite ammo then if the remaining ammo is greater than the amount required to refill the clip then it will fully refill
        else if (weapon.weaponRemainingAmmo >= weapon.weaponDetails.weaponClipAmmoCapacity)
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponDetails.weaponClipAmmoCapacity;
        }
        //else set the clip to the remaining ammo
        else
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponRemainingAmmo;
        }

        //reset weapon reloading timer
        weapon.weaponReloadTimer = 0f;

        //set the weapon as not reloading
        weapon.isWeaponReloading = false;

        //call weapon reloaded event to the weapon
        weaponReloadedEvent.CallWeaponReloadedEvent(weapon);

    }


    //set active weapon event handler
    private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent setActiveWeaponEvent, SetActiveWeaponEventArgs setActiveWeaponEventArgs)
    {

        if(setActiveWeaponEventArgs.weapon.isWeaponReloading)
        {
            if(reloadWeaponCoroutine != null)
            {
                StopCoroutine(reloadWeaponCoroutine);
            }

            reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(setActiveWeaponEventArgs.weapon, 0));
        }

    }

}
