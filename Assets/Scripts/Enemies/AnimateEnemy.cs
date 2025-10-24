using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
    
    private Enemy enemy;

    private void Awake() 
    {

        //load components
        enemy = GetComponent<Enemy>();

    }


    private void OnEnable() 
    {

        //subscribe to movement event
        enemy.movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;

        //subscribe to idle event
        enemy.idleEvent.OnIdle += IdleEvent_OnIdle;

        //subscribe to weapon aim event
        enemy.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;

    }


    //on weapon aim event handler
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {

        InitialiseAimAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);

    }


    private void OnDisable() 
    {

        //unsubscribe from movement event
        enemy.movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;

        //unsubscribe from idle event
        enemy.idleEvent.OnIdle -= IdleEvent_OnIdle;

        //unsubscribe to weapon aim event
        enemy.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;

    }


    //on movement event handler
    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {

        /*if(enemy.transform.position.x < GameManager.Instance.GetPlayer().transform.position.x)
        {
            SetAimWeaponAnimationParameters(AimDirection.Right);
        }
        else 
        {
            SetAimWeaponAnimationParameters(AimDirection.Left);
        }  This code I do not need because it will make the enemies only move left and right (not up or down) but I will save it just incase I wanna make changes in it*/

        SetMovementAnimationParameters();
    }


    //on idle event handler
    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {

        SetIdleAnimationParameters();

    }


    //initialise aim animation parameters
    private void InitialiseAimAnimationParameters()
    {

        enemy.animator.SetBool(Settings.aimUp, false);
        enemy.animator.SetBool(Settings.aimUpRight, false);
        enemy.animator.SetBool(Settings.aimUpLeft, false);
        enemy.animator.SetBool(Settings.aimRight, false);
        enemy.animator.SetBool(Settings.aimLeft, false);
        enemy.animator.SetBool(Settings.aimDown, false);

    }


    //set movement animation parameters
    private void SetMovementAnimationParameters() 
    {

        //set moving
        enemy.animator.SetBool(Settings.isIdle, false);
        enemy.animator.SetBool(Settings.isMoving, false);

    }


    //set idle animation parameters
    private void SetIdleAnimationParameters()
    {

        //set idle
        enemy.animator.SetBool(Settings.isIdle, false);
        enemy.animator.SetBool(Settings.isMoving, false);

    }


    //set aim animation parameters 
    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {

        
        //set aim direction
        switch(aimDirection)
        {
            case AimDirection.Up:
                enemy.animator.SetBool(Settings.aimUp, true);
                break;

            case AimDirection.UpRight:
                enemy.animator.SetBool(Settings.aimUpRight, true);
                break;

            case AimDirection.UpLeft:
                enemy.animator.SetBool(Settings.aimUpLeft, true);
                break;

            case AimDirection.Right: 
                enemy.animator.SetBool(Settings.aimRight, true);
                break;
            
            case AimDirection.Left: 
                enemy.animator.SetBool(Settings.aimLeft, true);
                break;

            case AimDirection.Down:
                enemy.animator.SetBool(Settings.aimDown, true);
                break;
        }

    }

}
