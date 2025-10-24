using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

#region REQUIRE COMPONENTS

[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(DealContactDamage))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(Destroyed))]
[RequireComponent(typeof(EnemyWeaponAI))]
[RequireComponent(typeof(AimWeaponEvent))]
[RequireComponent(typeof(AimWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(ReloadWeapon))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(EnemyMovementAI))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(MovementToPosition))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(MaterializeEffect))]
[RequireComponent(typeof(AnimateEnemy))]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))] //prevent the enemy from walking through objects
[RequireComponent(typeof(PolygonCollider2D))]


#endregion REQUIRE COMPONENTS

[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
 
    [HideInInspector] public EnemyDetailsSO enemyDetails;
    private HealthEvent healthEvent;
    private Health health;
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    private FireWeapon fireWeapon;
    private SetActiveWeaponEvent setActiveWeaponEvent;
    private EnemyMovementAI enemyMovementAI;
    [HideInInspector] public MovementToPositionEvent movementToPositionEvent;
    [HideInInspector] public IdleEvent idleEvent;
    private MaterializeEffect materializeEffect;
    private CircleCollider2D circleCollider2D;
    private PolygonCollider2D polygonCollider2D;
    [HideInInspector] public SpriteRenderer[] spriteRendererArray;
    [HideInInspector] public Animator animator;

    private void Awake() 
    {

        //load components
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        fireWeapon = GetComponent<FireWeapon>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        enemyMovementAI = GetComponent<EnemyMovementAI>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        idleEvent = GetComponent<IdleEvent>();
        materializeEffect = GetComponent<MaterializeEffect>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        spriteRendererArray = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }


    private void OnEnable() 
    {

        //sub to health event
        healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;

    }


    private void OnDisable() 
    {

        //unsub to health event
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;

    }


    //handle health lost event
    private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {

        if(healthEventArgs.healthAmount <= 0) //check if health is less than or equal to zero
        {
            EnemyDestroyed(); //if true destroy enemy
        }

    }


    //enemy destroyed
    private void EnemyDestroyed() 
    {

        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(false, health.GetStartingHealth()); //the starting health is the number of points gained

    }


    //initialise the enemy 
    public void EnemyInitialization(EnemyDetailsSO enemyDetails, int enemySpawnNumber, DungeonLevelSO dungeonLevel)
    {

        this.enemyDetails = enemyDetails;
        
        SetEnemyMovementUpdateFrame(enemySpawnNumber);

        SetEnemyStartingHealth(dungeonLevel);

        SetEnemyStartingWeapon();

        //SetEnemyAnimationSpeed(); 
        //Prob won't need as my course teaching this is using a different method of drawing each frame and I am not.

        //materialise enemy
        StartCoroutine(MaterializeEnemy());

    }


    //set enemy movement update frame
    private void SetEnemyMovementUpdateFrame(int enemySpawnNumber)
    {

        //set frame number that enemy should process it's updates
        enemyMovementAI.SetUpdateFrameNumber(enemySpawnNumber % Settings.targetFrameRateToSpreadPathfindingOver);

    }


    //set the enemies health
    private void SetEnemyStartingHealth(DungeonLevelSO dungeonLevel)
    {

        //get the enemy health for the dungeon level
        foreach(EnemyHealthDetails enemyHealthDetails in enemyDetails.enemyHealthDetailsArray)
        {
            if(enemyHealthDetails.dungeonLevel == dungeonLevel)
            {
                health.SetStartingHealth(enemyHealthDetails.enemyHealthAmount);
                return;
            }
        }
        health.SetStartingHealth(Settings.defaultEnemyHealth); //set the default health if there is no entry for the health on dungeon

    }


    //set enemy starting weapon from the weaponDetailsSO
    private void SetEnemyStartingWeapon()
    {

        //process if enemy has a weapon
        if(enemyDetails.enemyWeapon != null)
        {
            Weapon weapon = new Weapon() { weaponDetails = enemyDetails.enemyWeapon, weaponReloadTimer = 0f, weaponClipRemainingAmmo = enemyDetails.enemyWeapon.weaponClipAmmoCapacity, 
            weaponRemainingAmmo = enemyDetails.enemyWeapon.weaponAmmoCapacity, isWeaponReloading = false };

            //set the weapon for the enemy
            setActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);
        }

    }


    //this will set the animator speed to move based on the speed of the enemy and I'll add the code but hide it as my enemies are animated individually through unity and I am not sure it will make much of a difference 
    /*
    private void SetEnemyAnimationSpeed() 
    {
        //set animator speed to match movement speed
        animator.speed = enemyMovementAI.moveSpeed / Settings.baseSpeedForEnemyAnimations;
    } Remember if I add this code in to go to the Settings script and define the baseSpeedForEnemyAnimations. Also check lecture 118 of Robs course if confused on why I hid this.
    */

    private IEnumerator MaterializeEnemy()
    {

        //disable the collider, movement AI, and Weapon AI
        EnemyEnable(false);

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(enemyDetails.enemyMaterializeShader, enemyDetails.enemyMaterializeColor, enemyDetails.enemyMaterializeTime,
        spriteRendererArray, enemyDetails.enemyStandardMaterial));

        //enable the collider, movement AI, and Weapon AI
        EnemyEnable(true);

    }


    private void EnemyEnable(bool isEnabled)
    {

        //Enable/Disable colliders
        circleCollider2D.enabled = isEnabled;
        polygonCollider2D.enabled = isEnabled;

        //Enable/Disable movement AI
        enemyMovementAI.enabled = isEnabled;

        //Enable/Disable Fire Weapon
        fireWeapon.enabled = isEnabled;

    }


}
