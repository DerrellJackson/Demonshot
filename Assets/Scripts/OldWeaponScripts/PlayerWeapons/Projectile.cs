using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Projectile : MonoBehaviour
{
 public float speed; 
 public float lifeTime;

 public GameObject explosion;

 public int damage;

 void Start()
 //our way of destroying the projectile after its life is passed, we are calling the function below and it is after "lifeTime" is over
 { Invoke("DestroyProjectile", lifeTime);}
 void Update() 
 //this will make it framerate independent and it will move forward now 
 {transform.Translate(Vector2.up * speed * Time.deltaTime);}
 
 //calling the function to destroy it, we are spawning the 'explosion' particle effect and deleting the projectile: the transform is the spot we put the explosion and the quaternion is the rotation of it
 void DestroyProjectile() 
 {  Instantiate(explosion, transform.position, Quaternion.identity);
    Destroy(gameObject);}

    private void OnTriggerEnter2D(Collider2D collision) 
    {
     if(collision.tag == "Enemy")
     {collision.GetComponent<AllEnemyScript>().TakeDamage(damage);
     DestroyProjectile();
     }   
      if(collision.tag == "Boss")
     {collision.GetComponent<AllBossScript>().TakeDamage(damage);
     DestroyProjectile();
     }   
      if(collision.tag == "ghostEnemy")
     {collision.GetComponent<AllEnemyScript>().TakeDamage(damage);
     DestroyProjectile();
     }   
    }
}
*/