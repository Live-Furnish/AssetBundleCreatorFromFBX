using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FixNormalMaps : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        // Check if the asset is being imported as part of a model
        //if (assetPath.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase) || assetPath.EndsWith(".obj", System.StringComparison.OrdinalIgnoreCase))
        //{
        // Load the texture importer
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        // Check if the asset is a texture and if it's a normal map
        //if (IsNormalMap(texture))
        //{
        //    // Set the texture type to Normal map and apply other settings
        //    textureImporter.textureType = TextureImporterType.NormalMap;
        //}
        textureImporter.maxTextureSize = 2048;
        textureImporter.isReadable = true;
    }
    public static bool IsNormalMap(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();

        foreach (Color pixel in pixels)
        {
            // Check if the RGB channels contain values outside the [0, 1] range
            if (pixel.r < 0f || pixel.r > 1f ||
                pixel.g < 0f || pixel.g > 1f ||
                pixel.b < 0f || pixel.b > 1f)
            {
                return true; // Return true if any pixel is outside the expected range
            }
        }

        return false; // Return false if all pixels are within the expected range
    }

    //void OnPostprocessTexture(Texture2D texture)
    //{
    //    TextureImporter textureImporter = (TextureImporter)assetImporter;

    //    //if (IsNormalMap(texture))
    //    //{
    //        // Set the texture type to Normal map and apply other settings
    //        textureImporter.textureType = TextureImporterType.NormalMap;
    //    //textureImporter.convertToNormalmap = true;
    //    //textureImporter.mipmapEnabled = false; // Disable mipmaps for normal maps
    //    //textureImporter.alphaSource = TextureImporterAlphaSource.None; // Clear alpha
    //    // textureImporter.alphaIsTransparency = false; // Disable alpha is transparency
    //    // Reimport the texture to apply the changes
    //    textureImporter.maxTextureSize = 2048;
    //    textureImporter.isReadable = true;
    //    // Set compression settings for normal maps
    //    textureImporter.textureCompression = TextureImporterCompression.Compressed;

    //    // Reimport the texture to apply the changes
    //    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    //    // }
    //}

    //void OnPostprocessModel(GameObject model)
    //{
    //    List<string> defaultMaterialKeys = new List<string>() { MaterialKeys._MainTexKey, MaterialKeys._BumpMapKey, MaterialKeys._OcclusionMapKey, MaterialKeys.roughnessKey, MaterialKeys._AlphaTexture, MaterialKeys._MetallicGlossMap };

    //    // Get all renderers in the model
    //    Renderer[] renderers = model.GetComponentsInChildren<Renderer>();

    //    foreach (Renderer renderer in renderers)
    //    {
    //        // Get all materials of the renderer
    //        Material[] materials = renderer.sharedMaterials;

    //        foreach (Material material in materials)
    //        {
    //            // Iterate through all texture properties of the material
    //            foreach (string texturePropertyName in defaultMaterialKeys)
    //            {
    //                if (material.HasProperty(texturePropertyName))
    //                {
    //                    // Get the texture associated with the property name
    //                    Texture texture = material.GetTexture(texturePropertyName);

    //                    if (texture != null)
    //                    {
    //                        string assetPath = AssetDatabase.GetAssetPath(texture);
    //                        TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(assetPath);
    //                        TextureImporterSettings setting = new TextureImporterSettings();
    //                        // Check if the texture is a normal map
    //                        if (texturePropertyName.Contains("_BumpMap"))
    //                        {
    //                            textureImporter.maxTextureSize = 2048;
    //                            textureImporter.textureType = TextureImporterType.NormalMap;
    //                            //textureImporter.convertToNormalmap = true;
    //                            textureImporter.isReadable = true;
    //                        }
    //                        else
    //                        {
    //                            textureImporter.isReadable = true;

    //                            textureImporter.maxTextureSize = 2048;

    //                            // Apply changes
    //                            //AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}