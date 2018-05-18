using UnityEditor;
using UnityEngine;

public class ExampleWindow : EditorWindow {

    Color color;

    [MenuItem("Window/Example")]
    public static void ShowWindow()
    {
        GetWindow<ExampleWindow>("Example");
    }

    private void OnGUI()
    {
        //window code
        GUILayout.Label("This is a Label",EditorStyles.boldLabel);

        color = EditorGUILayout.ColorField("Color", color);

        if(GUILayout.Button("Press Me"))
        {
            foreach(GameObject obj in Selection.gameObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial.color = color;
                }
            }
        }
    }

}
