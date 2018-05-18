using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class PlacementWindow : EditorWindow {

    //List of the selected faces
    public static List<PathTileElement> selectedFace = new List<PathTileElement>();
    //Base Gameobject used to create the tiles
    GameObject tile;

    public bool editMode = false;

    public bool EditMode
    {
        get { return editMode; }
        set
        {
            editMode = value;
            if (editMode)
            {
                //Add the function to interact to the scene view
                SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
                SceneView.onSceneGUIDelegate += this.OnSceneGUI;
            }
            else
            {
                SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
                ResetSelection();
            }
        }
    }


    void Awake()
    {
        //Load the base prefab for the tile
        tile = AssetDatabase.LoadAssetAtPath("Assets/Prefab/Face.prefab",typeof(GameObject)) as GameObject;
    }

    //Make the window accessible from window
    [MenuItem("Window/PathEditor")]
    public static void ShowWindow()
    {
        //Compulsory to use window editor
        GetWindow<PlacementWindow>("Path Editor");
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    private void OnGUI()
    {
        //Display text on the window
        GUILayout.Label("Activate Edit mode");
        //Display Toggle button (boolean) on the window
        EditMode = EditorGUILayout.Toggle("Edit mode",EditMode);
        //Make the elements within layed out horizontally
        GUILayout.BeginHorizontal();
            //create a button and launch funtction
            if (GUILayout.Button("AddPath"))
            {
                CreatePathTile();
            }
            if (GUILayout.Button("RemovePath"))
            {
                RemovePathTile();
            }
        GUILayout.EndHorizontal();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        //If click on the scene view and is left mouse button clicked
        if (Event.current.type == EventType.MouseDown&&Event.current.button==0)
        {
            //make a ray from the scene view camera
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            //if find a element a tile can be placed on
            if (Physics.Raycast(ray,out hit,1000,1<<LayerMask.NameToLayer("Walkable")))
            {
                //Create a element storing the selected face
                PathTileElement element = new PathTileElement(hit.transform.gameObject, hit.normal);
                //If the face is already selected, deselect it
                if (ContainsElement(element))
                {
                    RemoveElement(element);
                }
                else
                {
                    //else, add it to the selection
                    selectedFace.Add(element);
                }
            }
            else
            {
                ResetSelection();
            }
            //Use the event so the editor does not take it as usual
            Event.current.Use();
        }
        DisplaySelection();
    }

    //Empty the selection
    void ResetSelection()
    {
        selectedFace = new List<PathTileElement>();
        DisplaySelection();
    }

    //Display each Face selected
    void DisplaySelection()
    {
        //Handles is the object from unity to draw in scene view
        Handles.color = Color.red;
        foreach (PathTileElement element in selectedFace)
        {
            Vector3 scale = Vector3.one;
            Vector3 normal = element.normal;
            normal = new Vector3(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));
            scale -= normal;
            Vector3 position = element.tile.transform.position;
            position += element.normal * 0.5f;
            Handles.DrawWireCube(position, scale);
        }
    }

    //Remove each selected face
    void RemovePathTile()
    {
        foreach (PathTileElement element in selectedFace)
        {
            RemoveFace(element);
        }
        ResetSelection();
    }

    //Remove the selected face from the scene
    void RemoveFace(PathTileElement element)
    {
        if (DoesFaceExists(element.tile, element.normal))
        {
            //Delete the object corresponding to the selected face
            Transform child = GetFaceByOrientation(element.tile,element.normal);
            DestroyImmediate(child.gameObject);
        }
    }

    //Return the tile object corresponding to the selected face
    Transform GetFaceByOrientation(GameObject obj,Vector3 normal)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            //Get each child and compare the angle
            Transform child = obj.transform.GetChild(i);
            if (child.GetComponent<PathTile>() != null)
            {
                Vector3 rotation = GetOrientationByNormal(normal);
                Quaternion finalRotation = Quaternion.Euler(rotation);
                if (finalRotation == child.localRotation)
                {
                    return child;
                }
            }
        }
        return null;
    }

    //Remove the selected element 
    void RemoveElement(PathTileElement element)
    {
        for(int i = 0; i < selectedFace.Count; i++)
        {
            if (ContainsElement(selectedFace[i]))
            {
                selectedFace.RemoveAt(i);
                return;
            }
        }
    }

    //Return if the current element is in the selected faces or not
    bool ContainsElement(PathTileElement elt)
    {
        foreach (PathTileElement element in selectedFace)
        {
            if (element.normal==elt.normal&&element.tile==elt.tile)
            {
                return true;
            }
        }
        return false;
    }

    //Create a tile corresponding to each selected face
    void CreatePathTile()
    {
        foreach(PathTileElement element in selectedFace)
        {
            Vector3 normal = element.normal;
            if (element.tile.GetComponentInChildren<PathTile>()==null)
            {
                //Create the object and set its orientation
                GameObject side = Instantiate(tile, element.tile.transform.position, Quaternion.identity, element.tile.transform);
                SetOrientation(side,normal);
            }
            else
            {
                if (!DoesFaceExists(element.tile,normal))
                {
                    GameObject side = Instantiate(tile, element.tile.transform.position, Quaternion.identity, element.tile.transform);
                    SetOrientation(side, normal);
                }
            }
        }
        ResetSelection();
    }

    //Return if the tile corresponding to the selected face exists or not
    bool DoesFaceExists(GameObject obj,Vector3 normal)
    {
        for(int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = obj.transform.GetChild(i);
            if (child.GetComponent<PathTile>()!=null)
            {
                Vector3 rotation = GetOrientationByNormal(normal);
                Quaternion finalRotation = Quaternion.Euler(rotation);
                if (finalRotation == child.localRotation)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Convert the negative angles to a positive value
    Vector3 ConvertPositiveAngle(Vector3 eulerAngle)
    {
        float angle = eulerAngle.x;
        angle = (angle<0) ?angle+360:angle;
        eulerAngle.x = angle;

        angle = eulerAngle.y;
        angle = (angle < 0) ? angle + 360 : angle;
        eulerAngle.y = angle;

        angle = eulerAngle.z;
        angle = (angle < 0) ? angle + 360 : angle;
        eulerAngle.z = angle;
        return eulerAngle;
    }

    //Set a tile orientation according to the selected face
    void SetOrientation(GameObject obj, Vector3 normal)
    {
        Vector3 rotation = GetOrientationByNormal(normal);
        obj.transform.localEulerAngles = rotation;
    }

    //Return the orientation corresponding to the normal of the face
    Vector3 GetOrientationByNormal(Vector3 normal)
    {
        Vector3 rotation = Vector3.zero;
        if (normal.x != 0)
        {
            rotation.z = -90 * normal.x;
        }

        if (normal.z != 0)
        {
            rotation.x = 90 * normal.z;
        }

        if (normal.y != 0)
        {
            rotation.x = 180;
        }
        rotation = ConvertPositiveAngle(rotation);
        return rotation;
    }
}
