Shader "Custom/InvisibleShadowCast"
{
    Properties
    {
        [Toggle] _Visible ("Visible", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }
            ZWrite Off
            ColorMask 0
            Cull Off
        }

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        float _Visible;

        void surf (Input IN, inout SurfaceOutput o)
        {
            if (_Visible > 0)
            {
                // Object is visible
                o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
                o.Alpha = 1;
            }
            else
            {
                discard;
            }
        }
        ENDCG
    }

    FallBack "Diffuse"
}