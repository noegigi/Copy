using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : MonoBehaviour {

    protected Node forward = null;

    protected Node backward = null;

    protected Node left = null;

    protected Node right = null;

    //Distance from the camera to the tile
    public float distance = 0;

    void Start()
    {
        UpdateDistance();
        //Add the function to update the path to the rotable object
        Rotate.updatePath += UpdateDistance;
    }

    //Calculate the distance
    void UpdateDistance()
    {
        distance = Vector3.Distance(Camera.main.transform.position, transform.position);
    }

    /*public PathTile GetFirstConnectedNode(PathTile current)
    {
        if(forward!=null)
            if (forward.isConnected)
            {
                if(forward.connection.parent!=current)
                    return forward.connection.parent;
            }
        if (left != null)
            if (left.isConnected)
            {
                if (left.connection.parent != current)
                    return left.connection.parent;
            }
        if (right != null)
            if (right.isConnected)
            {
                if (right.connection.parent != current)
                    return right.connection.parent;
            }
        if (backward != null)
            if (backward.isConnected)
            {
                if (backward.connection.parent != current)
                    return backward.connection.parent;
            }
        return null;
    }*/

    //Get a position as if the tile where on the same level as the others
    //Used to get the position relative to other tiles
    public Vector3 GetPlanarPosition()
    {
        Vector3 camPosition = Camera.main.transform.position;
        Vector3 position = transform.position;
        position -= camPosition;
        return position;
    }

    //get a random node to move randomly
    public PathTile GetRandomConnectedNode()
    {
        if (forward.isConnected)
        {
            return forward.connection.parent;
        }
        if(left!=null)
            if (left.isConnected)
            {
                return left.connection.parent;
            }
        if(right!=null)
            if (right.isConnected)
            {
                return right.connection.parent;
            }
        if (backward.isConnected)
        {
            return backward.connection.parent;
        }
        return null;
    }
}
