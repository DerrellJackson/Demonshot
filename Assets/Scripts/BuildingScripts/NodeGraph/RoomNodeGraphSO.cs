using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Assets>Create>ScriptableObjects>Dungeon
[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]
public class RoomNodeGraphSO : ScriptableObject 
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;//list of room node type
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();//the room node list
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();//the dictionary that we can look up


    private void Awake()
    {
        LoadRoomNodeDictionary();
    }


    //load the room node dictionary from the room node list
    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();//starts by clearing the dictionary

        //populate dictionary
        foreach (RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;//add each node to the dictionary with node ID as the key
        }
    }


    //get room node by room node type
    public RoomNodeSO GetRoomNode(RoomNodeTypeSO roomNodeType)
    {
        foreach(RoomNodeSO node in roomNodeList)
        {
            if (node.roomNodeType == roomNodeType)
            {
                return node;
            }
        }
        return null;
    }


    //get room node by room node ID
    public RoomNodeSO GetRoomNode(string roomNodeID)
    {
        if(roomNodeDictionary.TryGetValue(roomNodeID, out RoomNodeSO roomNode))
        {
            return roomNode;
        }
        return null;
    }


    //get child room nodes for supplies parent room node
    public IEnumerable<RoomNodeSO> GetChildRoomNodes(RoomNodeSO parentRoomNode)
    {
        foreach(string childNodeID in parentRoomNode.childRoomNodeIDList)
        {
            yield return GetRoomNode(childNodeID);
        }
    }


    #region Editor Code

    //the following code should only run in the Unity Editor
    #if UNITY_EDITOR
    
    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null;
    [HideInInspector] public Vector2 linePosition;

    //repopulate node dictionary every time a change is made in the editor
    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }

    
    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position)
    {
        roomNodeToDrawLineFrom = node;
        linePosition = position;
    }
    #endif
    #endregion Editor Code
}
