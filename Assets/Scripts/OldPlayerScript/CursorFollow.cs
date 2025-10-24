using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{    [HideInInspector]
  public Transform player;
    void Start() 
  {
   
    player = GameObject.FindGameObjectWithTag("Player").transform;  
  }
    void Update()
    {  if(player != null){
       Cursor.visible = false;
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;  
     transform.position = Input.mousePosition;
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
     transform.rotation = rotation;         
    }
}
}