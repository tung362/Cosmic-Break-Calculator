Shader "Unlit/VideoPlayer"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BackgroundTex("Background Texture", 2D) = "white" {}
        _BackgroundColor("Background Color", Color) = (1, 0.5, 0.5, 1)
        _VideoColor("Video Color", Color) = (1, 0.5, 0.5, 1)
        [Toggle] _UseGrayscale("Use Grayscale", Float) = 1
        [Toggle] _SmoothGrayscale("Smooth Grayscale", Float) = 0
        _Step("Step", Range(0, 1)) = 1.0

    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _BackgroundTex;
            float4 _BackgroundColor;
            float4 _VideoColor;
            bool _UseGrayscale;
            bool _SmoothGrayscale;
            float _Step;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 mainTex = tex2D(_MainTex, i.uv);
                fixed4 backgroundTex = tex2D(_BackgroundTex, i.uv);

                float4 start = backgroundTex.rgba * _BackgroundColor;
                if (_UseGrayscale)
                {
                    if (_SmoothGrayscale) return lerp(start, dot(mainTex.rgb, float3(0.3, 0.59, 0.11)) * _VideoColor, _Step);
                    return lerp(start, ((mainTex.r + mainTex.b + mainTex.g) / 3) * _VideoColor, _Step);
                }
                return lerp(start, mainTex.rgba * _VideoColor, _Step);
            }
            ENDCG
        }
    }
}
