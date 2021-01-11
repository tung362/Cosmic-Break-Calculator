Shader "Unlit/Greyscale"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [Toggle] _SmoothGreyscale("Smooth Greyscale", Float) = 0
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
            float4 _MainTex_ST;
            bool _SmoothGreyscale;

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

                fixed4 c = tex2D(_MainTex, i.uv);
                if (_SmoothGreyscale) return dot(c.rgb, float3(0.3, 0.59, 0.11)) * i.color;
                return ((c.r + c.b + c.g) / 3) * i.color;
            }
            ENDCG
        }
    }
}
