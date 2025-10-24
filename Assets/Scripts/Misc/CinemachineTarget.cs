using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]

public class CinemachineTarget : MonoBehaviour
{
    
    private CinemachineTargetGroup cinemachineTargetGroup;
    #region Tooltip
    [Tooltip("Populate with the CursorTarget gameobject")]
    #endregion Tooltip
    [SerializeField] private Transform cursorTarget;


    private void Awake() 
    {

        //load components
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();

    }


    //start is called before the first frame update
    void Start() 
    {

        SetCinemachineTargetGroup();

    }


    //set the cinemachine camera target group
    private void SetCinemachineTargetGroup()
    {
        //change the radius of the player to adjust how much the camera can move away from the player. A greater value means it will move less.
        //create target group for cinemachine for the camera to follow 
        CinemachineTargetGroup.Target cinemachineGroupTarget_player = new CinemachineTargetGroup.Target { weight = 1f, radius = 3.5f, target = GameManager.Instance.GetPlayer().transform };

        //create target group for the cursor to get the camera to follow
        CinemachineTargetGroup.Target cinemachineGroupTarget_cursor = new CinemachineTargetGroup.Target { weight = 1f, radius = 1f, target = cursorTarget };

        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] { cinemachineGroupTarget_player, cinemachineGroupTarget_cursor };//<- cursor/player

        cinemachineTargetGroup.m_Targets = cinemachineTargetArray;

    }


    private void Update() 
    {
        //this is to get the player aiming in a direction and get the camera moving with it
        cursorTarget.position = HelperUtilities.GetMouseWorldPosition(); 

    }

}
