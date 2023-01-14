Shader "Unlit/NoiseBlendSharder"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _Blend("Blend",Range(0, 1)) = 0.5
        _NoiseAlpha("NoiseAlpha", float) = 1
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

            float4 _MainColor;
            sampler2D _MainTex;
            float4 _SubColor;
            sampler2D _SubTex;
            float _Blend;
            float _NoiseAlpha;

            float4 _MainTex_ST;

            /// <summary>
            /// �����_���Ȓl���擾����֐�
            /// </summary>
            float Random(fixed2 seed) {
                return frac(sin(dot(seed, fixed2(12.9898, 78.233))) * 43758.5453);
            }

            /// <summary>
            /// �m�C�Y�e�N�X�`���̐���
            /// </summary>
            fixed4 CreateNoiseColor(PSInput input)
            {
                float fColor = Random(input.uv);
                fixed4 color = fixed4(fColor, fColor, fColor, _NoiseAlpha);
                return color;
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
                fixed4 noiseColor = CreateNoiseColor(input);
                fixed4 color = main * (1 - _Blend) + noiseColor * _Blend;
                return color;
            }

            ENDCG
        }
    }


}
