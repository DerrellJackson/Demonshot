using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ItemCodeDescriptionAttribute))]
public class ItemCodeDescriptionDrawer : PropertyDrawer
{
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        
        //change the returned property height to be double to cater for the additional itme code description that will be drawn
        return EditorGUI.GetPropertyHeight(property) * 2;

    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        //using begin property / send property on the parent property means that prefab override logic workd on the netire property
        EditorGUI.BeginProperty(position, label, property); 

        if(property.propertyType == SerializedPropertyType.Integer)
        {

            EditorGUI.BeginChangeCheck(); // start of check for changed values

            //draw item code
            var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height/2), label, property.intValue);

            //draw item description
            EditorGUI.LabelField(new Rect(position.x, position.y + position.height/2, position.width, position.height/2), "Item Description:", GetItemDescription(property.intValue));

            //if item code valuse has changed then set value to new value 
            if(EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }

        }

        EditorGUI.EndProperty();

    }


    private string GetItemDescription(int itemCode)
    {

        ItemListSO itemListSO; 

        itemListSO = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjectAssets/Items/ItemListSO.asset", typeof(ItemListSO)) as ItemListSO;

        List<ItemDetails> itemDetailsList = itemListSO.itemDetails;

        ItemDetails itemDetail = itemDetailsList.Find(x => x.itemCode == itemCode); 

        if(itemDetail != null)
        {
            return itemDetail.itemDescription;
        }
        else 
        {
            return "";
        }
    
    }

}
