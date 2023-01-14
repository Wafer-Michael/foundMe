Shader "Unlit/NoiseBlendSharder"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _Blend("Blend",Range(0, 1)) = 0.5
        _NoiseAlpha("NoiseAlpha", float) = 1    //ノイズのα値
        _NoiseUpdateSpeed("NoiseUpdateSpeed", float) = 1
        _NoiseValue("NoiseValue", float) = 1    //ノイズ化する値
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

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

            float4 _MainColor;          //メインカラー
            sampler2D _MainTex;         //メインテクスチャ
            float _Blend;               //ブレンド値
            float _NoiseAlpha;          //ノイズα値
            float _NoiseUpdateSpeed;    //ノイズ更新速度
            float _NoiseValue;          //ノイズかする値。

            float4 _MainTex_ST;         //メインテクスチャ生成用

            /// <summary>
            /// ランダム関数
            /// </summary>
            float Random(fixed2 seed) {
                return frac(sin(dot(seed, fixed2(12.9898, 78.233))) * 43758.5453);
            }

            /// <summary>
            /// ノイズカラーの取得
            /// </summary>
            fixed4 CreateNoiseColor(PSInput input)
            {
                float2 offset = (float2(0, 1) * _Time.y * _NoiseUpdateSpeed);
                float fColor = Random(input.uv * offset);
                fixed4 color = fixed4(fColor, fColor, fColor, _NoiseAlpha);
                return color;
            }

            fixed4 Blend(PSInput input, fixed4 mainColor)
            {
                fixed4 noiseColor = CreateNoiseColor(input);
                //一定値以上ならノイズ化
                if(noiseColor.x <= _NoiseValue) {
                    return mainColor * (1 - _Blend) + noiseColor * _Blend;
                }

                return mainColor;
            }

            PSInput vert(VSInput input)
            {
                PSInput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);

                return output;
            }

            fixed4 frag(PSInput input) : SV_Target
            {
                //texture�̓ǂݍ���
                fixed4 main = tex2D(_MainTex, input.uv) * _MainColor;
                fixed4 color = Blend(input, main);
                return color;
            }

            ENDCG
        }
    }


}
