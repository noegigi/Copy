using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public class Nav : MonoBehaviour {

    //node the player is currently on
    PathTile currentNode;

    PathTile nextNode;

    //Target node the player wants to move to
    PathTile targetNode = null;

    //Path from the current node to the target node
    Queue<PathTile> path;

    [SerializeField]
    float speed = 1;

    [SerializeField]
    LayerMask layer;

    private void Start()
    {
        //Get the current node and set the player in position on it
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position,transform.up*-1,out hitInfo, 1))
        {
            currentNode = hitInfo.transform.GetComponentInChildren<NormalTile>();
            Vector3 position = currentNode.transform.position;
            position.y += 1;
            transform.position = position;
            nextNode = currentNode.GetRandomConnectedNode();
            StartCoroutine("MovingCoroutine");
        }
    }

    private void Update()
    {
        //When the player click on a path tile, get the target node and search a path to it
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,1000,layer))
            {
                /*targetNode = hit.transform.GetComponentInChildren<NormalTile>();
                path = null;
                if (move != null) StopCoroutine(move);
                FindPath();
                move = StartCoroutine(MoveAlongPath());*/
            }
        }
    }

    /*IEnumerator MoveAlongPath()
    {
        int i = 0;
        PathTile tile;
        while () {
            PathTile toCompare = (i < path.Count - 3) ? path[i + 2] : path[i + 1];
            if (toCompare.distance < path[i].distance)
            {
                Vector3 pos = path[i + 2].transform.position;
                pos.z -= 1;
                pos.y += 1;
                //transform.position = pos;
            }
            Vector3 position = path[i + 1].transform.position;
            position.y += 1;
            TweenFactory.Tween("moving", transform.position, position, speed, TweenScaleFunctions.Linear, (t) =>
            {
                transform.position = t.CurrentValue;
            }, (t) =>
            {
                currentNode = path[i];
            });
            yield return new WaitForSeconds(speed);
        }
        currentNode = path[path.Count-1];
    }*/

    Queue<PathTile> FindPath()
    {
        //TODO
        return path;
    }

    IEnumerator MovingCoroutine()
    {
        while (currentNode!=targetNode)
        {
            yield return new WaitForSeconds(speed);
            currentNode = nextNode;
            Vector3 position = currentNode.transform.position;
            position.y += 1;
            nextNode = currentNode.GetRandomConnectedNode();
            TweenFactory.Tween("moving",transform.position,position,speed,TweenScaleFunctions.Linear,(t)=> {
                transform.position = t.CurrentValue;
            });
        }
    }

}
