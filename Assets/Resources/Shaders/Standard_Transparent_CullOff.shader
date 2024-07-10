// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Transparent (CullOff)"
{
	Properties
	{
		[Header(URP 10x)][Header(DEBUG SETTINGS)][Enum(Off,0,On,1)]_ZWrite("ZWrite Mode", Int) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend("SrcBlend", Int) = 5
		[Enum(UnityEngine.Rendering.BlendMode)]_DstBlend("DstBlend", Int) = 10
		[Header(GENERAL SETTINGS)]_GeneralTilling("GeneralTilling", Vector) = (0,0,0,0)
		_GeneralOffset("GeneralOffset", Vector) = (0,0,0,0)
		[Toggle]_UseGeneralTillingOffset("UseGeneralTillingOffset", Float) = 1
		[Toggle]_UseGeneralRotation("UseGeneralRotation", Float) = 1
		_GeneralRotation("GeneralRotation", Range( 0 , 360)) = 0
		[Header(ALBEDO)]_MainTex("Albedo", 2D) = "white" {}
		_Color("AlbedoColor", Color) = (1,1,1,1)
		_MainTexRotation("MainTexRotation", Range( 0 , 360)) = 0
		[Header(OCCLUSION)]_OcclusionMap("Occlusion", 2D) = "white" {}
		_OcclusionMapRotation("OcclusionMapRotation", Range( 0 , 360)) = 0
		_OcclusionStrength("OcclusionStrength", Range( 0 , 1)) = 0
		[Toggle]_InvertOcclusionMap("InvertOcclusionMap", Float) = 0
		[Header(METALLIC)]_MetallicGlossMap("Metallic", 2D) = "white" {}
		_MetallicGlossMapRotation("MetallicGlossMapRotation", Range( 0 , 360)) = 0
		_Metallic("MetallicAmount", Range( 0 , 1)) = 0
	    [Toggle]_InvertMetallicMap("MetallicMap", Float) = 0
		[Header(ROUGHNESS)]_SmoothnessMap("SmoothnessMap", 2D) = "white" {}
		_SmoothnessMapRotation("SmoothnessMapRotation", Range( 0 , 360)) = 0
		_Glossiness("SmoothnessAmount", Range( 0 , 1)) = 0
		[Toggle]_InvertSmoothnessMap("RoughnessMap", Float) = 0
		[Header(NORMAL)][Normal]_BumpMap("NormalMap", 2D) = "bump" {}
		_BumpMapRotation("BumpMapRotation", Range( 0 , 360)) = 0
		_BumpScale("NormalAmount", Float) = 0
		[Toggle]_InvertNormalMap("InvertNormalMap", Float) = 0[Header(ALPHA)]_AlphaTexture("AlphaTexture", 2D) = "black" {}
		_AlphaTextureRotation("AlphaTextureRotation", Range( 0 , 360)) = 0
		[Toggle]_UseAlbedoAlpha("UseAlbedoAlpha", Float) = 0
		[Toggle]_InvertAlpha("InvertAlpha", Float) = 0
		_AlphaMaskAmount("AlphaMaskAmount", Range( 0 , 1)) = 0
		[Header(DETAIL)]_DetailAlbedoMap("DetailAlbedo", 2D) = "white" {}
		[Toggle]_UseDetailAlbedoMap("UseDetailAlbedoMap", Float) = 0
		_DetailAlbedoColor("DetailAlbedoColor", Color) = (1,1,1,1)
		_DetailAlbedoMapRotation("DetailAlbedoMapRotation", Range( 0 , 360)) = 0
		[KeywordEnum(UV1,UV2,UV3)] _DetailAlbedoUVSet("DetailAlbedoUVSet", Float) = 1
		[Header(EMISSION)]_EmissionMap("Emission", 2D) = "white" {}
		_EmissionMapRotation("EmissionMapRotation", Range( 0 , 360)) = 0
		[HDR]_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_EmissionAmount("EmissionAmount", Range( 0 , 1)) = 0
		[Header(HEIGHT)]_ParallaxMap("HeightMap", 2D) = "white" {}
		_ParallaxMapRotation("ParallaxMapRotation", Range( 0 , 360)) = 0
		_Parallax("Parallax", Range( 0 , 0.08)) = 0
		
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite [_ZWrite]
		Blend [_SrcBlend] [_DstBlend]

			// Add for configurator due to Transparent background
		Blend SrcAlpha OneMinusSrcAlpha, One One
			BlendOp Add, Max
			//

		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _DETAILALBEDOUVSET_UV1 _DETAILALBEDOUVSET_UV2 _DETAILALBEDOUVSET_UV3
		struct Input
		{
			float2 uv_texcoord;
			float2 uv2_texcoord2;
			float2 uv3_texcoord3;
		};

		uniform int _DstBlend;
		uniform int _ZWrite;
		uniform int _SrcBlend;
		uniform sampler2D _ParallaxMap;
		uniform float _UseGeneralTillingOffset;
		uniform float2 _GeneralTilling;
		uniform float2 _GeneralOffset;
		uniform float4 _ParallaxMap_ST;
		uniform float _UseGeneralRotation;
		uniform float _GeneralRotation;
		uniform float _ParallaxMapRotation;
		uniform float _Parallax;
		uniform float _InvertNormalMap;
		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float _BumpMapRotation;
		uniform float _BumpScale;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _MainTexRotation;
		uniform float4 _Color;
		uniform sampler2D _DetailAlbedoMap;
		uniform float4 _DetailAlbedoMap_ST;
		uniform float _DetailAlbedoMapRotation;
		uniform float4 _DetailAlbedoColor;
		uniform float _UseDetailAlbedoMap;
		uniform sampler2D _EmissionMap;
		uniform float4 _EmissionMap_ST;
		uniform float _EmissionMapRotation;
		uniform float _EmissionAmount;
		uniform float4 _EmissionColor;
		uniform sampler2D _MetallicGlossMap;
		uniform float4 _MetallicGlossMap_ST;
		uniform float _MetallicGlossMapRotation;
		uniform float _Metallic;
		uniform float _InvertMetallicMap;
		uniform float _InvertSmoothnessMap;
		uniform sampler2D _SmoothnessMap;
		uniform float4 _SmoothnessMap_ST;
		uniform float _SmoothnessMapRotation;
		uniform float _Glossiness;
		uniform float _InvertOcclusionMap;
		uniform sampler2D _OcclusionMap;
		uniform float4 _OcclusionMap_ST;
		uniform float _OcclusionMapRotation;
		uniform float _OcclusionStrength;
		uniform float _InvertAlpha;
		uniform float _UseAlbedoAlpha;
		uniform sampler2D _AlphaTexture;
		uniform float4 _AlphaTexture_ST;
		uniform float _AlphaTextureRotation;
		uniform float _AlphaMaskAmount;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float useGeneralTillingOffset52 = (( _UseGeneralTillingOffset )?( 1.0 ):( 0.0 ));
			float2 uv_TexCoord147 = v.texcoord.xy * _GeneralTilling + _GeneralOffset;
			float2 generalUV148 = uv_TexCoord147;
			float2 uv_ParallaxMap = v.texcoord.xy * _ParallaxMap_ST.xy + _ParallaxMap_ST.zw;
			float useGeneralRotation49 = (( _UseGeneralRotation )?( 1.0 ):( 0.0 ));
			float generalRotation70 = _GeneralRotation;
			float cos6_g74 = cos( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _ParallaxMapRotation ) ) ) );
			float sin6_g74 = sin( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _ParallaxMapRotation ) ) ) );
			float2 rotator6_g74 = mul( ( useGeneralTillingOffset52 == 1.0 ? generalUV148 : uv_ParallaxMap ) - float2( 0.5,0.5 ) , float2x2( cos6_g74 , -sin6_g74 , sin6_g74 , cos6_g74 )) + float2( 0.5,0.5 );
			float3 ase_vertexNormal = v.normal.xyz;
			float4 HEIGHT176 = ( ( tex2Dlod( _ParallaxMap, float4( rotator6_g74, 0, 0.0) ) * float4( ase_vertexNormal , 0.0 ) ) * _Parallax );
			v.vertex.xyz += HEIGHT176.rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float useGeneralTillingOffset52 = (( _UseGeneralTillingOffset )?( 1.0 ):( 0.0 ));
			float2 uv_TexCoord147 = i.uv_texcoord * _GeneralTilling + _GeneralOffset;
			float2 generalUV148 = uv_TexCoord147;
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float useGeneralRotation49 = (( _UseGeneralRotation )?( 1.0 ):( 0.0 ));
			float generalRotation70 = _GeneralRotation;
			float cos6_g71 = cos( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _BumpMapRotation ) ) ) );
			float sin6_g71 = sin( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _BumpMapRotation ) ) ) );
			float2 rotator6_g71 = mul( ( useGeneralTillingOffset52 == 1.0 ? generalUV148 : uv_BumpMap ) - float2( 0.5,0.5 ) , float2x2( cos6_g71 , -sin6_g71 , sin6_g71 , cos6_g71 )) + float2( 0.5,0.5 );
			float3 temp_output_244_13 = UnpackScaleNormal( tex2D( _BumpMap, rotator6_g71 ), _BumpScale );
			float3 break124 = temp_output_244_13;
			float3 appendResult17 = (float3(break124.y , break124.x , break124.z));
			float3 NORMAL30 = (( _InvertNormalMap )?( appendResult17 ):( temp_output_244_13 ));
			o.Normal = NORMAL30;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float cos6_g49 = cos( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _MainTexRotation ) ) ) );
			float sin6_g49 = sin( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _MainTexRotation ) ) ) );
			float2 rotator6_g49 = mul( ( useGeneralTillingOffset52 == 1.0 ? generalUV148 : uv_MainTex ) - float2( 0.5,0.5 ) , float2x2( cos6_g49 , -sin6_g49 , sin6_g49 , cos6_g49 )) + float2( 0.5,0.5 );
			float4 finalAlbedoColor304 = ( tex2D( _MainTex, rotator6_g49 ) * _Color );
			float2 uv2_DetailAlbedoMap = i.uv2_texcoord2 * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			float2 uv_DetailAlbedoMap = i.uv_texcoord * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			float2 uv3_DetailAlbedoMap = i.uv3_texcoord3 * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			#if defined(_DETAILALBEDOUVSET_UV1)
				float2 staticSwitch311 = uv_DetailAlbedoMap;
			#elif defined(_DETAILALBEDOUVSET_UV2)
				float2 staticSwitch311 = uv2_DetailAlbedoMap;
			#elif defined(_DETAILALBEDOUVSET_UV3)
				float2 staticSwitch311 = uv3_DetailAlbedoMap;
			#else
				float2 staticSwitch311 = uv2_DetailAlbedoMap;
			#endif
			float cos6_g72 = cos( radians( _DetailAlbedoMapRotation ) );
			float sin6_g72 = sin( radians( _DetailAlbedoMapRotation ) );
			float2 rotator6_g72 = mul( staticSwitch311 - float2( 0.5,0.5 ) , float2x2( cos6_g72 , -sin6_g72 , sin6_g72 , cos6_g72 )) + float2( 0.5,0.5 );
			float4 temp_output_313_0 = tex2D( _DetailAlbedoMap, rotator6_g72 );
			float temp_output_314_0 = (temp_output_313_0).a;
			float4 lerpResult229 = lerp( finalAlbedoColor304 , ( temp_output_313_0 * temp_output_314_0 * _DetailAlbedoColor ) , (( _UseDetailAlbedoMap )?( temp_output_314_0 ):( 0.0 )));
			float4 ALBEDO29 = lerpResult229;
			o.Albedo = ALBEDO29.rgb;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float cos6_g76 = cos( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _EmissionMapRotation ) ) ) );
			float sin6_g76 = sin( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _EmissionMapRotation ) ) ) );
			float2 rotator6_g76 = mul( ( useGeneralTillingOffset52 == 1.0 ? generalUV148 : uv_EmissionMap ) - float2( 0.5,0.5 ) , float2x2( cos6_g76 , -sin6_g76 , sin6_g76 , cos6_g76 )) + float2( 0.5,0.5 );
			float4 EMISSION295 = ( tex2D( _EmissionMap, rotator6_g76 ) * _EmissionAmount * _EmissionColor );
			o.Emission = EMISSION295.rgb;
			float2 uv_MetallicGlossMap = i.uv_texcoord * _MetallicGlossMap_ST.xy + _MetallicGlossMap_ST.zw;
			float cos6_g75 = cos( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _MetallicGlossMapRotation ) ) ) );
			float sin6_g75 = sin( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _MetallicGlossMapRotation ) ) ) );
			float2 rotator6_g75 = mul( ( useGeneralTillingOffset52 == 1.0 ? generalUV148 : uv_MetallicGlossMap ) - float2( 0.5,0.5 ) , float2x2( cos6_g75 , -sin6_g75 , sin6_g75 , cos6_g75 )) + float2( 0.5,0.5 );
			//float4 METALLIC274 = ( tex2D( _MetallicGlossMap, rotator6_g75 ) * _Metallic );
			float4 temp_output_metallic = tex2D(_MetallicGlossMap, rotator6_g75);
			float4 METALLIC274 = (((_InvertMetallicMap) ? ((1.0 - temp_output_metallic)) : (temp_output_metallic)) * _Metallic);
			o.Metallic = METALLIC274.r;
			float2 uv_SmoothnessMap = i.uv_texcoord * _SmoothnessMap_ST.xy + _SmoothnessMap_ST.zw;
			float cos6_g73 = cos( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _SmoothnessMapRotation ) ) ) );
			float sin6_g73 = sin( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _SmoothnessMapRotation ) ) ) );
			float2 rotator6_g73 = mul( ( useGeneralTillingOffset52 == 1.0 ? generalUV148 : uv_SmoothnessMap ) - float2( 0.5,0.5 ) , float2x2( cos6_g73 , -sin6_g73 , sin6_g73 , cos6_g73 )) + float2( 0.5,0.5 );
			float4 temp_output_243_0 = tex2D( _SmoothnessMap, rotator6_g73 );
			float4 ROUGHNESS38 = ( (( _InvertSmoothnessMap )?( ( 1.0 - temp_output_243_0 ) ):( temp_output_243_0 )) * _Glossiness );
			o.Smoothness = ROUGHNESS38.r;
			float2 uv_OcclusionMap = i.uv_texcoord * _OcclusionMap_ST.xy + _OcclusionMap_ST.zw;
			float cos6_g70 = cos( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _OcclusionMapRotation ) ) ) );
			float sin6_g70 = sin( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _OcclusionMapRotation ) ) ) );
			float2 rotator6_g70 = mul( ( useGeneralTillingOffset52 == 1.0 ? generalUV148 : uv_OcclusionMap ) - float2( 0.5,0.5 ) , float2x2( cos6_g70 , -sin6_g70 , sin6_g70 , cos6_g70 )) + float2( 0.5,0.5 );
			float4 temp_output_246_0 = tex2D( _OcclusionMap, rotator6_g70 );
			float4 AO195 = ( (( _InvertOcclusionMap )?( ( 1.0 - temp_output_246_0 ) ):( temp_output_246_0 )) * _OcclusionStrength );
			o.Occlusion = AO195.r;
			float2 uv_AlphaTexture = i.uv_texcoord * _AlphaTexture_ST.xy + _AlphaTexture_ST.zw;
			float cos6_g62 = cos( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _AlphaTextureRotation ) ) ) );
			float sin6_g62 = sin( radians( ( 360.0 - ( useGeneralRotation49 == 1.0 ? generalRotation70 : _AlphaTextureRotation ) ) ) );
			float2 rotator6_g62 = mul( ( useGeneralTillingOffset52 == 1.0 ? generalUV148 : uv_AlphaTexture ) - float2( 0.5,0.5 ) , float2x2( cos6_g62 , -sin6_g62 , sin6_g62 , cos6_g62 )) + float2( 0.5,0.5 );
			float lerpResult326 = lerp( 1.0 , (( _InvertAlpha )?( ( 1.0 - (( _UseAlbedoAlpha )?( (finalAlbedoColor304).a ):( (tex2D( _AlphaTexture, rotator6_g62 )).r )) ) ):( (( _UseAlbedoAlpha )?( (finalAlbedoColor304).a ):( (tex2D( _AlphaTexture, rotator6_g62 )).r )) )) , _AlphaMaskAmount);
			float ALPHA212 = lerpResult326;
			o.Alpha = ALPHA212;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float2 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				float4 tSpace0 : TEXCOORD4;
				float4 tSpace1 : TEXCOORD5;
				float4 tSpace2 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack1.zw = customInputData.uv2_texcoord2;
				o.customPack1.zw = v.texcoord1;
				o.customPack2.xy = customInputData.uv3_texcoord3;
				o.customPack2.xy = v.texcoord2;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.uv2_texcoord2 = IN.customPack1.zw;
				surfIN.uv3_texcoord3 = IN.customPack2.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
