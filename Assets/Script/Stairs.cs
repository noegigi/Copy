using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : PathTile {

    protected Vector3 center;

    public GameObject node;

    protected bool isWalkable = false;

	void Awake () {
        PlaceNode();
    }

    //Place the node to form a stair
    virtual protected void PlaceNode()
    {
        Vector3 size = Vector3.one/2;
        center = transform.localPosition;

        Vector3 spawnPosition = center;
        spawnPosition.x += size.x;
        spawnPosition.y += size.y;
        forward = Instantiate(node, spawnPosition, Quaternion.identity, transform).GetComponent<Node>();
        forward.transform.localPosition = spawnPosition;
        forward.parent = this;
        forward.connect += OnConnection;

        spawnPosition = center;
        spawnPosition.x -= size.x;
        spawnPosition.y -= size.y;
        backward = Instantiate(node, spawnPosition, Quaternion.identity, transform).GetComponent<Node>();
        backward.transform.localPosition = spawnPosition;
        backward.parent = this;
        backward.connect += OnConnection;
    }

    protected void OnConnection()
    {
        isWalkable = true;
    }

	void OnDrawGizmos () {
        Vector3 position = transform.localPosition;
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
