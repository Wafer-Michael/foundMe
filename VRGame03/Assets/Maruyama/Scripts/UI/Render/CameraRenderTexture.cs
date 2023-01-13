using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRenderTexture : MonoBehaviour
{   
    private IEnumerable Rendering(Camera camera, Vector2Int size, string savePath)
    {
        //���t���[���̃J�����̃����_�����O��҂�
        yield return new WaitForEndOfFrame();

        var renderTexture = new RenderTexture(size.x, size.y, 0);
        camera.targetTexture = renderTexture;

        //�J�����̕`����e�N�X�`���ɏ�������
        camera.Render();

        //���݃A�N�e�B�u��RenderTexture���L���b�V��
        var cache = RenderTexture.active;

        //Pixel����ǂݍ��ނ��߂ɃA�N�e�B�u�Ɏw��
        RenderTexture.active = renderTexture;
        var texture = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

        //RenderTexture.actie����ǂݍ���
        texture.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
        //�e�N�X�`���̕ۑ�
        texture.Apply();

        //�㏈��
        RenderTexture.active = cache;
        camera.targetTexture = null;
        Destroy(renderTexture);

        yield return null;
    }
}
