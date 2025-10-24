using System;
using UnityEngine;

public class Node : IComparable<Node>
{
    
    public Vector2Int gridPosition; 
    public int gCost = 0; //distance from the starting node
    public int hCost = 0; //distance from finishing node
    public Node parentNode; //for the ability to trace way back


    public Node(Vector2Int gridPosition)
    {

        this.gridPosition = gridPosition;

        parentNode = null;

    }

    public int FCost 
    {

        get 
        {
            return gCost + hCost;
        }

    }


    public int CompareTo(Node nodeToCompare)
    {

        //compare will be <0 if this instance Fcost is less than nodeToCompare.FCost
        //compare will be >0 if this instance Fcost is more than nodeToCompare.FCost
        //compare will be ==0 if the values are the same

        int compare = FCost.CompareTo(nodeToCompare.FCost); //calc return value based on FCost

        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return compare;
    }

}
