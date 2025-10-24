using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script will be used to move object either in the way of the enemy or to hit enemies with a weapon that follows the mouse
public class Draggable : MonoBehaviour
{

    Vector3 mousePositionOffset;

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown() 
    {
    mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }
 
 
    private void OnMouseDrag() 
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }
}
