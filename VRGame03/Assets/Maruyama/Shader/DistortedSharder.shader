Shader "Unlit/DistortedSharder"
{
    Properties
    {
        _mainColor("MainColor", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _fineness("Fineness", float) = 8
        _strength("Strength", float) = 0.05
        _speed("Speed", float) = 0.5
        _testValue("TestValue", float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //シード値からランダムな値を取得
            fixed2 Random(fixed2 seed) {
                fixed2 st = fixed2( dot(seed, fixed2(127.1,311.7)),
                             dot(seed, fixed2(269.5,183.3)) );
                return -1.0 + 2.0 * frac(sin(st) * 43758.5453123);
            }

            //パーリンノイズの値を計算
            float CalculatePerlinNoiseValue(fixed2 st) 
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

            fixed4 CreatePerlinNoiseColor(fixed2 st)
            {
                float fColor = CalculatePerlinNoiseValue(st);
                return fixed4(fColor, fColor, fColor, 1);
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
            float4 _mainColor;      //メインカラー
            float _fineness;        //ノイズの細かさ
            float _strength;        //歪みの強さ
            float _speed;           //歪みのスピード
            float _testValue;

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
                //fixed2 noiseUv = fixed2(input.uv.x + cos(_Time.y * _speed), input.uv.y);
                fixed2 noiseUv = fixed2(input.uv.x + (_Time.y * _speed), input.uv.y);
                //fixed2 noiseUv = fixed2(input.uv.x + _testValue, input.uv.y);
                fixed4 noiseColor = CreatePerlinNoiseColor(noiseUv * _fineness) * _strength;

                fixed2 uv = input.uv;
                uv.x += noiseColor.r;

                fixed4 color = tex2D(_MainTex, uv);

                if (color.a <= 0) {
                    discard;
                }

                return color * _mainColor;
            }
            ENDCG
        }
    }
}
