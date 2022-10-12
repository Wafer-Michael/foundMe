using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        public float speed;         //�V���b�g�X�s�[�h
        public float interval;      //�V���b�g�Ԋu
        public GameObject prefab;   //��������v���n�u
        [SerializeField]
        private GameObject createPositionObject;    //�e��������ꏊ���L�^����v���n�u
        public Vector3 CreatePosition => createPositionObject.transform.position;   //�e�𐶐�����ꏊ
    }

    [SerializeField]
    private Parametor m_param;  //�p�����[�^
    public Parametor Param => m_param;

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="direction">������</param>
    /// <returns>���������e�I�u�W�F�N�g</returns>
    public GameObject Shot(Vector3 direction)
    {
        //�e�̐���
        var bulletObject = Instantiate(m_param.prefab, m_param.CreatePosition, Quaternion.identity);
        //�e�Ɍ��������Ƃ�`����B
        var bullet = bulletObject.GetComponent<BulletBase>();
        bullet?.Shot(direction, m_param.speed);

        return bulletObject;
    }
}
