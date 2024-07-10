using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Collections;

public static class LiveFurnishEditorUtility
{
    #region STATIC FIELDS
    // SCALE
    public static float scaleMinAllowed = 0.98f;
    public static float scaleMaxAllowed = 1.01f;
    public static int maxChildIterations = 1000; // Scale check recursion itelations for safe side


    // ASSET BUNDLE
    public static bool windowAB = true;
    public static bool macAB = true;
    public static bool iosAB = false;
    public static bool webglAB = false;
    public static bool scaleCheckAllGood = true;

    public static bool twoK = true;
    public static bool fourK = false;

    private static Material _resizeMat = new Material(Shader.Find("Unlit/Texture"));
    #endregion


    public static bool MakeTexturesReadable()
    {
        var assets = AssetDatabase.FindAssets("t:texture", null).Select(o => AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(o)) as TextureImporter);
        var eligibleAssets = assets.Where(o => o != null).Where(o => !o.isReadable || o.maxTextureSize > 2048);

        bool nothingChanged = true;

        foreach (var textureImporter in eligibleAssets)
        {
            Debug.Log("texture name is " + textureImporter.assetPath);
            textureImporter.isReadable = true;
            
            if (fourK && !twoK)
                textureImporter.maxTextureSize = 4096;
            else if (twoK)
                textureImporter.maxTextureSize = 2048;

            nothingChanged = true;
            AssetDatabase.ImportAsset(textureImporter.assetPath, ImportAssetOptions.ForceUpdate);
        }
        return nothingChanged;
    }


    public static bool ScaleAdjustment(Transform _transform)
    {
        #region Safe Check
        if (maxChildIterations <= 0)
            return scaleCheckAllGood;
        else
            maxChildIterations--;
        #endregion


        if (_transform.childCount > 0)
        {
            foreach (Transform child in _transform)
                ScaleAdjustment(child);
        }

        if ((_transform.localScale.x > scaleMinAllowed && _transform.localScale.x < scaleMaxAllowed) &&
        (_transform.localScale.y > scaleMinAllowed && _transform.localScale.y < scaleMaxAllowed) &&
        (_transform.localScale.z > scaleMinAllowed && _transform.localScale.z < scaleMaxAllowed))
        {
            return scaleCheckAllGood;
        }
        else
        {
            //_transform.localScale = Vector3.one;
            Debug.LogWarning("(ROOT: " + _transform.root.name + ") Scale Issue----- " + "<b>" + _transform.name + "</b>");
            scaleCheckAllGood = false;
            return scaleCheckAllGood;
        }
    }


    public static void ClearAssetBundleNames()
    {
        foreach (var s in AssetDatabase.GetAllAssetBundleNames())
            AssetDatabase.RemoveAssetBundleName(s, true);
    }


    public static void SetBundleName(GameObject[] _gameObjects)
    {
        if (_gameObjects.Length == 0)
            Debug.LogError("No Object is Selected!");
        else
        {
            ClearAssetBundleNames();

            for (int i = 0; i < _gameObjects.Length; i++)
            {
                if (!IsPrefab(_gameObjects[i].transform.root.gameObject))
                {
                    Debug.LogError("<b>" + _gameObjects[i].transform.root.name + "</b> is not a PREFAB");
                    continue;
                }

                string assetPath = AssetDatabase.GetAssetPath(_gameObjects[i]);
                AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant(_gameObjects[i].transform.root.name, "");
                Debug.LogWarning("<b>" + _gameObjects[i].transform.root.name + "</b> ----- Name Set");
            }
        }
    }

    public static void SetBundleName(Object[] _objects)
    {
        if (_objects.Length == 0)
            Debug.LogError("No Object is Selected!");
        else
        {
            ClearAssetBundleNames();

            for (int i = 0; i < _objects.Length; i++)
            {
                string assetPath = AssetDatabase.GetAssetPath(_objects[i]);
                AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant(_objects[i].name, "");
                Debug.LogWarning("<b>" + _objects[i].name + "</b> ----- Name Set");
            }

            Debug.Log("Done");
        }
    }

    public static void SetBundleName(string _path, string _name)
    {
        AssetImporter.GetAtPath(_path).SetAssetBundleNameAndVariant(_name, "");
    }




    public static void BuildABs()
    {
        // 1 Make textures Read Write
        // 2 Check for Scale
        // 3 chECK for room setup object and script &&  chECK for room 2d center object 
        // 3 chECK for prop setup object and script
        // 4  


        //		var assets = AssetDatabase.FindAssets("t:texture", null).Select(o => AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(o)) as TextureImporter);
        //		var eligibleAssets = assets.Where(o => o != null).Where(o => !o.isReadable);
        //
        //		foreach (var textureImporter in eligibleAssets)
        //		{
        //
        //			Debug.Log("texture name is " + textureImporter.assetPath);
        //			textureImporter.isReadable = true;
        //		}


        // Put the bundles in a folder called "ABs" within the Assets folder.

        if (string.IsNullOrEmpty(LiveFurnishEditor.bundlesLocation))
        {
            Debug.LogError("Asset Bundle <b>Location</b> is <b>NULL</b>");
            return;
        }

        if (Directory.Exists(LiveFurnishEditor.bundlesLocation))
        {
            if (!Directory.Exists(LiveFurnishEditor.bundlesLocation + "/win") && windowAB)
                Directory.CreateDirectory(LiveFurnishEditor.bundlesLocation + "/win");

            if (!Directory.Exists(LiveFurnishEditor.bundlesLocation + "/mac") && macAB)
                Directory.CreateDirectory(LiveFurnishEditor.bundlesLocation + "/mac");

            if (!Directory.Exists(LiveFurnishEditor.bundlesLocation + "/ios") && iosAB)
                Directory.CreateDirectory(LiveFurnishEditor.bundlesLocation + "/ios");

            if (!Directory.Exists(LiveFurnishEditor.bundlesLocation + "/webgl") && webglAB)
                Directory.CreateDirectory(LiveFurnishEditor.bundlesLocation + "/webgl");
        }
        else
        {
            Debug.LogError("Location does not exist");
            return;
        }

        if (windowAB)
            BuildPipeline.BuildAssetBundles(LiveFurnishEditor.bundlesLocation + "/win", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        if (macAB)
            BuildPipeline.BuildAssetBundles(LiveFurnishEditor.bundlesLocation + "/mac", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
        if (iosAB)
            BuildPipeline.BuildAssetBundles(LiveFurnishEditor.bundlesLocation + "/ios", BuildAssetBundleOptions.None, BuildTarget.iOS);
        if (webglAB)
            BuildPipeline.BuildAssetBundles(LiveFurnishEditor.bundlesLocation + "/webgl", BuildAssetBundleOptions.None, BuildTarget.WebGL);



        //		BuildPipeline.BuildAssetBundles("D:/unityprojects/LiveFurnish/Rooms/AssetBundles/FromProject/Mac", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXIntel);
    }



    private static bool IsPrefab(GameObject _gameObject)
    {
        return PrefabUtility.GetPrefabParent(_gameObject) == null &&
            PrefabUtility.GetPrefabObject(_gameObject.transform) != null;
    }

    private static bool IsPrefabInstance(GameObject _gameObject)
    {
        return PrefabUtility.GetPrefabParent(_gameObject) != null &&
            PrefabUtility.GetPrefabObject(_gameObject.transform) != null;
    }

}
