using UnityEditor;
using UnityEngine;

public class EditorUtilities : EditorWindow
{

    public enum PivotPlacement
    {
        YP,
        YC,
        YN,

        XP,
        XC,
        XN,

        ZP,
        ZC,
        ZN
    }


    public string[] XPlacement = new string[]
    {
        "Left",
        "Center",
        "Right"
    };
    public string[] YPlacement = new string[]
    {
        "Top",
        "Center",
        "Bottom"
    };
    public string[] ZPlacement = new string[]
    {
        "Back",
        "Center",
        "Front"
    };


    private static int XPivot = 1;
    private static int YPivot = 1;
    private static int ZPivot = 1;

    [MenuItem("LF Utilities/Editor")]
    public static void ShowUtilityWindow()
    {
        GetWindow<EditorUtilities>("LF Utility Editor").minSize = new Vector2(280, 280);
    }


    void OnGUI()
    {
        // Styles
        GUIStyle headingLableStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 16, fontStyle = FontStyle.Bold };
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { fixedHeight = 24 };

        // DROP DOWN
        GUILayout.Space(8);
        XPivot = EditorGUILayout.Popup("X Placement", XPivot, XPlacement);
        YPivot = EditorGUILayout.Popup("Y Placement", YPivot, YPlacement);
        ZPivot = EditorGUILayout.Popup("Z Placement", ZPivot, ZPlacement);
        GUILayout.Space(8);


