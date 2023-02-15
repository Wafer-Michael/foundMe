using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ImabeSizeAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var _image = GetComponent<RawImage>();
        float rate = (float)_image.texture.width / _image.texture.height;
        float imageHeight = _image.rectTransform.sizeDelta.y;
        _image.rectTransform.sizeDelta = new Vector2(imageHeight * rate, imageHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
