using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostEffect : MonoBehaviour
{
    [SerializeField, Range(0.0f, 10.0f)]
    float m_thickness = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    float m_threshold = 0.01f;
    [SerializeField]
    Color m_color;
    [SerializeField]
    int m_Stencil;

    [SerializeField]
    Material m_material;

    private void Awake()
    {
        Initialized();
    }

    void Initialized()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;

        var camera = GetComponent<Camera>();
        if(camera.allowMSAA)
        {
            return;
        }

        var comBuffer = new CommandBuffer();
        comBuffer.name = "Outline";

        // レンダリング結果のテクスチャを取得
        int tempTexID = Shader.PropertyToID("_PostEffect");
        comBuffer.GetTemporaryRT(tempTexID, -1, -1);

        // レンダリング結果を更新
        comBuffer.Blit(BuiltinRenderTextureType.CameraTarget, tempTexID);
        comBuffer.Blit(tempTexID, BuiltinRenderTextureType.CameraTarget, m_material);

        comBuffer.ReleaseTemporaryRT(tempTexID);

        // コマンドバッファの追加先を登録
        camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, comBuffer);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        m_material.SetFloat("_Thickness", m_thickness);
        m_material.SetFloat("_Threshold", m_threshold);
        m_material.SetColor("_Color", m_color);
        m_material.SetInt("_Value", m_Stencil);
        Graphics.Blit(source, destination, m_material);
    }
}
