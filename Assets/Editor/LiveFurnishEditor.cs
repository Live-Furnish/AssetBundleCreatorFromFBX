using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LiveFurnishEditor : EditorWindow
{
    public string[] type = new string[]
    {
        "None",
        "Room (Staging)",
        "Room (No-Staging)",
        "Prop",
        "Material"
    };

    private static int selectedType = 0;
    //private string footerWarningText = "This is a sample warning";
    //private static bool showWarning = false;
    private static Vector2 scrollPos;


    private static string typeNotSelected_TEXT = "<b>Type</b> select krlo bhai!";
    private static string allGood_TEXT = "All Good";

    private static string noPropSelected_TEXT = "Kuch <b>SELECT</b> to krlo bhai!";
    private static string propDataMissing_TEXT = "<b>Prop Objects Data</b> not found on selected object. Please click <b>Setup</b> button if you want to Add.";
    private static string propDataAdded_TEXT = "<b>Prop Object Data</b> added on ----- ";
    private static string propDataAlreadyPresent_TEXT = "<b>Prop Object Data</b> already present on the selected object ----- ";
    private static string propDataDeleted_TEXT = "<b>Props Object Data</b> is DELETED from ------ ";

    private static string roomDataMissing_TEXT = "<b>Room Object Data</b> not found in the current scene. Please click <b>Setup</b> button if you want to Add.";
    private static string roomDataAdded_TEXT = "<b>Room Objects Data</b> added to the scene";
    private static string roomDataAlreadyPresent_TEXT = "<b>Room Objects Data</b> already present in the scene";
    private static string roomDataDeleted_TEXT = "<b>Room Objects Data</b> is DELETED from ------ ";


    public static string bundlesLocation = "D:/LF Asset Bundle";
    public static bool makeReadable = true;

    [MenuItem("Live Furnish/Editor")]
    public static void ShowWindow()
    {
        //GetWindow<LiveFurnishEditor>().Close();
        selectedType = 0;
        GetWindow<LiveFurnishEditor>("LF Editor").minSize = new Vector2(280, 280);
        makeReadable = true;
        LiveFurnishEditorUtility.windowAB = true;
        LiveFurnishEditorUtility.macAB = true;
        LiveFurnishEditorUtility.iosAB = false;
    }


    void OnGUI()
    {
        //EditorGUILayout.BeginVertical();
        //scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // Styles
        GUIStyle headingLableStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 16, fontStyle = FontStyle.Bold };
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { fixedHeight = 24 };



        // DROP DOWN
        GUILayout.Space(8);
        selectedType = EditorGUILayout.Popup("Type", selectedType, type);
        GUILayout.Space(8);

        if (selectedType == 0)
        {
            // MakeTextureReadable
            if (GUILayout.Button("Make Texture Readable", buttonStyle, GUILayout.ExpandWidth(true)))
                MakeTextureReadable();

            LiveFurnishEditorUtility.twoK = EditorGUILayout.Toggle("2K", LiveFurnishEditorUtility.twoK, GUILayout.ExpandWidth(false));
            LiveFurnishEditorUtility.fourK = EditorGUILayout.Toggle("4K", LiveFurnishEditorUtility.fourK, GUILayout.ExpandWidth(false));
        }
        else
        {
            if (selectedType == 4)
            {
                //Set Material Names
                if (GUILayout.Button("Set Material Names", buttonStyle, GUILayout.ExpandWidth(true)))
                    CreateAssetBundleMaterialNames(selectedType);
            }
            else
            {
                makeReadable = EditorGUILayout.Toggle("Make Texture Readable", makeReadable, GUILayout.ExpandWidth(true));

                EditorGUILayout.BeginHorizontal();

                // ScaleCheck
                if (GUILayout.Button("Scale Check", buttonStyle, GUILayout.ExpandWidth(true)))
                    ScaleCheck(selectedType);

                // SetupObjectCheck
                if (GUILayout.Button("Setup " + type[selectedType], buttonStyle, GUILayout.ExpandWidth(true)))
                    SetupCheck(selectedType);

                EditorGUILayout.EndHorizontal();
            }


            #region Seperator
            GUILayout.Space(4);
            GUILayout.Label("--------------------", headingLableStyle, GUILayout.ExpandWidth(true));
            GUILayout.Space(2);
            #endregion


            EditorGUILayout.BeginHorizontal();
            if (selectedType == 3)
            {
                // CreateAssetBundlePropNames
                if (GUILayout.Button("Set Prop Names", buttonStyle, GUILayout.ExpandWidth(true)))
                    CreateAssetBundlePropNames(selectedType);
            }

            if (selectedType == 1 || selectedType == 2)
            {
                // ClearAssetBundleNames
                if (GUILayout.Button("Set Room Name", buttonStyle, GUILayout.ExpandWidth(true)))
                    ClearAndSetAssetBundleRoomName();
            }

            // CreateAssetBundles
            GUILayout.Space(4);
            if (GUILayout.Button("Create Asset Bundles", buttonStyle, GUILayout.ExpandWidth(true)))
                CreateAssetBundles(selectedType);

            EditorGUILayout.EndHorizontal();

            // Location
            //GUILayout.Space(4);
            bundlesLocation = EditorGUILayout.TextField("Location: ", bundlesLocation);
            //GUILayout.Space(8);


            // Check Box
            LiveFurnishEditorUtility.windowAB = EditorGUILayout.Toggle("Windows", LiveFurnishEditorUtility.windowAB, GUILayout.ExpandWidth(false));
            LiveFurnishEditorUtility.macAB = EditorGUILayout.Toggle("Mac", LiveFurnishEditorUtility.macAB, GUILayout.ExpandWidth(false));
            LiveFurnishEditorUtility.iosAB = EditorGUILayout.Toggle("iOS", LiveFurnishEditorUtility.iosAB, GUILayout.ExpandWidth(false));
            LiveFurnishEditorUtility.webglAB = EditorGUILayout.Toggle("WebGL", LiveFurnishEditorUtility.webglAB, GUILayout.ExpandWidth(false));


            #region Extras
            if (selectedType != 2 && selectedType != 4)
            {
                #region Seperator
                GUILayout.Space(4);
                GUILayout.Label("--------------------", headingLableStyle, GUILayout.ExpandWidth(true));
                GUILayout.Space(2);
                #endregion

                EditorGUILayout.BeginHorizontal();

                // DuplicateCheckx
                if (GUILayout.Button("Duplicate Check", buttonStyle, GUILayout.ExpandWidth(true)))
                    DuplicateCheck();

                // EmptyCheck
                if (GUILayout.Button("Empty Check", buttonStyle, GUILayout.ExpandWidth(true)))
                    EmptyCheck();

                EditorGUILayout.EndHorizontal();
            }
            #endregion
        }

    }





    private static void MakeTextureReadable()
    {
        Debug.Log("Making Texture Readable");

        if (LiveFurnishEditorUtility.MakeTexturesReadable())
            Debug.LogWarning(allGood_TEXT);
    }

    public static void ScaleCheck(int _selectedType)
    {
        // Room
        if (_selectedType == 1 || _selectedType == 2)
        {
            //Debug.Log("ScaleCheck : " + selectedType);
            LiveFurnishEditorUtility.maxChildIterations = 1000;
            LiveFurnishEditorUtility.scaleCheckAllGood = true;
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            bool allGood = true;
            foreach (var gameObject in rootObjects)
            {
                if (!LiveFurnishEditorUtility.ScaleAdjustment(gameObject.transform.root))
                    allGood = false;
            }

            if (allGood)
                Debug.LogWarning(allGood_TEXT);
            return;
        }


        // Prop
        if (_selectedType == 3)
        {
            if (Selection.gameObjects.Length == 0)
            {
                Debug.LogError(noPropSelected_TEXT);
                return;
            }
            else
            {
                LiveFurnishEditorUtility.maxChildIterations = 1000;
                LiveFurnishEditorUtility.scaleCheckAllGood = true;
                bool allGood = true;
                foreach (var gameObject in Selection.gameObjects)
                {
                    //if (IsPrefab(gameObject))
                    //{
                    if (!LiveFurnishEditorUtility.ScaleAdjustment(gameObject.transform.root))
                        allGood = false;
                    //}
                    //else Debug.Log("Not a Prefab : " + gameObject);
                }

                if (allGood)
                    Debug.LogWarning(allGood_TEXT);
            }

            return;
        }
    }

    private static void SetupCheck(int _selectedType)
    {
        #region Room (Staging)
        if (_selectedType == 1)
        {
            PropObjectsData[] propObjectsData = FindObjectsOfType<PropObjectsData>();
            if (propObjectsData.Length > 0)
            {
                foreach (var propData in propObjectsData)
                {
                    Debug.LogWarning(propDataDeleted_TEXT + propData.gameObject.name);
                    DestroyImmediate(propData);
                }
            }

            if (SetupRoom.ExportResurce())
                Debug.LogWarning(roomDataAdded_TEXT);
            else
                Debug.LogWarning(roomDataAlreadyPresent_TEXT);

            return;
        }
        #endregion



        #region Room (NO-Staging)
        if (_selectedType == 2)
        {
            bool allGood = true;

            PropObjectsData[] propObjectsData = FindObjectsOfType<PropObjectsData>();
            if (propObjectsData.Length > 0)
            {
                foreach (var propData in propObjectsData)
                {
                    Debug.LogWarning(propDataDeleted_TEXT + propData.gameObject.name);
                    allGood = false;
                    DestroyImmediate(propData);
                }
            }

            RoomObjectsData[] roomObjectsData = FindObjectsOfType<RoomObjectsData>();
            if (roomObjectsData.Length > 0)
            {
                foreach (var roomData in roomObjectsData)
                {
                    Debug.LogWarning(roomDataDeleted_TEXT + roomData.gameObject.name);
                    allGood = false;
                    DestroyImmediate(roomData.topViewCenter.gameObject);
                    DestroyImmediate(roomData.gameObject);
                }
            }

            if (allGood)
                Debug.LogWarning(allGood_TEXT);

            return;
        }
        #endregion



        #region Prop
        if (_selectedType == 3)
        {
            RoomObjectsData[] roomObjectsData = FindObjectsOfType<RoomObjectsData>();
            if (roomObjectsData.Length > 0)
            {
                foreach (var roomData in roomObjectsData)
                {
                    Debug.LogWarning("<b>Room Objects Data</b> is also FOUND in the SCENE on object ------ " + roomData.gameObject.name);
                    //DestroyImmediate(roomData);
                }
            }

            if (Selection.gameObjects.Length == 0)
            {
                Debug.LogError("SELECT to krlo bhai jispe <b>PropObjectsData</b> add krna hai!");
                return;
            }
            else
            {
                foreach (var gameobject in Selection.gameObjects)
                {
                    if (!gameobject.transform.root.GetComponent<PropObjectsData>())
                    {
                        gameobject.transform.root.gameObject.AddComponent<PropObjectsData>();
                        Debug.LogWarning(propDataAdded_TEXT + gameobject.transform.root.name);
                    }
                    else
                        Debug.LogWarning(propDataAlreadyPresent_TEXT + gameobject.transform.root.name);
                }
            }

            return;
        }
        #endregion


        Debug.LogError(typeNotSelected_TEXT);
    }



    private static void CreateAssetBundlePropNames(int _selectedType)
    {
        if (selectedType == 3)
        {
            if (Selection.gameObjects.Length == 0)
            {
                Debug.LogError("NO Prop is Selected");
                return;
            }
            else
            {
                foreach (var gameobject in Selection.gameObjects)
                {
                    if (!gameobject.transform.root.GetComponent<PropObjectsData>())
                    {
                        gameobject.transform.root.gameObject.AddComponent<PropObjectsData>();
                        Debug.LogWarning(propDataAdded_TEXT + gameobject.transform.root.name);
                    }
                }
            }

            LiveFurnishEditorUtility.SetBundleName(Selection.gameObjects);
        }
    }

    private static void CreateAssetBundleMaterialNames(int _selectedType)
    {
        if (_selectedType == 4)
        {
            if (Selection.objects.Length == 0)
            {
                Debug.LogError("NO Material is Selected");
                return;
            }

            LiveFurnishEditorUtility.SetBundleName(Selection.objects);
            return;
        }
    }


    private static void ClearAndSetAssetBundleRoomName()
    {
        LiveFurnishEditorUtility.ClearAssetBundleNames();
        LiveFurnishEditorUtility.SetBundleName(SceneManager.GetActiveScene().path, SceneManager.GetActiveScene().name);
        Debug.LogWarning("Asset Bundle Names Cleared And Room Name Set");
    }

    private static void CreateAssetBundles(int _selectedType)
    {
        bool _createBundles = true;
        foreach (var gameobject in Selection.gameObjects)
        {
            if (!gameobject.transform.root.GetComponent<PropObjectsData>())
            {
                _createBundles = false;
            }
        }
        if (!_createBundles)
        {
            Debug.LogWarning("Please Setup PropObjectsData Script");
            return;
        }

        foreach (var gameobject in Selection.gameObjects)
        {
            if (!HierarchyCheck(gameobject))
            {
                Debug.LogWarning("Hierarchy Script is Missing a reference on -> " + gameobject.name);
                return;
            }
        }

        foreach (var gameobject in Selection.gameObjects)
        {
            MeshFilter[] meshFilters = gameobject.GetComponentsInChildren<MeshFilter>();
            foreach (var meshFilter in meshFilters)
            {
                if (meshFilter == null)
                {
                    Debug.LogWarning("Mesh Filter is Missing on -> " + meshFilter.gameObject.name);
                    return;
                }
                else if (meshFilter.sharedMesh == null)
                {
                    Debug.LogWarning("Mesh is Missing on -> " + meshFilter.gameObject.name);
                    return;
                }
                else if (meshFilter.sharedMesh.vertexCount < 1)
                {
                    Debug.LogWarning("Mesh has no Verteces on -> " + meshFilter.gameObject.name);
                    return;
                }
            }
        }



        if (makeReadable)
            MakeTextureReadable();

        if (_selectedType == 3)
        {
            LiveFurnishEditorUtility.BuildABs();
            return;
        }

        if (_selectedType == 4)
        {
            LiveFurnishEditorUtility.BuildABs();
            return;
        }

        if (_selectedType == 1 || _selectedType == 2)
        {
            LiveFurnishEditorUtility.BuildABs();
            return;
        }
    }


    #region EXTRAS
    int maxIterations = 1000;
    private static bool duplicateCheckAllGood;
    private void DuplicateCheck()
    {
        #region PROP
        if (selectedType == 3)
        {
            PropObjectsData[] propObjectsData = new PropObjectsData[Selection.gameObjects.Length];

            if (propObjectsData.Length == 0)
            {
                Debug.LogError(propDataMissing_TEXT);
                return;
            }

            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                propObjectsData[i] = Selection.gameObjects[i].transform.root.GetComponent<PropObjectsData>();
                if (propObjectsData[i] == null)
                {
                    Debug.LogError(propDataMissing_TEXT);
                    return;
                }
            }

            bool allGood = true;
            duplicateCheckAllGood = true;
            foreach (var propData in propObjectsData)
            {
                maxIterations = 200;
                if (!CountFrquencyPropData(propData.gameObject.transform.root, propData))
                    allGood = false;
            }

            if (allGood)
                Debug.LogWarning(allGood_TEXT);

            return;
        }
        #endregion


        #region ROOM (STAGING)
        if (selectedType == 1)
        {
            RoomObjectsData roomObjectsData = FindObjectOfType<RoomObjectsData>();
            if (roomObjectsData == null)
            {
                Debug.LogError(roomDataMissing_TEXT);
                return;
            }

            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            bool allGood = true;
            duplicateCheckAllGood = true;
            foreach (var gameObject in rootObjects)
            {
                maxIterations = 200;
                if (!CountFrquencyRoomData(gameObject.transform.root, roomObjectsData))
                    allGood = false;
            }

            if (allGood)
                Debug.LogWarning(allGood_TEXT);
            return;
        }
        #endregion

        Debug.LogError(typeNotSelected_TEXT);
    }
    private bool CountFrquencyRoomData(Transform _parent, RoomObjectsData _roomObjectsData)
    {
        if (maxIterations < 0)
            return duplicateCheckAllGood;
        else
            maxIterations--;

        if (_parent.childCount > 0)
        {
            foreach (Transform child in _parent)
                CountFrquencyRoomData(child, _roomObjectsData);
        }


        int frequency = 0;
        string lists = "";
        // noTextureObjects
        _roomObjectsData.noTextureObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + "noTextureObjects";
            }
        });

        // childObjects
        _roomObjectsData.childObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | childObjects";
            }
        });

        // wallHangingObjects
        _roomObjectsData.wallHangingObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | wallHangingObjects";
            }
        });

        // roofProps
        _roomObjectsData.roofProps.ForEach(delegate (Transform obj)
        {
            if (obj == _parent) frequency++;
            lists = lists + " | roofProps";
        });

        // lightParentObjects
        _roomObjectsData.lightParentObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | lightParentObjects";
            }
        });

        // staticObjects
        _roomObjectsData.staticObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | staticObjects";
            }
        });

        // walls
        _roomObjectsData.walls.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | wallHangingObjects";
            }
        });

        // floors
        _roomObjectsData.floors.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | floors";
            }
        });

        // windows
        _roomObjectsData.windows.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | windows";
            }
        });

        // doors
        _roomObjectsData.doors.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | doors";
            }
        });

        // roofs
        _roomObjectsData.roofs.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | roofs";

            }
        });

        // ignoreRaycastObjects
        _roomObjectsData.ignoreRaycastObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | ignoreRaycastObjects";
            }
        });

        if (_roomObjectsData.outerWall == _parent)
        {
            frequency++;
            lists = lists + " | outerWall";
        }

        //if (frequency == 0)
        //    Debug.LogWarning("<b>" + _parent.name + "</b>" + "-----is <b>NOT</b> referenced anywhere");


        if (frequency > 1)
        {
            duplicateCheckAllGood = false;
            Debug.LogWarning("(ROOT: " + _parent.root.name + ") <b>" + _parent.name + "</b>" + " Found in -----" + lists);
        }

        return duplicateCheckAllGood;
    }
    private bool CountFrquencyPropData(Transform _parent, PropObjectsData _propObjectsData)
    {
        if (maxIterations < 0)
            return duplicateCheckAllGood;
        else
            maxIterations--;

        if (_parent.childCount > 0)
        {
            foreach (Transform child in _parent)
                CountFrquencyPropData(child, _propObjectsData);
        }


        int frequency = 0;
        string lists = "";
        // noTextureObjects
        _propObjectsData.noTextureObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + "noTextureObjects";
            }
        });

        // childObjects
        _propObjectsData.childObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | childObjects";
            }
        });

        // wallHangingObjects
        _propObjectsData.wallHangingObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | wallHangingObjects";
            }
        });

        // roofProps
        _propObjectsData.roofProps.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | roofProps";
            }
        });

        // lightParentObjects
        _propObjectsData.lightParentObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | lightParentObjects";
            }
        });

        // staticObjects
        _propObjectsData.staticObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | staticObjects";
            }
        });

        // walls
        _propObjectsData.walls.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | wallHangingObjects";
            }
        });

        // floors
        _propObjectsData.floors.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | floors";
            }
        });

        // windows
        _propObjectsData.windows.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | windows";
            }
        });

        // doors
        _propObjectsData.doors.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | doors";
            }
        });

        // roofs
        _propObjectsData.roofs.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | roofs";

            }
        });

        // ignoreRaycastObjects
        _propObjectsData.ignoreRaycastObjects.ForEach(delegate (Transform obj)
        {
            if (obj == _parent)
            {
                frequency++;
                lists = lists + " | ignoreRaycastObjects";
            }
        });


        if (frequency > 1)
        {
            duplicateCheckAllGood = false;
            Debug.LogWarning("(ROOT: " + _parent.root.name + ") <b>" + _parent.name + "</b>" + " Found in -----" + lists);
        }

        return duplicateCheckAllGood;
    }


    private void EmptyCheck()
    {
        #region PROP
        if (selectedType == 3)
        {
            PropObjectsData[] propObjectsData = new PropObjectsData[Selection.gameObjects.Length];

            if (propObjectsData.Length == 0)
            {
                Debug.LogError(propDataMissing_TEXT);
                return;
            }

            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                propObjectsData[i] = Selection.gameObjects[i].transform.root.GetComponent<PropObjectsData>();
                if (propObjectsData[i] == null)
                {
                    Debug.LogError(propDataMissing_TEXT);
                    return;
                }
            }

            int index = 0;
            string empty = "";
            bool isAssigned = true;
            bool allGood = true;
            foreach (var propData in propObjectsData)
            {
                // noTextureObjects
                empty = "<b>noTextureObjects</b>-----";
                if (propData.noTextureObjects.Count > 0)
                {
                    propData.noTextureObjects.ForEach(delegate (Transform obj)
                    {
                        if (obj == null)
                        {
                            isAssigned = false;
                            allGood = false;
                            empty = empty + " | " + index;
                        }
                        index++;
                    });
                    if (!isAssigned)
                        Debug.LogWarning("(ROOT: " + propData.transform.root.name + ") " + empty);
                }



                index = 0;
                isAssigned = true;
                // childObjects
                empty = "<b>childObjects</b>-----";
                if (propData.childObjects.Count > 0)
                {
                    propData.childObjects.ForEach(delegate (Transform obj)
                    {
                        if (obj == null)
                        {
                            isAssigned = false;
                            allGood = false;
                            empty = empty + " | " + index;
                        }
                        index++;
                    });
                    if (!isAssigned)
                        Debug.LogWarning("(ROOT: " + propData.transform.root.name + ") " + empty);
                }


                index = 0;
                isAssigned = true;
                // wallHangingObjects
                empty = "<b>wallHangingObjects</b>-----";
                if (propData.wallHangingObjects.Count > 0)
                {
                    propData.wallHangingObjects.ForEach(delegate (Transform obj)
                    {
                        if (obj == null)
                        {
                            isAssigned = false;
                            allGood = false;
                            empty = empty + " | " + index;
                        }
                        index++;
                    });
                    if (!isAssigned)
                        Debug.LogWarning("(ROOT: " + propData.transform.root.name + ") " + empty);
                }


                index = 0;
                isAssigned = true;
                // roofProps
                empty = "<b>roofProps</b>-----";
                if (propData.roofProps.Count > 0)
                {
                    propData.roofProps.ForEach(delegate (Transform obj)
                    {
                        if (obj == null)
                        {
                            isAssigned = false;
                            allGood = false;
                            empty = empty + " | " + index;
                        }
                        index++;
                    });
                    if (!isAssigned)
                        Debug.LogWarning("(ROOT: " + propData.transform.root.name + ") " + empty);
                }


                index = 0;
                isAssigned = true;
                // lightParentObjects
                empty = "<b>lightParentObjects</b>-----";
                if (propData.lightParentObjects.Count > 0)
                {
                    propData.lightParentObjects.ForEach(delegate (Transform obj)
                    {
                        if (obj == null)
                        {
                            isAssigned = false;
                            allGood = false;
                            empty = empty + " | " + index;
                        }
                        index++;
                    });
                    if (!isAssigned)
                        Debug.LogWarning("(ROOT: " + propData.transform.root.name + ") " + empty);
                }
            }


            if (allGood)
                Debug.LogWarning("All Good");
            return;
        }
        #endregion


        #region ROOM STAGGING
        if (selectedType == 1)
        {
            RoomObjectsData roomData = FindObjectOfType<RoomObjectsData>();
            if (roomData == null)
            {
                Debug.LogError(roomDataMissing_TEXT);
                return;
            }


            int index = 0;
            string empty = "";
            bool isAssigned = true;
            bool allGood = true;
            // noTextureObjects
            empty = "<b>noTextureObjects</b>-----";
            if (roomData.noTextureObjects.Count > 0)
            {
                roomData.noTextureObjects.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }



            index = 0;
            isAssigned = true;
            // childObjects
            empty = "<b>childObjects</b>-----";
            if (roomData.childObjects.Count > 0)
            {
                roomData.childObjects.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // wallHangingObjects
            empty = "<b>wallHangingObjects</b>-----";
            if (roomData.wallHangingObjects.Count > 0)
            {
                roomData.wallHangingObjects.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // roofProps
            empty = "<b>roofProps</b>-----";
            if (roomData.roofProps.Count > 0)
            {
                roomData.roofProps.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // lightParentObjects
            empty = "<b>lightParentObjects</b>-----";
            if (roomData.lightParentObjects.Count > 0)
            {
                roomData.lightParentObjects.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // staticObjects
            empty = "<b>staticObjects</b>-----";
            if (roomData.staticObjects.Count > 0)
            {
                roomData.staticObjects.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // walls
            empty = "<b>walls</b>-----";
            if (roomData.walls.Count > 0)
            {
                roomData.walls.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // floors
            empty = "<b>floors</b>-----";
            if (roomData.floors.Count > 0)
            {
                roomData.floors.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // windows
            empty = "<b>windows</b>-----";
            if (roomData.windows.Count > 0)
            {
                roomData.windows.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // doors
            empty = "<b>doors</b>-----";
            if (roomData.doors.Count > 0)
            {
                roomData.doors.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // roofs
            empty = "<b>roofs</b>-----";
            if (roomData.roofs.Count > 0)
            {
                roomData.roofs.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            index = 0;
            isAssigned = true;
            // ignoreRaycastObjects
            empty = "<b>ignoreRaycastObjects</b>-----";
            if (roomData.ignoreRaycastObjects.Count > 0)
            {
                roomData.ignoreRaycastObjects.ForEach(delegate (Transform obj)
                {
                    if (obj == null)
                    {
                        allGood = false;
                        isAssigned = false;
                        empty = empty + " | " + index;
                    }
                    index++;
                });
                if (!isAssigned)
                    Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") " + empty);
            }


            // topViewCenter
            if (roomData.topViewCenter == null)
            {
                allGood = false;
                Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") <b>topViewCenter</b>----- EMPTY");
            }


            // outerWall
            if (roomData.outerWall == null)
            {
                allGood = false;
                Debug.LogWarning("(ROOT: " + roomData.transform.root.name + ") <b>outerWall</b>----- EMPTY");
            }


            if (allGood)
                Debug.LogWarning(allGood_TEXT);

            return;
        }
        #endregion
    }


    private static bool HierarchyCheck(GameObject _gameObject)
    {
        Hierarchy _hierarchy = _gameObject.transform.root.GetComponent<Hierarchy>();

        if (_hierarchy)
        {
            foreach (var relation in _hierarchy.relationships)
            {
                if (relation.parent == null) return false;
                foreach (var child in relation.childs)
                {
                    if (child == null) return false;
                }
            }

        }

        return true;
    }
    #endregion

}
