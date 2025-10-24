using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TTHIS CLASS DOES NOT WORK YET. / I CREATED IT ON 11/27/2023 at 1 AM to test with
public class TileMapPrefabs : MonoBehaviour
{
    
    private static TileMapPrefabs instance;//this will hold the prefab of this class

    public static TileMapPrefabs Instance
    {//if there is no game resource it will load one in
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<TileMapPrefabs>("TileMapPrefabs");
            }
            return instance;
        }
    }

    
    #region Tooltip
    [Tooltip("Spring Farm prefab")]
    #endregion 
    public GameObject springFarmPrefab;

    #region Tooltip
    [Tooltip("Summer Farm prefab")]
    #endregion 
    public GameObject summerFarmPrefab;

    #region Tooltip
    [Tooltip("Autumn Farm prefab")]
    #endregion 
    public GameObject autumnFarmPrefab;

    #region Tooltip
    [Tooltip("Winter Farm prefab")]
    #endregion 
    public GameObject winterFarmPrefab;

}
