Shader "Custom/BlendTextureShader"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _SubColor("SubColor", Color) = (1,1,1,1)
        _SubTex("SubTexture", 2D) = "white" {}
        _Blend("Blend",Range(0, 1)) = 1
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
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                float4 _MainColor;
                sampler2D _MainTex;
                float4 _SubColor;
                sampler2D _SubTex;
                float _Blend;
                float4 _MainTex_ST;

                PSInput vert(VSInput input)
                {
                    PSInput output;
                    output.vertex = UnityObjectToClipPos(input.vertex);
                    output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                    UNITY_TRANSFER_FOG(output, output.vertex);
                    return output;
                }

                fixed4 frag(PSInput input) : SV_Target
                {
                    // sample the texture
                    fixed4 main = tex2D(_MainTex, input.uv) * _MainColor;
                    fixed4 sub = tex2D(_SubTex, input.uv) * _SubColor;
                    fixed4 color = main * (1 - _Blend) + sub * _Blend;
                    // apply fog
                    UNITY_APPLY_FOG(input.fogCoord, color);
                    return color;
                }
                ENDCG
            }
        }
    }
    //FallBack "Diffuse"

