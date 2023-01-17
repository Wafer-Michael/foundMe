using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 視界範囲の影響データ影響を与える
/// </summary>
public class EyeScopeImpactCellManager : MonoBehaviour
{
    private EyeSearchRange m_eyeRange;

    private SelfImpactCellController m_selfImpactCellController;

    private void Awake()
    {
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_selfImpactCellController = GetComponent<SelfImpactCellController>();
    }

    private void Update()
    {
        UpdateEyeScope_FloodFill();
    }

    private void UpdateEyeScope_FloodFill()
    {
        //現在位置のCellが存在しないなら処理をしない
        if (!m_selfImpactCellController.HasCurrentCell()) {
            return;
        }

        ImpactCell startCell = m_selfImpactCellController.GetCurrentCell();
        CellMap<ImpactCell> cellMap = AIDirector.Instance.GetImpactCellMap();

        //startCell = cellMap.FindDirectionCell(m_selfImpactCellController.GetCurrentCell().GetIndex(), transform.forward);
        var openCells = new Queue<ImpactCell>();
        var closeCells = new Queue<ImpactCell>();
        openCells.Enqueue(startCell);

        while(openCells.Count != 0)
        {
            var currentCell = openCells.Dequeue();  //詮索するセルを取得
            closeCells.Enqueue(currentCell);        //クローズリストに登録

            //八方向のセルを取得
            var cells = cellMap.FindEightDirectionCells(currentCell.GetIndex());

            foreach(var cell in cells)
            {
                //オープンデータに登録できるかどうか
                if(IsAddOpenCells(cell, openCells, closeCells))
                {
                    openCells.Enqueue(cell);
                }
            }
        }

        foreach(var c in closeCells)
        {
            c.SetDangerValue(0);
        }
    }

    private bool IsAddOpenCells(ImpactCell cell, Queue<ImpactCell> openCells, Queue<ImpactCell> closeCells)
    {
        //すでにオープンデータに登録されている場合
        if(openCells.Contains(cell)) {
            return false;
        }

        //すでにクローズデータに登録されている場合
        if (closeCells.Contains(cell)) {
            return false;
        }

        //視界内に存在しない場合。
        if (!m_eyeRange.IsInEyeRange(cell.GetPosition())) {
            return false;
        }

        return true;    //全ての条件をクリアしたため、視界内のセル
    }

}

