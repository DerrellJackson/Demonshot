using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class EnemyBulletScript : MonoBehaviour
{
  private PlayerHealthController playerhealthScript;
  private PlayerOldScript playerScript;
  private Vector2 targetPosition;

  public GameObject explosion;

  public float speed;

  public int damage;

  private void Start() {
    playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOldScript>();
    targetPosition = playerScript.transform.position;  
    playerhealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthController>();
  }
  private void Update() 
  {
    if (Vector2.Distance(transform.position, targetPosition) > .1f)
    {transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);}
    else {
      DestroyProjectile();
    }
  }
   //calling the function to destroy it, we are spawning the 'explosion' particle effect and deleting the projectile: the transform is the spot we put the explosion and the quaternion is the rotation of it
 void DestroyProjectile() 
 {  Instantiate(explosion, transform.position, Quaternion.identity);
    Destroy(gameObject);}
  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.tag == "Player")
    {playerhealthScript.TakeDamage(damage);
    DestroyProjectile();
    }
  }
}
*/