Shader "Custom/RandomNoiseSharder"
{
    Properties{
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Alpha("Alpha", float) = 1
    }

    SubShader{
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha //ìßâﬂèàóùâ¬î\
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        float _Alpha;

        struct Input {
            float2 uv_MainTex;
        };

        float random(fixed2 p) {
            return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
        }

        void surf(Input input, inout SurfaceOutputStandard output) {
            float fColor = random(input.uv_MainTex);
            output.Albedo = fixed4(fColor, fColor, fColor, _Alpha);
        }
        ENDCG
    }

    //FallBack "Diffuse"
}
