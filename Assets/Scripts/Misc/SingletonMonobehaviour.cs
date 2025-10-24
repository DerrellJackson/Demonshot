using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T: MonoBehaviour
//an abstract class is used so that other classes can inherit from this class
{

    private static T instance;

    public static T Instance 
    {//this makes it an instance
        get 
        {
            return instance;
        }
    }

    protected virtual void Awake() 
    {//this will delete duplicates
        if(instance == null)
        {
            instance = this as T; 
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