0;0;2560;1379;1137.862;-5911.486;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;50;-3545.206,-1114.969;Inherit;False;1345.067;2500.917;;33;62;60;163;219;65;252;98;251;218;145;64;178;162;52;97;144;201;177;148;70;51;49;147;200;48;69;71;73;279;280;299;302;303;VARIABLES;1,1,1,1;0;0
Node;AmplifyShaderEditor.ToggleSwitchNode;48;-3480.206,-1038.976;Inherit;False;Property;_UseGeneralRotation;UseGeneralRotation;7;0;Create;True;0;0;0;False;0;False;1;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;71;-3510.177,-616.7999;Inherit;False;Property;_GeneralTilling;GeneralTilling;4;1;[Header];Create;True;1;GENERAL SETTINGS;0;0;False;0;False;0,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;73;-3507.221,-486.4978;Inherit;False;Property;_GeneralOffset;GeneralOffset;5;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;69;-3519.177,-929.7999;Inherit;False;Property;_GeneralRotation;GeneralRotation;8;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;70;-3225.177,-929.7999;Inherit;False;generalRotation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;85;-1551.2,-1619.846;Inherit;False;2568.002;1300.394;;27;247;29;229;227;230;228;278;221;304;232;231;95;66;146;81;149;87;150;23;83;82;313;308;309;310;311;312;ALBEDO;0.8235294,0.5104133,0.3803922,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-3232.697,-1040.969;Inherit;False;useGeneralRotation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;51;-3497.654,-811.3911;Inherit;False;Property;_UseGeneralTillingOffset;UseGeneralTillingOffset;6;0;Create;True;0;0;0;False;0;False;1;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;147;-3322.806,-562.8829;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;82;-1437.999,-1190.808;Inherit;False;49;useGeneralRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-1426.187,-1114.116;Inherit;False;70;generalRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;64;-2820.096,-1065.592;Inherit;True;Property;_MainTex;Albedo;9;1;[Header];Create;False;1;ALBEDO;0;0;False;0;False;None;e81e6783e1c6c37409bd190d2ea7628b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RegisterLocalVarNode;148;-3104.806,-567.8829;Inherit;False;generalUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;-3222.654,-813.3911;Inherit;False;useGeneralTillingOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1494.105,-1025.47;Inherit;False;Property;_MainTexRotation;MainTexRotation;11;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-1462.099,-1520.356;Inherit;False;52;useGeneralTillingOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;149;-1387.965,-1445.264;Inherit;False;148;generalUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Compare;81;-1145.786,-1127.403;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;150;-1428.965,-1359.264;Inherit;False;0;64;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;-2498.049,-1052.911;Inherit;False;albedoTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Compare;146;-1115.737,-1406.014;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;-1020.97,-1531.6;Inherit;False;65;albedoTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;95;-975.7101,-1194.34;Inherit;False;2;0;FLOAT;360;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;214;-1488.73,6191.901;Inherit;False;3463.298;1109.529;;20;205;212;306;307;275;305;215;276;322;316;321;323;325;320;324;315;319;318;317;326;ALPHA;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;200;-2788.917,640.0939;Inherit;True;Property;_AlphaTexture;AlphaTexture;34;1;[Header];Create;True;1;ALPHA;0;0;False;0;False;None;None;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;317;-1114.788,6834.709;Inherit;False;49;useGeneralRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;319;-1169.894,7000.047;Inherit;False;Property;_AlphaTextureRotation;AlphaTextureRotation;35;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;231;-670.1266,-1295.781;Inherit;False;Property;_Color;AlbedoColor;10;0;Create;False;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;247;-725.6522,-1459.094;Inherit;False;TextureDegreeRotator;-1;;49;5f28bc49243818546bb1e8d97bb48d35;0;4;1;SAMPLER2D;;False;3;FLOAT2;0,0;False;9;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT3;13;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;318;-1102.976,6911.402;Inherit;False;70;generalRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;324;-1152.943,6501.883;Inherit;False;52;useGeneralTillingOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;179;-1503.22,5352.667;Inherit;False;1935.317;749.0734;;16;195;196;250;249;193;246;188;189;187;185;183;186;184;182;180;181;AO;0.3803922,0.8235294,0.8140223,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-377.2662,-1378.472;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;201;-2507.917,630.0939;Inherit;False;alphaMaskTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;320;-1088.943,6565.883;Inherit;False;148;generalUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;125;-1514.358,3332.345;Inherit;False;2000.637;763.5486;;16;38;8;7;159;160;139;155;140;158;157;156;136;127;131;133;243;ROUGHNESS;0.7591239,0.8235294,0.3803922,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;96;-1539.136,-54.09507;Inherit;False;1995.422;979.4093;;15;152;153;151;154;104;107;106;108;113;30;14;17;124;112;248;NORMAL;0.7803922,0.6577063,0.5764706,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;325;-1120.943,6661.883;Inherit;False;0;200;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;315;-822.5751,6898.116;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;304;-62.03261,-1378.502;Inherit;False;finalAlbedoColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;177;-2811.169,372.0848;Inherit;True;Property;_OcclusionMap;Occlusion;30;1;[Header];Create;False;1;OCCLUSION;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;106;-1448.6,673.501;Inherit;False;Property;_BumpMapRotation;BumpMapRotation;13;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;97;-2809.684,-843.9545;Inherit;True;Property;_BumpMap;NormalMap;12;2;[Header];[Normal];Create;False;1;NORMAL;0;0;False;0;False;None;None;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.CommentaryNode;164;-1508.497,4405.762;Inherit;False;1604.317;741.0734;;16;176;55;53;57;56;173;169;165;170;172;174;171;168;167;166;245;HEIGHT;0.4137073,0.8235294,0.3803922,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;133;-1494.937,3957.871;Inherit;False;Property;_SmoothnessMapRotation;SmoothnessMapRotation;24;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;182;-1378.207,5858.398;Inherit;False;70;generalRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;-1395.018,3869.225;Inherit;False;70;generalRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;218;-2781.983,877.5653;Inherit;True;Property;_DetailAlbedoMap;DetailAlbedo;39;1;[Header];Create;False;1;DETAIL;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;144;-2825.166,-122.1749;Inherit;True;Property;_SmoothnessMap;SmoothnessMap;23;1;[Header];Create;True;1;SMOOTHNESS;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;127;-1406.831,3792.532;Inherit;False;49;useGeneralRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;104;-1392.494,508.1628;Inherit;False;49;useGeneralRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;180;-1446.125,5947.043;Inherit;False;Property;_OcclusionMapRotation;OcclusionMapRotation;31;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;181;-1390.019,5781.704;Inherit;False;49;useGeneralRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;107;-1380.681,584.8555;Inherit;False;70;generalRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;321;-816.9429,6613.883;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;323;-720.9429,6485.883;Inherit;False;201;alphaMaskTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;316;-652.499,6831.176;Inherit;False;2;0;FLOAT;360;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;166;-1383.484,4911.493;Inherit;False;70;generalRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;276;-128.1085,6724.429;Inherit;True;304;finalAlbedoColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;281;-1532.584,1274.931;Inherit;False;2000.637;763.5486;;15;296;295;294;292;291;290;289;288;287;286;285;284;283;282;297;EMISSION;0.7591239,0.8235294,0.3803922,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;157;-1376.036,3540.493;Inherit;False;148;generalUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Compare;183;-1097.806,5845.112;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;257;-1510.811,2388.536;Inherit;False;2000.637;763.5486;;14;274;273;271;268;267;266;265;264;263;262;261;260;259;258;METALLIC;0.7591239,0.8235294,0.3803922,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-1451.402,5000.138;Inherit;False;Property;_ParallaxMapRotation;ParallaxMapRotation;28;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;153;-1360.38,249.8083;Inherit;False;148;generalUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;186;-1380.985,5613.249;Inherit;False;0;177;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;151;-1434.514,174.7163;Inherit;False;52;useGeneralTillingOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;184;-1339.985,5527.249;Inherit;False;148;generalUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;158;-1417.036,3626.493;Inherit;False;0;144;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;219;-2502.983,880.5653;Inherit;False;detailAlbedo;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;310;-991.2103,-781.3351;Inherit;False;2;218;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;167;-1395.296,4834.8;Inherit;False;49;useGeneralRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;156;-1450.17,3465.401;Inherit;False;52;useGeneralTillingOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;178;-2510.169,373.0848;Inherit;False;aoTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;152;-1401.38,335.8083;Inherit;False;0;97;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;162;-2819.493,106.6706;Inherit;True;Property;_ParallaxMap;HeightMap;27;1;[Header];Create;False;1;HEIGHT;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;185;-1414.119,5452.157;Inherit;False;52;useGeneralTillingOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;98;-2492.495,-844.2542;Inherit;False;normalTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FunctionNode;322;-464.943,6597.883;Inherit;False;TextureDegreeRotator;-1;;62;5f28bc49243818546bb1e8d97bb48d35;0;4;1;SAMPLER2D;;False;3;FLOAT2;0,0;False;9;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT3;13;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;309;-991.2103,-908.3351;Inherit;False;1;218;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;145;-2526.166,-119.1749;Inherit;False;roughnessTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;308;-989.1602,-1031.609;Inherit;False;0;218;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;136;-1114.618,3855.938;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;108;-1091.638,534.1165;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;260;-1459.391,3014.062;Inherit;False;Property;_MetallicGlossMapRotation;MetallicGlossMapRotation;21;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;284;-1413.245,1811.811;Inherit;False;70;generalRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;283;-1481.164,1900.456;Inherit;False;Property;_EmissionMapRotation;EmissionMapRotation;17;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;248;-1025.082,693.0942;Inherit;False;Property;_BumpScale;NormalAmount;14;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;170;-1386.262,4666.344;Inherit;False;0;162;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;168;-1345.262,4580.344;Inherit;False;148;generalUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;305;195.2277,6791.586;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;189;-927.73,5778.172;Inherit;False;2;0;FLOAT;360;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;-953.6866,142.53;Inherit;False;98;normalTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;112;-895.5875,466.4174;Inherit;False;2;0;FLOAT;360;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;187;-1067.757,5566.499;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;163;-2519.493,108.6706;Inherit;False;heightTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Compare;169;-1103.083,4898.206;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;171;-1419.396,4505.252;Inherit;False;52;useGeneralTillingOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;140;-992.7732,3398.964;Inherit;False;145;roughnessTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;279;-2811.547,-599.2515;Inherit;True;Property;_EmissionMap;Emission;16;1;[Header];Create;False;1;EMISSION;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ComponentMaskNode;215;-130.8225,6622.189;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;312;-718.003,-783.7175;Inherit;False;Property;_DetailAlbedoMapRotation;DetailAlbedoMapRotation;42;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;154;-1085.271,286.1773;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;259;-1403.285,2848.723;Inherit;False;49;useGeneralRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;251;-2819.464,-351.1498;Inherit;True;Property;_MetallicGlossMap;Metallic;20;1;[Header];Create;False;1;METALLIC;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;221;-667.2353,-1109.86;Inherit;False;219;detailAlbedo;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Compare;155;-1120.172,3552.806;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;311;-721.0204,-931.8066;Inherit;False;Property;_DetailAlbedoUVSet;DetailAlbedoUVSet;43;0;Create;True;0;0;0;False;0;False;0;1;1;True;;KeywordEnum;3;UV1;UV2;UV3;Create;True;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;258;-1391.472,2925.416;Inherit;False;70;generalRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;139;-869.9716,3724.05;Inherit;False;2;0;FLOAT;360;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;282;-1425.058,1735.117;Inherit;False;49;useGeneralRotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;188;-972.9902,5440.913;Inherit;False;178;aoTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;261;-1372.49,2596.684;Inherit;False;148;generalUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;280;-2519.677,-569.4432;Inherit;False;emissionTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FunctionNode;243;-717.4187,3529.327;Inherit;False;TextureDegreeRotator;-1;;73;5f28bc49243818546bb1e8d97bb48d35;0;4;1;SAMPLER2D;;False;3;FLOAT2;0,0;False;9;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT3;13;COLOR;0
Node;AmplifyShaderEditor.Compare;262;-1111.072,2912.129;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;172;-978.2672,4494.008;Inherit;False;163;heightTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;173;-933.0071,4831.269;Inherit;False;2;0;FLOAT;360;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;275;456.8914,6770.429;Inherit;False;Property;_UseAlbedoAlpha;UseAlbedoAlpha;36;0;Create;True;0;0;0;False;0;False;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;252;-2527.594,-321.3416;Inherit;False;metallicTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FunctionNode;313;-384.9171,-1128.335;Inherit;False;TextureDegreeRotator;-1;;72;5f28bc49243818546bb1e8d97bb48d35;0;4;1;SAMPLER2D;;False;3;FLOAT2;0,0;False;9;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT3;13;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;263;-1413.49,2682.684;Inherit;False;0;251;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;174;-1073.034,4619.594;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;264;-1446.623,2521.592;Inherit;False;52;useGeneralTillingOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;244;-720.0349,266.6935;Inherit;False;TextureDegreeRotator;-1;;71;5f28bc49243818546bb1e8d97bb48d35;0;4;1;SAMPLER2D;;False;3;FLOAT2;0,0;False;9;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT3;13;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;246;-727.0776,5549.649;Inherit;False;TextureDegreeRotator;-1;;70;5f28bc49243818546bb1e8d97bb48d35;0;4;1;SAMPLER2D;;False;3;FLOAT2;0,0;False;9;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT3;13;COLOR;0
Node;AmplifyShaderEditor.Compare;287;-1132.845,1798.524;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;288;-1394.263,1483.079;Inherit;False;148;generalUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;285;-1468.396,1407.987;Inherit;False;52;useGeneralTillingOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;286;-1435.263,1569.079;Inherit;False;0;279;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;124;-328.2387,378.6832;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;291;-1010.999,1341.55;Inherit;False;280;emissionTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Compare;290;-1138.399,1495.392;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;314;-48.61516,-1114.57;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;289;-888.1979,1666.635;Inherit;False;2;0;FLOAT;360;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;249;-365.1336,5645.947;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;160;-375.8334,3620.453;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;307;707.88,6864.248;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;266;-989.2263,2455.155;Inherit;False;252;metallicTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.NormalVertexDataNode;53;-601.1914,4727.425;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;278;-57.86612,-923.3936;Inherit;False;Property;_DetailAlbedoColor;DetailAlbedoColor;41;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;245;-732.3547,4602.745;Inherit;False;TextureDegreeRotator;-1;;74;5f28bc49243818546bb1e8d97bb48d35;0;4;1;SAMPLER2D;;False;3;FLOAT2;0,0;False;9;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT3;13;COLOR;0
Node;AmplifyShaderEditor.Compare;265;-1116.626,2608.997;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;267;-866.4254,2780.241;Inherit;False;2;0;FLOAT;360;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;228;-61.05057,-1246.903;Inherit;False;Constant;_Float2;Float 2;26;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;798.3159,7018.042;Inherit;False;Property;_AlphaMaskAmount;AlphaMaskAmount;38;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;292;-735.6451,1471.913;Inherit;False;TextureDegreeRotator;-1;;76;5f28bc49243818546bb1e8d97bb48d35;0;4;1;SAMPLER2D;;False;3;FLOAT2;0,0;False;9;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT3;13;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;306;876.726,6776.403;Inherit;False;Property;_InvertAlpha;InvertAlpha;37;0;Create;True;0;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;268;-713.8727,2585.518;Inherit;False;TextureDegreeRotator;-1;;75;5f28bc49243818546bb1e8d97bb48d35;0;4;1;SAMPLER2D;;False;3;FLOAT2;0,0;False;9;FLOAT;0;False;14;FLOAT;0;False;2;FLOAT3;13;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-686.0617,4896.598;Float;False;Property;_Parallax;Parallax;29;0;Create;True;0;0;0;False;0;False;0;0;0;0.08;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;250;-185.1336,5563.947;Inherit;False;Property;_InvertOcclusionMap;InvertOcclusionMap;33;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;294;-411.0765,1623.735;Inherit;False;Property;_EmissionAmount;EmissionAmount;19;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-410.6617,4612.597;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-229.8497,3701.149;Inherit;False;Property;_Glossiness;SmoothnessAmount;25;0;Create;False;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;271;-389.3039,2738.34;Inherit;False;Property;_Metallic;MetallicAmount;22;0;Create;False;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;193;-225.7845,5715.502;Float;False;Property;_OcclusionStrength;OcclusionStrength;32;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;230;224.638,-1257.173;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;159;-200.963,3546.979;Inherit;False;Property;_InvertSmoothnessMap;InvertSmoothnessMap;26;0;Create;True;0;0;0;False;0;False;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;227;262.5316,-1058.534;Inherit;True;Property;_UseDetailAlbedoMap;UseDetailAlbedoMap;40;0;Create;True;0;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;297;-345.5127,1716.025;Inherit;False;Property;_EmissionColor;EmissionColor;18;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;17;-178.235,381.183;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;326;1196.242,6786.399;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;67.05579,3554.937;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-267.6151,4614.397;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;273;-79.39822,2585.128;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;75.21054,5562.579;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;296;-101.1707,1471.523;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;229;537.9453,-1382.171;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;14;-27.72928,267.8845;Inherit;False;Property;_InvertNormalMap;InvertNormalMap;15;0;Create;True;0;0;0;False;0;False;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;212;1447.963,6791.862;Inherit;False;ALPHA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;805.8143,-1389.671;Inherit;False;ALBEDO;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;224.4826,267.9723;Inherit;False;NORMAL;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;274;83.67178,2581.937;Inherit;False;METALLIC;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;176;-112.7563,4611.952;Inherit;False;HEIGHT;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;295;61.89929,1468.332;Inherit;False;EMISSION;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;223.1259,3553.746;Inherit;False;ROUGHNESS;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;195;223.5208,5565.357;Inherit;False;AO;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;1802.891,-976.0306;Inherit;False;30;NORMAL;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-3454.011,-294.2599;Inherit;False;Constant;_FullDegrees;FullDegrees;12;0;Create;True;0;0;0;False;0;False;360;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;299;-3426.85,77.14251;Inherit;False;Property;_ZWrite;ZWrite Mode;1;2;[Header];[Enum];Create;False;2;URP 10x;DEBUG SETTINGS;0;1;Off,0,On,1;True;0;False;1;1;False;0;1;INT;0
Node;AmplifyShaderEditor.GetLocalVarNode;298;1800.83,-893.0032;Inherit;False;295;EMISSION;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;213;1815.777,-542.4019;Inherit;False;212;ALPHA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;255;1810.094,-806.2982;Inherit;False;274;METALLIC;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-3253.416,-293.8841;Inherit;False;fullDegrees;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;1824.786,-452.8279;Inherit;False;176;HEIGHT;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;197;1816.941,-639.0525;Inherit;False;195;AO;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;1798.589,-716.213;Inherit;False;38;ROUGHNESS;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;1804.49,-1056.031;Inherit;False;29;ALBEDO;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;302;-3424.439,166.3215;Inherit;False;Property;_SrcBlend;SrcBlend;2;1;[Enum];Create;False;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;5;5;False;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;303;-3422.439,252.3215;Inherit;False;Property;_DstBlend;DstBlend;3;1;[Enum];Create;False;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;10;10;False;0;1;INT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2103.546,-920.8358;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Transparent (CullOff);False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;True;299;0;False;300;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;True;302;10;True;303;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;301;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;70;0;69;0
WireConnection;49;0;48;0
WireConnection;147;0;71;0
WireConnection;147;1;73;0
WireConnection;148;0;147;0
WireConnection;52;0;51;0
WireConnection;81;0;82;0
WireConnection;81;2;83;0
WireConnection;81;3;23;0
WireConnection;65;0;64;0
WireConnection;146;0;87;0
WireConnection;146;2;149;0
WireConnection;146;3;150;0
WireConnection;95;1;81;0
WireConnection;247;1;66;0
WireConnection;247;3;146;0
WireConnection;247;9;95;0
WireConnection;232;0;247;0
WireConnection;232;1;231;0
WireConnection;201;0;200;0
WireConnection;315;0;317;0
WireConnection;315;2;318;0
WireConnection;315;3;319;0
WireConnection;304;0;232;0
WireConnection;321;0;324;0
WireConnection;321;2;320;0
WireConnection;321;3;325;0
WireConnection;316;1;315;0
WireConnection;183;0;181;0
WireConnection;183;2;182;0
WireConnection;183;3;180;0
WireConnection;219;0;218;0
WireConnection;178;0;177;0
WireConnection;98;0;97;0
WireConnection;322;1;323;0
WireConnection;322;3;321;0
WireConnection;322;9;316;0
WireConnection;145;0;144;0
WireConnection;136;0;127;0
WireConnection;136;2;131;0
WireConnection;136;3;133;0
WireConnection;108;0;104;0
WireConnection;108;2;107;0
WireConnection;108;3;106;0
WireConnection;305;0;276;0
WireConnection;189;1;183;0
WireConnection;112;1;108;0
WireConnection;187;0;185;0
WireConnection;187;2;184;0
WireConnection;187;3;186;0
WireConnection;163;0;162;0
WireConnection;169;0;167;0
WireConnection;169;2;166;0
WireConnection;169;3;165;0
WireConnection;215;0;322;0
WireConnection;154;0;151;0
WireConnection;154;2;153;0
WireConnection;154;3;152;0
WireConnection;155;0;156;0
WireConnection;155;2;157;0
WireConnection;155;3;158;0
WireConnection;311;1;308;0
WireConnection;311;0;309;0
WireConnection;311;2;310;0
WireConnection;139;1;136;0
WireConnection;280;0;279;0
WireConnection;243;1;140;0
WireConnection;243;3;155;0
WireConnection;243;9;139;0
WireConnection;262;0;259;0
WireConnection;262;2;258;0
WireConnection;262;3;260;0
WireConnection;173;1;169;0
WireConnection;275;0;215;0
WireConnection;275;1;305;0
WireConnection;252;0;251;0
WireConnection;313;1;221;0
WireConnection;313;3;311;0
WireConnection;313;9;312;0
WireConnection;174;0;171;0
WireConnection;174;2;168;0
WireConnection;174;3;170;0
WireConnection;244;1;113;0
WireConnection;244;3;154;0
WireConnection;244;9;112;0
WireConnection;244;14;248;0
WireConnection;246;1;188;0
WireConnection;246;3;187;0
WireConnection;246;9;189;0
WireConnection;287;0;282;0
WireConnection;287;2;284;0
WireConnection;287;3;283;0
WireConnection;124;0;244;13
WireConnection;290;0;285;0
WireConnection;290;2;288;0
WireConnection;290;3;286;0
WireConnection;314;0;313;0
WireConnection;289;1;287;0
WireConnection;249;0;246;0
WireConnection;160;0;243;0
WireConnection;307;0;275;0
WireConnection;245;1;172;0
WireConnection;245;3;174;0
WireConnection;245;9;173;0
WireConnection;265;0;264;0
WireConnection;265;2;261;0
WireConnection;265;3;263;0
WireConnection;267;1;262;0
WireConnection;292;1;291;0
WireConnection;292;3;290;0
WireConnection;292;9;289;0
WireConnection;306;0;275;0
WireConnection;306;1;307;0
WireConnection;268;1;266;0
WireConnection;268;3;265;0
WireConnection;268;9;267;0
WireConnection;250;0;246;0
WireConnection;250;1;249;0
WireConnection;56;0;245;0
WireConnection;56;1;53;0
WireConnection;230;0;313;0
WireConnection;230;1;314;0
WireConnection;230;2;278;0
WireConnection;159;0;243;0
WireConnection;159;1;160;0
WireConnection;227;0;228;0
WireConnection;227;1;314;0
WireConnection;17;0;124;1
WireConnection;17;1;124;0
WireConnection;17;2;124;2
WireConnection;326;1;306;0
WireConnection;326;2;205;0
WireConnection;8;0;159;0
WireConnection;8;1;7;0
WireConnection;57;0;56;0
WireConnection;57;1;55;0
WireConnection;273;0;268;0
WireConnection;273;1;271;0
WireConnection;196;0;250;0
WireConnection;196;1;193;0
WireConnection;296;0;292;0
WireConnection;296;1;294;0
WireConnection;296;2;297;0
WireConnection;229;0;304;0
WireConnection;229;1;230;0
WireConnection;229;2;227;0
WireConnection;14;0;244;13
WireConnection;14;1;17;0
WireConnection;212;0;326;0
WireConnection;29;0;229;0
WireConnection;30;0;14;0
WireConnection;274;0;273;0
WireConnection;176;0;57;0
WireConnection;295;0;296;0
WireConnection;38;0;8;0
WireConnection;195;0;196;0
WireConnection;62;0;60;0
WireConnection;0;0;31;0
WireConnection;0;1;32;0
WireConnection;0;2;298;0
WireConnection;0;3;255;0
WireConnection;0;4;44;0
WireConnection;0;5;197;0
WireConnection;0;9;213;0
WireConnection;0;11;59;0
ASEEND*/
//CHKSM=E8D26BEC97E58F7D7E91645E31BC3E587BAC4681