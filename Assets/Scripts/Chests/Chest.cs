using System.Collections;
using TMPro;
using UnityEngine;
using static PlayerHealth;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MaterializeEffect))]
public class Chest : MonoBehaviour, IUseable
{
    
    #region Tooltip
    [Tooltip("Set this to the color to be used for the material effect")]
    #endregion Tooltip
    [ColorUsage(false, true)]
    [SerializeField] private Color materializeColor;

    #region Tooltip 
    [Tooltip("Set this to the time it will take to materialize the chest")]
    #endregion Tooltip 
    [SerializeField] private float materializeTime = 3f;
    
    #region Tooltip
    [Tooltip("Populate with the Item Spawn Point transform")]
    #endregion Tooltip 
    [SerializeField] private Transform itemSpawnPoint;
    
    private int healthPercent; //if chest has health
    private WeaponDetailsSO weaponDetails; //if contains weapon
    private int ammoPercent; //if contains ammo 
    //private ItemDetailsSo itemDetails; //for when I add the item script for buffs
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MaterializeEffect materializeEffect;
    private bool isEnabled = false;
    private ChestState chestState = ChestState.closed;
    private GameObject chestItemGameObject;
    private ChestItem chestItem; 
    private TextMeshPro messageTextTMP;


    private void Awake() 
    {

        //cache components 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        materializeEffect = GetComponent<MaterializeEffect>();
        messageTextTMP = GetComponent<TextMeshPro>();

    }


    //initialize the chest and either make it visible immediately or materialize it
    public void Initialize(bool shouldMaterialize, int healthPercent, WeaponDetailsSO weaponDetails, int ammoPercent)
    {

        this.healthPercent = healthPercent;
        this.weaponDetails = weaponDetails;
        this.ammoPercent = ammoPercent;

        if(shouldMaterialize)
        {
            StartCoroutine(MaterializeChest());
        }
        else 
        {
            EnableChest();
        }

    }


    //materialize the chest
    private IEnumerator MaterializeChest() 
    {

        SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] { spriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance.materializeShader, materializeColor, materializeTime, spriteRendererArray, GameResources.Instance.litMaterial));
        //change if not using lit material

        EnableChest();

    }


    //enable the chest 
    private void EnableChest()
    {

        //set to use 
        isEnabled = true;

    }


    //use the chest (may wanna add vanish stuff here)
    public void UseItem() 
    {

        if(!isEnabled) return;

        switch(chestState)
        {
            case ChestState.closed:
                OpenChest();
                break;

            case ChestState.healthItem:
                CollectHealthItem();
                break; 

            case ChestState.ammoItem: 
                CollectAmmoItem();
                break;

            case ChestState.weaponItem:
                CollectWeaponItem();
                break; 

 //           case ChestState.powerItem:
   //             CollectPowerItem();
     //           break;

            case ChestState.empty:
                return;
               // VanishChest(); //add code here / prob remove the return spot

            default: 
                return;
        }

    }


    //open the chest
    private void OpenChest() 
    {

        animator.SetBool(Settings.use, true);

        //chest open sound effect 
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);

        //check if the player already has the weapon and if so set the weapon to null
        if(weaponDetails != null)
        {
            if(GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
                weaponDetails = null;
        }

        UpdateChestState();

    }


    //create items based on what should be spawned and the chest state
    private void UpdateChestState() 
    {

        if(healthPercent != 0)
        {
            chestState = ChestState.healthItem;
            InstantiateHealthItem();
        }

        else if (ammoPercent != 0)
        {
            chestState = ChestState.ammoItem;
            InstantiateAmmoItem();
        }
        
        else if (weaponDetails != null)
        {
            chestState = ChestState.weaponItem;
            InstantiateWeaponItem();
        }

        //add shield here

        else
        {
            chestState = ChestState.empty;
        }

    }


    //instantiate chest item
    private void InstantiateItem() 
    {

        chestItemGameObject = Instantiate(GameResources.Instance.chestItemPrefab, this.transform);

        chestItem = chestItemGameObject.GetComponent<ChestItem>();

    }


    //instantiate a health item for the player to collect
    private void InstantiateHealthItem() 
    {

        InstantiateItem();

        chestItem.Initialize(GameResources.Instance.heartIcon, healthPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);

    }


    //collect the health and add it to the player
    private void CollectHealthItem() 
    {

        //check item exists and has been materialized 
        if(chestItem == null || !chestItem.isItemMaterialized) return;

        //add health to player 
        //GameManager.Instance.GetPlayer().health.AddHealth(healthPercent);
        playerHealth.RestoreHealth(healthPercent);

        //play pickup sound effect
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);

        healthPercent = 0; 
        
        Destroy(chestItemGameObject);

        UpdateChestState();

    }


    //instantiate ammo item for player to colled
    private void InstantiateAmmoItem() 
    {

        InstantiateItem();

        chestItem.Initialize(GameResources.Instance.bulletIcon, ammoPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);

    }


    //collect the ammo item
    private void CollectAmmoItem()
    {

        //check if item exists and has been materialized
        if(chestItem == null || !chestItem.isItemMaterialized) return;

        Player player = GameManager.Instance.GetPlayer(); 

        //update ammo for current weapon 
        player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), ammoPercent);

        //play the sound effect 
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);

        ammoPercent = 0; 

        Destroy(chestItemGameObject);

        UpdateChestState();

    }


    //spawn in weapon if can
    private void InstantiateWeaponItem() 
    {

        InstantiateItem();

        chestItemGameObject.GetComponent<ChestItem>().Initialize(weaponDetails.weaponSprite, weaponDetails.weaponName, itemSpawnPoint.position, materializeColor);
    
    }


    //collect weapon and add it to the players weapon list
    private void CollectWeaponItem() 
    {

        //check if item exists + been materialized
        if(chestItem == null || !chestItem.isItemMaterialized) return;

        //if the player does not have the weapon then add it to them
        if(!GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
        {
            //add weapon to player
            GameManager.Instance.GetPlayer().AddWeaponToPlayer(weaponDetails);

            //play the pickup sound
            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
        }
        else 
        {
            //display messge saying already have
            StartCoroutine(DisplayMessage("Weapon\nOwned\nAlready", 1.8f));
        }
        weaponDetails = null;

        Destroy(chestItemGameObject);

        UpdateChestState();

    }

    
    //display the msg on chest
    private IEnumerator DisplayMessage(string messageText, float messageDisplayTime)
    {

        messageTextTMP.text = messageText;

        yield return new WaitForSeconds(messageDisplayTime);

        messageTextTMP.text = "";

    }


    //ADD PowerUp function here!!! IT WILL BE VERY SIMILAR TO WEAPON 

}
