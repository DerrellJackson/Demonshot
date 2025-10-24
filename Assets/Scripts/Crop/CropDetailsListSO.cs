using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropDetailsList", menuName = "Scriptable Objects/Crop/Crop Details List")]
public class CropDetailsListSO : ScriptableObject
{
   
    [SerializeField]
    public List<CropDetails> cropDetails;

    
    public CropDetails GetCropDetails(int seedItemCode)
    {

        return cropDetails.Find(x => x.seedItemCode == seedItemCode);

    }

}
