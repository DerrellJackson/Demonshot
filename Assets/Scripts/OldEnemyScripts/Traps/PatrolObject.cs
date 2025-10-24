using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script not complete yet: needs to dmg player still
public class PatrolObject : MonoBehaviour
{
    private GameObject[] patrolPoints;   
    int randomPoint;
    public float speed;
      [HideInInspector]
  public Transform player;
 


    public void Start() { if(player !=null)
    { 
      player = GameObject.FindGameObjectWithTag("Player").transform;  
      patrolPoints = GameObject.FindGameObjectsWithTag("patrolPoints");
      randomPoint = Random.Range(0, patrolPoints.Length);
    }
    }

 
    void Update()
    {
         { transform.position = Vector2.MoveTowards(transform.position, patrolPoints[randomPoint].transform.position, speed * Time.deltaTime);
      if (Vector2.Distance(transform.position, patrolPoints[randomPoint].transform.position) < 0.1f)
      {
        randomPoint = Random.Range(0, patrolPoints.Length);
      }
    }
    }
    
}
