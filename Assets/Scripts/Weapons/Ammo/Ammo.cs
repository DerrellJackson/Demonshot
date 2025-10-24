using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{

    #region Tooltip
    [Tooltip("Populate with child TrailRenderer component")]
    #endregion Tooltip
    [SerializeField] private TrailRenderer trailRenderer;

    private float ammoRange = 0f; // the range of the ammo
    private float ammoSpeed; // the speed of the ammo
    private Vector3 fireDirectionVector; // the direction the ammo fires at
    private float fireDirectionAngle; // the direction the ammo faces when fired
    private SpriteRenderer spriteRenderer; // ammo sprite
    private AmmoDetailsSO ammoDetails; 
    private float ammoChargeTimer; // for charging the bullets before launching
    private bool isAmmoMaterialSet = false;
    private bool overrideAmmoMovement;
    private bool isColliding = false;


    private void Awake() 
    {

        //cache sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    private void Update()
    {

        //ammo charge effect
        if(ammoChargeTimer > 0f)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }
        else if (!isAmmoMaterialSet)
        {
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        //don't move ammo if movement has been overridden, this ammo is part of an ammo pattern 
        if(!overrideAmmoMovement)
        {
        //calculate distance vector to move ammo
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        //disable after max range reached
        ammoRange -= distanceVector.magnitude;

        if(ammoRange < 0f)
        {
            if(ammoDetails.isPlayerAmmo)
            {
                //no multiplier
                StaticEventHandler.CallMultiplierEvent(false);
            }

            DisableAmmo();
        }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision) 
    {

        //if already colliding with something then return
        if(isColliding) return;

        //deal damage to the collision object
        DealDamage(collision);

        //show the ammo hit effect
        AmmoHitEffect();

        DisableAmmo();

    }


    private void DealDamage(Collider2D collision) 
    {

        Health health = collision.GetComponent<Health>();

        bool enemyHit = false;

        if(health != null)
        {
            //set isColliding to prevent ammo dealing damage multiple times
            isColliding = true;

            health.TakeDamage(ammoDetails.ammoDamage);

            //enemy hit 
            if(health.enemy != null)
            {
                enemyHit = true;
            }
        }

        //if player ammo then update multiplier 
        if(ammoDetails.isPlayerAmmo)
        {
            if(enemyHit)
            {
                //multiplier
                StaticEventHandler.CallMultiplierEvent(true);
            }
            else 
            {
                //no multiplier
                StaticEventHandler.CallMultiplierEvent(false);
            }
        }

    }


    //intialise the ammo being fired: using the ammoDetails, aimAngle, weaponAngle, and weaponAimDirectionVector. If this ammo is part of a pattern the ammo movement can be overriden be setting the overide to true    
    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false)
    {
     
        #region Ammo

        this.ammoDetails = ammoDetails;

        //initialise the isColliding
        isColliding = false;

        //set fire direction
        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        //set the sprite
        spriteRenderer.sprite = ammoDetails.ammoSprite;

        //set initial ammo material depending on whether there is an ammo charge period
        if(ammoDetails.ammoChargeTime > 0f)
        {
            //set ammo charging timer
            ammoChargeTimer = ammoDetails.ammoChargeTime;
            SetAmmoMaterial(ammoDetails.ammoChargeMaterial);
            isAmmoMaterialSet = false;
        }
        else 
        {
            ammoChargeTimer = 0f;
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        //set ammo range
        ammoRange = ammoDetails.ammoRange;

        //set ammo speed
        this.ammoSpeed = ammoSpeed;

        //override ammo movement
        this.overrideAmmoMovement = overrideAmmoMovement;

        //activate ammo gameobject
        gameObject.SetActive(true);
        


        #endregion Ammo


        #region Trail

        if(ammoDetails.isAmmoTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = ammoDetails.ammoTrailMaterial;
            trailRenderer.startWidth = ammoDetails.ammoTrailStartWidth;
            trailRenderer.endWidth = ammoDetails.ammoTrailEndWidth;
            trailRenderer.time = ammoDetails.ammoTrailTime;
        }
        else 
        {
            trailRenderer.emitting = false;
            trailRenderer.gameObject.SetActive(false);
        }

        #endregion Trail

    }


    //set ammo fire direction and angle based on the input angle and direction adjusted by the random speed
    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {

        //calculate random spread angle btween the minimum and maximimum
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        //get a random spread toggle of 1 or -1 
        int spreadToggle = Random.Range(0, 2) * 2 - 1;

        if(weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else 
        {
            fireDirectionAngle = weaponAimAngle;
        }

        //adjust the ammo fire angle by random spread
        fireDirectionAngle += spreadToggle * randomSpread;

        //set ammo rotation
        transform.eulerAngles = new Vector3(0f, 0f, fireDirectionAngle);

        //set ammo fire direction
        fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);

    }


    //disable the ammo by returning it to the pool
    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }


    //display the ammo hit effect
    private void AmmoHitEffect() 
    {

        //process if a hit has been specified
        if(ammoDetails.ammoHitEffect != null && ammoDetails.ammoHitEffect.ammoHitEffectPrefab != null)
        {
            //get the ammo hit effect gameobject from the pool with the particle system component
            AmmoHitEffect ammoHitEffect = (AmmoHitEffect)PoolManager.Instance.ReuseComponent
            (ammoDetails.ammoHitEffect.ammoHitEffectPrefab, transform.position, Quaternion.identity);

            //set the hit effect
            ammoHitEffect.SetHitEffect(ammoDetails.ammoHitEffect);

            //make it active
            ammoHitEffect.gameObject.SetActive(true);
        }

    }


    public void SetAmmoMaterial(Material material)
    {
        spriteRenderer.material = material;
    }


    public GameObject GetGameObject()
    {

        return gameObject;

    }


    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {

        HelperUtilities.ValidateCheckNullValue(this, nameof(trailRenderer), trailRenderer);

    }

#endif
    #endregion Validation


}
