using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HealthSkill;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementByVelocity))]
[DisallowMultipleComponent]
public class MovementByVelocity : MonoBehaviour
{
   
    private Rigidbody2D rigidBody2D;
    private MovementByVelocityEvent movementByVelocityEvent;
    public bool isPlayer;

    private void Awake() 
    {

        //load components
        rigidBody2D = GetComponent<Rigidbody2D>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>(); 

    }
    

    private void OnEnable() 
    {

        //subscribe to movement event
        movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;

    }


    private void OnDisable()
    {

        //unsubscribe from movement event
        movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;

    }


    //on movement event 
    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {

        MoveRigidBody(movementByVelocityArgs.moveDirection, movementByVelocityArgs.moveSpeed);

    }


    public bool increaseSpeed(float moveSpeed)
    {
        moveSpeed = 0f;
        return true;
    }
   
    //move the rigidbody component
    private void MoveRigidBody(Vector2 moveDirection, float moveSpeed)
    {

        if(!increaseSpeed(moveSpeed))
        {
        rigidBody2D.velocity = moveDirection * moveSpeed;
        }
        else 
        rigidBody2D.velocity = moveDirection * moveSpeed;
    }

}
