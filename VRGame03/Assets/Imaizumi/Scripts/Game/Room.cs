using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Room : MonoBehaviour
{
    [SerializeField]
    GameObject generator;

    // Start is called before the first frame update
    void Start()
    {
        DecisionDoorNumber();
    }

    void DecisionDoorNumber()
    {
        var door = FindChildTag(this.gameObject, "Door");
        var doorTex = FetchTextureName(door);

        var wall = FindChildTag(this.gameObject, "Wall");
        var wallTex = FetchTextureName(wall);

        List<int> numbers = new List<int>();

        var numbergene = generator.GetComponent<NumberLockGenerator>();
        numbers.Add(numbergene.FetchNumber(wallTex, NumberLockGenerator.NumberType.WallPattern));
        numbers.Add(numbergene.FetchNumber(wallTex, NumberLockGenerator.NumberType.WallColor));
        numbers.Add(numbergene.FetchNumber(doorTex, NumberLockGenerator.NumberType.DoorColor));

        Debug.Log("lock Number " + numbers[0] + numbers[1] + numbers[2]);

        door.GetComponent<DoorLock>().SetLockNumbers(numbers);
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
