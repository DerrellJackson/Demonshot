public interface ISaveable
{

    string ISaveableUniqueID { get; set; } //property to get an ID

    GameObjectSave GameObjectSave { get; set; } //stores save data for the game object

    void ISaveableRegister(); //registers the load to the manager

    void ISaveableDeregister(); //deregisters the load to the manager

    void ISaveableStoreScene(string sceneName); //stores the scene to the manager

    void ISaveableRestoreScene(string sceneName); //destores the scene to the manager

}
