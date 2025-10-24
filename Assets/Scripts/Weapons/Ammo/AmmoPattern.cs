using UnityEngine;

public class AmmoPattern : MonoBehaviour, IFireable
{
   
    #region Tooltip
    [Tooltip("Populate the array with the child ammo gameobjects")]
    #endregion
    [SerializeField] private Ammo[] ammoArray;

    private float ammoRange;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private AmmoDetailsSO ammoDetails;
    private float ammoChargeTimer;

    public GameObject GetGameObject() 
    {

        return gameObject;

    }


    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement)
    {

        this.ammoDetails = ammoDetails;

        this.ammoSpeed = ammoSpeed;

        //set fire direction
        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        //set ammos range
        ammoRange = ammoDetails.ammoRange;

        //activate ammo pattern gameobject
        gameObject.SetActive(true);

        //loop through all the child ammo and initialise it
        foreach(Ammo ammo in ammoArray)
        {
            ammo.InitialiseAmmo(ammoDetails, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector, true);
        }

        //set ammo charge timer, which will hold the ammo briefly
        if(ammoDetails.ammoChargeTime > 0f)
        {
            ammoChargeTimer = ammoDetails.ammoChargeTime;
        }
        else 
        {
            ammoChargeTimer = 0f;
        }

    }


    private void Update() 
    {

        //ammo charge effect 
        if(ammoChargeTimer > 0f)
        {
            ammoChargeTimer -= Time.deltaTime;
            return; //if still charging then return
        }

        //calculate the distance vector to move ammo
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime; 

        transform.position += distanceVector;

        //rotate ammo 
        transform.Rotate(new Vector3(0f, 0f, ammoDetails.ammoRotationSpeed * Time.deltaTime));

        //disable ammo after max range
        ammoRange -= distanceVector.magnitude;

        if(ammoRange < 0f)
        {
            DisableAmmo();
        }

    }


    //set the ammo fire direction based on the input angle and adjusted by the random spread
    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {

        //calc random spread
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        //get random spread
        int spreadToggle = Random.Range(0, 2) * 2 - 1;

        if(weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else 
        {
            fireDirectionAngle = weaponAimAngle;
        }
        
        //adjust ammo fire angle by random spread
        fireDirectionAngle += spreadToggle * randomSpread;

        //set ammo fire direction 
        fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);

    }


    //disable the ammo making it go back to the object pool
    private void DisableAmmo() 
    {

        //disable the ammo pattern game object
        gameObject.SetActive(false);
        
    }


    #region Validation
#if UNITY_EDITOR

    private void OnValidate() 
    {

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(ammoArray), ammoArray);

    }

#endif 
    #endregion


}
