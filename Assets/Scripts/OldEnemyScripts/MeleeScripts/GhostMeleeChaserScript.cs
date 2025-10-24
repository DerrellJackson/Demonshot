using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class GhostMeleeChaserScript : AllEnemyScript
{   //this is the dumb script, meaning he WILL stop following if you leave his distance
    public Rigidbody2D theRB;
    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    public bool shouldShoot;

    private Animator anim;

    public GameObject enemyBullet;
    public Transform shotPoint;
    public float fireRate;
    private float fireCounter;
   
    public override void Start() 
    {
    anim = GetComponent<Animator>();
    base.Start();
    }
    

    void Update()
    {
       if(player !=null)
        {
            if(Vector3.Distance(transform.position, player.transform.position) < rangeToChasePlayer)
            {
                moveDirection = player.transform.position - transform.position;
            }
                else
            {// may wanna make a script without this line so the enemy continues to move off in space
                moveDirection = Vector3.zero;
            }
            moveDirection.Normalize();

            theRB.velocity = moveDirection * speed;

            if(moveDirection != Vector3.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

            if(shouldShoot)
            {
                fireCounter -= Time.deltaTime;
                if(fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(enemyBullet, shotPoint.position, shotPoint.rotation);
                }
            }
        }
}
private void OnTriggerStay2D(Collider2D collision) 
 
 {  if(collision.tag == "Player"){
      StartCoroutine(Attack()); }
     }   
    
  
  IEnumerator Attack()
  {player.GetComponent<PlayerHealthController>().TakeDamage(damage);
    yield return null;
  }
}
*/