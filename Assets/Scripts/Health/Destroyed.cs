using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DestroyedEvent))]
[DisallowMultipleComponent]
public class Destroyed : MonoBehaviour
{
    
    private DestroyedEvent destroyedEvent;

    private void Awake() 
    {

        //load components
        destroyedEvent = GetComponent<DestroyedEvent>();

    }


    private void OnEnable()
    {

        //sub to destroyed event
        destroyedEvent.OnDestroyed += DestroyedEvent_OnDestroyed;

    }


    private void OnDisable()
    {

        //unsub to destroyed event
        destroyedEvent.OnDestroyed -= DestroyedEvent_OnDestroyed;

    }


    private void DestroyedEvent_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {

        if(destroyedEventArgs.playerDied)
        {
            gameObject.SetActive(false);
        }
        else 
        {
            Destroy(gameObject);
        }

    }

    

}
