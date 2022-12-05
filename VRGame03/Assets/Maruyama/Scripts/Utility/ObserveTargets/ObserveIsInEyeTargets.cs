using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

//--------------------------------------------------------------------------------------
/// �Ď��Ώۂ����E�͈͂ɂ��邩�ǂ����𔻒f����N���X
//--------------------------------------------------------------------------------------
public class ObserveIsInEyeTargets
{
    private List<GameObject> m_observeTargets = new List<GameObject>();  //�Ď��Ώ�

    private EyeSearchRange m_eyeRange;  //���E�Ǘ��R���|�[�l���g

    public ObserveIsInEyeTargets(List<GameObject> observeTargets, EyeSearchRange eyeRange) 
    {
        m_observeTargets = observeTargets;
        m_eyeRange = eyeRange;
    }

    public GameObject SearchIsInEyeTarget() {
        if(m_eyeRange == null) {    //���E�Ǘ����Ȃ��Ȃ珈�����ł��Ȃ�
            Debug.Log("EyeSearchRange�R���|�[�l���g�����݂��܂���B");
            return null;
        }

        foreach(var target in m_observeTargets) {
            if(target == null) {    //�^�[�Q�b�g�����݂��Ȃ��Ȃ�A�������΂��B
                continue;
            }

            //���E�͈͓��Ȃ�A�^�[�Q�b�g���擾
            if (m_eyeRange.IsInEyeRange(target.transform.position)) {
                return target;
            }
        }

        return null;    //�����ł��Ȃ��������߁Anull��Ԃ��B
    }

    public List<GameObject> SearchIsInEyeTargets() {
        var result = new List<GameObject>();

        if (m_eyeRange == null) {    //���E�Ǘ����Ȃ��Ȃ珈�����ł��Ȃ�
            Debug.Log("EyeSearchRange�R���|�[�l���g�����݂��܂���B");
            return result;
        }

        foreach(var target in m_observeTargets)
        {
            if(target == null) {
                continue;
            }

            //�^�[�Q�b�g�����E���Ȃ�A�z��ɓ����B
            if (m_eyeRange.IsInEyeRange(target.transform.position)) {
                //Debug.Log("�^�[�Q�b�g�ǉ�");
                result.Add(target);
            }
        }

        return result;
    }

    float IsNearTarget(GameObject left, GameObject right)
    {
        var toLeftRange = Vector3.Magnitude(left.transform.position - m_eyeRange.transform.position);
        var toRightRange = Vector3.Magnitude(right.transform.position - m_eyeRange.transform.position);

        //return toLeftRange.CompareTo(toRightRange);

        return toLeftRange - toRightRange;
    }

    public GameObject SerachNearIsInEyeTarget()
    {
        var targets = SearchIsInEyeTargets();

        //�\�[�g
        targets.OrderBy(value => (value.transform.position - m_eyeRange.transform.position).magnitude);

        foreach(var target in targets)
        {
            //�^�[�Q�b�g���^�[�Q�e�B���O��ԂȂ�
            var targeted = target.GetComponent<Targeted>();
            if (targeted.IsTarget()) {
                return target;
            }
        }

        return null;
    }

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public void AddObserveTarget(GameObject target) { m_observeTargets.Add(target); }

    public void SetObserveTargets(List<GameObject> targets) { m_observeTargets = targets; }

    public List<GameObject> GetObserveTargets() { return m_observeTargets; }

    public void ClearObserveTargets() { m_observeTargets.Clear(); }
}
