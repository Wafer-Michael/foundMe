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
        UpdateAccessItems();    //アイテムの更新

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

            //アイテムが非アクティブなら処理をしない
            if (!item.gameObject.activeSelf)
            {
                continue;
            }

            var range = (item.transform.position - transform.position).magnitude;

            bool isInRange = range <= m_accessRange;    //範囲内にいるかどうか

            //範囲内でIdle状態なら
            if (isInRange && item.CurrentState == Item.State.Idle)
            {
                item.SetState(Item.State.Access);   //アクセス状態にする。
                //Debug.Log("★アクセス");
                return;
            }

            //範囲外でAccess状態なら
            if (!isInRange && item.CurrentState == Item.State.Access)
            {
                item.SetState(Item.State.Idle);     //Idle状態にする。
                //Debug.Log("★アイドル");
                return;
            }
        }

        removeAction?.Invoke();
    }

    /// <summary>
    /// 近くのアイテムを取得する。
    /// </summary>
    public Item TakeNearItem()
    {
        foreach(var item in m_items)
        {
            //アクセス状態でないなら、飛ばす
            if(item.CurrentState != Item.State.Access) {
                continue;
            }

            item.SetState(Item.State.Getable);  //アイテムをゲット状態にする。
            m_itemBag.AddItem(item);            //アイテムをバッグにいれる。
            //Debug.Log("★ゲッター");
            return item;
        }

        return null;
    }

}
