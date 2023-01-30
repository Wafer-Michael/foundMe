Shader "Custom/GlassShader"
{
    Properties
    {
        _BaseColor("Base Color", color) = (1,1,1,1)
        _Adjustment("Rim Effect", Range(0,1)) = 0.25
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
    
    SubShader
    {
        Tags
        { 
            "Queue" = "Transparent" 
            "LightMode" = "ForwardBase"
        }
        LOD 200

        Cull Off // �B�ʏ����Ȃ�
        
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
        half _Glossiness;
        half _Metallic;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _BaseColor.rgb; // �e�N�X�`���̃J���[

            // �����ɑ΂��ĕ��s�ł���قǓ����Ɍ�����悤�ɂ���
            float border = 1 - abs(dot(IN.viewDir, IN.worldNormal));
            float alpha = (border * (1 - _Adjustment) + _Adjustment);
            o.Alpha = _BaseColor.a * alpha; // �����x
            
            o.Smoothness = _Glossiness; // ���˂̊��炩��
            o.Metallic = _Metallic; // ��������
        }
        ENDCG
    }
}
