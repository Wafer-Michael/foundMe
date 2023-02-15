Shader "Hidden/Outline"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Line Color", color) = (1,1,1,1)
        _Threshold("Threshold", Range(0,1)) = 0.5
        _Thickness("Thickness", float) = 1.0
        _Value("Stencil", Int) = 0
    }
    SubShader
    {
        //No culling or depth
        Cull Off
        ZWrite Off 
        ZTest Always
        Lighting off

        Tags{ "RenderType" = "Transparent"
                "Queue" = "Transparent"
                "IgnoreProjector" = "True" }

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _CameraDepthNormalsTexture;
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;
            float4 _Color;
            float _Threshold;
            float _Thickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            float3 sampleNormal(float2 uv)
            {
                float4 cdn = tex2D(_CameraDepthNormalsTexture, uv);
                float3 n = DecodeViewNormalStereo(cdn) * float3(1.0, 1.0, -1.0);
                return n;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 diffUV = _Thickness / 1000;
                diffUV.y *= _ScreenParams.x / _ScreenParams.y;

                half3 norm00 = sampleNormal(i.uv + half2(-diffUV.x, -diffUV.y));
                half3 norm01 = sampleNormal(i.uv + half2(-diffUV.x, 0.0));
                half3 norm02 = sampleNormal(i.uv + half2(-diffUV.x, diffUV.y));
                half3 norm10 = sampleNormal(i.uv + half2(0.0, -diffUV.y));
                half3 norm12 = sampleNormal(i.uv + half2(0.0, diffUV.y));
                half3 norm20 = sampleNormal(i.uv + half2(diffUV.x, -diffUV.y));
                half3 norm21 = sampleNormal(i.uv + half2(diffUV.x, 0.0));
                half3 norm22 = sampleNormal(i.uv + half2(diffUV.x, diffUV.y));

                half3 horizontalValue = 0;
                horizontalValue += norm00 * -1.0;
                horizontalValue += norm01 * -2.0;
                horizontalValue += norm02 * -1.0;
                horizontalValue += norm20;
                horizontalValue += norm21 * 2.0;
                horizontalValue += norm22;

                half3 verticalValue = 0;
                verticalValue += norm00;
                verticalValue += norm10 * 2.0;
                verticalValue += norm20;
                verticalValue += norm02 * -1.0;
                verticalValue += norm12 * -2.0;
                verticalValue += norm22 * -1.0;

                // この値が大きく正の方向を表す部分がアウトライン
                half3 outlineValues = verticalValue * verticalValue + horizontalValue * horizontalValue;
                half outlineValue = outlineValues.x + outlineValues.y + outlineValues.z;
                
                fixed4 texCol = tex2D(_MainTex, i.uv);
                return outlineValue - _Threshold / 10 > 0 ? _Color : texCol;
            }
            ENDCG
        }
    }
}
