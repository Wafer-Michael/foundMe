using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag : MonoBehaviour
{
    private HashSet<Item> m_items = new HashSet<Item>();    //���[�����A�C�e���ꗗ

    /// <summary>
    /// �A�C�e���̒ǉ�
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item) { m_items.Add(item); }

    /// <summary>
    /// �A�C�e���̍폜
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item) { m_items.Remove(item); }

    /// <summary>
    /// �A�C�e���̎擾
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T TakeItem<T>()
        where T : class
    {
        foreach(var item in m_items)
        {
            var t = item.GetComponent<T>();
            if (t != null) {
                RemoveItem(item);
                return t;
            }
        }

        return null;
    }
}
