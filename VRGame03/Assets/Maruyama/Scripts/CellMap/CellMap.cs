using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セルフィールドデータ
/// </summary>
public struct CellMapFieldData
{
    public int widthCount;     //横の長さ
    public int depthCount;     //奥行きの長さ

    public CellMapFieldData(int widthCount, int depthCount)
    {
        this.widthCount = widthCount;
        this.depthCount = depthCount;
    }
}

public class CellMap<CellType>
    where CellType : Cell
{
    private List<CellType> m_cells = new List<CellType>();	        //セルマップのセル配列

    private CellMapFieldData m_fieldData = new CellMapFieldData();  //セルマップのフィールドデータ

    public CellMap() {
        m_cells = new List<CellType>();
        m_fieldData = new CellMapFieldData();
    }

    /// <summary>
    /// 指定したインデックスの八方向のセルを取得する。
    /// </summary>
    /// <param name="currentIndex">開始セルの位置</param>
    /// <returns></returns>
    public List<CellType> SerchEightDirectionCells(int currentIndex)
    {
        var result = new List<CellType>();

        //設定したいインデックス配列
        int[] indices = {
            currentIndex - 1,
            currentIndex + 1,
            currentIndex + (m_fieldData.widthCount),
            currentIndex + (m_fieldData.widthCount + 1),
            currentIndex + (m_fieldData.widthCount - 1),
            currentIndex + (m_fieldData.depthCount),
            currentIndex + (m_fieldData.depthCount + 1),
            currentIndex + (m_fieldData.depthCount - 1)
        };

        //追加する。
        foreach(int index in indices)
        {
            //0以上、かつ、Count以下なら、インデックスオーバーしない
            if(0 <= index && index < m_cells.Count)
            {
                result.Add(m_cells[index]);
            }
        }

        return result;
    }

    //--------------------------------------------------------------------------------------
    ///	アクセッサ
    //--------------------------------------------------------------------------------------

    public void SetCells(List<CellType> cells) { m_cells = cells; }
    
    public List<CellType> GetCells() { return m_cells; }

    public void SetFieldData(CellMapFieldData data) { m_fieldData = data; }

    public CellMapFieldData GetFieldData() { return m_fieldData; }

    //--------------------------------------------------------------------------------------
    /// デバッグ
    //--------------------------------------------------------------------------------------

    private GameObject m_parentDebugDrawObject;                             //デバッグオブジェクトの親オブジェクト
    private List<DebugDrawComponent> m_debugDrawObjects = new List<DebugDrawComponent>();   //生成したデバッグオブジェクト

    public void CreateDebugDrawObjects(DebugDrawComponent prefab, Factory.CellMap.Parametor factoryParametor, DebugDrawComponent.Parametor? drawParametor = null)
    {
        m_parentDebugDrawObject = new GameObject();

        foreach(var cell in m_cells)
        {
            var drawObject = Object.Instantiate(prefab, cell.GetPosition(), Quaternion.identity, m_parentDebugDrawObject.transform);
            drawObject.transform.localScale = CalculateDebugObjectScale(factoryParametor);

            //表示用のパラメータを受け取っているなら
            if(drawParametor != null)
            {
                var drawComponent = drawObject.GetComponent<DebugDrawComponent>();  //表示系オブジェクトの取得
                if (drawComponent) {
                    drawComponent.Param = drawParametor.Value;
                }
            }

            m_debugDrawObjects.Add(drawObject);
        }
    }

    /// <summary>
    /// デバッグオブジェクトのスケール計算
    /// </summary>
    /// <param name="factoryParametor">生成パラメータ</param>
    /// <param name="scaleAdjust">スケールの調整値</param>
    /// <returns>生成したいスケールを返す</returns>
    public Vector3 CalculateDebugObjectScale(Factory.CellMap.Parametor factoryParametor, float scaleAdjust = 0.95f)
    {
        float width = factoryParametor.oneCellRect.width * scaleAdjust;
        float depth = factoryParametor.oneCellRect.depth * scaleAdjust;

        return new Vector3(width, 0.0f, depth);
    }

    public List<DebugDrawComponent> GetDebugDrawObjects() { return m_debugDrawObjects; }

}
