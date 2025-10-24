using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemy/Enemy Details")]
public class EnemyDetailsSO : ScriptableObject
{
 
    #region Header BASE ENEMY DETAILS
    [Space(10)]
    [Header("BASE ENEMY DETAILS")]
    #endregion


    #region Tooltip
    [Tooltip("The name of the enemy duh.")]
    #endregion
    public string enemyName;

    #region Tooltip
    [Tooltip("The prefab for the enemy")]
    #endregion
    public GameObject enemyPrefab;

    #region Tooltip
    [Tooltip("The distance to the player until the enemy starts chasing them")]
    #endregion
    public float chaseDistance = 25f;


    #region Header ENEMY MATERIAL
    [Space(10)]
    [Header("ENEMY MATERIAL")]
    #endregion 

    #region Tooltip
    [Tooltip("This is the standard lit shader material for the enemy ( used after the enemy is materialized) so use default lit shader unless enemy has weird glow or something.")]
    #endregion
    public Material enemyStandardMaterial;


    #region Header ENEMY MATERIALIZE SETTINGS 
    [Space(10)]
    [Header("ENEMY MATERIALIZE SETTINGS")]
    #endregion

    #region Tooltip 
    [Tooltip("The time in seconds that it taked for the enemy to materialize, keep it low as only bosses or unique enemies should take longer.")]
    #endregion
    public float enemyMaterializeTime;

    #region Tooltip
    [Tooltip("The shader to be used when the enemy materializes, so the glowy thing.")]
    #endregion
    public Shader enemyMaterializeShader;

    #region Tooltip
    [Tooltip("The colour to use when the enemy materializes. This is an HDR color so intensity can be set to cause gloqing / bloom")]
    #endregion
    [ColorUsage(true, true)] public Color enemyMaterializeColor;


    #region Header ENEMY WEAPON SETTINGS
    [Space(10)]
    [Header("ENEMY WEAPON SETTINGS")]
    #endregion

    #region Tooltip
    [Tooltip("The weapon enemy will use - keep as none if the enemy has no weapon")]
    #endregion
    public WeaponDetailsSO enemyWeapon;

    #region Tooltip
    [Tooltip("The minimum time delay interval in seconds between bursts of enemy shooting. This value should be greater than 0. A random value will be selected between the min and max value")]
    #endregion
    public float firingIntervalMin = 0.1f;

    #region Tooltip
    [Tooltip("The maximum time delay interval in seconds between bursts of enemy shooting. This value should be greater than 0. A random value will be selected between the min and max value")]
    #endregion
    public float firingIntervalMax = 1f;

    #region Tooltip
    [Tooltip("The minimum firing duration that the enemy shoots for during a firing burst. This value should be greater than 0. A random value will be selected between the min and max value.")]
    #endregion 
    public float firingDurationMin = 1f;

    #region Tooltip
    [Tooltip("The maximum firing duration that the enemy shoots for during a firing burst. This value should be greater than 0. A random value will be selected between the min and max value.")]
    #endregion
    public float firingDurationMax = 2f;

    #region Tooltip
    [Tooltip("Select if line of sight is required before the enemy fires. If not selected the enemy will fire regardless of obstacles whenever the player is 'in range'.")]
    #endregion
    public bool firingLineOfSightRequired;


    #region Header ENEMY HEALTH
    [Space(10)]
    [Header("ENEMY HEALTH")]
    #endregion

    #region Tooltip
    [Tooltip("The health of the enemy for each level")]
    #endregion
    public EnemyHealthDetails[] enemyHealthDetailsArray;
    
    #region Tooltip
    [Tooltip("Select if it has an immunity period before getting hit again and select how long that immunity is for")]
    #endregion
    public bool isImmuneAfterHit = false;

    #region Tooltip
    [Tooltip("The immunity time in seconds after being hit")]
    #endregion
    public float hitImmunityTime;

    #region Tooltip
    [Tooltip("Select if the enemy health bar should be displayed for the enemy. (Don't check for most bosses)")]
    #endregion 
    public bool isHealthBarDisplayed = false;

    #region Validation
#if UNITY_EDITOR
    
    //validate the SO details entered
    private void OnValidate() 
    {

        HelperUtilities.ValidateCheckEmptyString(this, nameof(enemyName), enemyName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyPrefab), enemyPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(chaseDistance), chaseDistance, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyStandardMaterial), enemyStandardMaterial);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(enemyMaterializeTime), enemyMaterializeTime, true);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyMaterializeShader), enemyMaterializeShader);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(firingIntervalMin), firingIntervalMin, nameof(firingIntervalMax), firingIntervalMax, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(firingDurationMin), firingDurationMin, nameof(firingDurationMax), firingDurationMax, false);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(enemyHealthDetailsArray), enemyHealthDetailsArray);
        if(isImmuneAfterHit)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(hitImmunityTime), hitImmunityTime, false);
        }

    }

#endif 
    #endregion

}
