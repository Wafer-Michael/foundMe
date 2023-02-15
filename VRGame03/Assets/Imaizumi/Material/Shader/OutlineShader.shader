Shader "Unlit/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Line Color", color) = (1,1,1,1)
        _Thickness("Thickness", Range(0, 1)) = 0.0
        _Threshold("Threshold", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv       : TEXCOORD0;
            float3 normal : NORMAL;
        };

        struct v2f
        {
            float4 pos : SV_POSITION;
            float2 uv       : TEXCOORD0;
        };

        sampler2D _MainTex;
        sampler2D _MainTex_ST;
        float4 _Color;
        float _Threshold;

        ENDCG

        Pass
        {
            Name "BASE"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;// TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
        Pass
        {
            Name "OUTLINE"

            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); //頂点をMVP行列変換

                float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal)); //モデル座標系の法線をビュー座標系に変換
                float2 offset = TransformViewToProjection(norm.xy); //ビュー座標系に変換した法線を投影座標系に変換

                o.pos.xy += offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(o.pos.z) * _Threshold; //法線方向に頂点位置を押し出し

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
