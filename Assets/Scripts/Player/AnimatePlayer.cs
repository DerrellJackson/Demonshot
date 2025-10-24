using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    
    private Player player;

    private void Awake() 
    {

        //load components
        player = GetComponent<Player>();

    }


    private void OnEnable() 
    {

        //subscribe to movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;

        //subscribe to movement to position event
        player.movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;

        //subscribe to idle event
        player.idleEvent.OnIdle += IdleEvent_OnIdle;

        //subscribe to weapon aim event
    //    player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    
    }


    private void OnDisable()
    {

        //unsubscribe to movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;

        //unsubscribe to movement to position event
        player.movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;

        //unsubscribe from idle event
        player.idleEvent.OnIdle -= IdleEvent_OnIdle;

        //unsubscribe to weapon aim event
  //      player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;

    }


    //on movement by velocity event handler
    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {

        InitializeDashAnimationParameters();
        SetMovementAnimationParameters();

    }


    //on movement to position event handler
    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {

        InitializeAimAnimationParameters();
        InitializeDashAnimationParameters();
        SetMovementToPositionAnimationParameters(movementToPositionArgs);

    }


    //on idle event handler
    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {

        InitializeDashAnimationParameters();
        SetIdleAnimationParameters();

    }


    //on weapon aim event handler
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {

        InitializeAimAnimationParameters();
        InitializeDashAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);

    }


    //initialise aim animation parameters
    private void InitializeAimAnimationParameters()
    {

/*        player.animator.SetBool(Settings.aimUp, false);
        player.animator.SetBool(Settings.aimUpRight, false);
        player.animator.SetBool(Settings.aimUpLeft, false);
        player.animator.SetBool(Settings.aimRight, false);
        player.animator.SetBool(Settings.aimLeft, false);
        player.animator.SetBool(Settings.aimDown, false); */

    }


    //initialise dash animation parameters
    private void InitializeDashAnimationParameters()
    {

 /*       player.animator.SetBool(Settings.dashDown, false);
        player.animator.SetBool(Settings.dashRight, false);
        player.animator.SetBool(Settings.dashLeft, false);
        player.animator.SetBool(Settings.dashUp, false);       */ 

    }


    //set movement animation parameters 
    private void SetMovementAnimationParameters()
    {

        /*player.animator.SetBool(Settings.isMoving, true);
        player.animator.SetBool(Settings.isIdle, false);*/

    }


    //set movement to position animation parameters
    private void SetMovementToPositionAnimationParameters(MovementToPositionArgs movementToPositionArgs)
    {

        //animate the dash
   /*     if(movementToPositionArgs.isDashing)
        {
            if(movementToPositionArgs.moveDirection.x > 0f)
            {
                player.animator.SetBool(Settings.dashRight, true);
            }
            else if(movementToPositionArgs.moveDirection.x < 0f)
            {
                player.animator.SetBool(Settings.dashLeft, true);
            }
            else if(movementToPositionArgs.moveDirection.y < 0f)
            {
                player.animator.SetBool(Settings.dashDown, true);
            }
            else if(movementToPositionArgs.moveDirection.y > 0f)
            {
                player.animator.SetBool(Settings.dashUp, true);
            }
            

        } */

    }


    //set movement animation parameters
    private void SetIdleAnimationParameters()
    {

  /*      player.animator.SetBool(Settings.isMoving, false);
        player.animator.SetBool(Settings.isIdle, true); */

    }


    //set aim animation parameters
    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {

        //set aim direction
 /*       switch(aimDirection)
        {
            case AimDirection.Up:
                player.animator.SetBool(Settings.aimUp, true);
                break;
            
            case AimDirection.UpRight:
                player.animator.SetBool(Settings.aimUpRight, true);
                break;
            
            case AimDirection.UpLeft:
                player.animator.SetBool(Settings.aimUpLeft, true);
                break;
            
            case AimDirection.Right:
                player.animator.SetBool(Settings.aimRight, true);
                break;
            
            case AimDirection.Left:
                player.animator.SetBool(Settings.aimLeft, true);
                break;
            
            case AimDirection.Down:
                player.animator.SetBool(Settings.aimDown, true);
                break;
        }
 */
    } 


}
