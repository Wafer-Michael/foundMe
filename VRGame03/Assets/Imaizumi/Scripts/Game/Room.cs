using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Room : MonoBehaviour
{
    [SerializeField]
    GameObject m_mesh;

    // Start is called before the first frame update
    void Start()
    {
        SetDoorNumber();
    }

    void SetDoorNumber()
    {
        var door = FindChildTag(this.gameObject, "Door");
        var doorTex = FetchTextureName(door);
        Debug.Log(doorTex.name);
    }

    GameObject FindChildTag(GameObject parentObj, string tag)
    {
        GameObject result = null;

        var numChild = parentObj.transform.childCount;
        for(int i = 0; i < numChild; i++)
        {
            var child = parentObj.transform.GetChild(i).gameObject;
            if(child.tag == tag)
            {
                result = child;
            }
        }

        return result;
    }

    Texture FetchTextureName(GameObject gameObj)
    {
        Texture result = null;

        var mat = gameObj.GetComponent<Renderer>().material;
        var shader = mat.shader;

        var count = ShaderUtil.GetPropertyCount(shader);
        for (int i = 0; i < count; i++)
        {
            var type = ShaderUtil.GetPropertyType(shader, i);
            if (type == ShaderUtil.ShaderPropertyType.TexEnv)
            {
                var proName = ShaderUtil.GetPropertyName(shader, i);
                var tex = mat.GetTexture(proName);
                if (tex)
                {
                    result = tex;
                    Debug.Log(result.name);
                }
            }
        }

        return result;
    }
}
