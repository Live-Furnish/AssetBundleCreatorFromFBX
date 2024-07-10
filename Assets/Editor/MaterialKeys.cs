
public class MaterialKeys
{
    public const string opaqueShader = "Custom/Standard";//shader type to set in starting when right clicked
    public const string transparentShader = "Custom/Transparent";
    public const string transparentDoubleShader = "Custom/Transparent (CullOff)";//shader type to set in starting when right clicked
    public const string transmissionShader = "Custom/Transmission";
    public const string opaqueDoubleShader = "Custom/Standard (CullOff)";
    public const string outerWallShader = "Custom/InvisibleShadowCast";
    public const string _EmissionAmount = "_EmissionAmount";
    public const string _EmissionMapKey = "_EmissionMap";
    public const string _MainTexKey = "_MainTex";
    public const string _MainTexColorKey = "_Color";
    public const string _EmissionColorKey = "_EmissionColor";

    public const string _BumpMapKey = "_BumpMap"; // Normal Map
    public const string _DetailNormalMapKey = "_DetailNormalMap";
    public const string _OcclusionMapKey = "_OcclusionMap"; // AO Map
    public const string _InvertOcculsionMap = "_InvertOcclusionMap";
    public const string _BumpScaleKey = "_BumpScale";
    public const string _OcclusionStrengthKey = "_OcclusionStrength";
    public const string _SpecColorKey = "_SpecColor";
    public const string _InvertBumpMap = "_InvertNormalMap";
    //roughconst ness
    public const string roughnessKey = "_SmoothnessMap"; // Glossy
    public const string roughnessAmount = "_Glossiness";
    public const string _InvertRoughness = "_InvertSmoothnessMap";
    public const string _specularValue = "_GlossMapScale";
    //metalconst lic
    public const string _InvertMetallicMap = "_InvertMetallicMap";
    public const string _MetallicGlossMapRotation = "_MetallicGlossMapRotation";
    public const string _MetallicGlossMap = "_MetallicGlossMap";  //mettalic
    public const string _Metallic = "_Metallic";
    //transconst parency key
    public const string _UseAlbedoAlpha = "_UseAlbedoAlpha";
    public const string _InvertAlpha = "_InvertAlpha";
    public const string _AlphaMaskAmount = "_AlphaMaskAmount";
    public const string _AlphaTexture = "_AlphaTexture"; // Transparency
    public const string _AlphaTextureRotation = "_AlphaTextureRotation";           
    public const string _UVSec = "_UVSec";
    public const string _DetailAlbedoMap = "_DetailAlbedoMap";
    public const string generalRotationKey = "_GeneralRotation";
    public const string _UseGeneralTillingOffsetKey = "_UseGeneralTillingOffset";
    public const string _UseGeneralRotation = "_UseGeneralRotation";
    public const string _GeneralTilling = "_GeneralTilling";
    public const string _GeneralOffset = "_GeneralOffset";
    public const string _MainTexRotationKey = "_MainTexRotation";
    public const string _BumpMapRotationKey = "_BumpMapRotation";
    public const string _SmoothnessMapRotationKey = "_SmoothnessMapRotation";
    public const string _OcclusionMapRotationnKey = "_OcclusionMapRotation";
    public const string _EmissionMapRotationKey = "_EmissionMapRotation";

    public const string parallax = "_Parallax";
    public const string mode = "_Mode";
    public const string zWrite = "_ZWrite";
    public const string srcBlend = "_SrcBlend";
    public const string dstBlend = "_DstBlend";

    public const string alphaTestOn = "_ALPHATEST_ON";

    public const string alphaBlendOn = "_ALPHABLEND_ON";

    public const string alphaPremultiplyOn = "_ALPHAPREMULTIPLY_ON";

    // Transmission related
    public const string planar = "_Planar";
    public const string ior = "_IOR";
    public const string useTransmissionKey = "_UseTransmission";
    public const string hasTransmissionRoughnessMapKey = "_HasRoughnessMap";
    public const string hasTransmissionMapKey = "_HasTransmissionMap";
    public const float planarThreshold = 0.2f;
    public const float planarIOR = 2.95f;
}
