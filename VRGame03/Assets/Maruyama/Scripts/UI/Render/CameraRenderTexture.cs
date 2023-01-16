using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRenderTexture : MonoBehaviour
{   
    private IEnumerable Rendering(Camera camera, Vector2Int size, string savePath)
    {
        //元フレームのカメラのレンダリングを待つ
        yield return new WaitForEndOfFrame();

        var renderTexture = new RenderTexture(size.x, size.y, 0);
        camera.targetTexture = renderTexture;

        //カメラの描画をテクスチャに書き込み
        camera.Render();

        //現在アクティブなRenderTextureをキャッシュ
        var cache = RenderTexture.active;

        //Pixel情報を読み込むためにアクティブに指定
        RenderTexture.active = renderTexture;
        var texture = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

        //RenderTexture.actieから読み込み
        texture.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
        //テクスチャの保存
        texture.Apply();

        //後処理
        RenderTexture.active = cache;
        camera.targetTexture = null;
        Destroy(renderTexture);

        yield return null;
    }
}
