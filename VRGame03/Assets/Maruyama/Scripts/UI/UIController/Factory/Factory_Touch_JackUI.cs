using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory_Touch_JackUI : MonoBehaviour
{
    /// <summary>
    /// �y�A�f�[�^
    /// </summary>
    public struct PairData
    {
        public Jackable jakable;    //�W���b�N������
        public JackPointUI ui;      //UI

        public PairData(Jackable jackable, JackPointUI ui)
        {
            this.jakable = jackable;
            this.ui = ui;
        }
    }

    private const float PlaneAdjust = 10.0f;    //�v���[�����g�p�����Ƃ��̒����p���l

    [SerializeField]
    private GameObject m_floor;

    [SerializeField]
    private GameObject m_mapObject;     //MapVisual

    [SerializeField]
    private UIStretchController m_stretchParent; //�L�k�̐e�N���X

    [SerializeField]
    private GameObject m_createPrefab;   

    [SerializeField]
    private float m_depthAdjust = -0.005f;

    private List<Jackable> m_jackables = new List<Jackable>();          //�S�ẴW���b�N�I�u�W�F�N�g

    private List<PairData> m_pairDatas = new List<PairData>();          //���������|�C���g�̃y�A�f�[�^�z��

    private void Awake()
    {
        //�t�B�[���h��̃W���b�NUI��S�Ď擾
        m_jackables = new List<Jackable>(FindObjectsOfType<Jackable>());

        foreach(var jack in m_jackables)
        {
            var ratioSize = CalculateFloorRatioSize(jack);

            var offset = CalculateOffset(ratioSize);

            var position = m_mapObject.transform.position + (transform.rotation * offset);

            var newObject = Instantiate(m_createPrefab, position, Quaternion.identity, transform);
            newObject.transform.localRotation = Quaternion.identity;

            var jackUI = newObject.GetComponent<JackPointUI>();    //JackPointUI�擾

            m_pairDatas.Add(new PairData(jack, jackUI));           //�y�A�f�[�^�̐���
        }
    }

    /// <summary>
    /// �t�B�[���h�S�̂̃p�[�Z���e�[�W�̂ǂ̈ʒu�ɂ��邩���v�Z
    /// </summary>
    /// <param name="jack">�W���b�N�Ώ�</param>
    /// <returns></returns>
    private Vector2 CalculateFloorRatioSize(Jackable jack)
    {
        Vector3 halfSizeVector = (m_floor.transform.lossyScale * 0.5f) * PlaneAdjust;
        var inversePoint = jack.transform.position;

        float x = inversePoint.x / halfSizeVector.x;
        float y = inversePoint.z / halfSizeVector.z;

        var ratioSize = new Vector2(x, y);   //�����T�C�Y�𐶐�

        return ratioSize;
    }

    private Vector3 CalculateOffset(Vector2 ratioSize)
    {
        var offset = Vector3.zero;
        var halfSizeVector = m_mapObject.transform.lossyScale * 0.5f * PlaneAdjust;
        offset.x = ratioSize.x * halfSizeVector.x;
        offset.y = ratioSize.y * halfSizeVector.z;
        offset.z = m_depthAdjust;

        return offset;
    }

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public List<PairData> GetPariDatas() { return m_pairDatas; }

}
