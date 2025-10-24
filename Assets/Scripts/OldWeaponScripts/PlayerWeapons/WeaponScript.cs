using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{//the variables

    public GameObject projectile;
    public Transform[] shotPoint;
    public float timeBetweenShots;

    public bool isANormalWeapon;
    public bool isAbleToPickEnemiesUp;
    public bool isAbleToPickObjectsUp;
    public bool isBurstFire;
    public bool isRandomShot;
    public bool isABoomerang;
    public bool isAChargeWeapon;
    public bool isALeftHandMelee;
    public bool healsOnShots;
    public bool healsOnKills;
    public bool accuracyIncreasesDamage;
    
  
   
//exact time we can shoot at
    private float shotTime;
    
    Animator cameraAnim;
     [HideInInspector]
  public Transform player;

    private void Start() {
    cameraAnim = Camera.main.GetComponent<Animator>();  
      player = GameObject.FindGameObjectWithTag("Player").transform;  
    }

    private void Update()
    
    {    
        if (PauseManager.isGamePaused){return;}

        if(isANormalWeapon)
        {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;  
        //Rad2Deg converts it to Degrees and this statement is the angle the weapon must rotate around the face
        //of the cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;   
       //Quaternion is a rotation
       //So our quaternion rotation function is the .AngleAxis which we use the (angle) from the float above and on
       //the Z axis or: (Vecotr3.forward) this is used for 2D Games
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //this transform.rotation = rotation means we are just saying the rotation is to the above statement
        transform.rotation = rotation;
        //if we press left mouse button
        if(Input.GetMouseButton(0))
        //checking if we are allowed to shoot
        {if (Time.time >= shotTime)
        //spawn projectile
        { 
              Transform randomSpot = shotPoint[Random.Range(0, shotPoint.Length)];
              for(int i=0; i<3; i++)
        {
            cameraAnim.SetTrigger("shake");
            Instantiate(projectile, randomSpot.position, transform.rotation);
        }
        //recalculating the shots and deciding how long to wait between each shot
        shotTime = Time.time + timeBetweenShots;
        }
      }
    }
    
    
    if(isAbleToPickEnemiesUp)
    {

    }
  
  
   if(isAbleToPickObjectsUp)
    {

    }
   
   
    if(isBurstFire)
    {

    }
   
  
    if(isRandomShot)
    {

    }
   

    if(isABoomerang)
    {

    }
   
   
    if(isAChargeWeapon)
    {

    }
   
   
    if(isALeftHandMelee)
    {

    }
  
  
    if(healsOnShots)
    {

    }
   
   
    if(healsOnKills)
    {

    }
   
   
    if(accuracyIncreasesDamage)
    {

    }
  
  
  }
}
