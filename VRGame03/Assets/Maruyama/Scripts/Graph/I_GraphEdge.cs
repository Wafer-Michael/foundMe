using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_GraphEdge
{
    /// <summary>
    /// 手前のグラフを設定
    /// </summary>
    /// <param name="node"></param>
    public void SetFromNode(I_GraphNode node);

    /// <summary>
    /// 手前のグラフを取得
    /// </summary>
    /// <returns></returns>
    public I_GraphNode GetFromNode();

    /// <summary>
    /// 手前のグラフのインデックスを取得
    /// </summary>
    /// <returns></returns>
    public int GetFromIndex();

    /// <summary>
    /// 先のグラフを設定
    /// </summary>
    /// <param name="node"></param>
    public void SetToNode(I_GraphNode node);

    /// <summary>
    /// 先のグラフを取得
    /// </summary>
    /// <returns></returns>
    public I_GraphNode GetToNode();

    /// <summary>
    /// 先のグラフのインデックスを取得
    /// </summary>
    /// <returns></returns>
    public int GetToIndex();

}
