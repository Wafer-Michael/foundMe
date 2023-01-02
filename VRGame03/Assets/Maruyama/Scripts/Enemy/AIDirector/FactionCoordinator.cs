using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionCoordinator : CoordinatorBase
{
    Dictionary<System.Type, List<CoordinatorBase>> m_coodinatorsMap;   //コーディネータのマップ

    #region コンストラクタ

    public FactionCoordinator() :
        base()
    {
        m_coodinatorsMap = new Dictionary<System.Type, List<CoordinatorBase>>();
    }

    #endregion

    public override void OnCreate()
    {
        
    }

    public override void OnStart()
    {

    }

    public override bool OnUpdate()
    {
        foreach(var pair in m_coodinatorsMap)
        {
            foreach(var coordinator in pair.Value)
            {
                coordinator.OnUpdate();
            }
        }

        return false;
    }

    public override void OnExit()
    {

    }

    private CoordinatorType CreateCoordinator<CoordinatorType>()
        where CoordinatorType : CoordinatorBase, new()
    {
        var result = new CoordinatorType();

        result.OnCreate();
        result.OnStart();   //将来的に別のタイミングで呼ぶかも

        return result;
    }

    private CoordinatorType SearchAssignCoordinator<CoordinatorType>(I_FactionMember member)
        where CoordinatorType : CoordinatorBase, new()
    {
        var type = typeof(CoordinatorType);

        if (!m_coodinatorsMap.ContainsKey(type)) {  //マップに存在しないなら作成して返す。
            var newCoordinator = CreateCoordinator<CoordinatorType>();
            m_coodinatorsMap[type] = new List<CoordinatorBase>();
            m_coodinatorsMap[type].Add(newCoordinator);
            return newCoordinator;
        }

        //本来ならここで一番いいコーディネータを探す。(今は仮で一番前のデータを返す)
        var coodinators = m_coodinatorsMap[type];
        return coodinators[0] as CoordinatorType;
    }

    private void AssignCoordinator<CoordinatorType>(I_FactionMember member)
        where CoordinatorType : CoordinatorBase, new()
    {
        var type = typeof(CoordinatorBase);

        var coordinator = SearchAssignCoordinator<CoordinatorType>(member); //アサインするコーディネータの検索
        coordinator.AddMember(member);
    }

    public void TransitionCoordinator<CoordinatorType>(I_FactionMember member)
        where CoordinatorType : CoordinatorBase, new()
    {
        var type = typeof(CoordinatorType);

        if(member.GetAssignedCoordinator() != null) {   //すでにアサインしているなら
            //現在のコーディネータから外す。
            var coordinator = member.GetAssignedCoordinator();
            coordinator.RemoveMember(member);
        }

        AssignCoordinator<CoordinatorType>(member);
    }

    public override void AddMember(I_FactionMember member)
    {
        member.SetAssignedFaction(this);
        member.AssignedFactionEvent(this);
        m_members.Add(member);
    }

    public override void RemoveMember(I_FactionMember member)
    {
        member.SetAssignedFaction(null);
        member.UnsignedFactionEvent(this);
        m_members.Remove(member);
    }
}
