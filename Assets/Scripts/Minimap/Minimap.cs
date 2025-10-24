using Cinemachine;
using UnityEngine;

[DisallowMultipleComponent]
public class Minimap : MonoBehaviour
{
   
    #region Tooltip
    [Tooltip("Populate with the child MinimapPlayer gameobject")]
    #endregion Tooltip
    [SerializeField] private GameObject minimapPlayer;

    private Transform playerTransform; //position of the player

    private void Start() 
    {

        playerTransform = OldGameManager.Instance.GetPlayer().transform;

        //populate the player as the cinemachine camera target
        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = playerTransform; //makes camera follow the player transform at start

        //set the minimap player icon to the current players model
        SpriteRenderer spriteRenderer = minimapPlayer.GetComponent<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = OldGameManager.Instance.GetPlayerMinimapIcon();
        }

    }


    private void Update() 
    {

        //move the minimap player to follow the player
        if(playerTransform != null && minimapPlayer != null)
        {
            minimapPlayer.transform.position = playerTransform.position;
        }

    }


    #region Validation
#if UNITY_EDITOR

    private void OnValidate() 
    {

        HelperUtilities.ValidateCheckNullValue(this, nameof(minimapPlayer), minimapPlayer);

    }

#endif
    #endregion Validation


}
