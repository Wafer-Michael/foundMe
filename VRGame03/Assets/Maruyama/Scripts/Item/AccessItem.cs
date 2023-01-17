using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemBag))]
public class AccessItem : MonoBehaviour
{
    [SerializeField]
    private float m_accessRange = 3.0f;

    private List<Item> m_items = new List<Item>();
    private ItemBag m_itemBag;

    private void Awake()
    {
        m_itemBag = GetComponent<ItemBag>();
    }

    private void Start()
    {
        m_items = new List<Item>(FindObjectsOfType<Item>());
    }

    private void Update()
    {
        UpdateAccessItems();    //�A�C�e���̍X�V

        if (PlayerInputer.IsTakeNearItem())
        {
            TakeNearItem();
        }
    }

    private void UpdateAccessItems()
    {
        System.Action removeAction = null;

        foreach (var item in m_items)
        {
            if(item == null) {
                removeAction += () => m_items.Remove(item);
                continue;
            }

            //�A�C�e������A�N�e�B�u�Ȃ珈�������Ȃ�
            if (!item.gameObject.activeSelf)
            {
                continue;
            }

            var range = (item.transform.position - transform.position).magnitude;

            bool isInRange = range <= m_accessRange;    //�͈͓��ɂ��邩�ǂ���

            //�͈͓���Idle��ԂȂ�
            if (isInRange && item.CurrentState == Item.State.Idle)
            {
                item.SetState(Item.State.Access);   //�A�N�Z�X��Ԃɂ���B
                //Debug.Log("���A�N�Z�X");
                return;
            }

            //�͈͊O��Access��ԂȂ�
            if (!isInRange && item.CurrentState == Item.State.Access)
            {
                item.SetState(Item.State.Idle);     //Idle��Ԃɂ���B
                //Debug.Log("���A�C�h��");
                return;
            }
        }

        removeAction?.Invoke();
    }

    /// <summary>
    /// �߂��̃A�C�e�����擾����B
    /// </summary>
    public Item TakeNearItem()
    {
        foreach(var item in m_items)
        {
            //�A�N�Z�X��ԂłȂ��Ȃ�A��΂�
            if(item.CurrentState != Item.State.Access) {
                continue;
            }

            item.SetState(Item.State.Getable);  //�A�C�e�����Q�b�g��Ԃɂ���B
            m_itemBag.AddItem(item);            //�A�C�e�����o�b�O�ɂ����B
            //Debug.Log("���Q�b�^�[");
            return item;
        }

        return null;
    }

}
