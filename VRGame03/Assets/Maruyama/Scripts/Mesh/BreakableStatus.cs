using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class BreakableStatus : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor(3.0f);

    [System.Serializable]
    public struct Parametor
    {
        public float hp;    //���x�܂Ń_���[�W��ς����邱�Ƃ��ł��邩�ǂ���

        public Parametor(float hp)
        {
            this.hp = hp;
        }
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;  //�p�����[�^
    public Parametor Param => m_param;

    //private GK.BreakableSurface m_breakableSurface; //���鏈�����܂Ƃ߂���(�q�N���X�Ƃ��č��)

    private void Awake()
    {
        //m_breakableSurface = GetComponentInChildren<GK.BreakableSurface>();
    }

    public void Damaged(DamageData data, GK.BreakableSurface breakableSurface)
    {
        m_param.hp += -data.damageValue;

        if (IsDead()) { //���S�����Ȃ�
            m_param.hp = 0.0f;
            DeadProcess();
        }

        //���b�V�������鏈��
        breakableSurface?.Break(breakableSurface.transform.InverseTransformPoint(CalculationHitPoint(data)));
        //Break(data);
    }

    private Vector3 CalculationHitPoint(DamageData data)
    {
        //�����Ńq�b�g�|�C���g���v�Z
        var hitPoint = data.otherCollider.ClosestPoint(data.attacker.transform.position);
        return hitPoint;
    }

    /// <summary>
    /// ���b�V�������鏈��(���ݎg�p���Ă��Ȃ�)
    /// </summary>
    /// <param name="data">�_���[�W�f�[�^</param>
    private void Break(DamageData data)
    {
        if (!data.otherCollider) {
            Debug.Log("BreakableStatus::Break(DamageData data) : Collider�����݂��Ȃ����߁A���b�V��������Ȃ�");
            return;
        }

        var breakableSurface = data.otherCollider.GetComponent<GK.BreakableSurface>();
        if(breakableSurface == null) {
            Debug.Log("BreakableStatus::Break(DamageData data) : BreakableSurface�����݂��Ȃ����߁A���b�V��������Ȃ�");
            return;
        }

        var hitPoint = CalculationHitPoint(data);   //���������ꏊ�̌v�Z
        var children = GetComponentsInChildren<GK.BreakableSurface>();

        //��ԋ߂��ꏊ���擾���邽�߂Ƀ\�[�g
        var sortList = children.OrderBy(value =>
            {
                var childHitPoint = value.Collider.ClosestPoint(hitPoint);
                return (childHitPoint - hitPoint).magnitude;
            }
        );

        //��ԋ߂��^�[�Q�b�g�Ńu���C�N������B
        var target = sortList.ToArray()[0];
        target.Break(target.transform.InverseTransformPoint(hitPoint));
    }

    private void DeadProcess()
    {
        var children = GetComponentsInChildren<GK.BreakableSurface>();
        foreach(var child in children)
        {
            child.BreakSplit();
        }
    }

    public bool IsDead()
    {
        return m_param.hp <= 0.0f;
    }
}
