using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
//script not complete yet: needs to dmg player still
public class PatrolScript : AllEnemyScript
{
     private GameObject[] patrolPoints;   
    int randomPoint;

    public override void Start() { if(player !=null)
         { patrolPoints = GameObject.FindGameObjectsWithTag("patrolPoints");
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
*/