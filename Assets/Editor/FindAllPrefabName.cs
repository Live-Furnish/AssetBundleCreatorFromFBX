using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FindAllPrefabName 
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [MenuItem("LiveFurnish/FindAllPrefabs")]
    static void FindAllPrefabNames()
    {
        string[] guids= AssetDatabase.FindAssets("t:prefab");
        List<string> prefabNames = new List<string>();
        string assetsFolderPath = Application.dataPath;

        File.WriteAllLines("Assets" + "/PrefabNames.txt", new List<string>() { "" });
        File.AppendAllLines("Assets" + "/PrefabNames.txt", new List<string>() { "Prefab Names List" });
        File.AppendAllLines("Assets" + "/PrefabNames.txt", new List<string>() {"Project Path "+ assetsFolderPath });

        File.AppendAllLines("Assets" + "/PrefabNames.txt", new List<string>() { ""});


        File.AppendAllLines("Assets" + "/PrefabNames.txt", new List<string>() { "*******************************************" });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Debug.Log(go.name);
            prefabNames.Add(go.name);
        }
         prefabNames.Sort();

        File.AppendAllLines("Assets" + "/PrefabNames.txt", prefabNames);

        File.AppendAllLines("Assets" + "/PrefabNames.txt", new List<string>() { "" });

        File.AppendAllLines("Assets" + "/PrefabNames.txt", new List<string>() { "*******************************************" });

    }
}
