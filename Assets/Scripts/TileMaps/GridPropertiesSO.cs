using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridPropertiesSO", menuName = "Scriptable Objects/Grid Properties")]
public class GridPropertiesSO : ScriptableObject
{
 
    public SceneName sceneName;
    public int gridWidth;
    public int gridHeight;
    public int originX;
    public int originY;

    [SerializeField] public List<GridProperty> gridPropertyList;

}
