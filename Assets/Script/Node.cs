using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Node : MonoBehaviour {

    public bool isConnected = false;

    public Node connection = null;

    public PathTile parent;

    public delegate void Connection();
    public Connection connect;

    [SerializeField]
    LayerMask layer;

	// Use this for initialization
	void Start () {
        Rotate.updatePath += CalculatePath;
        CalculatePath();
	}

    public void Connect(Node node)
    {
        connection = node;
        isConnected = true;
    }

    void CalculatePath()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit[] hits = Physics.RaycastAll(ray,1000,layer);
        if (hits.Length > 1)
        {
            Node firstNode = hits[0].transform.GetComponent<Node>();
            Node secondNode = hits[1].transform.GetComponent<Node>();
            if (firstNode.parent.transform.parent!=secondNode.parent.transform.parent)
            {
                firstNode.Connect(secondNode);
                secondNode.Connect(firstNode);
                if (connect != null) connect();
            }
                
        }
        else
        {
            connection = null;
            isConnected = false;
        }
            
    }

    private void OnDrawGizmos()
    {
        float size = GetComponent<SphereCollider>().radius;
        Gizmos.color = new Color(0, 0, 255, .5f);
        Gizmos.DrawSphere(transform.position, .1f);
        if (isConnected)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.05f);
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(transform.position,connection.transform.position);
        }
    }
}
