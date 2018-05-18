using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(EditableLevel))]
public class EditableLevelEditor : Editor {

    /*private void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            Debug.Log(Event.current.mousePosition);
            Event.current.Use();
        }
    }*/

    void OnFocus()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if(Event.current.type == EventType.MouseDown)
        {
            Debug.Log("ok");
        }
    }
}
