using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateGUID))]
public class SceneItemsManager : SingletonMonobehaviour<SceneItemsManager>, ISaveable
{
  
    private Transform parentItem;
    [SerializeField] private GameObject itemPrefab = null;

    private string _iSaveableUniqueID; 
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    private GameObjectSave _gameObjectSave; 

    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }


    private void AfterSceneLoad() 
    {

        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;

    }


    protected override void Awake() 
    {

        base.Awake(); 

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID; 
        GameObjectSave = new GameObjectSave(); 

    }


    //destroy items currently in the scene 
    private void DestroySceneItems() 
    {

        //get all items in the scene
        Item[] itemsInScene = GameObject.FindObjectsOfType<Item>();

        //loop through all scene items and destroy them 
        for(int i = itemsInScene.Length - 1; i > -1; i--)
        {
            Destroy(itemsInScene[i].gameObject);
        }

    }


    public void InstantiateSceneItem(int itemCode, Vector3 itemPosition)
    {

        GameObject itemGameObject = Instantiate(itemPrefab, itemPosition, Quaternion.identity, parentItem);
        Item item = itemGameObject.GetComponent<Item>();
        item.Init(itemCode);

    }


    private void InstantiateSceneItems(List<SceneItem> sceneItemList)
    {

        GameObject itemGameObject;

        foreach (SceneItem sceneItem in sceneItemList)
        {
            itemGameObject = Instantiate(itemPrefab, new Vector3(sceneItem.position.x, sceneItem.position.y, sceneItem.position.z), Quaternion.identity, parentItem);

            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = sceneItem.itemCode;
            item.name = sceneItem.itemName;
        }

    }


    private void OnEnable() 
    {

        ISaveableRegister();
        StaticEventHandler.AfterSceneLoadEvent += AfterSceneLoad;

    }


    private void OnDisable() 
    {

    ISaveableDeregister();
    StaticEventHandler.AfterSceneLoadEvent -= AfterSceneLoad;

    }


    public void ISaveableRestoreScene(string sceneName)
    {

        if(GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            if(sceneSave.listSceneItem != null)
            {
                //scene list items found, destroy existing items in the scene 
                DestroySceneItems(); 

                //instantiate the list of scene items 
                InstantiateSceneItems(sceneSave.listSceneItem);
            }
        }

    }


    public void ISaveableRegister() 
    {

        SaveLoadManager.Instance.iSaveableObjectList.Add(this);

    }


    public void ISaveableDeregister()
    {

        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);

    }


    public void ISaveableStoreScene(string sceneName)
    {

        //remove old scene save for game object if it exists
        GameObjectSave.sceneData.Remove(sceneName);

        //get all items in the scene 
        List<SceneItem> sceneItemList = new List<SceneItem>(); 
        Item[] itemsInScene = FindObjectsOfType<Item>(); 

        //loop through all scene items 
        foreach(Item item in itemsInScene)
        {
            SceneItem sceneItem = new SceneItem();
            sceneItem.itemCode = item.ItemCode;
            sceneItem.position = new Vector3Serializable(item.transform.position.x, item.transform.position.y, item.transform.position.z);
            sceneItem.itemName = item.name;

            //add scene item to list 
            sceneItemList.Add(sceneItem);
        }

        //create list scene items dictionary in scene save and add to it
        SceneSave sceneSave = new SceneSave(); 
        sceneSave.listSceneItem = sceneItemList;

        //add scene save to gameobject 
        GameObjectSave.sceneData.Add(sceneName, sceneSave);

    }

}
