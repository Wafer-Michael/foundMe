using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_GraphEdge
{
    /// <summary>
    /// ��O�̃O���t��ݒ�
    /// </summary>
    /// <param name="node"></param>
    public void SetFromNode(I_GraphNode node);

    /// <summary>
    /// ��O�̃O���t���擾
    /// </summary>
    /// <returns></returns>
    public I_GraphNode GetFromNode();

    /// <summary>
    /// ��O�̃O���t�̃C���f�b�N�X���擾
    /// </summary>
    /// <returns></returns>
    public int GetFromIndex();

    /// <summary>
    /// ��̃O���t��ݒ�
    /// </summary>
    /// <param name="node"></param>
    public void SetToNode(I_GraphNode node);

    /// <summary>
    /// ��̃O���t���擾
    /// </summary>
    /// <returns></returns>
    public I_GraphNode GetToNode();

    /// <summary>
    /// ��̃O���t�̃C���f�b�N�X���擾
    /// </summary>
    /// <returns></returns>
    public int GetToIndex();

}
