using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    private List<EnemyBase> m_members = new List<EnemyBase>();

    [Header("フィールドのエリアマップ"), SerializeField]
    private FieldCellMap m_areaMap;

    [Header("フィールドのウェイポイントマップ"), SerializeField]
    private FieldWayPointsMap m_wayPointsMap;

    private List<FactionCoordinator> m_factionCoordinators = new List<FactionCoordinator>();    //ファクションコーディネーター群

    private void Awake()
    {
        NullCheck();

        //ウェイポイントにエリア情報を割り当てる。
        SettingArea();
    }

    private void Update()
    {
        //ファクションコーディネーターの更新
        foreach(var faction in m_factionCoordinators)
        {
            faction.OnUpdate();
        }
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// ファクションコーディネータの生成(将来的に改良)
    /// </summary>
    private FactionCoordinator CreateFactionCoordinator()
    {
        var newFaction = new FactionCoordinator();
        newFaction.OnCreate();
        newFaction.OnStart();   //開始時に呼び出したい処理(将来的に別の場所で呼ぶかも)

        return newFaction;
    }

    /// <summary>
    /// ファクションコーディネータの追加
    /// </summary>
    /// <param name="factionCoordinator">追加したいファクション</param>
    private void AddFactionCoordinator(FactionCoordinator factionCoodinator)
    {
        m_factionCoordinators.Add(factionCoodinator);
    }

    /// <summary>
    /// ファクションコーディネーターの削除
    /// </summary>
    /// <param name="factionCoordinator">削除したいファクション</param>
    public void RemoveFactionCoordinator(FactionCoordinator factionCoordinator)
    {
        factionCoordinator.OnExit();
        m_factionCoordinators.Remove(factionCoordinator);
    }

    public List<FactionCoordinator> GetFactionCoordinators() { return m_factionCoordinators; }

    //--------------------------------------------------------------------------------------
    /// 初期セッティング
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// エリアのセッティング
    /// </summary>
    private void SettingArea()
    {
        foreach (var node in m_wayPointsMap.GetWayPointsMap().GetGraph().GetNodes())
        {
            foreach (var area in m_areaMap.GetCellMap().GetCells())
            {
                if (area.GetParametor().rect.IsInRect(node.GetPosition()))
                {
                    node.SetParent(area);   //エリアを親に設定する。
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Null確認
    /// </summary>
    private void NullCheck()
    {
        //エネミーを全て配列に入れる
        if (m_members.Count == 0)
        {
            m_members = new List<EnemyBase>(FindObjectsOfType<EnemyBase>());
        }

        //エリア用のフィールドセルマップを取得
        if (m_areaMap == null)
        {
            m_areaMap = GetComponentInChildren<FieldCellMap>();
        }

        //ウェイポイントマップを取得
        if (m_wayPointsMap == null)
        {
            m_wayPointsMap = GetComponentInChildren<FieldWayPointsMap>();
        }
    }

}
