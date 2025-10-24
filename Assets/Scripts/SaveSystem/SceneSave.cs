using System.Collections.Generic;

[System.Serializable]
public class SceneSave 
{
  
    //string key is an identifier name that I choose for the list
    public List<SceneItem> listSceneItem;
    public Dictionary<string, GridPropertyDetails> gridPropertyDetailsDictionary;

}
