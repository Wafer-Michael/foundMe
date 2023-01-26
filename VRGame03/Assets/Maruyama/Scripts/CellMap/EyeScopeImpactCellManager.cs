using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���E�͈͂̉e���f�[�^�e����^����
/// </summary>
public class EyeScopeImpactCellManager : MonoBehaviour
{
    private EyeSearchRange m_eyeRange;                              //���E�͈�

    private SelfImpactCellController m_selfImpactCellController;    //�������g�̉e���}�b�v�X�V

    private void Awake()
    {
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_selfImpactCellController = GetComponent<SelfImpactCellController>();
    }

    private void Update()
    {
        var inScopeCells = FindEyeScopeCells_FloodFill();   //���E���̃Z�����擾

        foreach(var cell in inScopeCells) {
            cell.SetDangerValue(0); //�댯�x��0�ɂ���B
        }
    }

    private Queue<ImpactCell> FindEyeScopeCells_FloodFill()
    {
        //���݈ʒu��Cell�����݂��Ȃ��Ȃ珈�������Ȃ�
        if (!m_selfImpactCellController.HasCurrentCell()) {
            return null;
        }

        ImpactCell startCell = m_selfImpactCellController.GetCurrentCell();
        CellMap<ImpactCell> cellMap = AIDirector.Instance.GetImpactCellMap();

        var openCells = new Queue<ImpactCell>();
        var closeCells = new Queue<ImpactCell>();
        openCells.Enqueue(startCell);

        while(openCells.Count != 0)
        {
            var currentCell = openCells.Dequeue();  //�F������Z�����擾
            closeCells.Enqueue(currentCell);        //�N���[�Y���X�g�ɓo�^

            //�������̃Z�����擾
            var cells = cellMap.FindEightDirectionCells(currentCell.GetIndex());

            foreach(var cell in cells)
            {
                //�I�[�v���f�[�^�ɓo�^�ł��邩�ǂ���
                if(IsAddOpenCells(cell, openCells, closeCells)) {
                    openCells.Enqueue(cell);
                }
            }
        }

        return closeCells;
    }

    private bool IsAddOpenCells(ImpactCell cell, Queue<ImpactCell> openCells, Queue<ImpactCell> closeCells)
    {
        //���łɃI�[�v���f�[�^�ɓo�^����Ă���ꍇ
        if(openCells.Contains(cell)) {
            return false;
        }

        //���łɃN���[�Y�f�[�^�ɓo�^����Ă���ꍇ
        if (closeCells.Contains(cell)) {
            return false;
        }

        //���E���ɑ��݂��Ȃ��ꍇ�B
        if (!m_eyeRange.IsInEyeRange(cell.GetPosition())) {
            return false;
        }

        return true;    //�S�Ă̏������N���A�������߁A���E���̃Z��
    }

}

