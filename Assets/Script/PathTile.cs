using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : MonoBehaviour {

    protected Node forward = null;

    protected Node backward = null;

    protected Node left = null;

    protected Node right = null;

    public float distance = 0;

    void Start()
    {
        UpdateDistance();
        Rotate.updatePath += UpdateDistance;
    }

    void UpdateDistance()
    {
        distance = Vector3.Distance(Camera.main.transform.position, transform.position);
    }

    public PathTile GetFirstConnectedNode(PathTile current)
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
    }

    public Vector3 GetPlanarPosition()
    {
        return Vector3.one;
    }

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
