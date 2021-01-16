Shader "Unlit/AudioVisualizer"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _AudioColor("Audio Color", Color) = (1, 0.5, 0.5, 1)
        _Step("Step", Range(0, 1)) = 1.0

    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
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
            float4 _AudioColor;
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
                float4 textureColor = (mainTex.rgba * i.color) * _AudioColor;
                return lerp(float4(textureColor.r, textureColor.g, textureColor.b, 0), float4(textureColor.r, textureColor.g, textureColor.b, textureColor.a), _Step);
            }
            ENDCG
        }
    }
}
