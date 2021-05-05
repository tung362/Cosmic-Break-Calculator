﻿Shader "Unlit/Tiled"
{
    Properties
    {
        _TiledTex("Texture", 2D) = "white" {}
        _TileColor("Audio Color", Color) = (1, 0.5, 0.5, 1)
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

            sampler2D _TiledTex;
            float4 _TiledTex_ST;
            float4 _TileColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _TiledTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tiledTex = tex2D(_TiledTex, i.uv);
                float4 textureColor = tiledTex.rgba * _TileColor;
                return textureColor;
            }
            ENDCG
        }
    }
}
