using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Table : MonoBehaviour, IUseable
{

    #region Tooltip
    [Tooltip("The mass of the table to control the speed that it moves when pushed")]
    #endregion
    [SerializeField] private float itemMass;

    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private Rigidbody2D rigidBody2D;
    private bool itemUsed = false;

    
    private void Awake() 
    {

        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();

    }


    public void UseItem() 
    {
        
        //get item collider bounds 
        Bounds bounds = boxCollider2D.bounds;

        //calculate closest point to player on collider bounds
        Vector3 closestPointToPlayer = bounds.ClosestPoint(GameManager.Instance.GetPlayer().GetPlayerPosition());

        //if the player is to the right then flip left
        if(closestPointToPlayer.x == bounds.max.x)
        {
            animator.SetBool(Settings.flipLeft, true);
        }

        //if player is to the left flip right
        else if (closestPointToPlayer.x == bounds.min.x)
        {
            animator.SetBool(Settings.flipRight, true);
        }

        //if the player is below the table then flip up
        else if (closestPointToPlayer.y == bounds.min.y)
        {
            animator.SetBool(Settings.flipUp, true);
        }

        //if none of the others called flip down
        else if (closestPointToPlayer.y == bounds.max.y)
        {
            animator.SetBool(Settings.flipDown, true);
        }

         else
        {
            animator.SetBool(Settings.flipUp, true);
        }


        //set the layer to environment so bullets collide with the table
        gameObject.layer = LayerMask.NameToLayer("Environment");

        //set the mass of the object to the specified amount so that the player can move the item
        rigidBody2D.mass = itemMass;

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.tableFlip);

        itemUsed = true; //prevent from being flipped again

    }


    #region Validation
#if UNITY_EDITOR

    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(itemMass), itemMass, false);
    }

#endif 
    #endregion
    
}
