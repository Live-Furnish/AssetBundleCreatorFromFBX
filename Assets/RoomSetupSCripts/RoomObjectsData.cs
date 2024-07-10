using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectsData : MonoBehaviour {


    public static RoomObjectsData instance;

    public List<Transform> noTextureObjects = new List<Transform>();
    public List<Transform> childObjects = new List<Transform>();
    public List<Transform> wallHangingObjects = new List<Transform>();
    public List<Transform> roofProps = new List<Transform>();
    public List<Transform> staticObjects = new List<Transform>();
    public List<Transform> walls = new List<Transform>();
    public List<Transform> floors = new List<Transform>();
    public List<Transform> windows = new List<Transform>();
    public List<Transform> doors = new List<Transform>();
    public List<Transform> roofs = new List<Transform>();
    public List<Transform> ignoreRaycastObjects = new List<Transform>();
    public List<Transform> lightParentObjects = new List<Transform>();
    public Transform outerWall;
    public Transform topViewCenter;
    public List<Transform> backgrounds;

    private void OnEnable()
    {
        instance = this;

        for(int i=0; i<ignoreRaycastObjects.Count; i++)
        {
            ignoreRaycastObjects[i].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        for (int i = 0; i < walls.Count; i++)
        {
            if(!staticObjects.Contains(walls[i]))
            {
                staticObjects.Add(walls[i]);
            }
        }

        for (int i = 0; i < floors.Count; i++)
        {
            if (!staticObjects.Contains(floors[i]))
            {
                staticObjects.Add(floors[i]);
            }
        }
        for (int i = 0; i < windows.Count; i++)
        {
            if (!staticObjects.Contains(windows[i]))
            {
                staticObjects.Add(windows[i]);
            }
        }
        for (int i = 0; i < doors.Count; i++)
        {
            if (!staticObjects.Contains(doors[i]))
            {
                staticObjects.Add(doors[i]);
            }
        }
        for (int i = 0; i < roofs.Count; i++)
        {
            if (!staticObjects.Contains(roofs[i]))
            {
                staticObjects.Add(roofs[i]);
            }
        }

    }
}
