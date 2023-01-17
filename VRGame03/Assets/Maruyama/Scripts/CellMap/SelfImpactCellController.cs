using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfImpactCellController : MonoBehaviour
{
    private ImpactCell m_currentCell;   //現在自分が所属しているセル

    private void Start()
    {
        InitializeCell();   //セルの初期化
    }

    private void Update()
    {
        //現在のセルより外にでたら
        if (!m_currentCell.GetRectData().IsInRect(transform.position)) {
            UpdateCell();
        }
    }

    private void UpdateCell()
    {
        var cell = SearchNextCell();
        if (cell == null) {
            InitializeCell();
            return;
        }

        m_currentCell = cell;
    }

    private ImpactCell SearchNextCell()
    {
        var cellMap = AIDirector.Instance.GetImpactCellMap();
        var cells = cellMap.SerchEightDirectionCells(m_currentCell.GetIndex()); //八方向のセルを取得
        foreach (var cell in cells)
        {
            //セルの範囲内なら
            if (cell.GetRectData().IsInRect(transform.position))
            {
                return cell;
            }
        }

        return null;
    }

    private void InitializeCell()
    {
        foreach(var cell in AIDirector.Instance.GetImpactCellMap().GetCells())
        {
            //セルの範囲内なら
            if (cell.GetRectData().IsInRect(transform.position))
            {
                m_currentCell = cell;
                break;
            }
        }
    }

    //--------------------------------------------------------------------------------------
    ///	アクセッサ
    //--------------------------------------------------------------------------------------
}
