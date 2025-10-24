using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterBuildData.CharacterStats;
/*
public class PlayerOldScript : MonoBehaviour
{ //ALL THE CHARACTERSTATS NEED TO BE DEFINED STILL
 
  public CharacterStats maximumHearts;
  public CharacterStats maximumRebirths;
  public CharacterStats maximumShield;
  public CharacterStats ammoSpeed;
  public CharacterStats fireRate;
  public CharacterStats fireDistance;
  public CharacterStats weaponDamage;
  public CharacterStats criticalDmg;
  public CharacterStats criticalChance;
  public CharacterStats maximumSpeed;
  public CharacterStats maximumDashDistance;
  public CharacterStats dashRate;
  public CharacterStats maximumCameraView;
  public CharacterStats maximumKeys;

  
  public float speed;
  private Rigidbody2D rb;   
  private Animator anim; 
  private Vector2 moveAmount;

  public SpriteRenderer hair;
  public SpriteRenderer headSR;
  public SpriteRenderer leftEye;
  public SpriteRenderer rightEye;
  public SpriteRenderer nostrilL;
  public SpriteRenderer nostrilR;
  public SpriteRenderer mouth;
  public SpriteRenderer bodySR;
  public SpriteRenderer armL;
  public SpriteRenderer armR;
  public SpriteRenderer legL;
  public SpriteRenderer legR;


  public Transform weaponSpotToEquip;
  public Transform meleeSpotToEquip;


  int totalKeys = 0;
  int keysRemoved = 1;

  int totalCoins = 0;
  

  //my dash stuff
  private float activeMoveSpeed;
  public float dashSpeed;
  public float dashLength = .5f, dashCooldown = 1f, dashInvinsibiliy = .5f;
  private float dashCounter;
  private float dashCoolCounter;

  public static PlayerOldScript instance;
  
  private void Awake() 
  {
    instance = this; 
  }
 
  void Start() 
    {   
        activeMoveSpeed = speed;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();}
  
  
  void Update()
  //A vector2 variable is our X and Y cords
    {Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
     moveAmount = moveInput.normalized * activeMoveSpeed;

     if (moveInput != Vector2.zero)
     {
        anim.SetBool("IsRunning", true);
     }
     else {
        anim.SetBool("IsRunning", false);
     }
     //if I press space I dash as long as the counter and cool down is less than or equal to 0. 
      if(Input.GetKeyDown(KeyCode.Space)){
        if (dashCoolCounter <=0 && dashCounter <= 0)
      {  
      activeMoveSpeed = dashSpeed;
      dashCounter = dashLength;
      }
      }
      if (dashCounter > 0)
      {dashCounter -= Time.deltaTime;
      if(dashCounter <= 0) 
      {activeMoveSpeed = speed;
      dashCoolCounter = dashCooldown;}
      }
      if (dashCoolCounter > 0)
      {
        dashCoolCounter -= Time.deltaTime;
      }
     }
     
     private void FixedUpdate() 
     {
      rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);  
     }
   
    public void ChangeMeleeWeapon(MeleeWeaponScript meleeWeaponToEquip){
      Destroy(GameObject.FindGameObjectWithTag("MeleeWeapon"));
     Instantiate(meleeWeaponToEquip, meleeSpotToEquip.position, meleeSpotToEquip.rotation, meleeSpotToEquip.transform);
    }
    
    public void ChangeWeapon(WeaponScript  weaponToEquip){
      Destroy(GameObject.FindGameObjectWithTag("Weapon"));
      //Here we are equipping the weapon to the position and rotation of the player. "transform may be changed if I want to put it on his hand.
    Instantiate(weaponToEquip, weaponSpotToEquip.position, transform.rotation, weaponSpotToEquip.transform);
    }
 

    private void OnTriggerEnter2D(Collider2D collision) 
    {
      if (collision.tag == "key")
      {
        totalKeys += 1;
        Destroy(collision.gameObject);
      }      
      

      if (collision.tag == "chest")
      {
        if(totalKeys > 0) 
        {
          totalKeys -= 1;
          FindObjectOfType<UI_Scripts>().RemoveKeys(keysRemoved);
          Destroy(collision.gameObject);
        } else
        {
          //after i make a bubble script it will tell the player to get a key first
        }
      }


      if(collision.tag == "coin")
      {
        totalCoins += 1;
        Destroy(collision.gameObject);
      }


      if(collision.tag == "coinbag")
      {
        totalCoins += 5;
        Destroy(collision.gameObject);
      }

      if(collision.tag == "moneybag")
      {
        totalCoins += 10;
        Destroy(collision.gameObject);
      }

      if(collision.tag == "expensivebag")
      {
        totalCoins += 15;
        Destroy(collision.gameObject);
      }

      if(collision.tag == "impossiblebag")
      {
        totalCoins += 50;
        Destroy(collision.gameObject);
      }
      
      if(collision.tag == "cheapItem")
      {
        if(totalCoins >= 3)
      {
        totalCoins -= 3;
        FindObjectOfType<UI_Scripts>().RemoveCoins(3);
        Destroy(collision.gameObject);// will be changed later
      }
      }

      if(collision.tag == "regItem")
      {
        if(totalCoins >= 5)
      {
        totalCoins -= 5;
        FindObjectOfType<UI_Scripts>().RemoveCoins(5);
        Destroy(collision.gameObject);//will be changed later
      }
      }

      if(collision.tag == "priceyItem")
      {
        if(totalCoins >= 10)
      {
        totalCoins -= 10;
        FindObjectOfType<UI_Scripts>().RemoveCoins(10);
        Destroy(collision.gameObject);//will be changed later
      }
      }

      if(collision.tag == "richItem")
      {
        if(totalCoins >= 15)
      {
        totalCoins -= 15;
        FindObjectOfType<UI_Scripts>().RemoveCoins(15);
        Destroy(collision.gameObject);//will change later
      }
      }
}
}
*/