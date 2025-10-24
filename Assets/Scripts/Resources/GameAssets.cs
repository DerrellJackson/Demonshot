using UnityEngine;
using System.Reflection;

//this is used to store objects in it. VERY USEFUL FOR CALLING OBJECTS
public class GameAssets : MonoBehaviour
{
 private static GameAssets _i;

 public static GameAssets i  
 {
    get {
        if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
        return _i;
    }
 } 

 public Transform pfDamagePopup;
}
