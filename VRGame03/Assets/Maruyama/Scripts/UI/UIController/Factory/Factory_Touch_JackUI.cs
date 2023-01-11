using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory_Touch_JackUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_floor;

    [SerializeField]
    private GameObject m_mapObject; //MapVisual

    [SerializeField]
    private GameObject m_prefab;

    private List<Jackable> m_jackables = new List<Jackable>();

    private void Start()
    {
        //�t�B�[���h��̃W���b�NUI��S�Ď擾
        m_jackables = new List<Jackable>(FindObjectsOfType<Jackable>());

        foreach(var jack in m_jackables)
        {
            const float DepthAdjust = -0.005f;
            const float PlaneAdjust = 10.0f;    //�v���[�����g�p�����Ƃ��̒����p���l

            //�t�B�[���h�S�̂̃p�[�Z���e�[�W�̂ǂ̈ʒu�ɂ��邩���v�Z
            Vector3 halfSizeVector = (m_floor.transform.lossyScale * 0.5f) * PlaneAdjust;
            var inversePoint = jack.transform.position;
            //var inversePoint = m_floor.transform.InverseTransformPoint(jack.transform.position);

            float x = inversePoint.x / halfSizeVector.x;
            float y = inversePoint.z / halfSizeVector.z;

            var ratioSize = new Vector2(x, y);   //�����T�C�Y�𐶐�

            var offset = Vector3.zero;
            halfSizeVector = m_mapObject.transform.lossyScale * 0.5f * PlaneAdjust;
            Debug.Log("��" + halfSizeVector);
            offset.x = ratioSize.x * halfSizeVector.x;
            offset.y = ratioSize.y * halfSizeVector.z;
            offset.z = DepthAdjust;

            var position = m_mapObject.transform.position + (transform.rotation * offset);

            var newObj = Instantiate(m_prefab, position, Quaternion.identity, transform);
            newObj.transform.localRotation = Quaternion.identity;
        }
    }
}
