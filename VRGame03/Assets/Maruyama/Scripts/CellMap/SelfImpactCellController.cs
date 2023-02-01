using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������g�̂���Z���̏����X�V����
/// </summary>
public class SelfImpactCellController : MonoBehaviour
{
    private ImpactCell m_currentCell;   //���ݎ������������Ă���Z��

    private void Start()
    {
        InitializeCell();   //�Z���̏�����
    }

    private void Update()
    {
        if (m_currentCell == null) {
            Debug.Log("CurrentCell��Null�ł�");
            InitializeCell();
            return;
        }

        //���݂̃Z�����O�ɂł���
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
        var cells = cellMap.FindEightDirectionCells(m_currentCell.GetIndex()); //�������̃Z�����擾
        foreach (var cell in cells)
        {
            //�Z���͈͓̔��Ȃ�
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
            //�Z���͈͓̔��Ȃ�
            if (cell.GetRectData().IsInRect(transform.position))
            {
                m_currentCell = cell;
                break;
            }
        }
    }

    //--------------------------------------------------------------------------------------
    ///	�A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public ImpactCell GetCurrentCell() { return m_currentCell; }

    public bool HasCurrentCell() { return m_currentCell != null; }
}
