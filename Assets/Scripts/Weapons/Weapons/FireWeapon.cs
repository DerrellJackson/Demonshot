using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]

[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{

    private float firePreChargeTimer = 0f; 
    private float fireRateCoolDownTimer = 0f;
    private ActiveWeapon activeWeapon;
    private FireWeaponEvent fireWeaponEvent;
    private ReloadWeaponEvent reloadWeaponEvent;
    private WeaponFiredEvent weaponFiredEvent;

    private void Awake() 
    {

        //load components
        activeWeapon = GetComponent<ActiveWeapon>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();

    }


    private void OnEnable() 
    {

        //subscribe to the fire weapon event
        fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;

    }


    private void OnDisable() 
    {

        //unsubscribe to the fire weapon event
        fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;

    }


    private void Update() 
    {

        //decrease the cooldown time
        fireRateCoolDownTimer -= Time.deltaTime;

    }


    //handle the fire weapon event
    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent fireWeaponEvent, FireWeaponEventArgs fireWeaponEventArgs)
    {

        WeaponFire(fireWeaponEventArgs);

    }


    //fire the weapon
    private void WeaponFire(FireWeaponEventArgs fireWeaponEventArgs)
    {

        //handle the weapon precharge timer
        WeaponPreCharge(fireWeaponEventArgs); //fireWeaponEventArgs checks if weapon was fired previous frame

        //weapon fire
        if(fireWeaponEventArgs.fire) //if the boolean in the fireWeaponEventArgs is true (fire) then it will fire 
        {
            //test if the weapon is ready to fire first
            if(IsWeaponReadyToFire())
            {
                FireAmmo(fireWeaponEventArgs.aimAngle, fireWeaponEventArgs.weaponAimAngle, fireWeaponEventArgs.weaponAimDirectionVector);

                ResetCoolDownTimer();
                
                ResetPrechargeTimer(); //resets when not shooting
            }
        }

    }


    //handle the weapon precharge
    private void WeaponPreCharge(FireWeaponEventArgs fireWeaponEventArgs)
    {

        //weapon precharge
        if(fireWeaponEventArgs.firePreviousFrame)
        {
            //decrease precharge timer if fire button held previous frame
            firePreChargeTimer -= Time.deltaTime;
        }
        else 
        {
            //reset the precharge timer when it is not decreasing bc the previous frame
            ResetPrechargeTimer();
        }

    }


    //returns true if the weapon is ready to fire otherwise it will return false
    private bool IsWeaponReadyToFire()
    {

        //if there is no ammo and the weapon does not have infinite ammo then it will return false
        if(activeWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteAmmo)
        return false;

        //if the weapon is reloading then return false
        if(activeWeapon.GetCurrentWeapon().isWeaponReloading)
        return false;

        //if the weapon is cooling down or is not precharged then false
        if(firePreChargeTimer > 0f || fireRateCoolDownTimer > 0f)
        return false;

        //if no ammo is in the clip and the weapon does not have unlimited clip capacity then it will return false
        if(!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity && activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo <= 0)
        {
            //trigger a reload weapon event
            reloadWeaponEvent.CallReloadWeaponEvent(activeWeapon.GetCurrentWeapon(), 0);
            
            return false;
        }
        

        //weapon is ready so it will return true
        return true;

    }


    //set up ammo using ammo gameobject and component from the object pool
    private void FireAmmo(float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {

        AmmoDetailsSO currentAmmo = activeWeapon.GetCurrentAmmo();

        if(currentAmmo != null)
        {
           //fire ammo routine is called
           StartCoroutine(FireAmmoRoutine(currentAmmo, aimAngle, weaponAimAngle, weaponAimDirectionVector));
        }

    }


    //the coroutine to spawn multiple shots at once in the ammo details
    private IEnumerator FireAmmoRoutine(AmmoDetailsSO currentAmmo, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        int ammoCounter = 0;

        //get random ammo per shot 
        int ammoPerShot = Random.Range(currentAmmo.ammoSpawnAmountMin, currentAmmo.ammoSpawnAmountMax + 1);

        //get random interval between ammo
        float ammoSpawnInterval;

        if(ammoPerShot > 1)
        {
            ammoSpawnInterval = Random.Range(currentAmmo.ammoSpawnIntervalMin, currentAmmo.ammoSpawnIntervalMax);
        }
        else 
        {
            ammoSpawnInterval = 0f;
        }

        //loop for the number of ammo per shot
        while (ammoCounter < ammoPerShot)
        {
        ammoCounter++;

        //get the ammo prefab from the array
        GameObject ammoPrefab = currentAmmo.ammoPrefabArray[Random.Range(0, currentAmmo.ammoPrefabArray.Length)];

        //get random speed value
        float ammoSpeed = Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);

        //get gameobject with IFireable component
        IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);

        //initialise ammo
        ammo.InitialiseAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector); 

        //wait for ammo per shot timegap
        yield return new WaitForSeconds(ammoSpawnInterval);  
        }

        //reduce ammo clip count if not infinite clip capacity
        if(!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
        {
            activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo--;
            activeWeapon.GetCurrentWeapon().weaponRemainingAmmo--;
        }

        //call weapon fired event
        weaponFiredEvent.CallWeaponFiredEvent(activeWeapon.GetCurrentWeapon());

        //display the weapons shoot effect
        WeaponShootEffect(aimAngle);

        //weapon fired sound effect
        WeaponSoundEffect();

    }


    //reset cooldown timer
    private void ResetCoolDownTimer()
    {

        //reset the timer
        fireRateCoolDownTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponFireRate;

    }


    //reset precharge timer
    private void ResetPrechargeTimer()
    {

        //reset the precharge timer
        firePreChargeTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponPrechargeTime;

    }

    //displays the weapons shoot effect
    private void WeaponShootEffect(float aimAngle)
    {

        //process if there is a shoot effect and prefab
        if(activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect != null && activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect.weaponShootEffectPrefab != null)
        {
            //get the weapons shoot effect gameobject from the pool with particle system component
            WeaponShootEffect weaponShootEffect = (WeaponShootEffect)PoolManager.Instance.ReuseComponent
            (activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect.weaponShootEffectPrefab, activeWeapon.GetShootEffectPosition(), Quaternion.identity);

            //set the shoot effect
            weaponShootEffect.SetShootEffect(activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect, aimAngle);

            //set gameobject active bc it is auto diabled once finished
            weaponShootEffect.gameObject.SetActive(true);
        }

    }


    //play weapon shooting sound effect
    private void WeaponSoundEffect() 
    {

        if(activeWeapon.GetCurrentWeapon().weaponDetails.weaponFiringSoundEffect != null)
        {
            SoundEffectManager.Instance.PlaySoundEffect(activeWeapon.GetCurrentWeapon().weaponDetails.weaponFiringSoundEffect);
        }

    }

}
