using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject
{
   
   [SerializeField] string[] tasks;


    public IEnumerable<string> GetTask() 
    {

        yield return "Task 1";
        Debug.Log("");
        yield return "Task 2";

    }

    
}
