using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MoveItem : MonoBehaviour
{
   
    #region SOUND EFFECT 
    [Header("SOUND EFFECT")]
    #endregion SOUND EFFECT 

    #region Tooltip
    [Tooltip("The sound effect when the item is being moved")]
    #endregion Tooltip 
    [SerializeField] private SoundEffectSO moveSoundEffect;

    [HideInInspector] public BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidBody2d;
    private InstantiatedRoom instantiatedRoom;
    private Vector3 previousPosition;

    private void Awake() 
    {

        //get component references
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidBody2d = GetComponent<Rigidbody2D>();
        instantiatedRoom = GetComponentInParent<InstantiatedRoom>();

        //add this item to item obstacles array 
        instantiatedRoom.moveableItemsList.Add(this);

    }


    //update the obstacles positions when something comes into contact
    private void OnCollisionStay2D(Collision2D collision) 
    {

        UpdateObstacles();
        
    }


    //update the positions of the obstacles
    private void UpdateObstacles() 
    {

        //make sure the item stays within the bounds
        ConfineItemToRoomBounds();  //this did not seem to work + I really do not care if the player moves this out of the zone

        //update moveable items in obstacles array 
        previousPosition = transform.position;

        //play sound if it is moving
        if(Mathf.Abs(rigidBody2d.velocity.x) > 0.001f || Mathf.Abs(rigidBody2d.velocity.y) > 0.001f)
        {
            //play sound every ten frames
            if(moveSoundEffect != null && Time.frameCount % 28 == 0)
            {
                SoundEffectManager.Instance.PlaySoundEffect(moveSoundEffect);
            }
        }

    }


    //confine the item to stay within the room bounds
    private void ConfineItemToRoomBounds() 
    {

        Bounds itemBounds = boxCollider2D.bounds;
        Bounds roomBounds = instantiatedRoom.roomColliderBounds;

        //if the item is being pushed beyond the room bounds then set the item position to its previous position
        if(itemBounds.min.x <= roomBounds.min.x || itemBounds.max.x >= roomBounds.max.x || itemBounds.min.y <= roomBounds.min.y || itemBounds.max.y >= roomBounds.max.y)
        {
            transform.position = previousPosition;
        }

    }

}
