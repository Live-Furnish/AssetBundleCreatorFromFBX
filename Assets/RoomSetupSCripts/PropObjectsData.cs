using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Hierarchy;

public class PropObjectsData : MonoBehaviour {

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
    public List<Transform> backgrounds = new List<Transform>();
     public List<Relationship> cutoutObjects = new List<Relationship>();
    public List<BooleanObjectInfo> booleanObjects = new List<BooleanObjectInfo>();
    [HideInInspector]
    public string id;
    private void Start()
    {
        for(int i=0; i<ignoreRaycastObjects.Count; i++)
        {
            ignoreRaycastObjects[i].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

    }



    private void OnEnable()
    {
        for (int i = 0; i < childObjects.Count; i++)
        {
            if (childObjects[i] != null)
            {
                if (!(RoomObjectsData.instance.childObjects.Contains(childObjects[i])))
                {
                    RoomObjectsData.instance.childObjects.Add(childObjects[i]);
                }
            }
        }

        for (int i = 0; i < walls.Count; i++)
        {

            if (walls[i] != null)
            {
                if (!(RoomObjectsData.instance.walls.Contains(walls[i])))
                {
                    RoomObjectsData.instance.walls.Add(walls[i]);
                }

                if (!(RoomObjectsData.instance.staticObjects.Contains(walls[i])))
                {
                    RoomObjectsData.instance.staticObjects.Add(walls[i]);
                }
            }
        }

        for (int i = 0; i < floors.Count; i++)
        {
            if (floors[i] != null)
            {
                if (!(RoomObjectsData.instance.floors.Contains(floors[i])))
                {
                    RoomObjectsData.instance.floors.Add(floors[i]);
                }
                if (!(RoomObjectsData.instance.staticObjects.Contains(floors[i])))
                {
                    RoomObjectsData.instance.staticObjects.Add(floors[i]);
                }

            }
        }

        for (int i = 0; i < ignoreRaycastObjects.Count; i++)
        {
            if (ignoreRaycastObjects[i] != null)
            {
                ignoreRaycastObjects[i].gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

                if (!(RoomObjectsData.instance.ignoreRaycastObjects.Contains(ignoreRaycastObjects[i])))
                {
                    RoomObjectsData.instance.ignoreRaycastObjects.Add(ignoreRaycastObjects[i]);
                }
            }
        }

        for (int i = 0; i < noTextureObjects.Count; i++)
        {
            if (noTextureObjects[i] != null)
            {
                if (!(RoomObjectsData.instance.noTextureObjects.Contains(noTextureObjects[i])))
                {
                    RoomObjectsData.instance.noTextureObjects.Add(noTextureObjects[i]);
                }

            }
        }

        for (int i = 0; i < wallHangingObjects.Count; i++)
        {
            if (wallHangingObjects[i] != null)
            {
                if (!(RoomObjectsData.instance.wallHangingObjects.Contains(wallHangingObjects[i])))
                {
                    RoomObjectsData.instance.wallHangingObjects.Add(wallHangingObjects[i]);
                }
            }
        }

        for (int i = 0; i < staticObjects.Count; i++)
        {
            if (staticObjects[i] != null)
            {
                if (!(RoomObjectsData.instance.staticObjects.Contains(staticObjects[i])))
                {
                    RoomObjectsData.instance.staticObjects.Add(staticObjects[i]);
                }
            }
        }

        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i] != null)
            {
                if (!(RoomObjectsData.instance.windows.Contains(windows[i])))
                {
                    RoomObjectsData.instance.windows.Add(windows[i]);
                }

                if (!(RoomObjectsData.instance.staticObjects.Contains(windows[i])))
                {
                    RoomObjectsData.instance.staticObjects.Add(windows[i]);
                }
            }
        }

        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i] != null)
            {
                if (!(RoomObjectsData.instance.doors.Contains(doors[i])))
                {
                    RoomObjectsData.instance.doors.Add(doors[i]);
                }

                if (!(RoomObjectsData.instance.staticObjects.Contains(doors[i])))
                {
                    RoomObjectsData.instance.staticObjects.Add(doors[i]);
                }
            }
        }

        for (int i = 0; i < roofs.Count; i++)
        {
            if (roofs[i] != null)
            {
                if (!(RoomObjectsData.instance.roofs.Contains(roofs[i])))
                {
                    RoomObjectsData.instance.roofs.Add(roofs[i]);
                }

                if (!(RoomObjectsData.instance.staticObjects.Contains(roofs[i])))
                {
                    RoomObjectsData.instance.staticObjects.Add(roofs[i]);
                }
            }
        }

        for (int i = 0; i < roofProps.Count; i++)
        {
            if (roofProps[i] != null)
            {
                if (!(RoomObjectsData.instance.roofProps.Contains(roofProps[i])))
                {
                    RoomObjectsData.instance.roofProps.Add(roofProps[i]);
                }
            }
        }

        for (int i = 0; i < lightParentObjects.Count; i++)
        {
            if (lightParentObjects[i] != null)
            {
                if (!(RoomObjectsData.instance.lightParentObjects.Contains(lightParentObjects[i])))
                {
                    RoomObjectsData.instance.lightParentObjects.Add(lightParentObjects[i]);
                }
            }
        }
    }


    private void OnDestroy()
    {
        for (int i = 0; i < childObjects.Count; i++)
        {
            if (childObjects[i] != null)
            {
                if (RoomObjectsData.instance.childObjects.Contains(childObjects[i]))
                {
                    RoomObjectsData.instance.childObjects.Remove(childObjects[i]);
                }
            }
        }

        for (int i = 0; i < walls.Count; i++)
        {

            if (walls[i] != null)
            {
                if (RoomObjectsData.instance.walls.Contains(walls[i]))
                {
                    RoomObjectsData.instance.walls.Remove(walls[i]);
                }

                if (RoomObjectsData.instance.staticObjects.Contains(walls[i]))
                {
                    RoomObjectsData.instance.staticObjects.Remove(walls[i]);
                }
            }
        }

        for (int i = 0; i < floors.Count; i++)
        {
            if (floors[i] != null)
            {
                if (RoomObjectsData.instance.floors.Contains(floors[i]))
                {
                    RoomObjectsData.instance.floors.Remove(floors[i]);
                }

                if (RoomObjectsData.instance.staticObjects.Contains(floors[i]))
                {
                    RoomObjectsData.instance.staticObjects.Remove(floors[i]);
                }
            }
        }

        for (int i = 0; i < ignoreRaycastObjects.Count; i++)
        {
            if (ignoreRaycastObjects[i] != null)
            {
                if (RoomObjectsData.instance.ignoreRaycastObjects.Contains(ignoreRaycastObjects[i]))
                {
                    RoomObjectsData.instance.ignoreRaycastObjects.Remove(ignoreRaycastObjects[i]);
                }
            }
        }

        for (int i = 0; i < noTextureObjects.Count; i++)
        {
            if (noTextureObjects[i] != null)
            {
                if (RoomObjectsData.instance.noTextureObjects.Contains(noTextureObjects[i]))
                {
                    RoomObjectsData.instance.noTextureObjects.Remove(noTextureObjects[i]);
                }
            }
        }

        for (int i = 0; i < wallHangingObjects.Count; i++)
        {
            if (wallHangingObjects[i] != null)
            {
                if (RoomObjectsData.instance.wallHangingObjects.Contains(wallHangingObjects[i]))
                {
                    RoomObjectsData.instance.wallHangingObjects.Remove(wallHangingObjects[i]);
                }
            }
        }

        for (int i = 0; i < staticObjects.Count; i++)
        {
            if (staticObjects[i] != null)
            {
                if (RoomObjectsData.instance.staticObjects.Contains(staticObjects[i]))
                {
                    RoomObjectsData.instance.staticObjects.Remove(staticObjects[i]);
                }
            }
        }

        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i] != null)
            {
                if (RoomObjectsData.instance.windows.Contains(windows[i]))
                {
                    RoomObjectsData.instance.windows.Remove(windows[i]);
                }

                if (RoomObjectsData.instance.staticObjects.Contains(windows[i]))
                {
                    RoomObjectsData.instance.staticObjects.Remove(windows[i]);
                }
            }
        }

        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i] != null)
            {
                if (RoomObjectsData.instance.doors.Contains(doors[i]))
                {
                    RoomObjectsData.instance.doors.Remove(doors[i]);
                }

                if (RoomObjectsData.instance.staticObjects.Contains(doors[i]))
                {
                    RoomObjectsData.instance.staticObjects.Remove(doors[i]);
                }
            }
        }

        for (int i = 0; i < roofs.Count; i++)
        {
            if (roofs[i] != null)
            {
                if (RoomObjectsData.instance.roofs.Contains(roofs[i]))
                {
                    RoomObjectsData.instance.roofs.Remove(roofs[i]);
                }

                if (RoomObjectsData.instance.staticObjects.Contains(roofs[i]))
                {
                    RoomObjectsData.instance.staticObjects.Remove(roofs[i]);
                }
            }
        }

        for (int i = 0; i < roofProps.Count; i++)
        {
            if (roofProps[i] != null)
            {
                if (RoomObjectsData.instance.roofProps.Contains(roofProps[i]))
                {
                    RoomObjectsData.instance.roofProps.Remove(roofProps[i]);
                }
            }
        }

        for (int i = 0; i < lightParentObjects.Count; i++)
        {
            if (lightParentObjects[i] != null)
            {
                if (RoomObjectsData.instance.lightParentObjects.Contains(lightParentObjects[i]))
                {
                    RoomObjectsData.instance.lightParentObjects.Remove(lightParentObjects[i]);
                }
            }
        }
    }
    [ContextMenu("Reset Mesh Collider")]
    public void ResetMeshCollider()
    {
        List<MeshRenderer> transforms = transform.GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (var item in transforms)
        {
            AddRemoveCollider(item);
        }
    }
    public void AddRemoveCollider(MeshRenderer item)
    {
        Collider[] colls = item.GetComponents<MeshCollider>();
        foreach (var col in colls)
        {
            DestroyImmediate(col);
            item.gameObject.AddComponent<MeshCollider>();
        }
    }
}
