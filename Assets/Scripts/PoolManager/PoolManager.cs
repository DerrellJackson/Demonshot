using System;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class PoolManager : SingletonMonobehaviour<PoolManager>
{


    #region Tooltip
    [Tooltip("Populate this array with prefabs that you want to add to the pool, and specify the number of gameobjects to be created for each.")]
    #endregion
    [SerializeField] private Pool[] poolArray = null;
    private Transform objectPoolTransform;
    private Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();


    [System.Serializable]
    public struct Pool 
    {

        public int poolSize;
        public GameObject prefab; 
        public string componentType;

    }


    private void Start() 
    {

        //this singleton gameobject will be the object pool parent
        objectPoolTransform = this.gameObject.transform;

        //create the object pool on start
        for(int i = 0; i < poolArray.Length; i++)
        {
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
            }

    }


    //create the object pool with the specified prefabs/pool size for each
    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {

        int poolKey = prefab.GetInstanceID();

        string prefabName = prefab.name; //gets the prefab name

        GameObject parentGameObject = new GameObject(prefabName + "Anchor"); //create parent gameobject to parent the child objects to

        parentGameObject.transform.SetParent(objectPoolTransform);

        if(!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject;

                newObject.SetActive(false);

                poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));
            }
        }

    }


    //reuse gameobject component in the pool. 'prefab' is prefab containing the component. 'position' is world position where gameobject should appear when enabled. 'rotation' only needs to be set to rotate object
    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
    {

        int poolKey = prefab.GetInstanceID();

        if(poolDictionary.ContainsKey(poolKey))
        {
            //get the object from the pool queue
            Component componentToReuse = GetComponentFromPool(poolKey);

            //reset the gameobject
            ResetObject(position, rotation, componentToReuse, prefab);

            return componentToReuse;
        }
        else 
        {
            Debug.Log("No object pool for " + prefab);
            return null;
        }

    }

    
    //get component from pool using 'poolKey'
    private Component GetComponentFromPool(int poolKey)
    {

        Component componentToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(componentToReuse);

        if(componentToReuse.gameObject.activeSelf == true) 
        {
            componentToReuse.gameObject.SetActive(false);
        }
        return componentToReuse;

    }


    //reset the gameobject
    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {

        componentToReuse.transform.position = position;
        componentToReuse.transform.rotation = rotation;
        componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;

    }


    #region Validation
#if UNITY_EDITOR 
    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(poolArray), poolArray);
    }
#endif
    #endregion


}
