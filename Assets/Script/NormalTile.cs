using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : PathTile {

    protected Vector3 center;

    public GameObject node;

    protected bool isWalkable = false;

	void Awake () {
        PlaceNode();
    }

    //Create the nodes and place them on the four point of the face
    virtual protected void PlaceNode()
    {
        Vector3 size = Vector3.one/2;
        center = transform.localPosition;
        center.y += size.y;

        Vector3 spawnPosition = center;
        spawnPosition.x += size.x;
        forward = Instantiate(node, spawnPosition, Quaternion.identity, transform).GetComponent<Node>();
        forward.transform.localPosition = spawnPosition;
        forward.parent = this;
        forward.connect += OnConnection;

        spawnPosition = center;
        spawnPosition.x -= size.x;
        backward = Instantiate(node, spawnPosition, Quaternion.identity, transform).GetComponent<Node>();
        backward.transform.localPosition = spawnPosition;
        backward.parent = this;
        backward.connect += OnConnection;

        spawnPosition = center;
        spawnPosition.z += size.z;
        left = Instantiate(node, spawnPosition, Quaternion.identity, transform).GetComponent<Node>();
        left.transform.localPosition = spawnPosition;
        left.parent = this;
        left.connect += OnConnection;

        spawnPosition = center;
        spawnPosition.z -= size.z;
        right = Instantiate(node, spawnPosition, Quaternion.identity, transform).GetComponent<Node>();
        right.transform.localPosition = spawnPosition;
        right.parent = this;
        right.connect += OnConnection;
    }

    protected void OnConnection()
    {
        isWalkable = true;
    }

    //Show position and connection in Debug
	void OnDrawGizmos () {
        Vector3 position = transform.localPosition;
        Vector3 size = Vector3.one/2;
        position.y += size.y;
        position = transform.TransformPoint(position);
        Gizmos.color = new Color(0,0,255,0.5f);
        Gizmos.DrawSphere(position,0.2f);

        if (isWalkable)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(position, 0.1f);
        }
    }
}
