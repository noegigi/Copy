using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public class Nav : MonoBehaviour {

    PathTile currentNode;

    PathTile nextNode;

    PathTile targetNode = null;

    Coroutine move;

    [SerializeField]
    float speed = 1;

    [SerializeField]
    LayerMask layer;

    private void Start()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position,transform.up*-1,out hitInfo, 1))
        {
            currentNode = hitInfo.transform.GetComponentInChildren<NormalTile>();
            Vector3 position = currentNode.transform.position;
            position.y += 1;
            transform.position = position;
            nextNode = currentNode.GetRandomConnectedNode();
            //StartCoroutine("MovingCoroutine");
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,1000,layer))
            {
                targetNode = hit.transform.GetComponentInChildren<NormalTile>();
                //Debug.Log(targetNode);
                if (move != null) StopCoroutine(move);
                move = StartCoroutine(MoveAlongPath(FindPath()));
            }
        }
    }

    IEnumerator MoveAlongPath(List<PathTile> path)
    {
        for(int i=0;i<path.Count-1;i++)
        {
            PathTile toCompare = (i < path.Count - 3) ?path[i+2]:path[i+1];
            if (toCompare.distance < path[i].distance)
            {
                //Debug.Log("oui :" + nextNode.distance + " / " + currentNode.distance);
                Vector3 pos = path[i+2].transform.position;
                pos.z -= 1;
                pos.y += 1;
                //transform.position = pos;
            }
            Vector3 position = path[i+1].transform.position;
            position.y += 1;
            TweenFactory.Tween("moving", transform.position, position, speed, TweenScaleFunctions.Linear, (t) => {
                transform.position = t.CurrentValue;
            },(t)=>{
                currentNode = path[i];
            });
            Debug.Log(i);
            yield return new WaitForSeconds(speed);
        }
        currentNode = path[path.Count-1];
    }

    List<PathTile> FindPath()
    {
        List<PathTile> path = new List<PathTile>();
        path.Add(currentNode);
        int cpt = 0;
        while (path[path.Count-1]!=targetNode&&cpt<50)
        {
            cpt++;
            //Debug.Log(path[path.Count-1]+" : "+targetNode);
            PathTile next = path[path.Count - 1].GetFirstConnectedNode((path.Count == 1) ? currentNode : path[path.Count - 2]);
            if (next!=null)
            {
                path.Add(next);
            }
            
        }
        Debug.Log(path.Count);
        return path;
    }

    IEnumerator MovingCoroutine()
    {
        while (currentNode!=targetNode)
        {
            yield return new WaitForSeconds(speed);
            if (nextNode.distance < currentNode.distance)
            {
                Debug.Log("oui :" + nextNode.distance + " / " + currentNode.distance);
                Vector3 pos = nextNode.transform.position;
                pos.z -= 1;
                pos.y += 1;
                transform.position = pos;
            }
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
