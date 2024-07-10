using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.EditorCoroutines.Editor;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class CreateAssetBundleFromFbxFile
{    
    public class ModelData
    {
        public string propName;
        public string propId;
        public string jobId;
        public List<MaterialData> materialDetails = new List<MaterialData>();
    }

    public class MaterialData
    {
        public string material_name { get; set; }
        public string base_color { get; set; }
        public string main_tex { get; set; }
        public float metallic { get; set; }
        public string metallic_tex { get; set; }
        public float alpha { get; set; }
        public string alpha_tex { get; set; }
        public float roughness { get; set; }
        public string roughness_tex { get; set; }
        public string normal_tex { get; set; }
        public float normal { get; set; }
        public string occulsion_tex { get; set; }
        public Mapping mapping { get; set; }
        public Offset offset { get; set; }
    }

    public class Mapping
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }

    public class Offset
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }

  
    public const float importScale = 1f;
    public const string mainPath = "Assets/FBX/";
    public const string thumb = "Assets/";
    [SerializeField]
    private const string prefabSaveFolderPath = "Assets/Prefabs/"; // Folder path to save the prefab

    private const string assetBundleOutputPath = "Assets/AssetBundles/";

    private const float interval = 1f; // Time interval in seconds
    private static float lastExecutionTime;

    static CreateAssetBundleFromFbxFile()
    {
        // Register the Update method to the EditorApplication.update event
        EditorApplication.update += Update;
        lastExecutionTime = Time.realtimeSinceStartup;
    }
    public static bool isProcess = false;
    private static void Update()
    {
        if (!isProcess)
        {
            Debug.LogWarning("Checking Requested files in Directory!!!");

        }
        else
        {
            Debug.LogWarning("In Processing State!!!!");
        }

        float currentTime = Time.realtimeSinceStartup;
        if (currentTime - lastExecutionTime > interval)
        {
            if (!isProcess)
            {
                AssetDatabase.Refresh();
                isProcess = true;
                RunCoroutine();
                // Perform background task here
                //Debug.Log("Background task running...");

            }
            lastExecutionTime = currentTime;
        }
    }

    [MenuItem("Tools/Change FBX Import Settings")]
    static void ChangeSettings(string path)
    {
        ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
        if (importer != null)
        {
            // Modify import settings here
            // Example setting
            importer.materialLocation = ModelImporterMaterialLocation.External;
            importer.materialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
            importer.materialName = ModelImporterMaterialName.BasedOnMaterialName;
            // Apply the modifications
            importer.SaveAndReimport();
            Debug.Log("Import settings changed for the object: " + path);
        }
        else
        {
            Debug.LogError("Failed to find ModelImporter at path: " + path);
        }
    }

    static void ResizeTexture(string path)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;


        // Set the texture type to Normal map and apply other settings
        textureImporter.textureType = TextureImporterType.NormalMap;

        textureImporter.maxTextureSize = 2048;
        textureImporter.isReadable = true;
        textureImporter.SaveAndReimport();
    }

    [MenuItem("Tools/Run Editor Coroutine")]
    public static void RunCoroutine()
    {
        EditorCoroutineUtility.StartCoroutineOwnerless(MyEditorCoroutine());
    }

    [MenuItem("Tools/Run Editor Coroutine")]
    public static void StopCoroutine()
    {
        isProcess = false;
    }

    private static IEnumerator MyEditorCoroutine()
    {
        SelectModelForProcessing(mainPath);
        yield return null;
    }
    static string propName;
    static string propId;
    static string jobId;
    public static void SelectModelForProcessing(string path)
    {
        // Create folder if it doesn't exist
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // Get an array of directories (folders) from the specified path
        string[] directories = Directory.GetDirectories(path);
        AssetDatabase.Refresh();
        // Select first folder which contains model details
        if (directories.Length > 0)
        {
            // Read the json file present in the folder and check which maps are applied on the material and apply same map on the material with tilling and offset. Apply these setting on the prefab
            string[] jsonFile = Directory.GetFiles(directories[0], "*.json");
            if (jsonFile.Length > 0)
            {
                // Access files within each child folder
                string[] fbxFile = Directory.GetFiles(directories[0], "*.fbx");
                if (fbxFile.Length > 0)
                {
                    // Select FBX file
                    // need to change the import setting of the fbx file as mentioned in this method ChangeSettings
                    ChangeSettings(fbxFile[0]);

                    GameObject fbxObject = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(fbxFile[0]));
                    if (fbxObject != null)
                    {
                        fbxObject.AddComponent<PropObjectsData>();

                        string jsonString = File.ReadAllText(jsonFile[0]); // Read the entire file contents into a string

                        ModelData data = JsonConvert.DeserializeObject<ModelData>(jsonString);  // Deserialize the JSON string into a C# object
                        if (data != null)
                        {
                            //propName = data.propName;
                            fbxObject.name = fbxObject.name.Replace("(Clone)", "");
                            // for testing
                            propName = fbxObject.name;
                            propId = data.propId;
                            jobId = data.jobId;
                            //sfbxObject.name = propName;
                            LoadMaterialData(fbxObject, data.materialDetails);

                            // Create folder if it doesn't exist
                            if (!Directory.Exists(prefabSaveFolderPath))
                            {
                                Directory.CreateDirectory(prefabSaveFolderPath);
                            }

                            string prefabPath = Path.Combine(prefabSaveFolderPath, propName + ".prefab");

                            // You need to create a prefab from this fbx and store it in the prefab folder.
                            SavePrefab(fbxObject, prefabPath);

                            // You will create asset bundle out of the prefab file present in the prefab folder
                            EditorCoroutineUtility.StartCoroutineOwnerless(CreateAssetBundleAsync(propName, prefabPath, directories[0]));
                        }

                        //else
                        //{
                        //    #region for testing

                        //    //fbxObject.name = fbxObject.name.Replace("(Clone)", "");
                        //    //// for testing
                        //    //propName = fbxObject.name;
                        //    ////LoadMaterialData(fbxObject, data.materialDetails);

                        //    //// Create folder if it doesn't exist
                        //    //if (!Directory.Exists(prefabSaveFolderPath))
                        //    //{
                        //    //    Directory.CreateDirectory(prefabSaveFolderPath);
                        //    //}

                        //    //string prefabPath = Path.Combine(prefabSaveFolderPath, propName + ".prefab");

                        //    //// You need to create a prefab from this fbx and store it in the prefab folder.
                        //    //SavePrefab(fbxObject, prefabPath);

                        //    //// You will create asset bundle out of the prefab file present in the prefab folder
                        //    //EditorCoroutineUtility.StartCoroutineOwnerless(CreateAssetBundleAsync(propName, prefabPath, directories[0]));

                        //    #endregion

                        //    Debug.LogError("Json file not found: ");
                        //    DeleteDirectory(directories[0], "");
                        //}
                    }
                    else
                    {
                        Debug.LogError("Failed to load FBX asset: ");
                        DeleteDirectory(directories[0], "");
                    }
                }
                else
                {
                    Debug.LogWarning("FBx File Not Found!!!!!!!!!!!");
                    isProcess = false;
                }
            }
            else
            {
                Debug.LogWarning("JSON File Not Found!!!!!!!!!!!");
                isProcess = false;
            }
        }
        else
        {
            Debug.LogWarning("No Any File Found!!!!!!!!!!!");
            isProcess = false;
            //Debug.Log("Checking New file!!!!!!!!!!!!!!!!!!");
            //RunCoroutine();
        }
    }

    public static IEnumerator CreateAssetBundleAsync(string objectName, string prefabPath, string directoryPath)
    {
        AssetBundleBuild[] assetBundleBuilds = new AssetBundleBuild[1];
        assetBundleBuilds[0].assetBundleName = objectName;
        assetBundleBuilds[0].assetNames = new string[] { prefabPath };

        // Create folder if it doesn't exist
        if (!Directory.Exists(assetBundleOutputPath))
        {
            Directory.CreateDirectory(assetBundleOutputPath);
        }

        // Build the AssetBundle
        BuildPipeline.BuildAssetBundles(assetBundleOutputPath, assetBundleBuilds, BuildAssetBundleOptions.None, BuildTarget.WebGL);


        // Check if the AssetBundle exists (meaning the build process has completed)
        string bundlePath = Path.Combine(assetBundleOutputPath, objectName);
        while (!File.Exists(bundlePath))
        {
            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds
        }

        // AssetBundle created successfully
        Debug.Log("AssetBundle created successfully at: " + bundlePath);

        // You need to delete the prefab from the prefab folder and also delete the current model from the fbx file
        if (File.Exists(prefabPath))
        {
            // Delete the prefab file
            File.Delete(prefabPath);
            //Debug.Log("Prefab deleted successfully.");
        }

        //TODO: API Calling
        // After Asset bundle is created, you need to call the API provided by the backend to update the asset bundle belongs to that particular product id

        UplaodAssetBundle(directoryPath, bundlePath);
    }

    public static async Task<bool> UplaodAssetBundle(string directoryPath, string bundlePath)
    {
        if (await GetAssetBundleUploadPath() != null)
        {
            bool uploadStatus = await UploadAssetBundle(directoryPath);
            if (await UpdateAssetBundleUploadStatus(uploadStatus))
            {
                if (uploadStatus)
                {
                    Debug.Log("Upload process done Completely");
                }
                else
                {
                    Debug.LogWarning("Bundle Upload FAIL");
                }

                DeleteDirectory(directoryPath, bundlePath);
                return true;
            }
            else
            {
                Debug.LogWarning("Status Update for Bundle upload FAiL");

                DeleteDirectory(directoryPath, bundlePath);
                return false;
            }
        }
        else
        {
            Debug.LogWarning("GetAssetBundleUploadPath FAiL");

            DeleteDirectory(directoryPath, bundlePath);
            return false;
        }
    }
    public const string destinationFolderPath = "D:/BlenderRendering/renderingsystemcode/3D_Model_viewer/fbx_output";
    public static void DeleteDirectory(string directoryPath, string bundlePath)
    {
        return;
        if (Directory.Exists(directoryPath))
        {
            // Get the name of the source folder
            string folderName = new DirectoryInfo(directoryPath).Name;
            // Combine paths to create the destination folder path
            string destinationFolder = Path.Combine(destinationFolderPath, folderName);
            // Copy the source folder and its contents to the destination
            CopyFolder(directoryPath, destinationFolder);
            Debug.Log("Folder copied successfully.");

            Directory.Delete(directoryPath, true);
            Debug.Log("Directory deleted successfully.");

            // Delete the .meta file associated with the folder
            string metaFilePath = directoryPath + ".meta";
            if (File.Exists(metaFilePath))
            {
                File.Delete(metaFilePath);
            }
        }

        //// Check if the bundle file exists
        if (File.Exists(bundlePath))
        {
            string bundleManifestPath = bundlePath + ".manifest";
            // Delete the bundle file
            File.Delete(bundlePath);

            // Check if the bundle manifest file exists
            if (File.Exists(bundleManifestPath))
            {
                // Delete the bundle manifest file
                File.Delete(bundleManifestPath);
            }

            Debug.Log("Bundle deleted successfully.");
        }

        // Refresh the AssetDatabase
        AssetDatabase.Refresh();
        isProcess = false;
    }
    private static void CopyFolder(string sourceFolder, string destinationFolder)
    {
        // Create the destination folder if it doesn't exist
        if (!Directory.Exists(destinationFolder))
        {
            Directory.CreateDirectory(destinationFolder);
        }
        // Get files and copy them
        foreach (string file in Directory.GetFiles(sourceFolder))
        {
            string dest = Path.Combine(destinationFolder, Path.GetFileName(file));
            File.Copy(file, dest, true);
        }
        // Get directories and recursively copy them
        foreach (string subFolder in Directory.GetDirectories(sourceFolder))
        {
            string destSubFolder = Path.Combine(destinationFolder, Path.GetFileName(subFolder));
            CopyFolder(subFolder, destSubFolder);
        }
    }

    static string api_end_point = "https://dev2.imagine.io/";
    static string accessToken = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ0b2tlbl90eXBlIjoiYWNjZXNzIiwiZXhwIjoxNzM1ODgzOTUzLCJpYXQiOjE3MDEzMjM5NTMsImp0aSI6IjVhMzk4OTdmMzJiMDRmY2I4ZTM4NDdjMjk2OWFiNDY3IiwidXNlcl9pZCI6NDkxNSwibWVtYmVyIjo3MTgsIm9yZ2FuaXphdGlvbiI6NjIwLCJpc19lbWFpbF92ZXJpZmllZCI6dHJ1ZX0.wdU5r2BbVrTAbbygGCyntIVcITs81StTk623vrxI3SM";
    static AssetBundleUploadReqData assetBundleUploadData;
    static AssetBundleUploadReqData thumbnailUploadData;
    public static async Task<AssetBundleUploadReqData> GetAssetBundleUploadPath()
    {
        string url = api_end_point + "assetbundles/api/v3/get-products-viewer-job/detail/" + propId + "/";
        //Debug.Log("GetAssetBundleUploadPath 999- " + url);
        //Create request body
        // Dictionary<string, string> requestBody = new Dictionary<string, string>()
        // {
        //     {"asset_bundle_url_key", "newbundle"},
        //     {"status", "2"}
        // };
        // string requestBodyString = JsonConvert.SerializeObject(requestBody);
        // byte[] requestBodyData = System.Text.Encoding.UTF8.GetBytes(requestBodyString);

        //form data 
        WWWForm formData = new WWWForm();
        formData.AddField("file_url_key", propName);
        formData.AddField("thumbnail_name", "THUMBNAIL_" + propId + ".jpg");
        //formData.AddField("status", "2");

        using (UnityWebRequest req = UnityWebRequest.Post(url, formData))
        {
            req.method = "PATCH";
            // req.method = "PATCH";
            req.SetRequestHeader("Authorization", accessToken);
            // req.SetRequestHeader("Content-Type","multipart/form-data");

            // req.SetRequestHeader("")
            var requestOperation = req.SendWebRequest();
            while (!requestOperation.isDone)
            {
                await Task.Yield();
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
            }
            else
            {
                Debug.Log("Get UplaodAssetBundle Data: " + req.downloadHandler.text);
                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(req.downloadHandler.text);
                //Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData["data"].ToString());
                Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData["presigned_data"].ToString());

                Dictionary<string, object> thumbnail_data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData["presigned_url_thumbnail"].ToString());

                assetBundleUploadData = new AssetBundleUploadReqData();//JsonUtility.FromJson<ShoppableExpAssetBundleUploadReqData>(req.downloadHandler.text);
                assetBundleUploadData.data = new AssetBundleUploadReqData.Data();
                assetBundleUploadData.data.presigned_data = new AssetBundleUploadReqData.Presigned_data();
                assetBundleUploadData.data.presigned_data.url = data["url"].ToString();               
                assetBundleUploadData.data.presigned_data.fields = data["fields"].ToString();

                assetBundleUploadData.data.presigned_data.thumbnail_url = thumbnail_data["url"].ToString();
                assetBundleUploadData.data.presigned_data.thumbnail_fields = thumbnail_data["fields"].ToString();
                return assetBundleUploadData;
            }
        }
        return null;
    }

    public static async Task<bool> UpdateAssetBundleUploadStatus(bool uploadStatus)
    {
        string url = api_end_point + "assetbundles/api/v3/products-viewer-job/detail/" + jobId + "/";
        //Debug.Log("UpdateAssetBundleUploadStatus 777- " + url);
        //Create request body
        // Dictionary<string, string> requestBody = new Dictionary<string, string>()
        // {
        //     {"asset_bundle_url_key", "newbundle"},
        //     {"status", "2"}
        // };
        // string requestBodyString = JsonConvert.SerializeObject(requestBody);
        // byte[] requestBodyData = System.Text.Encoding.UTF8.GetBytes(requestBodyString);

        //form data 
        WWWForm formData = new WWWForm();
        // formData.AddField("asset_bundle_url_key", "newbundle");
        if (uploadStatus)
        {
            formData.AddField("status", "1");
        }
        else
        {
            formData.AddField("status", "3");
        }


        //3 for fail
        // 1 for success

        using (UnityWebRequest req = UnityWebRequest.Post(url, formData))
        {
            req.method = "PATCH";
            // req.method = "PATCH";
            req.SetRequestHeader("Authorization", accessToken);
            // req.SetRequestHeader("Content-Type","multipart/form-data");
            // req.SetRequestHeader("")
            var requestOperation = req.SendWebRequest();
            while (!requestOperation.isDone)
            {
                await Task.Yield();
            }
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
            }
            else
            {
                Debug.Log("Upload asset bundle status Updated:" + req.downloadHandler.text);
                return true;
            }
        }
        return false;
    }
    public static async Task<bool> UploadAssetBundle(string directoryPath)
    {
        string url = assetBundleUploadData.data.presigned_data.url;
        //Debug.Log("UploadAssetBundle 888- " + url);
        //Create request body
        // Dictionary<string, string> requestBody = new Dictionary<string, string>()
        // {
        //     {"asset_bundle_url_key", "newbundle"},
        //     {"status", "2"}
        // };
        // string requestBodyString = JsonConvert.SerializeObject(requestBody);
        // byte[] requestBodyData = System.Text.Encoding.UTF8.GetBytes(requestBodyString);

        //form data 

         Dictionary<string, string> fieldsData = JsonConvert.DeserializeObject<Dictionary<string, string>>(assetBundleUploadData.data.presigned_data.fields);

        WWWForm formData = new WWWForm();
        formData.AddField("key", fieldsData["key"]);
        formData.AddField("x-amz-algorithm", fieldsData["x-amz-algorithm"]);
        formData.AddField("x-amz-credential", fieldsData["x-amz-credential"]);
        formData.AddField("x-amz-date", fieldsData["x-amz-date"]);
        formData.AddField("policy", fieldsData["policy"]);
        formData.AddField("x-amz-signature", fieldsData["x-amz-signature"]);

        string bundlePath = Path.Combine(assetBundleOutputPath, propName);

        byte[] data = File.ReadAllBytes(bundlePath);
        formData.AddBinaryData("file", data, propName);

        

        // using(UnityWebRequest file_req = new UnityWebRequest(AssetBundleBuilder.assetbundle_out_path+"/newbundle")){
        //     var requestOperation = file_req.SendWebRequest();
        //     while (!requestOperation.isDone)
        //     {
        //         await Task.Yield();
        //     }
        //     if (file_req.result != UnityWebRequest.Result.Success)
        //     {
        //         Debug.Log(file_req.error);
        //     }
        //     else
        //     {
        //         Debug.Log("file loaded successfully:"+file_req.downloadHandler);
        //         formData.AddBinaryData("file",file_req.downloadHandler.data);
        //     }
        // }

        using (UnityWebRequest req = UnityWebRequest.Post(url, formData))
        {
            // req.method = "PATCH";
            // req.method = "PATCH";
            // req.SetRequestHeader("Authorization",accessToken);
            // req.SetRequestHeader("Content-Type","multipart/form-data");

            // req.SetRequestHeader("")
            var requestOperation = req.SendWebRequest();
            while (!requestOperation.isDone)
            {
                await Task.Yield();
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
            }
            else
            {
                Debug.Log("Bundle Upload successfully!!!!!!!!!!!!!!!!!");
                return await UploadThumbnail(directoryPath);
                //return true;
            }
        }

        return false;
    }

    public static async Task<bool> UploadThumbnail(string directoryPath)
    {
        string url = assetBundleUploadData.data.presigned_data.thumbnail_url;
        //Debug.Log("UploadAssetBundle 888- " + url);
        //Create request body
        // Dictionary<string, string> requestBody = new Dictionary<string, string>()
        // {
        //     {"asset_bundle_url_key", "newbundle"},
        //     {"status", "2"}
        // };
        // string requestBodyString = JsonConvert.SerializeObject(requestBody);
        // byte[] requestBodyData = System.Text.Encoding.UTF8.GetBytes(requestBodyString);

        //form data 

        Dictionary<string, string> fieldsData = JsonConvert.DeserializeObject<Dictionary<string, string>>(assetBundleUploadData.data.presigned_data.thumbnail_fields);

        WWWForm formData = new WWWForm();
        formData.AddField("key", fieldsData["key"]);
        formData.AddField("x-amz-algorithm", fieldsData["x-amz-algorithm"]);
        formData.AddField("x-amz-credential", fieldsData["x-amz-credential"]);
        formData.AddField("x-amz-date", fieldsData["x-amz-date"]);
        formData.AddField("policy", fieldsData["policy"]);
        formData.AddField("x-amz-signature", fieldsData["x-amz-signature"]);

       
        //string thumbnailPath = Path.Combine(thumb, "dummy-image-square.jpg");
        
        //// Convert the texture to PNG format
        //byte[] imageData = File.ReadAllBytes(thumbnailPath);

        //formData.AddBinaryData("file", imageData, "dummy-image-square.jpg", "image/jpg");


        string thumbnailPath = Path.Combine(directoryPath, "THUMBNAIL_" + propId + ".jpg");
        // Convert the texture to PNG format
        byte[] imageData = File.ReadAllBytes(thumbnailPath);

        formData.AddBinaryData("file", imageData, "THUMBNAIL_" + propId + ".jpg", "image/jpg");


        // using(UnityWebRequest file_req = new UnityWebRequest(AssetBundleBuilder.assetbundle_out_path+"/newbundle")){
        //     var requestOperation = file_req.SendWebRequest();
        //     while (!requestOperation.isDone)
        //     {
        //         await Task.Yield();
        //     }
        //     if (file_req.result != UnityWebRequest.Result.Success)
        //     {
        //         Debug.Log(file_req.error);
        //     }
        //     else
        //     {
        //         Debug.Log("file loaded successfully:"+file_req.downloadHandler);
        //         formData.AddBinaryData("file",file_req.downloadHandler.data);
        //     }
        // }

        using (UnityWebRequest req = UnityWebRequest.Post(url, formData))
        {
            // req.method = "PATCH";
            // req.method = "PATCH";
            // req.SetRequestHeader("Authorization",accessToken);
            // req.SetRequestHeader("Content-Type","multipart/form-data");

            // req.SetRequestHeader("")
            var requestOperation = req.SendWebRequest();
            while (!requestOperation.isDone)
            {
                await Task.Yield();
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(req.error);
            }
            else
            {
                Debug.Log("Thumbnail Upload successfully!!!!!!!!!!!!!!!!!");
                return true;
            }
        }

        return false;
    }

    public static void SavePrefab(GameObject obj, string prefabPath)
    {
        // Create the prefab from the GameObject
        PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);
        //Debug.Log("Prefab saved successfully: " + prefabPath);
        UnityEngine.Object.DestroyImmediate(obj);
        //Debug.Log("Object Destroyed successfully");
    }

    public static void LoadMaterialData(GameObject obj, List<MaterialData> materialDataList)
    {
        List<Transform> transform = obj.GetComponentsInChildren<Transform>().ToList();

        foreach (var trans in transform)
        {
            if (trans.GetComponent<SkinnedMeshRenderer>())
            {
                SkinnedMeshRenderer skinnedMesh = trans.GetComponent<SkinnedMeshRenderer>();
                Mesh mesh = skinnedMesh.sharedMesh;
                Material[] materials = skinnedMesh.sharedMaterials;

                MonoBehaviour.DestroyImmediate(skinnedMesh);
                trans.gameObject.AddComponent<MeshFilter>();
                trans.GetComponent<MeshFilter>().sharedMesh = mesh;

                trans.gameObject.AddComponent<MeshRenderer>();
                trans.GetComponent<MeshRenderer>().sharedMaterials = materials;
            }
        }
        List<Renderer> renderers = obj.GetComponentsInChildren<Renderer>().ToList();

        List<Material> objectMaterials = new List<Material>();

        foreach (var item in renderers)
        {
            item.gameObject.AddComponent<MeshCollider>();
            objectMaterials.AddRange(item.GetComponent<Renderer>().sharedMaterials.ToList());
        }

        foreach (var mat in objectMaterials)
        {
            mat.shader = Shader.Find("Custom/Standard");
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);

            if (mat.name.Contains(" (Instance)"))
            {
                mat.name = mat.name.Replace(" (Instance)", "");
            }
            else if (mat.name.Contains("(Instance)"))
            {
                mat.name = mat.name.Replace("(Instance)", "");
            }
            mat.SetVector("_GeneralTilling", new Vector2(1, 1));
            mat.SetFloat("_Parallax", 0f);
            mat.SetFloat("_OcclusionStrength", 1f);
            MaterialData materialData = materialDataList.Where(x => x.material_name.ToLower() == mat.name.ToLower()).Select(x => x).FirstOrDefault();
            if (materialData == null)
            {
                //EditorUtility.DisplayDialog("Message", mat.name + " is not found in the json", "", "");
                continue;
            }

            if (!string.IsNullOrEmpty(materialData.base_color))
                if (ColorUtility.TryParseHtmlString(materialData.base_color, out Color color))
                {
                    mat.SetColor("_Color", color);
                }

            mat.SetFloat("_Metallic", materialData.metallic);
            if (materialData.mapping != null)
            {
                mat.SetVector("_GeneralTilling", new Vector2(materialData.mapping.x, materialData.mapping.y));
            }

            mat.SetTextureScale("_MainTex", new Vector2(materialData.mapping.x, materialData.mapping.y));
            mat.SetTextureScale("_BumpMap", new Vector2(materialData.mapping.x, materialData.mapping.y));
            mat.SetTextureScale("_SmoothnessMap", new Vector2(materialData.mapping.x, materialData.mapping.y));
            mat.SetTextureScale("_MetallicGlossMap", new Vector2(materialData.mapping.x, materialData.mapping.y));
            mat.SetTextureScale("_OcclusionMap", new Vector2(materialData.mapping.x, materialData.mapping.y));

            mat.SetTextureScale("_MainTex", new Vector2(materialData.offset.x, materialData.offset.y));
            mat.SetTextureScale("_BumpMap", new Vector2(materialData.offset.x, materialData.offset.y));
            mat.SetTextureScale("_SmoothnessMap", new Vector2(materialData.offset.x, materialData.offset.y));
            mat.SetTextureScale("_MetallicGlossMap", new Vector2(materialData.offset.x, materialData.offset.y));
            mat.SetTextureScale("_OcclusionMap", new Vector2(materialData.offset.x, materialData.offset.y));

            mat.SetFloat("_Glossiness", materialData.roughness);





            mat.SetFloat("_BumpScale", materialData.normal);

            if (!string.IsNullOrEmpty(materialData.main_tex))
            {
                string path = GetFilePath(materialData.main_tex);

                if (!string.IsNullOrEmpty(path))
                {
                    Texture2D maintex = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));

                    mat.SetTexture("_MainTex", maintex);
                    mat.SetColor("_Color", Color.white);
                }
            }

            if (!string.IsNullOrEmpty(materialData.normal_tex))
            {
                string path = GetFilePath(materialData.normal_tex);

                if (!string.IsNullOrEmpty(path))
                {
                    ResizeTexture(path);

                    Texture2D normaltex = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));

                    mat.SetTexture("_BumpMap", normaltex);
                }
            }

            if (!string.IsNullOrEmpty(materialData.occulsion_tex))
            {
                string path = GetFilePath(materialData.occulsion_tex);

                if (!string.IsNullOrEmpty(path))
                {
                    Texture2D occulsiontex = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));

                    mat.SetTexture("_OcclusionMap", occulsiontex);
                }
            }

            if (!string.IsNullOrEmpty(materialData.metallic_tex))
            {
                string path = GetFilePath(materialData.metallic_tex);

                if (!string.IsNullOrEmpty(path))
                {
                    Texture2D metallictex = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));

                    mat.SetTexture("_MetallicGlossMap", metallictex);
                }
            }

            if (!string.IsNullOrEmpty(materialData.roughness_tex))
            {
                string path = GetFilePath(materialData.roughness_tex);

                if (!string.IsNullOrEmpty(path))
                {
                    Texture2D roughtex = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));

                    mat.SetTexture("_SmoothnessMap", roughtex);
                    mat.SetFloat("_Glossiness", 1);
                    mat.SetFloat("_InvertSmoothnessMap", 1);
                }
            }
            else
            {
                mat.SetFloat("_InvertSmoothnessMap", 0);
            }

            if (materialData.alpha < 1f || !string.IsNullOrEmpty(materialData.alpha_tex))
            {
                mat.shader = Shader.Find("Custom/Transparent");
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetFloat("_AlphaMaskAmount", materialData.alpha);
                mat.SetTextureScale("_AlphaTexture", new Vector2(1, 1));

                if (!string.IsNullOrEmpty(materialData.alpha_tex))
                {
                    string path = GetFilePath(materialData.alpha_tex);

                    if (!string.IsNullOrEmpty(path))
                    {
                        Texture2D alphatex = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));

                        mat.SetTexture("_AlphaTexture", alphatex);
                        mat.SetFloat("_AlphaMaskAmount", 1f);
                    }
                }
            }
        }
        Debug.Log("Material details set successfully");
    }

    public static string GetFilePath(string fileName)
    {
        string[] fileNames = Directory.GetFiles("Assets/", "*.*", SearchOption.AllDirectories);

        foreach (var path in fileNames)
        {
            if (Path.GetFileName(path) == fileName)
            {
                return path;
            }
        }
        return "";
    }

    //    {
    //  "materialDetails": [
    //    {
    //      "material_name": "damagedHelmet-Material_MR",
    //      "base_color": "#E9EAE4",
    //      "main_tex": null,
    //      "metallic": 0.0,
    //      "metallic_tex": null,
    //      "alpha": 1.0,
    //      "alpha_tex": null,
    //      "roughness": 0.1499999761581421,
    //      "roughness_tex": null,
    //      "normal": 1,
    //      "normal_tex": null,
    //      "mapping": {
    //        "X": 1,
    //        "y": 1,
    //        "z": 1
    //      },
    //      "offset": {
    //    "X": 0,
    //        "y": 0,
    //        "z": 0
    //      }
    //    }
    //  ],
    //  "objectName": "damagedHelmet"
    //}
    [Serializable]
    public class AssetBundleUploadReqData
    {
        [SerializeField]
        public Data data;
        [Serializable]
        public class Data
        {
            public Presigned_data presigned_data;
        }
        [Serializable]
        public class Presigned_data
        {
            public string url;
            public string fields;

            public string thumbnail_url;
            public string thumbnail_fields;
        }
    }
}