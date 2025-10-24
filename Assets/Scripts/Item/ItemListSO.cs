using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemListSO", menuName = "Scriptable Objects/Item/Item List")]
public class ItemListSO : ScriptableObject
{
   
   [SerializeField] public List<ItemDetails> itemDetails;

}
