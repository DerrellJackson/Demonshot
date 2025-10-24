using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapGridProperties : MonoBehaviour
{
    #if UNITY_EDITOR
    private Tilemap tilemap;
    [SerializeField] private GridPropertiesSO gridProperties = null;
    [SerializeField] private GridBoolProperty gridBoolProperty = GridBoolProperty.diggable;


    private void OnEnable() 
    {

        //only populate in the editor
        if(!Application.IsPlaying(gameObject))
        {
            tilemap = GetComponent<Tilemap>();

            if (gridProperties != null)
            {
                gridProperties.gridPropertyList.Clear();
            }
        }

    }


    private void OnDisable() 
    {

        //only populate in the editor 
        if(!Application.IsPlaying(gameObject))
        {
            UpdateGridProperties();

            if(gridProperties != null)
            {
                //this is required to make sure the updated gridproperty game object gets saved when the game is saved otherwise they are not saved
                EditorUtility.SetDirty(gridProperties);
            }
        }

    }


    private void UpdateGridProperties() 
    {

        //compress bounds 
        tilemap.CompressBounds();

        //only populate in the editor 
        if(!Application.IsPlaying(gameObject))
        {
            if(gridProperties != null) 
            {
                Vector3Int startCell = tilemap.cellBounds.min;
                Vector3Int endCell = tilemap.cellBounds.max;

                for(int x = startCell.x; x < endCell.x; x++)
                {
                    for(int y = startCell.y; y < endCell.y; y++)
                    {
                        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));

                        if(tile != null)
                        {
                            gridProperties.gridPropertyList.Add(new GridProperty(new GridCoordinate(x, y), gridBoolProperty, true));
                        }
                    }
                }
            }
        }

    }


    private void Update() 
    {

        //only populate in the editor
        if(!Application.IsPlaying(gameObject))
        {
            Debug.Log("DISABLE PROPERTY TILEMAPS");
        }

    }
    #endif
}
