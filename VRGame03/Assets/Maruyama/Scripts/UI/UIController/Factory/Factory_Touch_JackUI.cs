using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory_Touch_JackUI : MonoBehaviour
{
    /// <summary>
    /// ペアデータ
    /// </summary>
    public struct PairData
    {
        public Jackable jakable;    //ジャックされる者
        public JackPointUI ui;      //UI

        public PairData(Jackable jackable, JackPointUI ui)
        {
            this.jakable = jackable;
            this.ui = ui;
        }
    }

    private const float PlaneAdjust = 10.0f;    //プレーンを使用したときの調整用数値

    [SerializeField]
    private GameObject m_floor;

    [SerializeField]
    private GameObject m_mapObject;     //MapVisual

    [SerializeField]
    private UIStretchController m_stretchParent; //伸縮の親クラス

    [SerializeField]
    private GameObject m_createPrefab;   

    [SerializeField]
    private float m_depthAdjust = -0.005f;

    private List<Jackable> m_jackables = new List<Jackable>();          //全てのジャックオブジェクト

    private List<PairData> m_pairDatas = new List<PairData>();          //生成したポイントのペアデータ配列

    private void Awake()
    {
        //フィールド上のジャックUIを全て取得
        m_jackables = new List<Jackable>(FindObjectsOfType<Jackable>());

        foreach(var jack in m_jackables)
        {
            var ratioSize = CalculateFloorRatioSize(jack);

            var offset = CalculateOffset(ratioSize);

            var position = m_mapObject.transform.position + (transform.rotation * offset);

            var newObject = Instantiate(m_createPrefab, position, Quaternion.identity, transform);
            newObject.transform.localRotation = Quaternion.identity;

            var jackUI = newObject.GetComponent<JackPointUI>();    //JackPointUI取得

            m_pairDatas.Add(new PairData(jack, jackUI));           //ペアデータの生成
        }
    }

    /// <summary>
    /// フィールド全体のパーセンテージのどの位置にいるかを計算
    /// </summary>
    /// <param name="jack">ジャック対象</param>
    /// <returns></returns>
    private Vector2 CalculateFloorRatioSize(Jackable jack)
    {
        Vector3 halfSizeVector = (m_floor.transform.lossyScale * 0.5f) * PlaneAdjust;
        var inversePoint = jack.transform.position;

        float x = inversePoint.x / halfSizeVector.x;
        float y = inversePoint.z / halfSizeVector.z;

        var ratioSize = new Vector2(x, y);   //割合サイズを生成

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
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public List<PairData> GetPariDatas() { return m_pairDatas; }

}
