using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セルフィールドデータ
/// </summary>
public struct CellMapFieldData
{
    public float width;     //横の長さ
    public float depth;     //奥行きの長さ

    public CellMapFieldData(float width, float depth)
    {
        this.width = width;
        this.depth = depth;
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
