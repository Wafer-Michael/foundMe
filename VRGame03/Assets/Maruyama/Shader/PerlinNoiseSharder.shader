Shader "Unlit/PerlinNoiseSharder"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _fineness("Fineness", float) = 8  //ノイズの細かさ
        _alpha("Alpha", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
         
            fixed2 Random(fixed2 seed) {
                fixed2 st = fixed2( dot(seed, fixed2(127.1,311.7)),
                             dot(seed, fixed2(269.5,183.3)) );
                return -1.0 + 2.0 * frac(sin(st) * 43758.5453123);
            }

            float PerlinNoise(fixed2 st) 
            {
                fixed2 p = floor(st);
                fixed2 f = frac(st);
                fixed2 u = f * f * (3.0-2.0*f);

                float v00 = Random(p + fixed2(0,0));
                float v10 = Random(p + fixed2(1,0));
                float v01 = Random(p + fixed2(0,1));
                float v11 = Random(p + fixed2(1,1));

                return lerp( lerp( dot( v00, f - fixed2(0,0) ), dot( v10, f - fixed2(1,0) ), u.x ),
                            lerp( dot( v01, f - fixed2(0,1) ), dot( v11, f - fixed2(1,1) ), u.x ), 
                                u.y) + 0.5f;
            }

            struct VSInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct PSInput
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _fineness;    //ノイズの細かさ
            float _alpha;

            float4 _MainTex_ST;

            PSInput vert (VSInput input)
            {
                PSInput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }

            fixed4 frag (PSInput input) : SV_Target
            {
                float c = PerlinNoise(input.uv * _fineness); //(perlinNoise( IN.uv_MainTex*6))*0.5+0.5;
                fixed4 color = fixed4(c, c, c, 1);
                return color;
            }
            ENDCG
        }
    }
}
