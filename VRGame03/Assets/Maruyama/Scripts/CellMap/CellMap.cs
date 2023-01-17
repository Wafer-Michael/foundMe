using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMap<CellType>
    where CellType : Cell
{
    private List<CellType> m_cells;	//�Z���}�b�v�̃Z���z��

    public CellMap() {
        m_cells = new List<CellType>();
    }

    //--------------------------------------------------------------------------------------
    ///	�A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public void SetCells(List<CellType> cells) { m_cells = cells; }
    
    public List<CellType> GetCells() { return m_cells; }


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
