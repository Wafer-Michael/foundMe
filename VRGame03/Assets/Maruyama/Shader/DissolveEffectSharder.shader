Shader "Unlit/DissolveEffectSharder"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _MainColor("Sprite Color", Color) = (1,1,1,1)
        _MaskTex("Mask Texture", 2D) = "white" {}
        _Height("Height", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord  : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                return OUT;
            }

            sampler2D _MainTex;
            float4 _MainColor;
            sampler2D _MaskTex;
            float _Height;

            fixed4 frag(v2f IN) : SV_Target
            {
                float maskHeight = tex2D(_MaskTex, IN.texcoord).r;
                if (maskHeight > _Height) {
                    discard;
                }

                fixed4 color = tex2D(_MainTex, IN.texcoord) * _MainColor;
                float edgeHeight = 0.015;
                return lerp(
                    color,
                    fixed4(0, 4, 2, 0),
                    step(_Height - edgeHeight, maskHeight)
                );
            }
                ENDCG
        }
    }
}
