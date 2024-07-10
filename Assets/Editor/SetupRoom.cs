using UnityEditor;
using UnityEngine;

public class SetupRoom
{
    // check for scale first (0.98 - 1.02)


    public static bool ExportResurce()
    {
        bool canSetup = true;
        GameObject roomDataObject = GameObject.Find("Room Setup Object");

        if (!roomDataObject)
        {
            roomDataObject = new GameObject();
            roomDataObject.transform.localScale = Vector3.one;
            roomDataObject.transform.position = Vector3.zero;
            roomDataObject.transform.rotation = Quaternion.identity;
            roomDataObject.name = "Room Setup Object";
            roomDataObject.AddComponent<RoomObjectsData>();
        }
        else
        {
            canSetup = false;
            if (!roomDataObject.GetComponent<RoomObjectsData>())
            {
                roomDataObject.AddComponent<RoomObjectsData>();
                canSetup = true;
            }
            //roomDataObject = GameObject.Find("Room Setup Object");
        }


        if (GameObject.Find("2D Center") == null)
        {
            GameObject topViewCenterObject = new GameObject();
            topViewCenterObject.transform.localScale = Vector3.one;
            topViewCenterObject.transform.position = Vector3.zero;
            topViewCenterObject.transform.rotation = Quaternion.identity;
            topViewCenterObject.name = "2D Center";
            roomDataObject.GetComponent<RoomObjectsData>().topViewCenter = topViewCenterObject.transform;
        }
        else
        {
            roomDataObject.GetComponent<RoomObjectsData>().topViewCenter = GameObject.Find("2D Center").transform;
        }

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        SetupRefernces(roomDataObject.GetComponent<RoomObjectsData>(), allObjects);


        return canSetup;
    }



    private static void SetupRefernces(RoomObjectsData _roomDataObject, GameObject[] _allObjects)
    {
        foreach (var item in _allObjects)
        {
            //Debug.Log("item name is " + item.name);
            if (item.name.ToLower().Contains("wall") && !(item.name.ToLower().Contains("outer")))
            {
                //Debug.Log(item.transform);
                if (!_roomDataObject.walls.Contains(item.transform))
                {
                    _roomDataObject.walls.Add(item.transform);
                }
            }

            if (item.name.ToLower().Contains("wall") && item.name.ToLower().Contains("outer"))
            {
                if (_roomDataObject.outerWall == null)
                {
                    _roomDataObject.outerWall = item.transform;
                }
            }

            if (item.name.ToLower().Contains("floor"))
            {
                if (!_roomDataObject.floors.Contains(item.transform))
                {
                    _roomDataObject.floors.Add(item.transform);
                }
            }

            if (item.name.ToLower().Contains("roof"))
            {
                if (!_roomDataObject.roofs.Contains(item.transform))
                {
                    _roomDataObject.roofs.Add(item.transform);
                }
            }

            if (item.name.ToLower().Contains("door"))
            {
                if (!_roomDataObject.doors.Contains(item.transform))
                {
                    _roomDataObject.doors.Add(item.transform);
                }
            }

            if (item.name.ToLower().Contains("window"))
            {
                if (!_roomDataObject.windows.Contains(item.transform))
                {
                    _roomDataObject.windows.Add(item.transform);
                }
            }


            if (item.name.ToLower().Contains("skirting") || item.name.ToLower().Contains("skarting") || item.name.ToLower().Contains("skerting"))
            {
                if (!_roomDataObject.staticObjects.Contains(item.transform))
                {
                    _roomDataObject.staticObjects.Add(item.transform);
                }
            }


            // Static Objects
            if (item.transform.childCount > 0 && item.transform.root == item.transform)
            {
                if (!_roomDataObject.staticObjects.Contains(item.transform))
                {
                    _roomDataObject.staticObjects.Add(item.transform);

                }
            }
        }
    }

}




