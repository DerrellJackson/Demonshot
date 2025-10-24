using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class Door : MonoBehaviour
{

    #region Header OBJECT REFERENCES
    [Space(10)]
    [Header("OBJECT REFERENCES")]
    #endregion
    
    #region Tooltip
    [Tooltip("Populate this with the BoxCollider2D component onto the DoorCollider gameobject")]
    #endregion
    [SerializeField] private BoxCollider2D doorCollider;

    [HideInInspector] public bool isBossRoomDoor = false;
    private BoxCollider2D doorTrigger;
    private bool isOpen = false;
    private bool previouslyOpened = false;
    private Animator animator;


    private void Awake() 
    {

        //disable door collider by default
        doorCollider.enabled = false;

        //load components
        animator = GetComponent<Animator>();
        doorTrigger = GetComponent<BoxCollider2D>();

    }


    private void OnTriggerEnter2D(Collider2D collision) 
    {

        if(collision.tag == Settings.playerTag || collision.tag == Settings.playerWeapon)
        {
            OpenDoor();
        }

    }


    private void OnEnable() 
    {

        //when the parent gameobject is disabled( when the player moves far away enough from the room ) the animator gets reset. this will restore the animator state
        animator.SetBool(Settings.open, isOpen);

    }


    //open the barrier
    public void OpenDoor()
    {

        if(!isOpen)
        {
            isOpen = true;
            previouslyOpened = true;
            doorCollider.enabled = false;
            doorTrigger.enabled = false;

            //set open parameter in animator
            animator.SetBool(Settings.open, true);

            //play sound effect
            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.doorOpenCloseSoundEffect);
        }

    }


    //lock the barrier from the player
    public void LockDoor()
    {

        isOpen = false;
        doorCollider.enabled = true;
        doorTrigger.enabled = false;

        //play the closed barrier animation
        animator.SetBool(Settings.open, false);

    }


    //unlock the barrier (can be used when i wnanna make keys or something to destroy barrier)
    public void UnlockDoor()
    {

        doorCollider.enabled = false;
        doorTrigger.enabled = true;

        if(previouslyOpened == true) 
        {

            isOpen = false;
            OpenDoor();

        }

    }


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {

        HelperUtilities.ValidateCheckNullValue(this, nameof(doorCollider), doorCollider);

    }
#endif
    #endregion

}
