using System.Collections;
using UnityEngine;

//do not add any require components since it destroys the components when the item is drestroyed
[DisallowMultipleComponent]
public class DestroyableItem : MonoBehaviour
{
    
    #region Header HEALTH 
    [Header("HEALTH")]
    #endregion Header HEALTH 

    #region Tooltip
    [Tooltip("What the starting health for this item should be before it plays the destroy anim")]
    #endregion Tooltip
    [SerializeField] private int startingHealthAmount = 1;

    
    #region SOUND EFFECT 
    [Header("SOUND EFFECT")]
    #endregion SOUND EFFECT

    #region Tooltip
    [Tooltip("The sound effect that plays when this item is being destroyed")]
    #endregion Tooltip
    [SerializeField] private SoundEffectSO destroySoundEffect;

    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private HealthEvent healthEvent;
    private Health health;
    private CapsuleCollider2D capsuleCollider2D;
    private ReceiveContactDamage receiveContactDamage;


    private void Awake() 
    {

        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        health.SetStartingHealth(startingHealthAmount);
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        receiveContactDamage = GetComponent<ReceiveContactDamage>();

    }


    private void OnEnable() 
    {

        //sub to health lost event
        healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;

    }

    private void OnDisable() 
    {

        //unsub to health lost event
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;

    }


    private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {

        if(healthEventArgs.healthAmount <= 0f)
        {
            StartCoroutine(PlayAnimation());
        }

    }


    private IEnumerator PlayAnimation() 
    {

        //destroy the trigger collider 
        Destroy(boxCollider2D);
        Destroy(capsuleCollider2D);

        //play sound effect 
        if(destroySoundEffect != null )
        {
            SoundEffectManager.Instance.PlaySoundEffect(destroySoundEffect);
        }

        //trigger the destroy animation
        animator.SetBool(Settings.destroy, true);

        //this makes it so the player can walk over the object and it changes layer so player doesnt glitch through it
        this.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("DestroyedObjects");

        //let the animation play through
        while(!animator.GetCurrentAnimatorStateInfo(0).IsName(Settings.stateDestroyed))
        {
            yield return null;
        }

        //destroy all components other than the Sprite Renderer to display just the final anim
        Destroy(animator);
        Destroy(receiveContactDamage);
        Destroy(health);
        Destroy(healthEvent);
        Destroy(this);

    }


}
