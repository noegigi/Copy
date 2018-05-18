using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTileElement {

    public GameObject tile;

    public Vector3 normal;

    public PathTileElement(GameObject tile,Vector3 normal)
    {
        this.tile = tile;
        this.normal = normal;
    }

}
