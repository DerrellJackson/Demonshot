using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using System;

namespace RPG.Dialogue.Editor
{

    public class DialogueEditor : EditorWindow
    {

        DialogueSO selectedDialogue = null;
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] GUIStyle playerNodeStyle;
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode deletingNode = null;
        [NonSerialized] DialogueNode linkingParentNode = null;
        [NonSerialized] bool draggingCanvas = false;
        [NonSerialized] Vector2 draggingCanvasOffset;
        Vector2 scrollPosition;
        const float canvasSize = 4000;
 
        [MenuItem("Window/Dialogue Editor")] //this function should be called when using the window option
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), true, "Dialogue Editor");
        } 

        private void OnEnable() 
        {

            Selection.selectionChanged += OnSelectionChanged;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.normal.textColor = Color.blue;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);

        }


       /* private void OnDisable() 
        {

            Selection.selectionChanged -= OnSelectionChanged;

        }
*/

        private void OnSelectionChanged() 
        {

            DialogueSO newDialogue = Selection.activeObject as DialogueSO;
            if(newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }

        }


        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {

            DialogueSO dialogue = EditorUtility.InstanceIDToObject(instanceID) as DialogueSO;
            if(dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;

        }


        private void OnGUI()//this gets the text to show up where I want it to go 
        {

            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No selected Dialogue");
            }
            else 
            {
                ProcessEvents(); 

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                GUILayoutUtility.GetRect(canvasSize, canvasSize);

                //seperated foreach so the lines go below the boxes
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
               {
                    DrawConnections(node);
               }
               foreach (DialogueNode node in selectedDialogue.GetAllNodes())
               {
                    DrawNode(node);
               }

                EditorGUILayout.EndScrollView();

               if(creatingNode != null)
               {
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;
               }
               if(deletingNode != null)
               {
                selectedDialogue.DeleteNode(deletingNode);
                deletingNode = null;
               }
            }

        }


            private void ProcessEvents() 
            {

                if(Event.current.type == EventType.MouseDown && draggingNode == null)
                {
                    draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                    if(draggingNode != null)
                    {
                        draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition; 
                        Selection.activeObject = draggingNode;
                    }
                    else 
                    {
                        draggingCanvas = true;
                        draggingOffset = Event.current.mousePosition + scrollPosition;
                        Selection.activeObject = selectedDialogue;
                    }
                }
                else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
                {
                    draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);

                    GUI.changed = true;
                    //Repaint();                    
                }
                else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
                {
                    scrollPosition = draggingCanvasOffset - Event.current.mousePosition;

                    GUI.changed = true;
                }
                else if (Event.current.type == EventType.MouseUp && draggingNode != null)
                {
                    draggingNode = null;
                }
                else if (Event.current.type == EventType.MouseUp && draggingCanvas)
                {
                    draggingCanvas = false;
                }

            }


            private void DrawNode(DialogueNode node)
            {

                GUIStyle style = nodeStyle;
                if(node.IsPlayerSpeaking())
                {
                    style = playerNodeStyle;
                }
                GUILayout.BeginArea(node.GetRect(), style);
                //EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel); //make the label field white

                node.SetText(EditorGUILayout.TextField(node.GetText()));
                //string newUniqueID = EditorGUILayout.TextField(node.uniqueID); //not needed as it is using machine to make unique id

                    GUILayout.BeginHorizontal();

                    if(GUILayout.Button("-"))
                    {
                        deletingNode = node;
                    }
                    DrawLinkButtons(node);
                    if (GUILayout.Button("+"))
                    {
                        creatingNode = node;
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.EndArea();

            }


            private void DrawLinkButtons(DialogueNode node) 
            {
                
                if(linkingParentNode == null)
                    {
                        if(GUILayout.Button("Connect"))
                        {
                            linkingParentNode = node;
                        }
                    }
                    else if (linkingParentNode == node) 
                    {
                        if(GUILayout.Button("cancel"))
                        {
                            linkingParentNode = null;
                        }
                    }
                    else if(linkingParentNode.GetChildren().Contains(node.name))
                    {
                        if(GUILayout.Button("Disconnect"))
                        {
                            linkingParentNode.RemoveChild(node.name);
                            linkingParentNode = null;
                        }
                    }
                    else 
                    {
                        if(GUILayout.Button("Add as child"))
                        {
                            linkingParentNode.AddChild(node.name);
                            linkingParentNode = null;
                        }
                    }

            }



            private void DrawConnections(DialogueNode node) 
            {

                Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
                foreach(DialogueNode childNode in selectedDialogue.GetAllChildren(node))
                {
                    Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y); 
                    Vector3 controlPointOffset = endPosition - startPosition;
                    controlPointOffset.y = 0;
                    controlPointOffset.x *= 0.8f;
                    Handles.DrawBezier(startPosition, endPosition,
                    startPosition + controlPointOffset, 
                    endPosition - controlPointOffset, 
                    Color.blue, null, 4f);
                }

            }


            private DialogueNode GetNodeAtPoint(Vector2 point)
            {

                DialogueNode foundNode = null;
                foreach(DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    if(node.GetRect().Contains(point))
                    {
                        foundNode = node;
                    }
                }
                return foundNode;

            }

    }
    
}