        if (GUILayout.Button("Add Hierarachy", buttonStyle, GUILayout.ExpandWidth(true)))
        {
            PivotPlacement _x = PivotPlacement.XC;
            PivotPlacement _y = PivotPlacement.YC;
            PivotPlacement _z = PivotPlacement.ZC;

            if (XPivot == 0) _x = PivotPlacement.XN;
            else if (XPivot == 2) _x = PivotPlacement.XP;

            if (YPivot == 2) _y = PivotPlacement.YN;
            else if (YPivot == 0) _y = PivotPlacement.YP;

            if (ZPivot == 0) _z = PivotPlacement.ZN;
            else if (ZPivot == 2) _z = PivotPlacement.ZP;

            SetHierarchy(_x, _y, _z);
        }
    }





    private static void SetHierarchy(PivotPlacement _x, PivotPlacement _y, PivotPlacement _z)
    {
        Debug.Log(_x + "-" + _y + "-" + _z);
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogError("Nothing is Selected.");
            return;
        }
        Debug.Log(Selection.gameObjects[0].transform.root.name);
        if (Selection.gameObjects[0].GetComponentInParent<Hierarchy>() == null)
        {
            Debug.LogError("Hierarchy is Missing.");
            return;
        }

        //GameObject min = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //min.name = "_pivot";
        //min.transform.SetPositionAndRotation(GetPivot(GetBounds(Selection.gameObjects), _x, _y, _z), Quaternion.identity);
        //min.transform.localScale = Vector3.one * 0.1f;

        GameObject _empty = new GameObject("Empty_Parent");
        _empty.transform.SetParent(Selection.gameObjects[0].transform.parent);
        _empty.transform.SetPositionAndRotation(GetPivot(GetBounds(Selection.gameObjects), _x, _y, _z), Quaternion.identity);

        Hierarchy hierarchy = Selection.gameObjects[0].GetComponentInParent<Hierarchy>();


        Hierarchy.Relationship _new = new Hierarchy.Relationship();
        _new.parent = _empty.transform;
        foreach (GameObject selected in Selection.gameObjects)
        {
// selected.transform.SetParent(_empty.transform);
            _new.childs.Add(selected.transform);
        }
        hierarchy.relationships.Add(_new);
    }


    private static Bounds GetBounds(GameObject[] _gameObjects)
    {
        Bounds totalBounds = new Bounds(Vector3.zero, Vector3.one);
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            Mesh ms = _gameObjects[i].GetComponent<MeshFilter>().sharedMesh;
            int vc = ms.vertexCount;
            for (int j = 0; j < vc; j++)
            {
                if (i == 0 && j == 0)
                {
                    totalBounds = new Bounds(_gameObjects[i].transform.TransformPoint(ms.vertices[j]), Vector3.zero);
                }
                else
                {
                    totalBounds.Encapsulate(_gameObjects[i].transform.TransformPoint(ms.vertices[j]));
                }
            }
        }

        return totalBounds;
    }

    private static Vector3 GetPivot(Bounds _bounds, PivotPlacement _X, PivotPlacement _Y, PivotPlacement _Z)
    {
        Vector3 _pivot = _bounds.center;

        Vector3 _min = _bounds.min;
        Vector3 _center = _bounds.center;
        Vector3 _max = _bounds.max;

        if (_X == PivotPlacement.XN)
        {
            if (_Y == PivotPlacement.YP)
            {
                if (_Z == PivotPlacement.ZP)
                    _pivot = new Vector3(_min.x, _max.y, _max.z);
                else if (_Z == PivotPlacement.ZC)
                    _pivot = new Vector3(_min.x, _max.y, _center.z);
                else if (_Z == PivotPlacement.ZN)
                    _pivot = new Vector3(_min.x, _max.y, _min.z);
            }
            else if (_Y == PivotPlacement.YC)
            {
                if (_Z == PivotPlacement.ZP)
                    _pivot = new Vector3(_min.x, _center.y, _max.z);
                else if (_Z == PivotPlacement.ZC)
                    _pivot = new Vector3(_min.x, _center.y, _center.z);
                else if (_Z == PivotPlacement.ZN)
                    _pivot = new Vector3(_min.x, _center.y, _min.z);
            }
            else if (_Y == PivotPlacement.YN)
            {
                if (_Z == PivotPlacement.ZP)
                    _pivot = new Vector3(_min.x, _min.y, _max.z);
                else if (_Z == PivotPlacement.ZC)
                    _pivot = new Vector3(_min.x, _min.y, _center.z);
                else if (_Z == PivotPlacement.ZN)
                    _pivot = _min;
            }
        }
        if (_X == PivotPlacement.XC)
        {
            if (_Y == PivotPlacement.YP)
            {
                if (_Z == PivotPlacement.ZP)
                    _pivot = new Vector3(_center.x, _max.y, _max.z);
                else if (_Z == PivotPlacement.ZC)
                    _pivot = new Vector3(_center.x, _max.y, _center.z);
                else if (_Z == PivotPlacement.ZN)
                    _pivot = new Vector3(_center.x, _max.y, _min.z);
            }
            else if (_Y == PivotPlacement.YC)
            {
                if (_Z == PivotPlacement.ZP)
                    _pivot = new Vector3(_center.x, _center.y, _max.z);
                else if (_Z == PivotPlacement.ZC)
                    _pivot = new Vector3(_center.x, _center.y, _center.z);
                else if (_Z == PivotPlacement.ZN)
                    _pivot = new Vector3(_center.x, _center.y, _min.z);
            }
            else if (_Y == PivotPlacement.YN)
            {
                if (_Z == PivotPlacement.ZP)
                    _pivot = new Vector3(_center.x, _min.y, _max.z);
                else if (_Z == PivotPlacement.ZC)
                    _pivot = new Vector3(_center.x, _min.y, _center.z);
                else if (_Z == PivotPlacement.ZN)
                    _pivot = new Vector3(_center.x, _min.y, _min.z);
            }
        }
        if (_X == PivotPlacement.XP)
        {
            if (_Y == PivotPlacement.YP)
            {
                if (_Z == PivotPlacement.ZP)
                    _pivot = _max;
                else if (_Z == PivotPlacement.ZC)
                    _pivot = new Vector3(_max.x, _max.y, _center.z);
                else if (_Z == PivotPlacement.ZN)
                    _pivot = new Vector3(_max.x, _max.y, _min.z);
            }
            else if (_Y == PivotPlacement.YC)
            {
                if (_Z == PivotPlacement.ZP)
                    _pivot = new Vector3(_max.x, _center.y, _max.z);
                else if (_Z == PivotPlacement.ZC)
                    _pivot = new Vector3(_max.x, _center.y, _center.z);
                else if (_Z == PivotPlacement.ZN)
                    _pivot = new Vector3(_max.x, _center.y, _min.z);
            }
            else if (_Y == PivotPlacement.YN)
            {
                if (_Z == PivotPlacement.ZP)
                    _pivot = new Vector3(_max.x, _min.y, _max.z);
                else if (_Z == PivotPlacement.ZC)
                    _pivot = new Vector3(_max.x, _min.y, _center.z);
                else if (_Z == PivotPlacement.ZN)
                    _pivot = new Vector3(_max.x, _min.y, _min.z);
            }
        }

        return _pivot;
    }
}
