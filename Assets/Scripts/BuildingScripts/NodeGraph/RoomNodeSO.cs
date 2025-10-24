using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id; //Id of the room node
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>();//list of parent room node IDs
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();//list of child room node IDs
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;//holds room node graph
    public RoomNodeTypeSO roomNodeType;//holds room node type
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;//holds the list of types

#region Editor Code
        [HideInInspector] public Rect rect;
        [HideInInspector] public bool isLeftClickDragging = false;
        [HideInInspector] public bool isSelected = false;

        //Initialise node
        public void Initialise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
        {
            this.rect = rect; 
            this.id = Guid.NewGuid().ToString();
            this.name = "RoomNode";
            this.roomNodeGraph = nodeGraph;
            this.roomNodeType = roomNodeType;

            //load room node type list
            roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
        }

        //draw node witrh the node style
        public void Draw(GUIStyle nodeStyle)
        {
            //draw node box usiong begin area
            GUILayout.BeginArea(rect, nodeStyle);

            //start region to detect popup selection changes
            EditorGUI.BeginChangeCheck();

            //if the room node has a parent or is of type entrance then display a label else display a popup
            if(parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
            {
                //display a label that can't be changed
                EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);    
            }
            else
            {
                //display a popup ising the RoomNodeType name values that can be selected from (default to the currently set roomNodeType)

                int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);//findindex method to find the roomNodeType

                int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());//creates a popup based on the string array

                roomNodeType = roomNodeTypeList.list[selection];

                //if the room type selection has changed making child connections potentially invalid
                if(roomNodeTypeList.list[selected].isCorridor && !roomNodeTypeList.list[selection].isCorridor || 
                !roomNodeTypeList.list[selected].isCorridor && roomNodeTypeList.list[selection].isCorridor || !roomNodeTypeList.list[selected].isBossRoom && roomNodeTypeList.list
                [selection].isBossRoom)
                {
                     if (childRoomNodeIDList.Count > 0)
         {
            for (int i = childRoomNodeIDList.Count - 1; i  >= 0; i --)
            {
               //get child room node
               RoomNodeSO childRoomNode = roomNodeGraph.GetRoomNode(childRoomNodeIDList[i]);

               //if the child room node is not null
               if(childRoomNode != null)
               {
                  //remove childID from the parent room node
                  RemoveChildRoomNodeIDFromRoomNode(childRoomNode.id);
                 childRoomNode.RemoveParentRoomNodeIDFromRoomNode(id);
               }
            }
         }
                }
            }

            if(EditorGUI.EndChangeCheck())
              
                EditorUtility.SetDirty(this);
                GUILayout.EndArea();
        }

        public string[] GetRoomNodeTypesToDisplay()
        {
            string[] roomArray = new string[roomNodeTypeList.list.Count];//creates empty string array

            for(int i = 0; i < roomNodeTypeList.list.Count; i++)
            {
                if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
                {
                    roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
                }
            }
            return roomArray;
        }

        //process events for the node
        public void ProcessEvents(Event currentEvent)
        {//determines what type of event is happening in the editor
            switch (currentEvent.type) 
            {
                //process mouse down events
                case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

                //process mouse up events
                case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

                //process mouse drag events
                case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;

                default:
                   break;
            }
        }


        //process mouse down events
        private void ProcessMouseDownEvent(Event currentEvent)
        {
            //left click down
            if(currentEvent.button == 0)
            {
                ProcessLeftClickDownEvent();
            }

            //right click down 
            else if (currentEvent.button == 1)
            {
                ProcessRightClickDownEvent(currentEvent);
            }
        }


        //process left click down event
        private void ProcessLeftClickDownEvent()
        {
            Selection.activeObject = this;//returns the actual objects selection and allows it to be changed correctly

            //toggle node selection
            if(isSelected == true)
            {
                isSelected = false;
            }
            else
            {
                isSelected = true;
            }
        }


        //process right click down
        private void ProcessRightClickDownEvent(Event currentEvent)
        {
            roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);//called from the RoomNodeGraphSO script
        }


        //process mouse up event 
        private void ProcessMouseUpEvent(Event currentEvent)
        {
            //if left click up
            if (currentEvent.button == 0)
            {
                ProcessLeftClickUpEvent();
            }
        }
        
        
        //process left click up event
        private void ProcessLeftClickUpEvent()
        {
            if (isLeftClickDragging)
            {
                isLeftClickDragging = false;
            }
        }


        //process mouse drag event
        private void ProcessMouseDragEvent(Event currentEvent)
        {
            //process left click drag event
            if(currentEvent.button == 0)
            {
                ProcessLeftMouseDragEvent(currentEvent);
            }
        }


        //process left mouse drag event
        private void ProcessLeftMouseDragEvent(Event currentEvent)
        {
            isLeftClickDragging = true;

            DragNode(currentEvent.delta);//captures the relative mouvement of the mouse to the last event
            GUI.changed = true;
        }


        //drag node 
        public void DragNode(Vector2 delta)
        {
            rect.position += delta;//move node position by delta value
            EditorUtility.SetDirty(this);//tells unity something happened on unity
        }


        //adds childID to the node(returns true if the node had been added, false otherwise)
        public bool AddChildRoomNodeIDToRoomNode(string childID)
        {
            //checks if child node can be added validly to parent
            if(IsChildRoomValid(childID))
            {
            childRoomNodeIDList.Add(childID);
            return true;
            }
            return false;
        }


        //checks if the child node can be validly added to the parent node - return true if it can otherwise return false
        public bool IsChildRoomValid(string childID)
        {
            bool isConnectedBossNodeAlready = false;
            //check if there is already a connected boss room in the node graph
            foreach(RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
            {
                if(roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
                    isConnectedBossNodeAlready = true;
            }

            //if the child node had a type of boss room and there is already a connected boss room node then return false
            if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isBossRoom && isConnectedBossNodeAlready)
                return false;

            //if the child node had a type of none then return false
            if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isNone)
                return false;

            //if the node already had a child with this child ID then return false
            if(childRoomNodeIDList.Contains(childID))
                return false;
                
            //if the node ID and child ID are the same return false
            if(id == childID)
                return false;

            //if the childID is already in the parent list return false
            if(parentRoomNodeIDList.Contains(childID))
                return false;

            //if the child node already has a parent return false
            if(roomNodeGraph.GetRoomNode(childID).parentRoomNodeIDList.Count > 0)
                return false;

            //if child is a corridor and the other node is corridor return false
            if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && roomNodeType.isCorridor)
                return false;

            //if child node is not a corridor and this node is not a corridor return false
            //NOTE: this may either be removed or changed where I have smaller corridors or none
            if(!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && !roomNodeType.isCorridor)
                return false;

            //if adding a corridor check that this node has < the maximum permitted child corridors
            if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count >= Settings.maxChildCorridors)
                return false;
            
            //if the child room node is an entrance then return false
            if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isEntrance)
                return false;

            //if adding a room to a corridor check that this corridor node does not already have a room added
            if(!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count > 0)
                return false;

            return true;
        }


        //adds parentID to  the node(returns true if the node had been added, false otherwise)
        public bool AddParentRoomNodeIDToRoomNode(string parentID)
        {
            parentRoomNodeIDList.Add(parentID);
            return true;
        }

        //remove childID from the node (returns true if the node has been removed, false otherwise)
        public bool RemoveChildRoomNodeIDFromRoomNode(string childID)
        {
            //if the node contains the child ID then remove it
            if(childRoomNodeIDList.Contains(childID))
            {
                childRoomNodeIDList.Remove(childID);
                return true;
            }
            return false;
        }

        //remove parentID from the node (returns true if the node has been removed, false otherwise)
        public bool RemoveParentRoomNodeIDFromRoomNode(string parentID)
        {
            //if the node contains the parent ID then remove it
            if(parentRoomNodeIDList.Contains(parentID))
            {
                parentRoomNodeIDList.Remove(parentID);
                return true;
            }
            return false;
        }


#endregion Editor Code 
    
}
