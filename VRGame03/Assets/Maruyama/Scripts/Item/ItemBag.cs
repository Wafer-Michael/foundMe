using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag : MonoBehaviour
{
    private HashSet<Item> m_items = new HashSet<Item>();    //収納したアイテム一覧

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item) { m_items.Add(item); }

    /// <summary>
    /// アイテムの削除
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item) { m_items.Remove(item); }

    /// <summary>
    /// アイテムの取得
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
