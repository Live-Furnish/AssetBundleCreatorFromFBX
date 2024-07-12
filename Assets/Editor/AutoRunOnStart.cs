using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class AutoRunOnStart
{
    static AutoRunOnStart()
    {
        // This method will be called when the Unity editor starts.
        EditorApplication.delayCall += OnEditorStarted;
    }

    static void OnEditorStarted()
    {
        // Perform tasks you want to automate here.
        Debug.Log("Unity Editor started. Running automatic tasks...");
        CreateAssetBundleFromFbxFile.RunCoroutine();
        // Example: Load a scene or run a function
        // EditorSceneManager.LoadScene(0); // Example of loading a scene

        // Example: Run a specific function
        // YourScriptName.YourFunctionName(); // Replace with your script and function name
    }
}
