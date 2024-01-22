Shader "SyntyStudios/Zombie"
{
    Properties
    {
        _Texture("Texture", 2D) = "white" {}
        _Blood("Blood", 2D) = "white" {}
        _BloodColor("BloodColor", Color) = (0.6470588, 0.2569204, 0.2569204, 0)
        _BloodAmount("BloodAmount", Range(0, 1)) = 0
        _Emissive("Emissive", 2D) = "white" {}
        _EmissiveColor("Emissive Color", Color) = (0, 0, 0, 0)
        [HideInInspector] _texcoord("", 2D) = "white" {}
        [HideInInspector] _texcoord2("", 2D) = "white" {}
        [HideInInspector] __dirty("", Int) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalRenderPipeline"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _Texture;
            float4 _Texture_ST;
            float4 _BloodColor;
            sampler2D _Blood;
            float4 _Blood_ST;
            float _BloodAmount;
            sampler2D _Emissive;
            float4 _Emissive_ST;
            float4 _EmissiveColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Texture);
                o.uv2 = TRANSFORM_TEX(v.uv2, _Blood);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv_Texture = i.uv * _Texture_ST.xy + _Texture_ST.zw;
                float2 uv2_Blood = i.uv2 * _Blood_ST.xy + _Blood_ST.zw;
                fixed4 lerpResult33 = lerp(fixed4(0, 0, 0, 0), tex2D(_Blood, uv2_Blood), _BloodAmount);
                fixed4 lerpResult18 = lerp(tex2D(_Texture, uv_Texture), _BloodColor, lerpResult33);
                fixed3 emission = (tex2D(_Emissive, i.uv) * _EmissiveColor).rgb;
                fixed4 finalColor = fixed4(lerpResult18.rgb + emission, 1);
                return finalColor;
            }
            ENDCG
        }
    }
}