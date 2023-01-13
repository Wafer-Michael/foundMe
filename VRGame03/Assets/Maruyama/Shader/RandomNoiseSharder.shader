Shader "Custom/RandomNoiseSharder"
{
    Properties{
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Alpha("Alpha", float) = 1
    }

    SubShader{
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha 
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Alpha;
            float4 _MainTex_ST;

            struct VSInput {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct PSInput {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float random(fixed2 p) {
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
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
                float fColor = random(input.uv);
                fixed4 color = fixed4(fColor, fColor, fColor, _Alpha);
                return color;
            }

            ENDCG
        }
    }
}
