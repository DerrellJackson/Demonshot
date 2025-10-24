using UnityEngine;
using Cinemachine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
 
    private void OnEnable() 
    {

        StaticEventHandler.AfterSceneLoadEvent += SwitchBoundingShape;

    }


    private void OnDisable() 
    {

        StaticEventHandler.AfterSceneLoadEvent -= SwitchBoundingShape;

    }

    
    //switch the collider that cinemachine uses to define the edges of the screen
    private void SwitchBoundingShape()
    {

        //get the polygon collider on the 'boundsconfiner' gameobject which is used by the cinemachine to prevent the camera from going out of bounds
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();

        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;

        //clear cache
        cinemachineConfiner.InvalidatePathCache();

    }

}
