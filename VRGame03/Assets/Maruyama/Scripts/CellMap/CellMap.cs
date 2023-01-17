using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

/// <summary>
/// �Z���t�B�[���h�f�[�^
/// </summary>
public struct CellMapFieldData
{
    public int widthCount;     //���̒���
    public int depthCount;     //���s���̒���

    public CellMapFieldData(int widthCount, int depthCount)
    {
        this.widthCount = widthCount;
        this.depthCount = depthCount;
    }
}

public class CellMap<CellType>
    where CellType : Cell
{
    private List<CellType> m_cells = new List<CellType>();	        //�Z���}�b�v�̃Z���z��

    private CellMapFieldData m_fieldData = new CellMapFieldData();  //�Z���}�b�v�̃t�B�[���h�f�[�^

    public CellMap() {
        m_cells = new List<CellType>();
        m_fieldData = new CellMapFieldData();
    }

    /// <summary>
    /// �������̃C���f�b�N�X���擾
    /// </summary>
    /// <param name="currentIndex"></param>
    /// <returns></returns>
    public int[] GetEightDirectionIndices(int currentIndex)
    {
        //�ݒ肵�����C���f�b�N�X�z��
        int[] indices = {
            currentIndex - 1,
            currentIndex + 1,
            currentIndex + (m_fieldData.widthCount),
            currentIndex + (m_fieldData.widthCount + 1),
            currentIndex + (m_fieldData.widthCount - 1),
            currentIndex - (m_fieldData.widthCount),
            currentIndex - (m_fieldData.widthCount + 1),
            currentIndex - (m_fieldData.widthCount - 1)
        };

        return indices;
    }

    /// <summary>
    /// ���������擾����B
    /// </summary>
    /// <param name="currentIndex"></param>
    /// <returns></returns>
    public List<Vector3> GetEightDirections(int currentIndex)
    {
        var result = new List<Vector3>();

        var currentCell = m_cells[currentIndex];
        var cells = FindEightDirectionCells(currentIndex);

        foreach(var cell in cells)
        {
            var toCellVec = cell.GetPosition() - currentCell.GetPosition();
            result.Add(toCellVec.normalized);
        }

        return result;
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�̔������̃Z�����擾����B
    /// </summary>
    /// <param name="currentIndex">�J�n�Z���̈ʒu</param>
    /// <returns></returns>
    public List<CellType> FindEightDirectionCells(int currentIndex)
    {
        var result = new List<CellType>();

        //�ݒ肵�����C���f�b�N�X�z��
        var indices = GetEightDirectionIndices(currentIndex);

        //�ǉ�����B
        foreach(int index in indices)
        {
            //0�ȏ�A���ACount�ȉ��Ȃ�A�C���f�b�N�X�I�[�o�[���Ȃ�
            if(0 <= index && index < m_cells.Count)
            {
                result.Add(m_cells[index]);
            }
        }

        return result;
    }

    /// <summary>
    /// �w�肵�������̃Z�����擾
    /// </summary>
    /// <param name="currentIndex"></param>
    /// <param name="forward"></param>
    /// <returns></returns>
    public CellType FindDirectionCell(int currentIndex, Vector3 forward)
    {
        var currentCell = m_cells[currentIndex];
        var cells = FindEightDirectionCells(currentIndex);

        if(cells.Count == 0) {
            return null;
        }

        foreach (var cell in cells)
        {
            var toCellVec = cell.GetPosition() - currentCell.GetPosition();
            var newDot = Vector3.Dot(forward.normalized, toCellVec.normalized);
            var newRad = Mathf.Acos(newDot);
        }

        var sortCells = cells.OrderBy(cell => {
            var toCellVec = cell.GetPosition() - currentCell.GetPosition();
            var newDot = Vector3.Dot(forward.normalized, toCellVec.normalized);
            var newRad = Mathf.Acos(newDot);
            return newRad;
        });

        return sortCells.ToArray()[0];
    }

    //--------------------------------------------------------------------------------------
    ///	�A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public void SetCells(List<CellType> cells) { m_cells = cells; }
    
    public List<CellType> GetCells() { return m_cells; }

    public void SetFieldData(CellMapFieldData data) { m_fieldData = data; }

    public CellMapFieldData GetFieldData() { return m_fieldData; }

    //--------------------------------------------------------------------------------------
    /// �f�o�b�O
    //--------------------------------------------------------------------------------------

    private GameObject m_parentDebugDrawObject;                             //�f�o�b�O�I�u�W�F�N�g�̐e�I�u�W�F�N�g
    private List<DebugDrawComponent> m_debugDrawObjects = new List<DebugDrawComponent>();   //���������f�o�b�O�I�u�W�F�N�g

    public void CreateDebugDrawObjects(DebugDrawComponent prefab, Factory.CellMap.Parametor factoryParametor, DebugDrawComponent.Parametor? drawParametor = null)
    {
        m_parentDebugDrawObject = new GameObject();

        foreach(var cell in m_cells)
        {
            var drawObject = Object.Instantiate(prefab, cell.GetPosition(), Quaternion.identity, m_parentDebugDrawObject.transform);
            drawObject.transform.localScale = CalculateDebugObjectScale(factoryParametor);

            //�\���p�̃p�����[�^���󂯎���Ă���Ȃ�
            if(drawParametor != null)
            {
                var drawComponent = drawObject.GetComponent<DebugDrawComponent>();  //�\���n�I�u�W�F�N�g�̎擾
                if (drawComponent) {
                    drawComponent.Param = drawParametor.Value;
                }
            }

            m_debugDrawObjects.Add(drawObject);
        }
    }

    /// <summary>
    /// �f�o�b�O�I�u�W�F�N�g�̃X�P�[���v�Z
    /// </summary>
    /// <param name="factoryParametor">�����p�����[�^</param>
    /// <param name="scaleAdjust">�X�P�[���̒����l</param>
    /// <returns>�����������X�P�[����Ԃ�</returns>
    public Vector3 CalculateDebugObjectScale(Factory.CellMap.Parametor factoryParametor, float scaleAdjust = 0.95f)
    {
        float width = factoryParametor.oneCellRect.width * scaleAdjust;
        float depth = factoryParametor.oneCellRect.depth * scaleAdjust;

        return new Vector3(width, 0.0f, depth);
    }

    public List<DebugDrawComponent> GetDebugDrawObjects() { return m_debugDrawObjects; }

}
