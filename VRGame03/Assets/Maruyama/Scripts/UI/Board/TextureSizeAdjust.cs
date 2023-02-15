using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TextureSizeAdjust : MonoBehaviour
{
    void Start()
    {
        //float rate = (float)_image.texture.width / _image.texture.height;
        //float imageHeight = _image.rectTransform.sizeDelta.y;
        //_image.rectTransform.sizeDelta = new Vector2(imageHeight * rate, imageHeight);
    }

    private void Awake()
    {
        var render = GetComponent<Renderer>();
        attacheWebImageToGameobject_appropriately(render.material.mainTexture, gameObject);
    }

    /// <summary>
    /// //�w�肵���E�F�u�摜��ǂݍ���ŃQ�[���I�u�W�F�N�g�̃e�N�X�`���Ƃ��ĕ\��(�K�؂ɕ\���T�C�Y�𒲐�)
    /// �ǂݍ��݉摜���ő�ŕ\�������悤�ɕ\��������������������܂�
    /// </summary>
    /// <param name="url"></param>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static void attacheWebImageToGameobject_appropriately(Texture texture, GameObject gObj)
    {
        //gObj.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(0.8f, 0.5f));
        //gObj.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2((Img_x - (Obj_x * Img_y / Obj_y)) / (2 * Img_x), 1f));

        //WWW texturewww = new WWW(url);
        //yield return texturewww;
        gObj.GetComponent<Renderer>().material.mainTexture = texture;

        float Obj_x = gObj.transform.lossyScale.x;
        float Obj_y = gObj.transform.lossyScale.z;
        float Img_x = (float)texture.width;
        float Img_y = (float)texture.height;

        float aspectRatio_Obj = Obj_x / Obj_y;
        float aspectRatio_Img = Img_x / Img_y;

        if (aspectRatio_Img > aspectRatio_Obj)
        {
            Debug.Log("����Width");
            //�C���[�W�T�C�Y�̂ق������ɒ����ꍇ
            gObj.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(aspectRatio_Obj / aspectRatio_Img, 1f));
            gObj.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2((Img_x - (Obj_x * Img_y / Obj_y)) / (2 * Img_x), 1f));
        }
        else
        {
            Debug.Log("����Height");
            //�C���[�W�T�C�Y�̂ق����c�ɒ����ꍇ
            gObj.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1f, aspectRatio_Img / aspectRatio_Obj));
            gObj.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(1f, (Img_y - Obj_y * Img_x / Obj_x) / (2 * Img_y)));
        }
    }
}
