using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    #region Tooltip
    [Tooltip("MovementDetailsSO scriptable object containing details such as speed")]
    #endregion Tooltip 

    [SerializeField] private MovementDetailsSO movementDetails;
   
    #region Tooltip
    [Tooltip("The player WeaponShootPosition gameobject in the hierarchy")]
    #endregion Tooltip

    [SerializeField] private Transform weaponShootPosition;

    private Player player;
    private bool leftMouseDownPreviousFrame = false;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private Coroutine playerDashCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;
    private float playerDashCooldownTimer = 0f;
    private bool isPlayerMovementDisabled = false;
    
    [HideInInspector] public bool isPlayerDashing = false;
  
    


    private void Awake() 
    {

        //load components
        player = GetComponent<Player>();

        moveSpeed = movementDetails.GetMoveSpeed();

    }


    private void Start()
    {

        //create the wait for fixed update so it can be used in coroutine
        waitForFixedUpdate = new WaitForFixedUpdate();
        
        //set starting weapon
       // SetStartingWeapon();

    }

    //disabled for new inventory system
    //set the players starting weapon
    /*private void SetStartingWeapon()
    {

        int index = 1;

        //loop through all the weapons in the index
        foreach(Weapon weapon in player.weaponList)
        {
            if(weapon.weaponDetails == player.playerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }
            index++;
        }

    }
*/

    private void Update() 
    {
        //if the player movement is disabled then return
        if(isPlayerMovementDisabled) return;

        //if the player is dashing then return
        if(isPlayerDashing) return;

        //process the player movement input
        MovementInput();

        //process the player weapon input
       // WeaponInput();

        //process player use item input
        UseItemInput();

        //the player dash cooldown timer
        PlayerDashCooldownTimer();

    }


    //player movement input
    private void MovementInput() 
    {

        //get movement input
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        bool spaceBarPressed = Input.GetKey(KeyCode.Space);

        //create a direction vector based on the input
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        //adjust distance for diagonal movement (pythagoras approximation)
        if(horizontalMovement != 0f && verticalMovement != 0f)
        {
            direction *= 0.7f;
        }

        //if there is movement either move or dash
        if(direction != Vector2.zero)
        {
            if(!spaceBarPressed)
            {
            //trigger movement event
            player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            //else if the player dash is not cooled down
            else if (playerDashCooldownTimer <= 0f) 
            {
                PlayerDash((Vector3)direction);
            }
        }
        
        //trigger idle event
        else 
        {
            player.idleEvent.CallIdleEvent();
        }

    }


    //player dash
    private void PlayerDash(Vector3 direction)
    {

        playerDashCoroutine = StartCoroutine(PlayerDashRoutine(direction));

    }


    //player dash coroutine
    private IEnumerator PlayerDashRoutine(Vector3 direction)
    {

        //minDistance used to decide when to exit coroutine loop
        float minDistance = 0.2f;

        isPlayerDashing = true;

        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.dashDistance;

        while(Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.dashSpeed, direction, isPlayerDashing);

            //yield and wait for fixed update
            yield return waitForFixedUpdate;
        }

        isPlayerDashing = false;

        //set cooldown timer
        playerDashCooldownTimer = movementDetails.dashCooldownTime;

        player.transform.position = targetPosition;

    }


    private void PlayerDashCooldownTimer()
    {

        if(playerDashCooldownTimer >= 0f)
        {
            playerDashCooldownTimer -= Time.deltaTime;
        }

    }


    //weapon input
    private void WeaponInput()
    {

        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;

        //aim weapon input
        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);

        //fire weapon input
        //FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, playerAimDirection);

        //switch weapon input
       // SwitchWeaponInput();

        //reload weapon input
       // ReloadWeaponInput();

    }


    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDirection)
    {

        //get mouse world position
        Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();

        //calculate direction vector of mouse cursor from weapon shoot position
        weaponDirection = (mouseWorldPosition - weaponShootPosition.position);

        //calculate direction vector of mouse cursor from player transform position
        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        //get weapon to cursor angle
        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        //get player to cursor angle
        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        //set player aim direction
        playerAimDirection = HelperUtilities.GetAimDirection(playerAngleDegrees);

        //trigger weapon aim event
        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
        
    }

    //disabled for new inventory system
    //when left button is triggered to shoot
    /*private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDegrees, AimDirection playerAimDirection)
    {

        //fire when the button is pressed
        if(Input.GetMouseButton(0))
        {
            player.fireWeaponEvent.CallFireWeaponEvent(true, leftMouseDownPreviousFrame, playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
            leftMouseDownPreviousFrame = true;
        }
        else
        {
            leftMouseDownPreviousFrame = false;
        }

    }
*/

    //disabled for inventory switching instead
    /*private void SwitchWeaponInput()
    {

        //switch weapon if using the mouse scroll wheel
        if(Input.mouseScrollDelta.y < 0f)
        {
            PreviousWeapon();
        }
        
        if(Input.mouseScrollDelta.y > 0f)
        {
            NextWeapon();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeaponByIndex(1);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeaponByIndex(2);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeaponByIndex(3);
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetWeaponByIndex(4);
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetWeaponByIndex(5);
        }

        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetWeaponByIndex(6);
        }

        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetWeaponByIndex(7);
        }

        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetWeaponByIndex(8);
        }

        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetWeaponByIndex(9);
        }

        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetWeaponByIndex(10);
        }

        if(Input.GetKeyDown(KeyCode.Minus))
        {
            SetCurrenetWeaponToFirstInTheList();
        }

    }
*/

    //stop dashing
    private void StopPlayerDashRoutine()
    {

        if(playerDashCoroutine != null)
        {
            StopCoroutine(playerDashCoroutine);

            isPlayerDashing = false;
        }

    }


    //enables the players movement
    public void EnablePlayer() 
    {

        isPlayerMovementDisabled = false;

    }
    

    //disables the players movement
    public void DisablePlayer()
    {

        isPlayerMovementDisabled = true;
        player.idleEvent.CallIdleEvent();

    }


    //disabled for new inventory system
    /*private void SetWeaponByIndex(int weaponIndex)
    {

        if(weaponIndex - 1 < player.weaponList.Count) //checking that the weapon list count is not reached
        {
            currentWeaponIndex = weaponIndex;
            player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.weaponList[weaponIndex - 1]);
        }

    }


    private void NextWeapon()
    {

        currentWeaponIndex++;

        //if statement is to make it go back to starting weapon once reaching end of weapons
        if(currentWeaponIndex > player.weaponList.Count)
        {
            currentWeaponIndex = 1;
        }

        SetWeaponByIndex(currentWeaponIndex);

    }


    private void PreviousWeapon()
    {

        currentWeaponIndex--;

        //if statement is to make it go back to ending weapon once reaching negative zone of weapons
        if(currentWeaponIndex < 1)
        {
            currentWeaponIndex = player.weaponList.Count;
        }

        SetWeaponByIndex(currentWeaponIndex);

    }


    private void ReloadWeaponInput()
    {

        Weapon currentWeapon = player.activeWeapon.GetCurrentWeapon();

        //check if the weapon is already reloading
        if(currentWeapon.isWeaponReloading) return;

        //remaining ammo is less than clip capacity then return and not infinite ammo then return
        if(currentWeapon.weaponRemainingAmmo < currentWeapon.weaponDetails.weaponClipAmmoCapacity && !currentWeapon.weaponDetails.hasInfiniteAmmo)
        return;

        //check to see if clip is already full
        if(currentWeapon.weaponClipRemainingAmmo == currentWeapon.weaponDetails.weaponClipAmmoCapacity) return;

        //check to see if the player is trying to reload
        if(Input.GetKeyDown(KeyCode.R))
        {
            //call reload weapon event
            player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), 0); 
        }

    } 
*/

    //use the nearest item within 2 unity units from the player
    private void UseItemInput()
    {

        if(Input.GetKeyDown(KeyCode.E))
        {
            float useItemRadius = 0.65f;

            //check for useable items
            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(player.GetPlayerPosition(), useItemRadius);

            //loop through detected items to see if any are able to be used
            foreach(Collider2D collider2D in collider2DArray)
            {
                IUseable iUseable = collider2D.GetComponent<IUseable>();

                if(iUseable != null)
                {
                    iUseable.UseItem();
                }
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision) 
    {

        //if collided with something the player will stop dashing into the wall or object
        StopPlayerDashRoutine();

    }


    private void OnCollisionStay2D(Collision2D collision)
    {

        //if in the collision with something it will stop the dash from continuing
        StopPlayerDashRoutine();

    }


    //disabled for now for new inventory system
    //sets the current weapon to the first in the player list
    /*private void SetCurrenetWeaponToFirstInTheList()
    {

        //create a new temporary list
        List<Weapon> tempWeaponList = new List<Weapon>();

        //adds the current weapon to first in the newly created temp list
        Weapon currentWeapon = player.weaponList[currentWeaponIndex - 1];
        currentWeapon.weaponListPosition = 1;
        tempWeaponList.Add(currentWeapon);

        //loop through existing weapon list and add them while skipping the current weapon
        int index = 2;

        foreach(Weapon weapon in player.weaponList)
        {
            if(weapon == currentWeapon) continue; //skip the weapon if it is the one we are currently holding

            tempWeaponList.Add(weapon);
            weapon.weaponListPosition = index;
            index++;
        }
        
        //assign new list
        player.weaponList = tempWeaponList;

        currentWeaponIndex = 1;

        //set current weapon
        SetWeaponByIndex(currentWeaponIndex);

    }
*/

    #region Validation
#if UNITY_EDITOR

    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion Validation 

}
