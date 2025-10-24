using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//going to need to add a maxHealth and currentHealth soon
//public class PlayerHealthController : MonoBehaviour
/*{
    public static PlayerHealthController instance;
     public Animator hurtAnim;
     public int health;
     public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private Transform player;

    public float damageInvincLength = 1f;
    private float invincibleCount;

    private void Awake() 
    {
      
      instance = this;

    }

    public void Start()
    {   
       player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int damageAmount) 
  {
    if(invincibleCount <= 0)
    {
    health -= damageAmount;
    invincibleCount = damageInvincLength;

    //the invis flash
    PlayerOldScript.instance.bodySR.color = new Color(255f, PlayerOldScript.instance.bodySR.color.g, PlayerOldScript.instance.bodySR.color.b, .5f);
    PlayerOldScript.instance.hair.color = new Color(255f, PlayerOldScript.instance.hair.color.g, PlayerOldScript.instance.hair.color.b, .5f);
    PlayerOldScript.instance.headSR.color = new Color(255f, PlayerOldScript.instance.headSR.color.g, PlayerOldScript.instance.headSR.color.b, .5f);
    PlayerOldScript.instance.leftEye.color = new Color(255f, PlayerOldScript.instance.leftEye.color.g, PlayerOldScript.instance.leftEye.color.b, .5f);
    PlayerOldScript.instance.rightEye.color = new Color(255f, PlayerOldScript.instance.rightEye.color.g, PlayerOldScript.instance.rightEye.color.b, .5f);
    PlayerOldScript.instance.nostrilL.color = new Color(255f, PlayerOldScript.instance.nostrilL.color.g, PlayerOldScript.instance.nostrilL.color.b, .5f);
    PlayerOldScript.instance.nostrilR.color = new Color(255f, PlayerOldScript.instance.nostrilR.color.g, PlayerOldScript.instance.nostrilR.color.b, .5f);
    PlayerOldScript.instance.mouth.color = new Color(255f, PlayerOldScript.instance.mouth.color.g, PlayerOldScript.instance.mouth.color.b, .5f);
    PlayerOldScript.instance.armL.color = new Color(255f, PlayerOldScript.instance.armL.color.g, PlayerOldScript.instance.armL.color.b, .5f);
    PlayerOldScript.instance.armR.color = new Color(255f, PlayerOldScript.instance.armR.color.g, PlayerOldScript.instance.armR.color.b, .5f);
    PlayerOldScript.instance.legL.color = new Color(255f, PlayerOldScript.instance.legL.color.g, PlayerOldScript.instance.legL.color.b, .5f);
    PlayerOldScript.instance.legR.color = new Color(255f, PlayerOldScript.instance.legR.color.g, PlayerOldScript.instance.legR.color.b, .5f);



    DamagePopUp.Create(transform.position, damageAmount);
    UpdateHealthUI(health);
    hurtAnim.SetTrigger("hurt");
    if (health <= 0)
    {
      PlayerOldScript.instance.gameObject.SetActive(false);
      UI_Scripts.instance.deathScreen.SetActive(true);
    }
  }
  }
    
    public void Heal(int healAmount)//REMEMBER THIS WILL BE CHANGED SOON FOR ADDING A MAX HEALTH FUNCTION
   { if (health + healAmount > 6)
    {
      health = 6;
    } else 
    {health += healAmount;
    }
    UpdateHealthUI(health);
    }

       //How we get the hearts showing either empty or full
    public void UpdateHealthUI(int currentHealth){
      for (int i = 0; i < hearts.Length; i++)
      {if (i < currentHealth)
      {
        hearts[i].sprite = fullHeart;
      }else{
        hearts[i].sprite = emptyHeart;
      }}
    }

    private void Update() 
    {
      if(invincibleCount > 0)
      {
        invincibleCount -= Time.deltaTime;

        if(invincibleCount <= 0)
        {

          //no longer invis
          PlayerOldScript.instance.bodySR.color = new Color(1f, PlayerOldScript.instance.bodySR.color.g, PlayerOldScript.instance.bodySR.color.b, 1f);
          PlayerOldScript.instance.hair.color = new Color(1f, PlayerOldScript.instance.hair.color.g, PlayerOldScript.instance.hair.color.b, 1f);
          PlayerOldScript.instance.headSR.color = new Color(1f, PlayerOldScript.instance.headSR.color.g, PlayerOldScript.instance.headSR.color.b, 1f);
          PlayerOldScript.instance.leftEye.color = new Color(1f, PlayerOldScript.instance.leftEye.color.g, PlayerOldScript.instance.leftEye.color.b, 1f);
          PlayerOldScript.instance.rightEye.color = new Color(1f, PlayerOldScript.instance.rightEye.color.g, PlayerOldScript.instance.rightEye.color.b, 1f);
          PlayerOldScript.instance.nostrilL.color = new Color(1f, PlayerOldScript.instance.nostrilL.color.g, PlayerOldScript.instance.nostrilL.color.b, 1f);
          PlayerOldScript.instance.nostrilR.color = new Color(1f, PlayerOldScript.instance.nostrilR.color.g, PlayerOldScript.instance.nostrilR.color.b, 1f);
          PlayerOldScript.instance.mouth.color = new Color(1f, PlayerOldScript.instance.mouth.color.g, PlayerOldScript.instance.mouth.color.b, 1f);
          PlayerOldScript.instance.armL.color = new Color(1f, PlayerOldScript.instance.armL.color.g, PlayerOldScript.instance.armL.color.b, 1f);
          PlayerOldScript.instance.armR.color = new Color(1f, PlayerOldScript.instance.armR.color.g, PlayerOldScript.instance.armR.color.b, 1f);
          PlayerOldScript.instance.legL.color = new Color(1f, PlayerOldScript.instance.legL.color.g, PlayerOldScript.instance.legL.color.b, 1f);
          PlayerOldScript.instance.legR.color = new Color(1f, PlayerOldScript.instance.legR.color.g, PlayerOldScript.instance.legR.color.b, 1f);
        }
      }
    }
}*/