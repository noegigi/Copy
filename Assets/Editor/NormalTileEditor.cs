using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NormalTile))]
public class NormalTileEditor : Editor {
    //Used to test the editor scripting
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NormalTile tile = (NormalTile) target;

        if(GUILayout.Button("Get Planar Position"))
        {
            Debug.Log(tile.name +" : "+tile.GetPlanarPosition());
        }
    }

}
