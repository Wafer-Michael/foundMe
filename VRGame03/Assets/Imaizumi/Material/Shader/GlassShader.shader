Shader "Custom/GlassShader"
{
    Properties
    {
        _BaseColor("Base Color", color) = (1,1,1,1)
        _Adjustment("Rim Effect", Range(-1,1)) = 0.25
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 200

        Cull Off
        CGPROGRAM

#pragma surface surf Standard alpha:fade
        #pragma target 3.0

        struct Input
        {
            float3 worldNormal;
            float3 viewDir;
        };

        fixed4 _BaseColor;
        float _Adjustment;
        half _Metallic;
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _BaseColor.rgb;
            float border = 1 - abs(dot(IN.viewDir, IN.worldNormal));
            float alpha = (border * (1 - _Adjustment) + _Adjustment);
            o.Alpha = _BaseColor.a * alpha;
            o.Metallic = _Metallic;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
