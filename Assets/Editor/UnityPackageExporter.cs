using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UnityPackageExporter : MonoBehaviour
{
    
    [MenuItem("UnityPackageExporter/Export Package")]
    static void Export()
    {

        EditorCoroutine.start(CreatePackageFromPrefab());
    }
   

    private static IEnumerator CreatePackageFromPrefab()
    {
       

        string BasePath = "Assets/";
        string packageFolderName = "Exported Unity Packages";

        if (!Directory.Exists(BasePath + packageFolderName))
        {

            Directory.CreateDirectory(BasePath + packageFolderName);

        }
        string packageFolderPath = BasePath + packageFolderName + "/";


        foreach (var item in Selection.objects)
        {
            var exportedPackageAssetList = new List<string>();

            string path = AssetDatabase.GetAssetPath(item);

            Debug.Log(path);
            exportedPackageAssetList.Add(AssetDatabase.GetAssetPath(item));
            string packagePath = packageFolderPath + item.name + ".unitypackage";

            if(File.Exists(packagePath))
            {
                File.Delete(packagePath);
            }

            //Export Shaders and Prefabs with their dependencies into a .unitypackage
            AssetDatabase.ExportPackage(exportedPackageAssetList.ToArray(), packagePath,
                ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);

            while(!File.Exists(packagePath))
            {
                Debug.Log("processing");
                yield return null;
            }
        }

        EditorUtility.DisplayDialog("Export Prefab as Package", "All prefabs have been processed","Ok");
      yield return null;
    }
}
